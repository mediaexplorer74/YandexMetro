// Decompiled with JetBrains decompiler
// Type: Yandex.Log.EmptyLogger
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Log.Interfaces;

namespace Yandex.Log
{
  internal class EmptyLogger : ILogger
  {
    public void Configure(string uri, string ipToCountryUrl)
    {
    }

    public void Init(string appId, string appKey)
    {
    }

    public void SendActivity(string activityName)
    {
    }

    public void SendError(Exception ex)
    {
    }

    public void SendCrash(object args)
    {
    }

    public void OnAplicationActivated(object args)
    {
    }
  }
}
