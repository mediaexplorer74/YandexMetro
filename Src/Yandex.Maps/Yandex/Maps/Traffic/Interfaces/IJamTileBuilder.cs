﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamTileBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Traffic.DTO.Tracks;

namespace Yandex.Maps.Traffic.Interfaces
{
  [ComVisible(false)]
  public interface IJamTileBuilder : ITileFactory<IJamTile>
  {
    IJamTile CreateTile(ITileInfo tileInfo, JamMeta meta);
  }
}