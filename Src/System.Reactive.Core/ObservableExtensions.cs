// Decompiled with JetBrains decompiler
// Type: System.ObservableExtensions
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;

namespace System
{
  public static class ObservableExtensions
  {
    public static IDisposable Subscribe<T>(this IObservable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, onError, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, onError, onCompleted));
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static IDisposable SubscribeSafe<T>(this IObservable<T> source, IObserver<T> observer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      switch (source)
      {
        case ObservableBase<T> _:
          return source.Subscribe(observer);
        case IProducer<T> producer:
          return producer.SubscribeRaw(observer, false);
        default:
          IDisposable disposable = Disposable.Empty;
          try
          {
            disposable = source.Subscribe(observer);
          }
          catch (Exception ex)
          {
            observer.OnError(ex);
          }
          return disposable;
      }
    }
  }
}
