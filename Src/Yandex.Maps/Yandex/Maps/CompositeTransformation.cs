// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.CompositeTransformation
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  internal class CompositeTransformation : ICompositeTransformation
  {
    public double CenterX { get; set; }

    public double CenterY { get; set; }

    public double Rotation { get; set; }

    public double ScaleX { get; set; }

    public double ScaleY { get; set; }

    public double SkewX { get; set; }

    public double SkewY { get; set; }

    public double TranslateX { get; set; }

    public double TranslateY { get; set; }
  }
}
