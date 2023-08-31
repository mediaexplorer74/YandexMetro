// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.Subject
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace System.Reactive.Subjects
{
  public static class Subject
  {
    public static ISubject<TSource, TResult> Create<TSource, TResult>(
      IObserver<TSource> observer,
      IObservable<TResult> observable)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return observable != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(observer, observable) : throw new ArgumentNullException(nameof (observable));
    }

    public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(
      ISubject<TSource, TResult> subject)
    {
      return subject != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), (IObservable<TResult>) subject) : throw new ArgumentNullException(nameof (subject));
    }

    public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(
      ISubject<TSource, TResult> subject,
      IScheduler scheduler)
    {
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      return scheduler != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), ((IObservable<TResult>) subject).ObserveOn<TResult>(scheduler)) : throw new ArgumentNullException(nameof (scheduler));
    }

    private class AnonymousSubject<T, U> : ISubject<T, U>, IObserver<T>, IObservable<U>
    {
      private readonly IObserver<T> _observer;
      private readonly IObservable<U> _observable;

      public AnonymousSubject(IObserver<T> observer, IObservable<U> observable)
      {
        this._observer = observer;
        this._observable = observable;
      }

      public void OnCompleted() => this._observer.OnCompleted();

      public void OnError(Exception error)
      {
        if (error == null)
          throw new ArgumentNullException(nameof (error));
        this._observer.OnError(error);
      }

      public void OnNext(T value) => this._observer.OnNext(value);

      public IDisposable Subscribe(IObserver<U> observer) => observer != null ? this._observable.Subscribe(observer) : throw new ArgumentNullException(nameof (observer));
    }
  }
}
