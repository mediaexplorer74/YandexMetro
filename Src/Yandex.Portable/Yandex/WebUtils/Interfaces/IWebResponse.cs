// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IWebResponse
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.IO;
using System.Net;

namespace Yandex.WebUtils.Interfaces
{
  public interface IWebResponse : IDisposable
  {
    WebHeaderCollection Headers { get; }

    Yandex.Net.HttpStatusCode StatusCode { get; }

    CacheValidators CacheValidators { get; }

    Stream GetResponseStream();

    void Close();
  }
}
