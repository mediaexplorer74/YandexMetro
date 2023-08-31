// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ClassicEventProducer`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal abstract class ClassicEventProducer<TDelegate, TArgs> : EventProducer<TDelegate, TArgs>
  {
    private readonly Action<TDelegate> _addHandler;
    private readonly Action<TDelegate> _removeHandler;

    public ClassicEventProducer(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(scheduler)
    {
      this._addHandler = addHandler;
      this._removeHandler = removeHandler;
    }

    protected override IDisposable AddHandler(TDelegate handler)
    {
      this._addHandler(handler);
      return Disposable.Create((Action) (() => this._removeHandler(handler)));
    }
  }
}
