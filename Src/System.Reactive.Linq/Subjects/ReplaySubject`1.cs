// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.ReplaySubject`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Subjects
{
  public sealed class ReplaySubject<T> : 
    ISubject<T>,
    ISubject<T, T>,
    IObserver<T>,
    IObservable<T>,
    IDisposable
  {
    private const int InfiniteBufferSize = 2147483647;
    private readonly int _bufferSize;
    private readonly TimeSpan _window;
    private readonly IScheduler _scheduler;
    private readonly IStopwatch _stopwatch;
    private readonly Queue<TimeInterval<T>> _queue;
    private bool _isStopped;
    private Exception _error;
    private ImmutableList<ScheduledObserver<T>> _observers;
    private bool _isDisposed;
    private readonly object _gate = new object();

    public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
    {
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (window < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (window));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      this._bufferSize = bufferSize;
      this._window = window;
      this._scheduler = scheduler;
      this._stopwatch = this._scheduler.StartStopwatch();
      this._queue = new Queue<TimeInterval<T>>();
      this._isStopped = false;
      this._error = (Exception) null;
      this._observers = new ImmutableList<ScheduledObserver<T>>();
    }

    public ReplaySubject(int bufferSize, TimeSpan window)
      : this(bufferSize, window, SchedulerDefaults.Iteration)
    {
    }

    public ReplaySubject()
      : this(int.MaxValue, TimeSpan.MaxValue, SchedulerDefaults.Iteration)
    {
    }

    public ReplaySubject(IScheduler scheduler)
      : this(int.MaxValue, TimeSpan.MaxValue, scheduler)
    {
    }

    public ReplaySubject(int bufferSize, IScheduler scheduler)
      : this(bufferSize, TimeSpan.MaxValue, scheduler)
    {
    }

    public ReplaySubject(int bufferSize)
      : this(bufferSize, TimeSpan.MaxValue, SchedulerDefaults.Iteration)
    {
    }

    public ReplaySubject(TimeSpan window, IScheduler scheduler)
      : this(int.MaxValue, window, scheduler)
    {
    }

    public ReplaySubject(TimeSpan window)
      : this(int.MaxValue, window, SchedulerDefaults.Iteration)
    {
    }

    public bool HasObservers
    {
      get
      {
        ImmutableList<ScheduledObserver<T>> observers = this._observers;
        return observers != null && observers.Data.Length > 0;
      }
    }

    private void Trim(TimeSpan now)
    {
      while (this._queue.Count > this._bufferSize)
        this._queue.Dequeue();
      while (this._queue.Count > 0 && now.Subtract(this._queue.Peek().Interval).CompareTo(this._window) > 0)
        this._queue.Dequeue();
    }

    public void OnNext(T value)
    {
      ScheduledObserver<T>[] scheduledObserverArray = (ScheduledObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          TimeSpan elapsed = this._stopwatch.Elapsed;
          this._queue.Enqueue(new TimeInterval<T>(value, elapsed));
          this.Trim(elapsed);
          scheduledObserverArray = this._observers.Data;
          foreach (ObserverBase<T> observerBase in scheduledObserverArray)
            observerBase.OnNext(value);
        }
      }
      if (scheduledObserverArray == null)
        return;
      foreach (ScheduledObserver<T> scheduledObserver in scheduledObserverArray)
        scheduledObserver.EnsureActive();
    }

    public void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      ScheduledObserver<T>[] scheduledObserverArray = (ScheduledObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          TimeSpan elapsed = this._stopwatch.Elapsed;
          this._isStopped = true;
          this._error = error;
          this.Trim(elapsed);
          scheduledObserverArray = this._observers.Data;
          foreach (ObserverBase<T> observerBase in scheduledObserverArray)
            observerBase.OnError(error);
          this._observers = new ImmutableList<ScheduledObserver<T>>();
        }
      }
      if (scheduledObserverArray == null)
        return;
      foreach (ScheduledObserver<T> scheduledObserver in scheduledObserverArray)
        scheduledObserver.EnsureActive();
    }

    public void OnCompleted()
    {
      ScheduledObserver<T>[] scheduledObserverArray = (ScheduledObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          TimeSpan elapsed = this._stopwatch.Elapsed;
          this._isStopped = true;
          this.Trim(elapsed);
          scheduledObserverArray = this._observers.Data;
          foreach (ObserverBase<T> observerBase in scheduledObserverArray)
            observerBase.OnCompleted();
          this._observers = new ImmutableList<ScheduledObserver<T>>();
        }
      }
      if (scheduledObserverArray == null)
        return;
      foreach (ScheduledObserver<T> scheduledObserver in scheduledObserverArray)
        scheduledObserver.EnsureActive();
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
      ScheduledObserver<T> observer1 = observer != null ? new ScheduledObserver<T>(this._scheduler, observer) : throw new ArgumentNullException(nameof (observer));
      int n = 0;
      ReplaySubject<T>.RemovableDisposable removableDisposable = new ReplaySubject<T>.RemovableDisposable(this, observer1);
      lock (this._gate)
      {
        this.CheckDisposed();
        this.Trim(this._stopwatch.Elapsed);
        this._observers = this._observers.Add(observer1);
        n = this._queue.Count;
        foreach (TimeInterval<T> timeInterval in this._queue)
          observer1.OnNext(timeInterval.Value);
        if (this._error != null)
        {
          ++n;
          observer1.OnError(this._error);
        }
        else if (this._isStopped)
        {
          ++n;
          observer1.OnCompleted();
        }
      }
      observer1.EnsureActive(n);
      return (IDisposable) removableDisposable;
    }

    private void Unsubscribe(ScheduledObserver<T> observer)
    {
      lock (this._gate)
      {
        if (this._isDisposed)
          return;
        this._observers = this._observers.Remove(observer);
      }
    }

    private void CheckDisposed()
    {
      if (this._isDisposed)
        throw new ObjectDisposedException(string.Empty);
    }

    public void Dispose()
    {
      lock (this._gate)
      {
        this._isDisposed = true;
        this._observers = (ImmutableList<ScheduledObserver<T>>) null;
      }
    }

    private sealed class RemovableDisposable : IDisposable
    {
      private readonly ReplaySubject<T> _subject;
      private readonly ScheduledObserver<T> _observer;

      public RemovableDisposable(ReplaySubject<T> subject, ScheduledObserver<T> observer)
      {
        this._subject = subject;
        this._observer = observer;
      }

      public void Dispose()
      {
        this._observer.Dispose();
        this._subject.Unsubscribe(this._observer);
      }
    }
  }
}
