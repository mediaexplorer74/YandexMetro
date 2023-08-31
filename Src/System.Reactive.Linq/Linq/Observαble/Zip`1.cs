// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Zip`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Zip<TSource> : Producer<IList<TSource>>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;

    public Zip(IEnumerable<IObservable<TSource>> sources) => this._sources = sources;

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Zip<TSource>._ obj = new Zip<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<IList<TSource>>
    {
      private readonly Zip<TSource> _parent;
      private object _gate;
      private Queue<TSource>[] _queues;
      private bool[] _isDone;
      private IDisposable[] _subscriptions;

      public _(Zip<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TSource>[] array = this._parent._sources.ToArray<IObservable<TSource>>();
        int length = array.Length;
        this._queues = new Queue<TSource>[length];
        for (int index = 0; index < length; ++index)
          this._queues[index] = new Queue<TSource>();
        this._isDone = new bool[length];
        this._subscriptions = (IDisposable[]) new SingleAssignmentDisposable[length];
        this._gate = new object();
        for (int index1 = 0; index1 < length; ++index1)
        {
          int index2 = index1;
          SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
          this._subscriptions[index2] = (IDisposable) assignmentDisposable;
          Zip<TSource>._.O observer = new Zip<TSource>._.O(this, index2);
          assignmentDisposable.Disposable = array[index2].SubscribeSafe<TSource>((IObserver<TSource>) observer);
        }
        return (IDisposable) new CompositeDisposable(this._subscriptions)
        {
          Disposable.Create((Action) (() =>
          {
            foreach (Queue<TSource> queue in this._queues)
              queue.Clear();
          }))
        };
      }

      private void OnNext(int index, TSource value)
      {
        lock (this._gate)
        {
          this._queues[index].Enqueue(value);
          if (((IEnumerable<Queue<TSource>>) this._queues).All<Queue<TSource>>((Func<Queue<TSource>, bool>) (q => q.Count > 0)))
          {
            this._observer.OnNext((IList<TSource>) ((IEnumerable<Queue<TSource>>) this._queues).Select<Queue<TSource>, TSource>((Func<Queue<TSource>, TSource>) (q => q.Dequeue())).ToList<TSource>());
          }
          else
          {
            if (!((IEnumerable<bool>) this._isDone).Where<bool>((Func<bool, int, bool>) ((x, i) => i != index)).All<bool>(Stubs<bool>.I))
              return;
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
      }

      private void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      private void OnCompleted(int index)
      {
        lock (this._gate)
        {
          this._isDone[index] = true;
          if (((IEnumerable<bool>) this._isDone).All<bool>(Stubs<bool>.I))
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
          else
            this._subscriptions[index].Dispose();
        }
      }

      private class O : IObserver<TSource>
      {
        private readonly Zip<TSource>._ _parent;
        private readonly int _index;

        public O(Zip<TSource>._ parent, int index)
        {
          this._parent = parent;
          this._index = index;
        }

        public void OnNext(TSource value) => this._parent.OnNext(this._index, value);

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.OnCompleted(this._index);
      }
    }
  }
}
