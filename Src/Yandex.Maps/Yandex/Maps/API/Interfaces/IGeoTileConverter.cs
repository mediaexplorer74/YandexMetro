﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.IGeoTileConverter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Positioning;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface IGeoTileConverter
  {
    void GetTileInfoGeoCoordinates(
      ITileInfo tileInfo,
      out GeoCoordinate topLeft,
      out GeoCoordinate bottomRight);
  }
}
