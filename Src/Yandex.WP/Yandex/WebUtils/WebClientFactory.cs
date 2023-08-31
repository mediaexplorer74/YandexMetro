// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.WebClientFactory
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Globalization;
using System.Net;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  public class WebClientFactory : IWebClientFactory
  {
    public IHttpWebRequest CreateGetHttpWebRequest(Uri uri)
    {
      this.UpdateUriGet(ref uri);
      return (IHttpWebRequest) WebClientFactory.UpdateAcceptLanguage(new HttpWebRequestWrapper(WebRequest.CreateHttp(uri)));
    }

    public IHttpWebRequest CreatePostHttpWebRequest(Uri uri, string boundary)
    {
      this.UpdateUriPost(ref uri);
      return (IHttpWebRequest) WebClientFactory.UpdateAcceptLanguage(new HttpWebRequestWrapper(WebRequest.CreateHttp(uri))
      {
        ContentType = "multipart/form-data; boundary=" + boundary,
        Method = "POST"
      });
    }

    protected virtual void UpdateUriGet(ref Uri uri)
    {
    }

    protected virtual void UpdateUriPost(ref Uri uri)
    {
    }

    protected static void AddUriQueryParameter(ref Uri uri, string queryParameter)
    {
      UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.LocalPath)
      {
        Query = string.IsNullOrEmpty(uri.Query) ? queryParameter : uri.Query.Substring(1) + "&" + queryParameter
      };
      uri = uriBuilder.Uri;
    }

    private static HttpWebRequestWrapper UpdateAcceptLanguage(HttpWebRequestWrapper httpWebRequest)
    {
      httpWebRequest.SetHeader(HttpRequestHeader.AcceptLanguage, CultureInfo.CurrentCulture.Name);
      return httpWebRequest;
    }
  }
}
