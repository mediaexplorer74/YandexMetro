// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.HttpWebRequestWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Net;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal class HttpWebRequestWrapper : IHttpWebRequest
  {
    private readonly HttpWebRequest _httpWebRequest;
    private readonly bool _enableHeaderModification;

    public HttpWebRequestWrapper(HttpWebRequest httpWebRequest)
    {
      this._enableHeaderModification = Environment.OSVersion.Version >= new Version(7, 1);
      this._httpWebRequest = httpWebRequest != null ? httpWebRequest : throw new ArgumentNullException(nameof (httpWebRequest));
    }

    public IWebResponse EndGetResponse(IAsyncResult asyncResult) => (IWebResponse) new WebResponseWrapper((HttpWebResponse) this._httpWebRequest.EndGetResponse(asyncResult));

    public string ContentType
    {
      get => this._httpWebRequest.ContentType;
      set => this._httpWebRequest.ContentType = value;
    }

    public string Method
    {
      get => this._httpWebRequest.Method;
      set => this._httpWebRequest.Method = value;
    }

    public long ContentLength
    {
      get
      {
        long result;
        return !long.TryParse(this._httpWebRequest.Headers[HttpRequestHeader.ContentLength], out result) ? 0L : result;
      }
      set => this._httpWebRequest.Headers[HttpRequestHeader.ContentLength] = value.ToString();
    }

    public bool AcceptGzipEncoding
    {
      set
      {
        if (value)
          this.SetHeader(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
        else
          this.SetHeader(HttpRequestHeader.AcceptEncoding, (string) null);
      }
    }

    public CacheValidators CacheValidators
    {
      set
      {
        if (value == null)
          return;
        this.SetHeader(HttpRequestHeader.IfModifiedSince, value.LastModified);
        this.SetHeader(HttpRequestHeader.IfNoneMatch, value.EntityTag);
      }
    }

    public void SetHeader(HttpRequestHeader key, string value)
    {
      if (!this._enableHeaderModification)
        return;
      this.Headers[key] = value;
    }

    public void SetHeader(string key, string value)
    {
      if (!this._enableHeaderModification)
        return;
      this.Headers[key] = value;
    }

    public void BeginGetRequestStream(AsyncCallback callback, object state) => this._httpWebRequest.BeginGetRequestStream(callback, state);

    public Stream EndGetRequestStream(IAsyncResult asyncResult) => this._httpWebRequest.EndGetRequestStream(asyncResult);

    public IAsyncResult BeginGetResponse(AsyncCallback callback, object state) => this._httpWebRequest.BeginGetResponse(callback, state);

    public bool AllowReadStreamBuffering
    {
      get => this._httpWebRequest.AllowReadStreamBuffering;
      set => this._httpWebRequest.AllowReadStreamBuffering = value;
    }

    public WebHeaderCollection Headers
    {
      get => this._httpWebRequest.Headers;
      set => this._httpWebRequest.Headers = value;
    }
  }
}
