// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Pattern`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Joins
{
  public class Pattern<TSource1> : Pattern
  {
    internal Pattern(IObservable<TSource1> first) => this.First = first;

    internal IObservable<TSource1> First { get; private set; }

    public Plan<TResult> Then<TResult>(Func<TSource1, TResult> selector) => selector != null ? (Plan<TResult>) new Plan<TSource1, TResult>(this, selector) : throw new ArgumentNullException(nameof (selector));
  }
}
