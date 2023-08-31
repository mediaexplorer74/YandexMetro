// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Units.Meters
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Units
{
  internal struct Meters
  {
    public Meters(double value)
      : this()
    {
      this.Value = value;
    }

    public double Value { get; set; }

    public static implicit operator double(Meters meters) => meters.Value;

    public static explicit operator Meters(double value) => new Meters(value);
  }
}
