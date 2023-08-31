// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Animation.ExponentialEase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Media.Animation
{
  internal class ExponentialEase : IEasingFunction
  {
    private const double E = 2.7182818284590451;
    private int _exponent;
    private double _multiplicator;

    public ExponentialEase() => this.Exponent = -3;

    public int Exponent
    {
      get => this._exponent;
      set
      {
        this._exponent = value;
        this._multiplicator = 1.0 / (Math.Pow(Math.E, (double) value) - 1.0);
      }
    }

    public double Ease(double normalizedTime) => (Math.Pow(Math.E, (double) this.Exponent * normalizedTime) - 1.0) * this._multiplicator;
  }
}
