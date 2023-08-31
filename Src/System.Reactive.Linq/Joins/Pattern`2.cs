// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Pattern`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Joins
{
  public class Pattern<TSource1, TSource2> : Pattern
  {
    internal Pattern(IObservable<TSource1> first, IObservable<TSource2> second)
    {
      this.First = first;
      this.Second = second;
    }

    internal IObservable<TSource1> First { get; private set; }

    internal IObservable<TSource2> Second { get; private set; }

    public Pattern<TSource1, TSource2, TSource3> And<TSource3>(IObservable<TSource3> other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      return new Pattern<TSource1, TSource2, TSource3>(this.First, this.Second, other);
    }

    public Plan<TResult> Then<TResult>(Func<TSource1, TSource2, TResult> selector) => selector != null ? (Plan<TResult>) new Plan<TSource1, TSource2, TResult>(this, selector) : throw new ArgumentNullException(nameof (selector));
  }
}
