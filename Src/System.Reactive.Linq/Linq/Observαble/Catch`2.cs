// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Catch`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Catch<TSource, TException> : Producer<TSource> where TException : Exception
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TException, IObservable<TSource>> _handler;

    public Catch(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler)
    {
      this._source = source;
      this._handler = handler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Catch<TSource, TException>._ obj = new Catch<TSource, TException>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Catch<TSource, TException> _parent;
      private SerialDisposable _subscription;

      public _(Catch<TSource, TException> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._subscription = new SerialDisposable();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._subscription;
      }

      public void OnNext(TSource value) => this._observer.OnNext(value);

      public void OnError(Exception error)
      {
        if (error is TException exception)
        {
          IObservable<TSource> source;
          try
          {
            source = this._parent._handler(exception);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
          this._subscription.Disposable = (IDisposable) assignmentDisposable;
          assignmentDisposable.Disposable = source.SubscribeSafe<TSource>((IObserver<TSource>) new Catch<TSource, TException>._.ε(this));
        }
        else
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }

      private class ε : IObserver<TSource>
      {
        private readonly Catch<TSource, TException>._ _parent;

        public ε(Catch<TSource, TException>._ parent) => this._parent = parent;

        public void OnNext(TSource value) => this._parent._observer.OnNext(value);

        public void OnError(Exception error)
        {
          this._parent._observer.OnError(error);
          this._parent.Dispose();
        }

        public void OnCompleted()
        {
          this._parent._observer.OnCompleted();
          this._parent.Dispose();
        }
      }
    }
  }
}
