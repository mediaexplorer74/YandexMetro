// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.CombineLatest`4
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class CombineLatest<T1, T2, T3, TResult> : Producer<TResult>
  {
    private readonly IObservable<T1> _source1;
    private readonly IObservable<T2> _source2;
    private readonly IObservable<T3> _source3;
    private readonly Func<T1, T2, T3, TResult> _resultSelector;

    public CombineLatest(
      IObservable<T1> source1,
      IObservable<T2> source2,
      IObservable<T3> source3,
      Func<T1, T2, T3, TResult> resultSelector)
    {
      this._source1 = source1;
      this._source2 = source2;
      this._source3 = source3;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CombineLatest<T1, T2, T3, TResult>._ obj = new CombineLatest<T1, T2, T3, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : CombineLatestSink<TResult>
    {
      private readonly CombineLatest<T1, T2, T3, TResult> _parent;
      private CombineLatestObserver<T1> _observer1;
      private CombineLatestObserver<T2> _observer2;
      private CombineLatestObserver<T3> _observer3;

      public _(
        CombineLatest<T1, T2, T3, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(3, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SingleAssignmentDisposable[] assignmentDisposableArray = new SingleAssignmentDisposable[3];
        for (int index = 0; index < 3; ++index)
          assignmentDisposableArray[index] = new SingleAssignmentDisposable();
        this._observer1 = new CombineLatestObserver<T1>(this._gate, (ICombineLatest) this, 0, (IDisposable) assignmentDisposableArray[0]);
        this._observer2 = new CombineLatestObserver<T2>(this._gate, (ICombineLatest) this, 1, (IDisposable) assignmentDisposableArray[1]);
        this._observer3 = new CombineLatestObserver<T3>(this._gate, (ICombineLatest) this, 2, (IDisposable) assignmentDisposableArray[2]);
        assignmentDisposableArray[0].Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        assignmentDisposableArray[1].Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        assignmentDisposableArray[2].Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        return (IDisposable) new CompositeDisposable((IDisposable[]) assignmentDisposableArray);
      }

      protected override TResult GetResult() => this._parent._resultSelector(this._observer1.Value, this._observer2.Value, this._observer3.Value);
    }
  }
}
