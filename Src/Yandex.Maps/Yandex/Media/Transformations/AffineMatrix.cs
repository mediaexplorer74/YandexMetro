// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Transformations.AffineMatrix
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media.Transformations.Interfaces;

namespace Yandex.Media.Transformations
{
  internal struct AffineMatrix : IAffineMatrix
  {
    public AffineMatrix(
      double m11,
      double m12,
      double m21,
      double m22,
      double offsetX,
      double offsetY)
      : this()
    {
      this.M11 = m11;
      this.M12 = m12;
      this.M21 = m21;
      this.M22 = m22;
      this.OffsetX = offsetX;
      this.OffsetY = offsetY;
    }

    public double M11 { get; private set; }

    public double M12 { get; private set; }

    public double M21 { get; private set; }

    public double M22 { get; private set; }

    public double OffsetX { get; private set; }

    public double OffsetY { get; private set; }
  }
}
