// Decompiled with JetBrains decompiler
// Type: System.Reactive.Timestamped`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Reactive
{
  public struct Timestamped<T> : IEquatable<Timestamped<T>>
  {
    private readonly DateTimeOffset _timestamp;
    private readonly T _value;

    public Timestamped(T value, DateTimeOffset timestamp)
    {
      this._timestamp = timestamp;
      this._value = value;
    }

    public T Value => this._value;

    public DateTimeOffset Timestamp => this._timestamp;

    public bool Equals(Timestamped<T> other) => other.Timestamp.Equals(this.Timestamp) && EqualityComparer<T>.Default.Equals(this.Value, other.Value);

    public static bool operator ==(Timestamped<T> first, Timestamped<T> second) => first.Equals(second);

    public static bool operator !=(Timestamped<T> first, Timestamped<T> second) => !first.Equals(second);

    public override bool Equals(object obj) => obj is Timestamped<T> other && this.Equals(other);

    public override int GetHashCode()
    {
      int num = (object) this.Value == null ? 1979 : this.Value.GetHashCode();
      return this._timestamp.GetHashCode() ^ num;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}@{1}", new object[2]
    {
      (object) this.Value,
      (object) this.Timestamp
    });
  }
}
