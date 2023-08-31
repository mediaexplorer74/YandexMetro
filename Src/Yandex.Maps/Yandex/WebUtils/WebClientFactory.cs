// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.WebClientFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Net;
using Yandex.Globalization;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal class WebClientFactory : IWebClientFactory
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
      httpWebRequest.SetHeader(HttpRequestHeader.AcceptLanguage, CultureInfo.CurrentUICulture.GetSpecificName());
      return httpWebRequest;
    }
  }
}
