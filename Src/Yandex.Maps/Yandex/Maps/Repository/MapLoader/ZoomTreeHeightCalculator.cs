// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.ZoomTreeHeightCalculator
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class ZoomTreeHeightCalculator : IZoomTreeHeightCalculator
  {
    private static byte[] _heightTreeForZooms;
    private readonly object _heightTreeLock;
    private readonly IZoomInfo _zoomInfo;

    public ZoomTreeHeightCalculator(IZoomInfo zoomInfo)
    {
      this._zoomInfo = zoomInfo != null ? zoomInfo : throw new ArgumentNullException(nameof (zoomInfo));
      this._heightTreeLock = new object();
    }

    public byte[] GetHeightTreeForZooms()
    {
      if (ZoomTreeHeightCalculator._heightTreeForZooms == null)
      {
        lock (this._heightTreeLock)
        {
          if (ZoomTreeHeightCalculator._heightTreeForZooms == null)
            this.CalculateTreeHeights();
        }
      }
      return ZoomTreeHeightCalculator._heightTreeForZooms;
    }

    private void CalculateTreeHeights()
    {
      ZoomTreeHeightCalculator._heightTreeForZooms = new byte[(int) this._zoomInfo.MaxZoom];
      for (byte index1 = 0; (int) index1 < (int) this._zoomInfo.MaxZoom; ++index1)
      {
        byte num1 = 0;
        long num2 = 4L << ((int) index1 << 1);
        for (long index2 = 1; index2 < num2; index2 <<= 8)
          ++num1;
        ZoomTreeHeightCalculator._heightTreeForZooms[(int) index1] = num1;
      }
    }
  }
}
