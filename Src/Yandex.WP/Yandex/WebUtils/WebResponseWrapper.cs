// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.WebResponseWrapper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.IO;
using System.Net;
using Yandex.WebUtils.Interfaces;

namespace Yandex.WebUtils
{
  public class WebResponseWrapper : IWebResponse, IDisposable
  {
    private bool _disposed;
    private HttpWebResponse _webResponse;

    public WebResponseWrapper(HttpWebResponse webResponse)
    {
      this._webResponse = webResponse != null ? webResponse : throw new ArgumentNullException(nameof (webResponse));
      this._disposed = false;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public Stream GetResponseStream() => this._webResponse.GetResponseStream();

    public WebHeaderCollection Headers => this._webResponse.Headers;

    public Yandex.Net.HttpStatusCode StatusCode => (Yandex.Net.HttpStatusCode) this._webResponse.StatusCode;

    public CacheValidators CacheValidators => new CacheValidators()
    {
      EntityTag = this._webResponse.Headers["ETag"],
      LastModified = this._webResponse.Headers["Last-Modified"]
    };

    public void Close() => this._webResponse.Close();

    ~WebResponseWrapper() => this.Dispose(false);

    private void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      int num = disposing ? 1 : 0;
      this._disposed = true;
      this._webResponse.Dispose();
      this._webResponse = (HttpWebResponse) null;
    }
  }
}
