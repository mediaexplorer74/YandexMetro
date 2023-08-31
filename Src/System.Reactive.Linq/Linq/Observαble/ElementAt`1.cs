// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ElementAt`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class ElementAt<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _index;
    private readonly bool _throwOnEmpty;

    public ElementAt(IObservable<TSource> source, int index, bool throwOnEmpty)
    {
      this._source = source;
      this._index = index;
      this._throwOnEmpty = throwOnEmpty;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ElementAt<TSource>._ observer1 = new ElementAt<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly ElementAt<TSource> _parent;
      private int _i;

      public _(ElementAt<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._i = this._parent._index;
      }

      public void OnNext(TSource value)
      {
        if (this._i == 0)
        {
          this._observer.OnNext(value);
          this._observer.OnCompleted();
          this.Dispose();
        }
        --this._i;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new ArgumentOutOfRangeException("index"));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
