// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Tile
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class Tile : ITile
  {
    public Tile(ITileInfo tileInfo, TileStatus tileStatus)
    {
      this.TileInfo = tileInfo != null ? tileInfo : throw new ArgumentNullException(nameof (tileInfo));
      this.Status = tileStatus;
    }

    public ITileInfo TileInfo { get; set; }

    public TileStatus Status { get; set; }

    public ulong Time { get; set; }

    public int HeaderLength { get; set; }

    public TileRecord[] Records { get; set; }

    public ushort Version { get; set; }

    public TileMetadata Metadata { get; set; }

    public object BitmapSource { get; set; }

    public short Scale { get; set; }

    public int DataOffset { get; set; }

    public int DataLength { get; set; }

    public byte[] Bytes { get; set; }

    public override string ToString() => string.Format("Tile: {0}, Status: {1}", (object) this.TileInfo.ToString(), (object) this.Status);
  }
}
