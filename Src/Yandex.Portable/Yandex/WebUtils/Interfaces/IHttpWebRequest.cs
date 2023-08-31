// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IHttpWebRequest
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.IO;
using System.Net;

namespace Yandex.WebUtils.Interfaces
{
  public interface IHttpWebRequest
  {
    bool AllowReadStreamBuffering { get; set; }

    string ContentType { get; set; }

    string Method { get; set; }

    long ContentLength { get; set; }

    bool AcceptGzipEncoding { set; }

    CacheValidators CacheValidators { set; }

    IAsyncResult BeginGetResponse(AsyncCallback callback, object state);

    IWebResponse EndGetResponse(IAsyncResult asyncResult);

    void BeginGetRequestStream(AsyncCallback getRequestStreamCallback, object state);

    Stream EndGetRequestStream(IAsyncResult asynchronousResult);

    void SetHeader(HttpRequestHeader key, string value);

    void SetHeader(string key, string value);
  }
}
