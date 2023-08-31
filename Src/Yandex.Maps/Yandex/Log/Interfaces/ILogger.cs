// Decompiled with JetBrains decompiler
// Type: Yandex.Log.Interfaces.ILogger
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Log.Interfaces
{
  internal interface ILogger
  {
    void Configure(string uri, string ipToCountryUrl);

    void Init(string appId, string appKey);

    void SendActivity(string activityName);

    void SendError(Exception ex);

    void SendCrash(object args);

    void OnAplicationActivated(object args);
  }
}
