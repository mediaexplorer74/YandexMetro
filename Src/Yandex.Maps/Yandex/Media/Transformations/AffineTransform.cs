// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Transformations.AffineTransform
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Media.Transformations.Interfaces;

namespace Yandex.Media.Transformations
{
  internal class AffineTransform : IAffineTransform, IGeneralTransform
  {
    public AffineTransform() => this.Matrix = (IAffineMatrix) new AffineMatrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);

    public IGeneralTransform Inverse
    {
      get
      {
        double num1 = this.Matrix.M11 * this.Matrix.M22 - this.Matrix.M12 * this.Matrix.M21;
        if (num1 == 0.0)
          return (IGeneralTransform) null;
        double num2 = 1.0 / num1;
        IAffineMatrix affineMatrix = (IAffineMatrix) new AffineMatrix(this.Matrix.M22 * num2, -this.Matrix.M12 * num2, -this.Matrix.M21 * num2, this.Matrix.M11 * num2, (this.Matrix.OffsetY * this.Matrix.M21 - this.Matrix.OffsetX * this.Matrix.M22) * num2, (this.Matrix.OffsetX * this.Matrix.M12 - this.Matrix.OffsetY * this.Matrix.M11) * num2);
        return (IGeneralTransform) new AffineTransform()
        {
          Matrix = affineMatrix
        };
      }
    }

    public Point Transform(Point source) => new Point(source.X * this.Matrix.M11 + source.Y * this.Matrix.M21 + this.Matrix.OffsetX, source.Y * this.Matrix.M12 + source.Y * this.Matrix.M22 + this.Matrix.OffsetY);

    public bool TryTransform(Point source, out Point destination)
    {
      destination = this.Transform(source);
      return true;
    }

    public IAffineMatrix Matrix { get; set; }
  }
}
