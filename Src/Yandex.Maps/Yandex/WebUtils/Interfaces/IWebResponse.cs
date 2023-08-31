﻿// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IWebResponse
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Net;

namespace Yandex.WebUtils.Interfaces
{
  internal interface IWebResponse : IDisposable
  {
    WebHeaderCollection Headers { get; }

    Yandex.Net.HttpStatusCode StatusCode { get; }

    CacheValidators CacheValidators { get; }

    Stream GetResponseStream();

    void Close();
  }
}
