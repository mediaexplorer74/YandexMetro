// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Multicast`3
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal class Multicast<TSource, TIntermediate, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<ISubject<TSource, TIntermediate>> _subjectSelector;
    private readonly Func<IObservable<TIntermediate>, IObservable<TResult>> _selector;

    public Multicast(
      IObservable<TSource> source,
      Func<ISubject<TSource, TIntermediate>> subjectSelector,
      Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
    {
      this._source = source;
      this._subjectSelector = subjectSelector;
      this._selector = selector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Multicast<TSource, TIntermediate, TResult>._ obj = new Multicast<TSource, TIntermediate, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>, IObserver<TResult>
    {
      private readonly Multicast<TSource, TIntermediate, TResult> _parent;

      public _(
        Multicast<TSource, TIntermediate, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IConnectableObservable<TIntermediate> connectableObservable;
        IObservable<TResult> source;
        try
        {
          connectableObservable = (IConnectableObservable<TIntermediate>) new ConnectableObservable<TSource, TIntermediate>(this._parent._source, this._parent._subjectSelector());
          source = this._parent._selector((IObservable<TIntermediate>) connectableObservable);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        return (IDisposable) new CompositeDisposable(new IDisposable[2]
        {
          source.SubscribeSafe<TResult>((IObserver<TResult>) this),
          connectableObservable.Connect()
        });
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
