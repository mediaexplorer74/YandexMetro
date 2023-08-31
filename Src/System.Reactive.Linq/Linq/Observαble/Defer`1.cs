// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Defer`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Defer<TValue> : Producer<TValue>, IEvaluatableObservable<TValue>
  {
    private readonly Func<IObservable<TValue>> _observableFactory;

    public Defer(Func<IObservable<TValue>> observableFactory) => this._observableFactory = observableFactory;

    protected override IDisposable Run(
      IObserver<TValue> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Defer<TValue>._ obj = new Defer<TValue>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    public IObservable<TValue> Eval() => this._observableFactory();

    private class _ : Sink<TValue>, IObserver<TValue>
    {
      private readonly Defer<TValue> _parent;

      public _(Defer<TValue> parent, IObserver<TValue> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TValue> source;
        try
        {
          source = this._parent.Eval();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        return source.SubscribeSafe<TValue>((IObserver<TValue>) this);
      }

      public void OnNext(TValue value) => this._observer.OnNext(value);

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
