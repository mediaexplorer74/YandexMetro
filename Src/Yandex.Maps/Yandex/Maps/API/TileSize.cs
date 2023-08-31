// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.TileSize
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class TileSize : ITileSize
  {
    public TileSize(uint width, uint height)
    {
      this.Width = width;
      this.Height = height;
    }

    public uint Width { get; private set; }

    public uint Height { get; private set; }
  }
}
