﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Select`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal abstract class Select<TResult> : Producer<TResult>
  {
    public abstract IObservable<TResult2> Ω<TResult2>(Func<TResult, TResult2> selector);
  }
}
