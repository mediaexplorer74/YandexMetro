// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.WebResponseWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Net;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  internal class WebResponseWrapper : IWebResponse, IDisposable
  {
    private readonly HttpWebResponse _webResponse;

    public WebResponseWrapper(HttpWebResponse webResponse) => this._webResponse = webResponse != null ? webResponse : throw new ArgumentNullException(nameof (webResponse));

    public Stream GetResponseStream() => this._webResponse.GetResponseStream();

    public WebHeaderCollection Headers => this._webResponse.Headers;

    public Yandex.Net.HttpStatusCode StatusCode => (Yandex.Net.HttpStatusCode) this._webResponse.StatusCode;

    public CacheValidators CacheValidators => new CacheValidators()
    {
      EntityTag = this._webResponse.Headers["ETag"],
      LastModified = this._webResponse.Headers["Last-Modified"]
    };

    public void Close() => this.Dispose();

    public void Dispose() => this._webResponse.Dispose();
  }
}
