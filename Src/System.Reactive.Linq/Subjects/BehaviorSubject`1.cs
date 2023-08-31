// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.BehaviorSubject`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Subjects
{
  public sealed class BehaviorSubject<T> : 
    ISubject<T>,
    ISubject<T, T>,
    IObserver<T>,
    IObservable<T>,
    IDisposable
  {
    private readonly object _gate = new object();
    private ImmutableList<IObserver<T>> _observers;
    private bool _isStopped;
    private T _value;
    private Exception _exception;
    private bool _isDisposed;

    public BehaviorSubject(T value)
    {
      this._value = value;
      this._observers = new ImmutableList<IObserver<T>>();
    }

    public bool HasObservers
    {
      get
      {
        ImmutableList<IObserver<T>> observers = this._observers;
        return observers != null && observers.Data.Length > 0;
      }
    }

    public void OnCompleted()
    {
      IObserver<T>[] iobserverArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          iobserverArray = this._observers.Data;
          this._observers = new ImmutableList<IObserver<T>>();
          this._isStopped = true;
        }
      }
      if (iobserverArray == null)
        return;
      foreach (IObserver<T> iobserver in iobserverArray)
        iobserver.OnCompleted();
    }

    public void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      IObserver<T>[] iobserverArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          iobserverArray = this._observers.Data;
          this._observers = new ImmutableList<IObserver<T>>();
          this._isStopped = true;
          this._exception = error;
        }
      }
      if (iobserverArray == null)
        return;
      foreach (IObserver<T> iobserver in iobserverArray)
        iobserver.OnError(error);
    }

    public void OnNext(T value)
    {
      IObserver<T>[] iobserverArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._value = value;
          iobserverArray = this._observers.Data;
        }
      }
      if (iobserverArray == null)
        return;
      foreach (IObserver<T> iobserver in iobserverArray)
        iobserver.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      Exception exception = (Exception) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._observers = this._observers.Add(observer);
          observer.OnNext(this._value);
          return (IDisposable) new BehaviorSubject<T>.Subscription(this, observer);
        }
        exception = this._exception;
      }
      if (exception != null)
        observer.OnError(exception);
      else
        observer.OnCompleted();
      return Disposable.Empty;
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
        this._observers = (ImmutableList<IObserver<T>>) null;
        this._value = default (T);
        this._exception = (Exception) null;
      }
    }

    private class Subscription : IDisposable
    {
      private readonly BehaviorSubject<T> _subject;
      private IObserver<T> _observer;

      public Subscription(BehaviorSubject<T> subject, IObserver<T> observer)
      {
        this._subject = subject;
        this._observer = observer;
      }

      public void Dispose()
      {
        if (this._observer == null)
          return;
        lock (this._subject._gate)
        {
          if (this._subject._isDisposed || this._observer == null)
            return;
          this._subject._observers = this._subject._observers.Remove(this._observer);
          this._observer = (IObserver<T>) null;
        }
      }
    }
  }
}
