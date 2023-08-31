// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Zip`5
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Zip<T1, T2, T3, T4, TResult> : Producer<TResult>
  {
    private readonly IObservable<T1> _source1;
    private readonly IObservable<T2> _source2;
    private readonly IObservable<T3> _source3;
    private readonly IObservable<T4> _source4;
    private readonly Func<T1, T2, T3, T4, TResult> _resultSelector;

    public Zip(
      IObservable<T1> source1,
      IObservable<T2> source2,
      IObservable<T3> source3,
      IObservable<T4> source4,
      Func<T1, T2, T3, T4, TResult> resultSelector)
    {
      this._source1 = source1;
      this._source2 = source2;
      this._source3 = source3;
      this._source4 = source4;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Zip<T1, T2, T3, T4, TResult>._ obj = new Zip<T1, T2, T3, T4, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : ZipSink<TResult>
    {
      private readonly Zip<T1, T2, T3, T4, TResult> _parent;
      private ZipObserver<T1> _observer1;
      private ZipObserver<T2> _observer2;
      private ZipObserver<T3> _observer3;
      private ZipObserver<T4> _observer4;

      public _(
        Zip<T1, T2, T3, T4, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(4, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SingleAssignmentDisposable[] assignmentDisposableArray = new SingleAssignmentDisposable[4];
        for (int index = 0; index < 4; ++index)
          assignmentDisposableArray[index] = new SingleAssignmentDisposable();
        this._observer1 = new ZipObserver<T1>(this._gate, (IZip) this, 0, (IDisposable) assignmentDisposableArray[0]);
        this._observer2 = new ZipObserver<T2>(this._gate, (IZip) this, 1, (IDisposable) assignmentDisposableArray[1]);
        this._observer3 = new ZipObserver<T3>(this._gate, (IZip) this, 2, (IDisposable) assignmentDisposableArray[2]);
        this._observer4 = new ZipObserver<T4>(this._gate, (IZip) this, 3, (IDisposable) assignmentDisposableArray[3]);
        this.Queues[0] = (ICollection) this._observer1.Values;
        this.Queues[1] = (ICollection) this._observer2.Values;
        this.Queues[2] = (ICollection) this._observer3.Values;
        this.Queues[3] = (ICollection) this._observer4.Values;
        assignmentDisposableArray[0].Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        assignmentDisposableArray[1].Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        assignmentDisposableArray[2].Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        assignmentDisposableArray[3].Disposable = this._parent._source4.SubscribeSafe<T4>((IObserver<T4>) this._observer4);
        return (IDisposable) new CompositeDisposable((IDisposable[]) assignmentDisposableArray)
        {
          Disposable.Create((Action) (() =>
          {
            this._observer1.Values.Clear();
            this._observer2.Values.Clear();
            this._observer3.Values.Clear();
            this._observer4.Values.Clear();
          }))
        };
      }

      protected override TResult GetResult() => this._parent._resultSelector(this._observer1.Values.Dequeue(), this._observer2.Values.Dequeue(), this._observer3.Values.Dequeue(), this._observer4.Values.Dequeue());
    }
  }
}
