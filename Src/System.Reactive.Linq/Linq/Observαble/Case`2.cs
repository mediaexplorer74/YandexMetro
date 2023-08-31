// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Case`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Case<TValue, TResult> : Producer<TResult>, IEvaluatableObservable<TResult>
  {
    private readonly Func<TValue> _selector;
    private readonly IDictionary<TValue, IObservable<TResult>> _sources;
    private readonly IObservable<TResult> _defaultSource;

    public Case(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IObservable<TResult> defaultSource)
    {
      this._selector = selector;
      this._sources = sources;
      this._defaultSource = defaultSource;
    }

    public IObservable<TResult> Eval()
    {
      IObservable<TResult> iobservable = (IObservable<TResult>) null;
      return this._sources.TryGetValue(this._selector(), out iobservable) ? iobservable : this._defaultSource;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Case<TValue, TResult>._ obj = new Case<TValue, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>, IObserver<TResult>
    {
      private readonly Case<TValue, TResult> _parent;

      public _(Case<TValue, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TResult> source;
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
        return source.SubscribeSafe<TResult>((IObserver<TResult>) this);
      }

      public void OnNext(TResult value) => this._observer.OnNext(value);

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
