// Decompiled with JetBrains decompiler
// Type: System.Reactive.Observer
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive
{
  public static class Observer
  {
    public static IObserver<T> ToObserver<T>(this Action<Notification<T>> handler)
    {
      if (handler == null)
        throw new ArgumentNullException(nameof (handler));
      return (IObserver<T>) new AnonymousObserver<T>((Action<T>) (x => handler(Notification.CreateOnNext<T>(x))), (Action<Exception>) (exception => handler(Notification.CreateOnError<T>(exception))), (Action) (() => handler(Notification.CreateOnCompleted<T>())));
    }

    public static Action<Notification<T>> ToNotifier<T>(this IObserver<T> observer) => observer != null ? (Action<Notification<T>>) (n => n.Accept(observer)) : throw new ArgumentNullException(nameof (observer));

    public static IObserver<T> Create<T>(Action<T> onNext) => onNext != null ? (IObserver<T>) new AnonymousObserver<T>(onNext) : throw new ArgumentNullException(nameof (onNext));

    public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
    {
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return onError != null ? (IObserver<T>) new AnonymousObserver<T>(onNext, onError) : throw new ArgumentNullException(nameof (onError));
    }

    public static IObserver<T> Create<T>(Action<T> onNext, Action onCompleted)
    {
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return onCompleted != null ? (IObserver<T>) new AnonymousObserver<T>(onNext, onCompleted) : throw new ArgumentNullException(nameof (onCompleted));
    }

    public static IObserver<T> Create<T>(
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return (IObserver<T>) new AnonymousObserver<T>(onNext, onError, onCompleted);
    }

    public static IObserver<T> AsObserver<T>(this IObserver<T> observer) => observer != null ? (IObserver<T>) new AnonymousObserver<T>(new Action<T>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted)) : throw new ArgumentNullException(nameof (observer));

    public static IObserver<T> Checked<T>(this IObserver<T> observer) => observer != null ? (IObserver<T>) new CheckedObserver<T>(observer) : throw new ArgumentNullException(nameof (observer));

    public static IObserver<T> Synchronize<T>(IObserver<T> observer) => observer != null ? (IObserver<T>) new SynchronizedObserver<T>(observer, new object()) : throw new ArgumentNullException(nameof (observer));

    public static IObserver<T> Synchronize<T>(IObserver<T> observer, bool preventReentrancy)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return preventReentrancy ? (IObserver<T>) new AsyncLockObserver<T>(observer, new AsyncLock()) : (IObserver<T>) new SynchronizedObserver<T>(observer, new object());
    }

    public static IObserver<T> Synchronize<T>(IObserver<T> observer, object gate)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return gate != null ? (IObserver<T>) new SynchronizedObserver<T>(observer, gate) : throw new ArgumentNullException(nameof (gate));
    }

    public static IObserver<T> Synchronize<T>(IObserver<T> observer, AsyncLock asyncLock)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return asyncLock != null ? (IObserver<T>) new AsyncLockObserver<T>(observer, asyncLock) : throw new ArgumentNullException(nameof (asyncLock));
    }

    public static IObserver<T> NotifyOn<T>(this IObserver<T> observer, IScheduler scheduler)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return scheduler != null ? (IObserver<T>) new ObserveOnObserver<T>(scheduler, observer, (IDisposable) null) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IObserver<T> NotifyOn<T>(
      this IObserver<T> observer,
      SynchronizationContext context)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return context != null ? (IObserver<T>) new ObserveOnObserver<T>((IScheduler) new SynchronizationContextScheduler(context), observer, (IDisposable) null) : throw new ArgumentNullException(nameof (context));
    }
  }
}
