// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.TileBlock
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class TileBlock : ITileBlock, IEquatable<TileBlock>
  {
    public TileBlock(int x, int y, byte zoom, BaseLayers type)
    {
      this.X = x;
      this.Y = y;
      this.Zoom = zoom;
      this.Type = type;
    }

    public override bool Equals(object obj) => this.EqualsInternal(obj as ITileBlock);

    public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Zoom.GetHashCode() ^ this.Type.GetHashCode();

    public byte Zoom { get; private set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public BaseLayers Type { get; private set; }

    public static bool operator ==(TileBlock a, ITileBlock b) => a != (ITileBlock) null && a.Equals((object) b);

    public static bool operator !=(TileBlock a, ITileBlock b) => !(a == b);

    public bool Equals(TileBlock other) => this.EqualsInternal((ITileBlock) other);

    public bool EqualsInternal(ITileBlock other) => other != null && (int) other.Zoom == (int) this.Zoom && other.X == this.X && other.Y == this.Y && other.Type == this.Type;
  }
}
