// Decompiled with JetBrains decompiler
// Type: System.Reactive.ScheduledObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
  internal class ScheduledObserver<T> : ObserverBase<T>, IDisposable
  {
    private bool _isAcquired;
    private bool _hasFaulted;
    private readonly Queue<Action> _queue = new Queue<Action>();
    private readonly IObserver<T> _observer;
    private readonly IScheduler _scheduler;
    private readonly SerialDisposable _disposable = new SerialDisposable();

    public ScheduledObserver(IScheduler scheduler, IObserver<T> observer)
    {
      this._scheduler = scheduler;
      this._observer = observer;
    }

    public void EnsureActive(int n) => this.EnsureActive();

    public void EnsureActive()
    {
      bool flag = false;
      lock (this._queue)
      {
        if (!this._hasFaulted)
        {
          if (this._queue.Count > 0)
          {
            flag = !this._isAcquired;
            this._isAcquired = true;
          }
        }
      }
      if (!flag)
        return;
      this._disposable.Disposable = this._scheduler.Schedule<object>((object) null, new Action<object, Action<object>>(this.Run));
    }

    private void Run(object state, Action<object> recurse)
    {
      Action action = (Action) null;
      lock (this._queue)
      {
        if (this._queue.Count > 0)
        {
          action = this._queue.Dequeue();
        }
        else
        {
          this._isAcquired = false;
          return;
        }
      }
      try
      {
        action();
      }
      catch
      {
        lock (this._queue)
        {
          this._queue.Clear();
          this._hasFaulted = true;
        }
        throw;
      }
      recurse(state);
    }

    protected override void OnNextCore(T value)
    {
      lock (this._queue)
        this._queue.Enqueue((Action) (() => this._observer.OnNext(value)));
    }

    protected override void OnErrorCore(Exception exception)
    {
      lock (this._queue)
        this._queue.Enqueue((Action) (() => this._observer.OnError(exception)));
    }

    protected override void OnCompletedCore()
    {
      lock (this._queue)
        this._queue.Enqueue((Action) (() => this._observer.OnCompleted()));
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this._disposable.Dispose();
    }
  }
}
