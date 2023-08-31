// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Repeat`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Repeat<TResult> : Producer<TResult>
  {
    private readonly TResult _value;
    private readonly int? _repeatCount;
    private readonly IScheduler _scheduler;

    public Repeat(TResult value, int? repeatCount, IScheduler scheduler)
    {
      this._value = value;
      this._repeatCount = repeatCount;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Repeat<TResult>._ obj = new Repeat<TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Repeat<TResult> _parent;

      public _(Repeat<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        ISchedulerLongRunning scheduler = this._parent._scheduler.AsLongRunning();
        return scheduler != null ? this.Run(scheduler) : this.Run(this._parent._scheduler);
      }

      private IDisposable Run(IScheduler scheduler) => !this._parent._repeatCount.HasValue ? scheduler.Schedule(new Action<Action>(this.LoopRecInf)) : scheduler.Schedule<int>(this._parent._repeatCount.Value, new Action<int, Action<int>>(this.LoopRec));

      private void LoopRecInf(Action recurse)
      {
        this._observer.OnNext(this._parent._value);
        recurse();
      }

      private void LoopRec(int n, Action<int> recurse)
      {
        if (n > 0)
        {
          this._observer.OnNext(this._parent._value);
          --n;
        }
        if (n == 0)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
        else
          recurse(n);
      }

      private IDisposable Run(ISchedulerLongRunning scheduler) => !this._parent._repeatCount.HasValue ? scheduler.ScheduleLongRunning(new Action<ICancelable>(this.LoopInf)) : scheduler.ScheduleLongRunning<int>(this._parent._repeatCount.Value, new Action<int, ICancelable>(this.Loop));

      private void LoopInf(ICancelable cancel)
      {
        TResult result = this._parent._value;
        while (!cancel.IsDisposed)
          this._observer.OnNext(result);
        this.Dispose();
      }

      private void Loop(int n, ICancelable cancel)
      {
        TResult result = this._parent._value;
        for (; n > 0 && !cancel.IsDisposed; --n)
          this._observer.OnNext(result);
        if (!cancel.IsDisposed)
          this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
