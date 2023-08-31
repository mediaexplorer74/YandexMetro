// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.EventProducer`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal abstract class EventProducer<TDelegate, TArgs> : Producer<TArgs>
  {
    private readonly IScheduler _scheduler;
    private readonly object _gate;
    private EventProducer<TDelegate, TArgs>.Session _session;

    public EventProducer(IScheduler scheduler)
    {
      this._scheduler = scheduler;
      this._gate = new object();
    }

    protected abstract TDelegate GetHandler(Action<TArgs> onNext);

    protected abstract IDisposable AddHandler(TDelegate handler);

    protected override IDisposable Run(
      IObserver<TArgs> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      lock (this._gate)
      {
        if (this._session == null)
          this._session = new EventProducer<TDelegate, TArgs>.Session(this);
        return this._session.Connect(observer);
      }
    }

    private class Session
    {
      private readonly EventProducer<TDelegate, TArgs> _parent;
      private readonly Subject<TArgs> _subject;
      private SingleAssignmentDisposable _removeHandler;
      private int _count;

      public Session(EventProducer<TDelegate, TArgs> parent)
      {
        this._parent = parent;
        this._subject = new Subject<TArgs>();
      }

      public IDisposable Connect(IObserver<TArgs> observer)
      {
        IDisposable connection = this._subject.Subscribe(observer);
        if (++this._count == 1)
        {
          try
          {
            this.Initialize();
          }
          catch (Exception ex)
          {
            --this._count;
            connection.Dispose();
            observer.OnError(ex);
            return Disposable.Empty;
          }
        }
        return Disposable.Create((Action) (() =>
        {
          connection.Dispose();
          lock (this._parent._gate)
          {
            if (--this._count != 0)
              return;
            this._parent._scheduler.Schedule(new Action(this._removeHandler.Dispose));
            this._parent._session = (EventProducer<TDelegate, TArgs>.Session) null;
          }
        }));
      }

      private void Initialize()
      {
        this._removeHandler = new SingleAssignmentDisposable();
        this._parent._scheduler.Schedule<TDelegate>(this._parent.GetHandler(new Action<TArgs>(this._subject.OnNext)), new Func<IScheduler, TDelegate, IDisposable>(this.AddHandler));
      }

      private IDisposable AddHandler(IScheduler self, TDelegate onNext)
      {
        IDisposable disposable;
        try
        {
          disposable = this._parent.AddHandler(onNext);
        }
        catch (Exception ex)
        {
          this._subject.OnError(ex);
          return Disposable.Empty;
        }
        this._removeHandler.Disposable = disposable;
        return Disposable.Empty;
      }
    }
  }
}
