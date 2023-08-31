// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.DefaultIfEmpty`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class DefaultIfEmpty<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TSource _defaultValue;

    public DefaultIfEmpty(IObservable<TSource> source, TSource defaultValue)
    {
      this._source = source;
      this._defaultValue = defaultValue;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DefaultIfEmpty<TSource>._ observer1 = new DefaultIfEmpty<TSource>._(this, observer, cancel);
      setSink((IDisposable) observer1);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observer1);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly DefaultIfEmpty<TSource> _parent;
      private bool _found;

      public _(DefaultIfEmpty<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._found = false;
      }

      public void OnNext(TSource value)
      {
        this._found = true;
        this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._found)
          this._observer.OnNext(this._parent._defaultValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
