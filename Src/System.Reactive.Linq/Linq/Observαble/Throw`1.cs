// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Throw`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class Throw<TResult> : Producer<TResult>
  {
    private readonly Exception _exception;
    private readonly IScheduler _scheduler;

    public Throw(Exception exception, IScheduler scheduler)
    {
      this._exception = exception;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Throw<TResult>._ obj = new Throw<TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Throw<TResult> _parent;

      public _(Throw<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run() => this._parent._scheduler.Schedule(new Action(this.Invoke));

      private void Invoke()
      {
        this._observer.OnError(this._parent._exception);
        this.Dispose();
      }
    }
  }
}
