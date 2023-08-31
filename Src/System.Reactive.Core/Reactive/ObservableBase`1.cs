// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObservableBase`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive
{
  public abstract class ObservableBase<T> : IObservable<T>
  {
    public IDisposable Subscribe(IObserver<T> observer)
    {
      AutoDetachObserver<T> autoDetachObserver = observer != null ? new AutoDetachObserver<T>(observer) : throw new ArgumentNullException(nameof (observer));
      if (CurrentThreadScheduler.IsScheduleRequired)
      {
        CurrentThreadScheduler.Instance.Schedule<AutoDetachObserver<T>>(autoDetachObserver, new Func<IScheduler, AutoDetachObserver<T>, IDisposable>(this.ScheduledSubscribe));
      }
      else
      {
        try
        {
          autoDetachObserver.Disposable = this.SubscribeCore((IObserver<T>) autoDetachObserver);
        }
        catch (Exception ex)
        {
          if (!autoDetachObserver.Fail(ex))
            throw;
        }
      }
      return (IDisposable) autoDetachObserver;
    }

    private IDisposable ScheduledSubscribe(IScheduler _, AutoDetachObserver<T> autoDetachObserver)
    {
      try
      {
        autoDetachObserver.Disposable = this.SubscribeCore((IObserver<T>) autoDetachObserver);
      }
      catch (Exception ex)
      {
        if (!autoDetachObserver.Fail(ex))
          throw;
      }
      return Disposable.Empty;
    }

    protected abstract IDisposable SubscribeCore(IObserver<T> observer);
  }
}
