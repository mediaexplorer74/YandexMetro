// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.UnavailableTile
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class UnavailableTile : ITile
  {
    private readonly System.Windows.Media.Imaging.BitmapSource _bitmapImage;
    private readonly ITileInfo _tileInfo;

    public UnavailableTile(System.Windows.Media.Imaging.BitmapSource bitmapImage, ITileInfo tileInfo, TileStatus tileStatus = TileStatus.NotFound)
    {
      this._bitmapImage = bitmapImage;
      this._tileInfo = tileInfo;
      this.Status = tileStatus;
    }

    public int DataOffset
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public int DataLength
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public byte[] Bytes
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public ITileInfo TileInfo
    {
      get => this._tileInfo;
      set => throw new NotImplementedException();
    }

    public TileStatus Status { get; set; }

    public ulong Time
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public int HeaderLength
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public TileRecord[] Records
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public ushort Version
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public TileMetadata Metadata
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public object BitmapSource
    {
      get => (object) this._bitmapImage;
      set => throw new NotImplementedException();
    }

    public short Scale
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
  }
}
