// Decompiled with JetBrains decompiler
// Type: Yandex.Log.Interfaces.ILogger
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Log.Interfaces
{
  public interface ILogger
  {
    void Configure(string uri, string ipToCountryUrl);

    void Init(string appId, string appKey);

    void SendActivity(string activityName);

    void SendError(Exception ex);

    void SendCrash(object args);

    void OnAplicationActivated(object args);
  }
}
