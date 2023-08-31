// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Thickness
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Media
{
  internal class Thickness
  {
    public Thickness(double uniformLength)
      : this(uniformLength, uniformLength, uniformLength, uniformLength)
    {
    }

    public Thickness(double left, double top, double right, double bottom)
    {
      this.Left = left;
      this.Top = top;
      this.Right = right;
      this.Bottom = bottom;
    }

    public double Left { get; set; }

    public double Top { get; set; }

    public double Right { get; set; }

    public double Bottom { get; set; }
  }
}
