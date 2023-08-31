// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.DisplayTileSize
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;
using Yandex.Media;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class DisplayTileSize : IDisplayTileSize, ITileSize
  {
    private readonly ITileSize _tileSize;

    public DisplayTileSize([NotNull] ITileSize tileSize) => this._tileSize = tileSize != null ? tileSize : throw new ArgumentNullException(nameof (tileSize));

    public uint Width { get; private set; }

    public uint Height { get; private set; }

    public Size Size { get; private set; }

    public void UpdateTileSize(double tilesStretchFactor)
    {
      this.Width = (uint) ((double) this._tileSize.Width * tilesStretchFactor);
      this.Height = (uint) ((double) this._tileSize.Height * tilesStretchFactor);
      this.Size = new Size((double) this.Width, (double) this.Height);
    }
  }
}
