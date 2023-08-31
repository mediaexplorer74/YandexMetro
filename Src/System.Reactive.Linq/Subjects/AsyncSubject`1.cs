// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.AsyncSubject`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Subjects
{
  public sealed class AsyncSubject<T> : 
    ISubject<T>,
    ISubject<T, T>,
    IObserver<T>,
    IObservable<T>,
    IDisposable
  {
    private readonly object _gate = new object();
    private ImmutableList<IObserver<T>> _observers;
    private bool _isDisposed;
    private bool _isStopped;
    private T _value;
    private bool _hasValue;
    private Exception _exception;

    public AsyncSubject() => this._observers = new ImmutableList<IObserver<T>>();

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
      T obj = default (T);
      bool flag = false;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          iobserverArray = this._observers.Data;
          this._observers = new ImmutableList<IObserver<T>>();
          this._isStopped = true;
          obj = this._value;
          flag = this._hasValue;
        }
      }
      if (iobserverArray == null)
        return;
      if (flag)
      {
        foreach (IObserver<T> iobserver in iobserverArray)
        {
          iobserver.OnNext(obj);
          iobserver.OnCompleted();
        }
      }
      else
      {
        foreach (IObserver<T> iobserver in iobserverArray)
          iobserver.OnCompleted();
      }
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
      lock (this._gate)
      {
        this.CheckDisposed();
        if (this._isStopped)
          return;
        this._value = value;
        this._hasValue = true;
      }
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      Exception exception = (Exception) null;
      T obj = default (T);
      bool flag = false;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._observers = this._observers.Add(observer);
          return (IDisposable) new AsyncSubject<T>.Subscription(this, observer);
        }
        exception = this._exception;
        flag = this._hasValue;
        obj = this._value;
      }
      if (exception != null)
        observer.OnError(exception);
      else if (flag)
      {
        observer.OnNext(obj);
        observer.OnCompleted();
      }
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
        this._exception = (Exception) null;
        this._value = default (T);
      }
    }

    private void OnCompleted(Action continuation, bool originalContext) => this.Subscribe((IObserver<T>) new AsyncSubject<T>.AwaitObserver(continuation, originalContext));

    public bool IsCompleted => this._isStopped;

    public T GetResult()
    {
      if (!this._isStopped)
      {
        ManualResetEvent e = new ManualResetEvent(false);
        this.OnCompleted((Action) (() => e.Set()), false);
        e.WaitOne();
      }
      this._exception.ThrowIfNotNull();
      if (!this._hasValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return this._value;
    }

    private class Subscription : IDisposable
    {
      private readonly AsyncSubject<T> _subject;
      private IObserver<T> _observer;

      public Subscription(AsyncSubject<T> subject, IObserver<T> observer)
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

    private class AwaitObserver : IObserver<T>
    {
      private readonly Action _callback;

      public AwaitObserver(Action callback, bool originalContext) => this._callback = callback;

      public void OnCompleted() => this.InvokeOnOriginalContext();

      public void OnError(Exception error) => this.InvokeOnOriginalContext();

      public void OnNext(T value)
      {
      }

      private void InvokeOnOriginalContext() => this._callback();
    }
  }
}
