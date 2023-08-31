// Decompiled with JetBrains decompiler
// Type: System.Reactive.TimeInterval`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Reactive
{
  public struct TimeInterval<T> : IEquatable<TimeInterval<T>>
  {
    private readonly TimeSpan _interval;
    private readonly T _value;

    public TimeInterval(T value, TimeSpan interval)
    {
      this._interval = interval;
      this._value = value;
    }

    public T Value => this._value;

    public TimeSpan Interval => this._interval;

    public bool Equals(TimeInterval<T> other) => other.Interval.Equals(this.Interval) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);

    public static bool operator ==(TimeInterval<T> first, TimeInterval<T> second) => first.Equals(second);

    public static bool operator !=(TimeInterval<T> first, TimeInterval<T> second) => !first.Equals(second);

    public override bool Equals(object obj) => obj is TimeInterval<T> other && this.Equals(other);

    public override int GetHashCode()
    {
      int num = (object) this.Value == null ? 1963 : this.Value.GetHashCode();
      return this.Interval.GetHashCode() ^ num;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}@{1}", new object[2]
    {
      (object) this.Value,
      (object) this.Interval
    });
  }
}
