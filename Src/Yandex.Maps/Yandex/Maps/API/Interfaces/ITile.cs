// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Interfaces.ITile
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;

namespace Yandex.Maps.API.Interfaces
{
  [ComVisible(true)]
  public interface ITile
  {
    int DataOffset { get; set; }

    int DataLength { get; set; }

    byte[] Bytes { get; set; }

    ITileInfo TileInfo { get; set; }

    TileStatus Status { get; set; }

    ulong Time { get; set; }

    int HeaderLength { get; set; }

    TileRecord[] Records { get; set; }

    ushort Version { get; set; }

    TileMetadata Metadata { get; set; }

    object BitmapSource { get; set; }

    short Scale { get; set; }
  }
}
