// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.ScheduledDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Disposables
{
  public sealed class ScheduledDisposable : ICancelable, IDisposable
  {
    private readonly IScheduler _scheduler;
    private volatile IDisposable _disposable;

    public ScheduledDisposable(IScheduler scheduler, IDisposable disposable)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (disposable == null)
        throw new ArgumentNullException(nameof (disposable));
      this._scheduler = scheduler;
      this._disposable = disposable;
    }

    public IScheduler Scheduler => this._scheduler;

    public IDisposable Disposable
    {
      get
      {
        IDisposable disposable = this._disposable;
        return disposable == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : disposable;
      }
    }

    public bool IsDisposed => this._disposable == BooleanDisposable.True;

    public void Dispose() => this.Scheduler.Schedule(new Action(this.DisposeInner));

    private void DisposeInner()
    {
      IDisposable disposable = Interlocked.Exchange<IDisposable>(ref this._disposable, (IDisposable) BooleanDisposable.True);
      if (disposable == BooleanDisposable.True)
        return;
      disposable.Dispose();
    }
  }
}
