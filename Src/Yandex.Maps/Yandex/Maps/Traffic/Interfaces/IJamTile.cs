// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamTile
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Traffic.Interfaces
{
  [ComVisible(false)]
  public interface IJamTile : ITile, Yandex.Common.ICloneable
  {
    DateTime NextUpdateAt { get; }

    DateTime JamsExpireAt { get; }

    IList<TrafficInformer> Informers { get; }

    IList<Track> JamTracks { get; set; }
  }
}
