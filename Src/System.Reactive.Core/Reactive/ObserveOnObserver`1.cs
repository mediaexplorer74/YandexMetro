// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObserveOnObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive
{
  internal class ObserveOnObserver<T> : ScheduledObserver<T>
  {
    private IDisposable _cancel;

    public ObserveOnObserver(IScheduler scheduler, IObserver<T> observer, IDisposable cancel)
      : base(scheduler, observer)
    {
      this._cancel = cancel;
    }

    protected override void OnNextCore(T value)
    {
      base.OnNextCore(value);
      this.EnsureActive();
    }

    protected override void OnErrorCore(Exception exception)
    {
      base.OnErrorCore(exception);
      this.EnsureActive();
    }

    protected override void OnCompletedCore()
    {
      base.OnCompletedCore();
      this.EnsureActive();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      Interlocked.Exchange<IDisposable>(ref this._cancel, (IDisposable) null)?.Dispose();
    }
  }
}
