// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.ITileInfo
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface ITileInfo
  {
    int X { get; }

    int Y { get; }

    byte Zoom { get; }

    BaseLayers Layer { get; }

    ushort MapVersion { get; set; }

    byte[] Checksum { get; set; }

    short Index { get; set; }

    int IdSort { get; set; }

    bool EqualsCoordinates(ITileInfo item);
  }
}
