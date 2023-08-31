﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DefaultConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  internal class DefaultConcurrencyAbstractionLayer : IConcurrencyAbstractionLayer
  {
    public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime) => (IDisposable) new DefaultConcurrencyAbstractionLayer.Timer(action, state, DefaultConcurrencyAbstractionLayer.Normalize(dueTime));

    public IDisposable StartPeriodicTimer(Action action, TimeSpan period) => !(period <= TimeSpan.Zero) ? (IDisposable) new DefaultConcurrencyAbstractionLayer.PeriodicTimer(action, period) : throw new ArgumentOutOfRangeException(nameof (period));

    public IDisposable QueueUserWorkItem(Action<object> action, object state)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (_ => action(_)), state);
      return Disposable.Empty;
    }

    public void Sleep(TimeSpan timeout) => Thread.Sleep(DefaultConcurrencyAbstractionLayer.Normalize(timeout));

    public IStopwatch StartStopwatch() => (IStopwatch) new DefaultStopwatch();

    public bool SupportsLongRunning => true;

    public void StartThread(Action<object> action, object state) => new Thread((ThreadStart) (() => action(state)))
    {
      IsBackground = true
    }.Start();

    private static TimeSpan Normalize(TimeSpan dueTime) => dueTime < TimeSpan.Zero ? TimeSpan.Zero : dueTime;

    private class Timer : IDisposable
    {
      private static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
      private Action<object> _action;
      private System.Threading.Timer _timer;
      private bool _hasAdded;
      private bool _hasRemoved;

      public Timer(Action<object> action, object state, TimeSpan dueTime)
      {
        this._action = action;
        this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), state, dueTime, TimeSpan.FromMilliseconds(-1.0));
        lock (DefaultConcurrencyAbstractionLayer.Timer.s_timers)
        {
          if (this._hasRemoved)
            return;
          DefaultConcurrencyAbstractionLayer.Timer.s_timers.Add(this._timer, (object) null);
          this._hasAdded = true;
        }
      }

      private void Tick(object state)
      {
        try
        {
          this._action(state);
        }
        finally
        {
          this.Dispose();
        }
      }

      public void Dispose()
      {
        this._action = Stubs<object>.Ignore;
        System.Threading.Timer key = (System.Threading.Timer) null;
        lock (DefaultConcurrencyAbstractionLayer.Timer.s_timers)
        {
          if (!this._hasRemoved)
          {
            key = this._timer;
            this._timer = (System.Threading.Timer) null;
            if (this._hasAdded && key != null)
              DefaultConcurrencyAbstractionLayer.Timer.s_timers.Remove(key);
            this._hasRemoved = true;
          }
        }
        key?.Dispose();
      }
    }

    private class PeriodicTimer : IDisposable
    {
      private static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
      private Action _action;
      private System.Threading.Timer _timer;

      public PeriodicTimer(Action action, TimeSpan period)
      {
        this._action = action;
        this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), (object) null, period, period);
        lock (DefaultConcurrencyAbstractionLayer.PeriodicTimer.s_timers)
          DefaultConcurrencyAbstractionLayer.PeriodicTimer.s_timers.Add(this._timer, (object) null);
      }

      private void Tick(object state) => this._action();

      public void Dispose()
      {
        System.Threading.Timer key = (System.Threading.Timer) null;
        lock (DefaultConcurrencyAbstractionLayer.PeriodicTimer.s_timers)
        {
          key = this._timer;
          this._timer = (System.Threading.Timer) null;
          if (key != null)
            DefaultConcurrencyAbstractionLayer.PeriodicTimer.s_timers.Remove(key);
        }
        if (key == null)
          return;
        key.Dispose();
        this._action = Stubs.Nop;
      }
    }
  }
}
