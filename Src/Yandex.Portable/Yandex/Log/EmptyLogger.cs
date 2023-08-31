// Decompiled with JetBrains decompiler
// Type: Yandex.Log.EmptyLogger
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using Yandex.Log.Interfaces;

namespace Yandex.Log
{
  public class EmptyLogger : ILogger
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
