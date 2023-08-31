// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.HttpWebRequestExtensions
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Y.Common.Extensions
{
  public static class HttpWebRequestExtensions
  {
    private static readonly Regex IsResponseCompressedRegex = new Regex("(gzip|deflate)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static void Post(
      this HttpWebRequest request,
      Action<PostState> requestCompleted,
      string postData = null,
      object userInputData = null)
    {
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.BeginGetRequestStream(new AsyncCallback(HttpWebRequestExtensions.OnRequestStreamReceived), (object) new PostState(request, requestCompleted, postData, userInputData));
    }

    private static void OnRequestStreamReceived(IAsyncResult ar)
    {
      PostState asyncState = (PostState) ar.AsyncState;
      string postData = asyncState.PostData;
      HttpWebRequest request = asyncState.Request;
      using (Stream requestStream = request.EndGetRequestStream(ar))
      {
        using (StreamWriter streamWriter = new StreamWriter(requestStream))
          streamWriter.Write(postData);
      }
      request.BeginGetResponse(new AsyncCallback(HttpWebRequestExtensions.OnResponseReceived), (object) asyncState);
    }

    private static void OnResponseReceived(IAsyncResult ar)
    {
      PostState asyncState = ar.AsyncState as PostState;
      try
      {
        using (Stream unGzippedStream = HttpWebRequestExtensions.GetUnGzippedStream((HttpWebResponse) asyncState.Request.EndGetResponse(ar)))
        {
          using (StreamReader streamReader = new StreamReader(unGzippedStream))
          {
            string end = streamReader.ReadToEnd();
            asyncState.ResponseContent = end;
          }
        }
      }
      catch (Exception ex)
      {
        if (ex is WebException)
          throw;
        else
          asyncState.ResponseError = ex;
      }
      asyncState.ResponseCompleted(asyncState);
    }

    public static Stream GetUnGzippedStream(HttpWebResponse response)
    {
      Stream responseStream = response.GetResponseStream();
      return response.IsCompressed() ? (Stream) new GZipStream(responseStream, CompressionMode.Decompress) : responseStream;
    }

    private static bool IsCompressed(this WebResponse response) => HttpWebRequestExtensions.IsResponseCompressedRegex.IsMatch(response.Headers["Content-Encoding"] ?? string.Empty);
  }
}
