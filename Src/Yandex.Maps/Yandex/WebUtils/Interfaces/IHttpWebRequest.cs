// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IHttpWebRequest
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Net;

namespace Yandex.WebUtils.Interfaces
{
  internal interface IHttpWebRequest
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
