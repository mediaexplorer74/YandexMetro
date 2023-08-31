// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.QueryLanguage
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq.Observαble;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;

namespace System.Reactive.Linq
{
  internal class QueryLanguage : IQueryLanguage
  {
    public virtual Pattern<TLeft, TRight> And<TLeft, TRight>(
      IObservable<TLeft> left,
      IObservable<TRight> right)
    {
      return new Pattern<TLeft, TRight>(left, right);
    }

    public virtual Plan<TResult> Then<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return new Pattern<TSource>(source).Then<TResult>(selector);
    }

    public virtual IObservable<TResult> When<TResult>(params Plan<TResult>[] plans) => this.When<TResult>((IEnumerable<Plan<TResult>>) plans);

    public virtual IObservable<TResult> When<TResult>(IEnumerable<Plan<TResult>> plans) => (IObservable<TResult>) new AnonymousObservable<TResult>((Func<IObserver<TResult>, IDisposable>) (observer =>
    {
      Dictionary<object, IJoinObserver> externalSubscriptions = new Dictionary<object, IJoinObserver>();
      object gate = new object();
      List<ActivePlan> activePlans = new List<ActivePlan>();
      IObserver<TResult> outObserver = Observer.Create<TResult>(new Action<TResult>(observer.OnNext), (Action<Exception>) (exception =>
      {
        foreach (IDisposable disposable in externalSubscriptions.Values)
          disposable.Dispose();
        observer.OnError(exception);
      }), new Action(observer.OnCompleted));
      try
      {
        foreach (Plan<TResult> plan in plans)
          activePlans.Add(plan.Activate(externalSubscriptions, outObserver, (Action<ActivePlan>) (activePlan =>
          {
            activePlans.Remove(activePlan);
            if (activePlans.Count != 0)
              return;
            outObserver.OnCompleted();
          })));
      }
      catch (Exception ex)
      {
        return this.Throw<TResult>(ex).Subscribe(observer);
      }
      CompositeDisposable compositeDisposable = new CompositeDisposable(externalSubscriptions.Values.Count);
      foreach (IJoinObserver joinObserver in externalSubscriptions.Values)
      {
        joinObserver.Subscribe(gate);
        compositeDisposable.Add((IDisposable) joinObserver);
      }
      return (IDisposable) compositeDisposable;
    }));

    public virtual IObservable<TSource> Amb<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Amb_<TSource>(first, second);
    }

    public virtual IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources) => QueryLanguage.Amb_<TSource>((IEnumerable<IObservable<TSource>>) sources);

    public virtual IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources) => QueryLanguage.Amb_<TSource>(sources);

    private static IObservable<TSource> Amb_<TSource>(IEnumerable<IObservable<TSource>> sources) => sources.Aggregate<IObservable<TSource>, IObservable<TSource>>(Observable.Never<TSource>(), (Func<IObservable<TSource>, IObservable<TSource>, IObservable<TSource>>) ((previous, current) => previous.Amb<TSource>(current)));

    private static IObservable<TSource> Amb_<TSource>(
      IObservable<TSource> leftSource,
      IObservable<TSource> rightSource)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Amb<TSource>(leftSource, rightSource);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(
      IObservable<TSource> source,
      Func<IObservable<TBufferClosing>> bufferClosingSelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Buffer<TSource, TBufferClosing>(source, bufferClosingSelector);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(
      IObservable<TSource> source,
      IObservable<TBufferOpening> bufferOpenings,
      Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
    {
      return source.Window<TSource, TBufferOpening, TBufferClosing>(bufferOpenings, bufferClosingSelector).SelectMany<IObservable<TSource>, IList<TSource>>(new Func<IObservable<TSource>, IObservable<IList<TSource>>>(this.ToList<TSource>));
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(
      IObservable<TSource> source,
      IObservable<TBufferBoundary> bufferBoundaries)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Buffer<TSource, TBufferBoundary>(source, bufferBoundaries);
    }

    public virtual IObservable<TSource> Catch<TSource, TException>(
      IObservable<TSource> source,
      Func<TException, IObservable<TSource>> handler)
      where TException : Exception
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Catch<TSource, TException>(source, handler);
    }

    public virtual IObservable<TSource> Catch<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Catch_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources) => QueryLanguage.Catch_<TSource>((IEnumerable<IObservable<TSource>>) sources);

    public virtual IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources) => QueryLanguage.Catch_<TSource>(sources);

    private static IObservable<TSource> Catch_<TSource>(IEnumerable<IObservable<TSource>> sources) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Catch<TSource>(sources);

    public virtual IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.CombineLatest<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public virtual IObservable<TResult> CombineLatest<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return QueryLanguage.CombineLatest_<TSource, TResult>(sources, resultSelector);
    }

    public virtual IObservable<IList<TSource>> CombineLatest<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.CombineLatest_<TSource, IList<TSource>>(sources, (Func<IList<TSource>, IList<TSource>>) (res => (IList<TSource>) res.ToList<TSource>()));
    }

    public virtual IObservable<IList<TSource>> CombineLatest<TSource>(
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.CombineLatest_<TSource, IList<TSource>>((IEnumerable<IObservable<TSource>>) sources, (Func<IList<TSource>, IList<TSource>>) (res => (IList<TSource>) res.ToList<TSource>()));
    }

    private static IObservable<TResult> CombineLatest_<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.CombineLatest<TSource, TResult>(sources, resultSelector);
    }

    public virtual IObservable<TSource> Concat<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Concat_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources) => QueryLanguage.Concat_<TSource>((IEnumerable<IObservable<TSource>>) sources);

    public virtual IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources) => QueryLanguage.Concat_<TSource>(sources);

    private static IObservable<TSource> Concat_<TSource>(IEnumerable<IObservable<TSource>> sources) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Concat<TSource>(sources);

    public virtual IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources) => this.Concat_<TSource>(sources);

    private IObservable<TSource> Concat_<TSource>(IObservable<IObservable<TSource>> sources) => this.Merge<TSource>(sources, 1);

    public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources) => QueryLanguage.Merge_<TSource>(sources);

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return QueryLanguage.Merge_<TSource>(sources, maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations), maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      int maxConcurrent,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(scheduler), maxConcurrent);
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      }).ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));
    }

    public virtual IObservable<TSource> Merge<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      }).ToObservable<IObservable<TSource>>(scheduler));
    }

    public virtual IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources) => QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) sources).ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));

    public virtual IObservable<TSource> Merge<TSource>(
      IScheduler scheduler,
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.Merge_<TSource>(((IEnumerable<IObservable<TSource>>) sources).ToObservable<IObservable<TSource>>(scheduler));
    }

    public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources) => QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(SchedulerDefaults.ConstantTimeOperations));

    public virtual IObservable<TSource> Merge<TSource>(
      IEnumerable<IObservable<TSource>> sources,
      IScheduler scheduler)
    {
      return QueryLanguage.Merge_<TSource>(sources.ToObservable<IObservable<TSource>>(scheduler));
    }

    private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Merge<TSource>(sources);

    private static IObservable<TSource> Merge_<TSource>(
      IObservable<IObservable<TSource>> sources,
      int maxConcurrent)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Merge<TSource>(sources, maxConcurrent);
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>((IEnumerable<IObservable<TSource>>) new IObservable<TSource>[2]
      {
        first,
        second
      });
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      params IObservable<TSource>[] sources)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>((IEnumerable<IObservable<TSource>>) sources);
    }

    public virtual IObservable<TSource> OnErrorResumeNext<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.OnErrorResumeNext_<TSource>(sources);
    }

    private static IObservable<TSource> OnErrorResumeNext_<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.OnErrorResumeNext<TSource>(sources);
    }

    public virtual IObservable<TSource> SkipUntil<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipUntil<TSource, TOther>(source, other);
    }

    public virtual IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources) => this.Switch_<TSource>(sources);

    private IObservable<TSource> Switch_<TSource>(IObservable<IObservable<TSource>> sources) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Switch<TSource>(sources);

    public virtual IObservable<TSource> TakeUntil<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeUntil<TSource, TOther>(source, other);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(
      IObservable<TSource> source,
      Func<IObservable<TWindowClosing>> windowClosingSelector)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.Observαble.Window<TSource, TWindowClosing>(source, windowClosingSelector);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(
      IObservable<TSource> source,
      IObservable<TWindowOpening> windowOpenings,
      Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
    {
      return windowOpenings.GroupJoin<TWindowOpening, TSource, TWindowClosing, Unit, IObservable<TSource>>(source, windowClosingSelector, (Func<TSource, IObservable<Unit>>) (_ => Observable.Empty<Unit>()), (Func<TWindowOpening, IObservable<TSource>, IObservable<TSource>>) ((_, window) => window));
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(
      IObservable<TSource> source,
      IObservable<TWindowBoundary> windowBoundaries)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.Observαble.Window<TSource, TWindowBoundary>(source, windowBoundaries);
    }

    public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource, TResult>(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      return QueryLanguage.Zip_<TSource>(sources).Select<IList<TSource>, TResult>(resultSelector);
    }

    public virtual IObservable<IList<TSource>> Zip<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return QueryLanguage.Zip_<TSource>(sources);
    }

    public virtual IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources) => QueryLanguage.Zip_<TSource>((IEnumerable<IObservable<TSource>>) sources);

    private static IObservable<IList<TSource>> Zip_<TSource>(
      IEnumerable<IObservable<TSource>> sources)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Zip<TSource>(sources);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      Func<TSource1, TSource2, TSource3, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Zip<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(
      IObservable<TSource1> source1,
      IObservable<TSource2> source2,
      IObservable<TSource3> source3,
      IObservable<TSource4> source4,
      Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Zip<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
    }

    public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(
      IObservable<TFirst> first,
      IEnumerable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
    }

    public virtual IObservable<TSource> AsObservable<TSource>(IObservable<TSource> source) => source is System.Reactive.Linq.Observαble.AsObservable<TSource> asObservable ? asObservable.Ω() : (IObservable<TSource>) new System.Reactive.Linq.Observαble.AsObservable<TSource>(source);

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return QueryLanguage.Buffer_<TSource>(source, count, count);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return QueryLanguage.Buffer_<TSource>(source, count, skip);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Buffer<TSource>(source, count, skip);
    }

    public virtual IObservable<TSource> Dematerialize<TSource>(
      IObservable<Notification<TSource>> source)
    {
      return source is System.Reactive.Linq.Observαble.Materialize<TSource> materialize ? materialize.Dematerialize() : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Dematerialize<TSource>(source);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source) => QueryLanguage.DistinctUntilChanged_<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);

    public virtual IObservable<TSource> DistinctUntilChanged<TSource>(
      IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.DistinctUntilChanged_<TSource, TKey>(source, keySelector, comparer);
    }

    private static IObservable<TSource> DistinctUntilChanged_<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, Stubs.Nop);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action onCompleted)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, onCompleted);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, onError, Stubs.Nop);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      return QueryLanguage.Do_<TSource>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TSource> Do<TSource>(
      IObservable<TSource> source,
      IObserver<TSource> observer)
    {
      return QueryLanguage.Do_<TSource>(source, new Action<TSource>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
    }

    private static IObservable<TSource> Do_<TSource>(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Do<TSource>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TSource> Finally<TSource>(
      IObservable<TSource> source,
      Action finallyAction)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Finally<TSource>(source, finallyAction);
    }

    public virtual IObservable<TSource> IgnoreElements<TSource>(IObservable<TSource> source) => source is System.Reactive.Linq.Observαble.IgnoreElements<TSource> ignoreElements ? ignoreElements.Ω() : (IObservable<TSource>) new System.Reactive.Linq.Observαble.IgnoreElements<TSource>(source);

    public virtual IObservable<Notification<TSource>> Materialize<TSource>(
      IObservable<TSource> source)
    {
      return (IObservable<Notification<TSource>>) new System.Reactive.Linq.Observαble.Materialize<TSource>(source);
    }

    public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source) => QueryLanguage.RepeatInfinite<IObservable<TSource>>(source).Concat<TSource>();

    private static IEnumerable<T> RepeatInfinite<T>(T value)
    {
      while (true)
        yield return value;
    }

    public virtual IObservable<TSource> Repeat<TSource>(
      IObservable<TSource> source,
      int repeatCount)
    {
      return Enumerable.Repeat<IObservable<TSource>>(source, repeatCount).Concat<TSource>();
    }

    public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source) => QueryLanguage.RepeatInfinite<IObservable<TSource>>(source).Catch<TSource>();

    public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount) => Enumerable.Repeat<IObservable<TSource>>(source, retryCount).Catch<TSource>();

    public virtual IObservable<TAccumulate> Scan<TSource, TAccumulate>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      return (IObservable<TAccumulate>) new System.Reactive.Linq.Observαble.Scan<TSource, TAccumulate>(source, seed, accumulator);
    }

    public virtual IObservable<TSource> Scan<TSource>(
      IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Scan<TSource>(source, accumulator);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, int count) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipLast<TSource>(source, count);

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      params TSource[] values)
    {
      return QueryLanguage.StartWith_<TSource>(source, SchedulerDefaults.ConstantTimeOperations, values);
    }

    public virtual IObservable<TSource> StartWith<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler,
      params TSource[] values)
    {
      return QueryLanguage.StartWith_<TSource>(source, scheduler, values);
    }

    private static IObservable<TSource> StartWith_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler,
      params TSource[] values)
    {
      return ((IEnumerable<TSource>) values).ToObservable<TSource>(scheduler).Concat<TSource>(source);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count) => QueryLanguage.TakeLast_<TSource>(source, count, SchedulerDefaults.Iteration);

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, count, scheduler);
    }

    private static IObservable<TSource> TakeLast_<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeLast<TSource>(source, count, scheduler);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.TakeLastBuffer<TSource>(source, count);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return QueryLanguage.Window_<TSource>(source, count, skip);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      int count)
    {
      return QueryLanguage.Window_<TSource>(source, count, count);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      int count,
      int skip)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.Observαble.Window<TSource>(source, count, skip);
    }

    public virtual IObservable<TResult> Cast<TResult>(IObservable<object> source) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Cast<object, TResult>(source);

    public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.DefaultIfEmpty<TSource>(source, default (TSource));

    public virtual IObservable<TSource> DefaultIfEmpty<TSource>(
      IObservable<TSource> source,
      TSource defaultValue)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.DefaultIfEmpty<TSource>(source, defaultValue);
    }

    public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Distinct<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);

    public virtual IObservable<TSource> Distinct<TSource>(
      IObservable<TSource> source,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Distinct<TSource, TSource>(source, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<TSource> Distinct<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Distinct<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<TSource> Distinct<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Distinct<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupBy_<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IGroupedObservable<TKey, TElement>>) new System.Reactive.Linq.Observαble.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, comparer);
    }

    public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
    {
      return QueryLanguage.GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, (Func<TSource, TSource>) (x => x), durationSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil_<TSource, TKey, TElement, TDuration>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IGroupedObservable<TKey, TElement>>) new System.Reactive.Linq.Observαble.GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
    }

    public virtual IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, IObservable<TRight>, TResult> resultSelector)
    {
      return QueryLanguage.GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    private static IObservable<TResult> GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, IObservable<TRight>, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public virtual IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return QueryLanguage.Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    private static IObservable<TResult> Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
    }

    public virtual IObservable<TResult> OfType<TResult>(IObservable<object> source) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.OfType<object, TResult>(source);

    public virtual IObservable<TResult> Select<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return source is System.Reactive.Linq.Observαble.Select<TSource> select ? select.Ω<TResult>(selector) : (IObservable<TResult>) new System.Reactive.Linq.Observαble.Select<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> Select<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, int, TResult> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Select<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TOther> SelectMany<TSource, TOther>(
      IObservable<TSource> source,
      IObservable<TOther> other)
    {
      return QueryLanguage.SelectMany_<TSource, TOther>(source, (Func<TSource, IObservable<TOther>>) (_ => other));
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector)
    {
      return QueryLanguage.SelectMany_<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.SelectMany<TSource, TResult>(source, selector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> onNext,
      Func<Exception, IObservable<TResult>> onError,
      Func<IObservable<TResult>> onCompleted)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.SelectMany<TSource, TResult>(source, selector);
    }

    public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return QueryLanguage.SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
    }

    public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, int count) => source is System.Reactive.Linq.Observαble.Skip<TSource> skip && skip._scheduler == null ? skip.Ω(count) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Skip<TSource>(source, count);

    public virtual IObservable<TSource> SkipWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> SkipWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count) => count == 0 ? this.Empty<TSource>() : QueryLanguage.Take_<TSource>(source, count);

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      int count,
      IScheduler scheduler)
    {
      return count == 0 ? this.Empty<TSource>(scheduler) : QueryLanguage.Take_<TSource>(source, count);
    }

    private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, int count) => source is System.Reactive.Linq.Observαble.Take<TSource> take && take._scheduler == null ? take.Ω(count) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Take<TSource>(source, count);

    public virtual IObservable<TSource> TakeWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> TakeWhile<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeWhile<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Where<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source is System.Reactive.Linq.Observαble.Where<TSource> where ? where.Ω(predicate) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Where<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Where<TSource>(
      IObservable<TSource> source,
      Func<TSource, int, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Where<TSource>(source, predicate);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeSpan, scheduler);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, timeShift, scheduler);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Buffer<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> Buffer<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.Buffer_<TSource>(source, timeSpan, count, scheduler);
    }

    private static IObservable<IList<TSource>> Buffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.Buffer<TSource>(source, timeSpan, count, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Delay_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Delay<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Delay<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Delay_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Delay_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Delay<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Delay<TSource, TDelay>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return QueryLanguage.Delay_<TSource, TDelay>(source, (IObservable<TDelay>) null, delayDurationSelector);
    }

    public virtual IObservable<TSource> Delay<TSource, TDelay>(
      IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return QueryLanguage.Delay_<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
    }

    private static IObservable<TSource> Delay_<TSource, TDelay>(
      IObservable<TSource> source,
      IObservable<TDelay> subscriptionDelay,
      Func<TSource, IObservable<TDelay>> delayDurationSelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Delay<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> DelaySubscription_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> DelaySubscription<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.DelaySubscription_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> DelaySubscription_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.DelaySubscription<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    private static IObservable<TResult> Generate_<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, TimeSpan> timeSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      return QueryLanguage.Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    private static IObservable<TResult> Generate_<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      Func<TState, DateTimeOffset> timeSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
    }

    public virtual IObservable<long> Interval(TimeSpan period) => QueryLanguage.Timer_(period, period, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<long> Interval(TimeSpan period, IScheduler scheduler) => QueryLanguage.Timer_(period, period, scheduler);

    public virtual IObservable<TSource> Sample<TSource>(
      IObservable<TSource> source,
      TimeSpan interval)
    {
      return QueryLanguage.Sample_<TSource>(source, interval, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Sample<TSource>(
      IObservable<TSource> source,
      TimeSpan interval,
      IScheduler scheduler)
    {
      return QueryLanguage.Sample_<TSource>(source, interval, scheduler);
    }

    private static IObservable<TSource> Sample_<TSource>(
      IObservable<TSource> source,
      TimeSpan interval,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Sample<TSource>(source, interval, scheduler);
    }

    public virtual IObservable<TSource> Sample<TSource, TSample>(
      IObservable<TSource> source,
      IObservable<TSample> sampler)
    {
      return QueryLanguage.Sample_<TSource, TSample>(source, sampler);
    }

    private static IObservable<TSource> Sample_<TSource, TSample>(
      IObservable<TSource> source,
      IObservable<TSample> sampler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Sample<TSource, TSample>(source, sampler);
    }

    public virtual IObservable<TSource> Skip<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.Skip_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Skip<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.Skip_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> Skip_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.Observαble.Skip<TSource> skip && skip._scheduler == scheduler ? skip.Ω(duration) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Skip<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.SkipLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> SkipLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.SkipLast_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> SkipLast_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipLast<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> SkipUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime)
    {
      return QueryLanguage.SkipUntil_<TSource>(source, startTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> SkipUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime,
      IScheduler scheduler)
    {
      return QueryLanguage.SkipUntil_<TSource>(source, startTime, scheduler);
    }

    private static IObservable<TSource> SkipUntil_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset startTime,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.Observαble.SkipUntil<TSource> skipUntil && skipUntil._scheduler == scheduler ? skipUntil.Ω(startTime) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.SkipUntil<TSource>(source, startTime, scheduler);
    }

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.Take_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Take<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.Take_<TSource>(source, duration, scheduler);
    }

    private static IObservable<TSource> Take_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.Observαble.Take<TSource> take && take._scheduler == scheduler ? take.Ω(duration) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.Take<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, scheduler, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TSource> TakeLast<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler timerScheduler,
      IScheduler loopScheduler)
    {
      return QueryLanguage.TakeLast_<TSource>(source, duration, timerScheduler, loopScheduler);
    }

    private static IObservable<TSource> TakeLast_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler timerScheduler,
      IScheduler loopScheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeLast<TSource>(source, duration, timerScheduler, loopScheduler);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      TimeSpan duration)
    {
      return QueryLanguage.TakeLastBuffer_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeLastBuffer_<TSource>(source, duration, scheduler);
    }

    private static IObservable<IList<TSource>> TakeLastBuffer_<TSource>(
      IObservable<TSource> source,
      TimeSpan duration,
      IScheduler scheduler)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.TakeLastBuffer<TSource>(source, duration, scheduler);
    }

    public virtual IObservable<TSource> TakeUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime)
    {
      return QueryLanguage.TakeUntil_<TSource>(source, endTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> TakeUntil<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime,
      IScheduler scheduler)
    {
      return QueryLanguage.TakeUntil_<TSource>(source, endTime, scheduler);
    }

    private static IObservable<TSource> TakeUntil_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset endTime,
      IScheduler scheduler)
    {
      return source is System.Reactive.Linq.Observαble.TakeUntil<TSource> takeUntil && takeUntil._scheduler == scheduler ? takeUntil.Ω(endTime) : (IObservable<TSource>) new System.Reactive.Linq.Observαble.TakeUntil<TSource>(source, endTime, scheduler);
    }

    public virtual IObservable<TSource> Throttle<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Throttle_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Throttle<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Throttle_<TSource>(source, dueTime, scheduler);
    }

    private static IObservable<TSource> Throttle_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Throttle<TSource>(source, dueTime, scheduler);
    }

    public virtual IObservable<TSource> Throttle<TSource, TThrottle>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Throttle<TSource, TThrottle>(source, throttleDurationSelector);
    }

    public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      IObservable<TSource> source)
    {
      return QueryLanguage.TimeInterval_<TSource>(source, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return QueryLanguage.TimeInterval_<TSource>(source, scheduler);
    }

    private static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<System.Reactive.TimeInterval<TSource>>) new System.Reactive.Linq.Observαble.TimeInterval<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, scheduler);
    }

    private static IObservable<TSource> Timeout_<TSource>(
      IObservable<TSource> source,
      TimeSpan dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>((Exception) new TimeoutException()), scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<TSource> Timeout<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return QueryLanguage.Timeout_<TSource>(source, dueTime, other, scheduler);
    }

    private static IObservable<TSource> Timeout_<TSource>(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IObservable<TSource> other,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Timeout<TSource>(source, dueTime, other, scheduler);
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, Observable.Throw<TSource>((Exception) new TimeoutException()));
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, other);
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, Observable.Throw<TSource>((Exception) new TimeoutException()));
    }

    public virtual IObservable<TSource> Timeout<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return QueryLanguage.Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
    }

    private static IObservable<TSource> Timeout_<TSource, TTimeout>(
      IObservable<TSource> source,
      IObservable<TTimeout> firstTimeout,
      Func<TSource, IObservable<TTimeout>> timeoutDurationSelector,
      IObservable<TSource> other)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
    }

    public virtual IObservable<long> Timer(TimeSpan dueTime) => QueryLanguage.Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<long> Timer(DateTimeOffset dueTime) => QueryLanguage.Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period) => QueryLanguage.Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period) => QueryLanguage.Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler) => QueryLanguage.Timer_(dueTime, scheduler);

    public virtual IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler) => QueryLanguage.Timer_(dueTime, scheduler);

    public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler) => QueryLanguage.Timer_(dueTime, period, scheduler);

    public virtual IObservable<long> Timer(
      DateTimeOffset dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return QueryLanguage.Timer_(dueTime, period, scheduler);
    }

    private static IObservable<long> Timer_(TimeSpan dueTime, IScheduler scheduler) => (IObservable<long>) new System.Reactive.Linq.Observαble.Timer(dueTime, new TimeSpan?(), scheduler);

    private static IObservable<long> Timer_(
      TimeSpan dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.Observαble.Timer(dueTime, new TimeSpan?(period), scheduler);
    }

    private static IObservable<long> Timer_(DateTimeOffset dueTime, IScheduler scheduler) => (IObservable<long>) new System.Reactive.Linq.Observαble.Timer(dueTime, new TimeSpan?(), scheduler);

    private static IObservable<long> Timer_(
      DateTimeOffset dueTime,
      TimeSpan period,
      IScheduler scheduler)
    {
      return (IObservable<long>) new System.Reactive.Linq.Observαble.Timer(dueTime, new TimeSpan?(period), scheduler);
    }

    public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source) => QueryLanguage.Timestamp_<TSource>(source, SchedulerDefaults.TimeBasedOperations);

    public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return QueryLanguage.Timestamp_<TSource>(source, scheduler);
    }

    private static IObservable<Timestamped<TSource>> Timestamp_<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<Timestamped<TSource>>) new System.Reactive.Linq.Observαble.Timestamp<TSource>(source, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeSpan, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, timeShift, scheduler);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.Observαble.Window<TSource>(source, timeSpan, timeShift, scheduler);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
    }

    public virtual IObservable<IObservable<TSource>> Window<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return QueryLanguage.Window_<TSource>(source, timeSpan, count, scheduler);
    }

    private static IObservable<IObservable<TSource>> Window_<TSource>(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      int count,
      IScheduler scheduler)
    {
      return (IObservable<IObservable<TSource>>) new System.Reactive.Linq.Observαble.Window<TSource>(source, timeSpan, count, scheduler);
    }

    public virtual Func<IObservable<TResult>> FromAsyncPattern<TResult>(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<IObservable<TResult>>) (() =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin((AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          return Observable.Throw<TResult>(ex, SchedulerDefaults.AsyncConversions);
        }
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(
      Func<T1, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, IObservable<TResult>>) (x =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          return Observable.Throw<TResult>(ex, SchedulerDefaults.AsyncConversions);
        }
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(
      Func<T1, T2, AsyncCallback, object, IAsyncResult> begin,
      Func<IAsyncResult, TResult> end)
    {
      return (Func<T1, T2, IObservable<TResult>>) ((x, y) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        try
        {
          IAsyncResult asyncResult = begin(x, y, (AsyncCallback) (iar =>
          {
            TResult result;
            try
            {
              result = end(iar);
            }
            catch (Exception ex)
            {
              subject.OnError(ex);
              return;
            }
            subject.OnNext(result);
            subject.OnCompleted();
          }), (object) null);
        }
        catch (Exception ex)
        {
          return Observable.Throw<TResult>(ex, SchedulerDefaults.AsyncConversions);
        }
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<IObservable<Unit>> FromAsyncPattern(
      Func<AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(
      Func<T1, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(
      Func<T1, T2, AsyncCallback, object, IAsyncResult> begin,
      Action<IAsyncResult> end)
    {
      return this.FromAsyncPattern<T1, T2, Unit>(begin, (Func<IAsyncResult, Unit>) (iar =>
      {
        end(iar);
        return Unit.Default;
      }));
    }

    public virtual IObservable<TSource> Start<TSource>(Func<TSource> function) => this.ToAsync<TSource>(function)();

    public virtual IObservable<TSource> Start<TSource>(Func<TSource> function, IScheduler scheduler) => this.ToAsync<TSource>(function, scheduler)();

    public virtual IObservable<Unit> Start(Action action) => this.ToAsync(action, SchedulerDefaults.AsyncConversions)();

    public virtual IObservable<Unit> Start(Action action, IScheduler scheduler) => this.ToAsync(action, scheduler)();

    public virtual Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function) => this.ToAsync<TResult>(function, SchedulerDefaults.AsyncConversions);

    public virtual Func<IObservable<TResult>> ToAsync<TResult>(
      Func<TResult> function,
      IScheduler scheduler)
    {
      return (Func<IObservable<TResult>>) (() =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function();
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function) => this.ToAsync<T, TResult>(function, SchedulerDefaults.AsyncConversions);

    public virtual Func<T, IObservable<TResult>> ToAsync<T, TResult>(
      Func<T, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T, IObservable<TResult>>) (first =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(
      Func<T1, T2, TResult> function)
    {
      return this.ToAsync<T1, T2, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(
      Func<T1, T2, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, IObservable<TResult>>) ((first, second) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(
      Func<T1, T2, T3, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(
      Func<T1, T2, T3, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, IObservable<TResult>>) ((first, second, third) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(
      Func<T1, T2, T3, T4, TResult> function)
    {
      return this.ToAsync<T1, T2, T3, T4, TResult>(function, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(
      Func<T1, T2, T3, T4, TResult> function,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, IObservable<TResult>>) ((first, second, third, fourth) =>
      {
        AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
        scheduler.Schedule((Action) (() =>
        {
          TResult result3 = default (TResult);
          TResult result4;
          try
          {
            result4 = function(first, second, third, fourth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(result4);
          subject.OnCompleted();
        }));
        return ((IObservable<TResult>) subject).AsObservable<TResult>();
      });
    }

    public virtual Func<IObservable<Unit>> ToAsync(Action action) => this.ToAsync(action, SchedulerDefaults.AsyncConversions);

    public virtual Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler) => (Func<IObservable<Unit>>) (() =>
    {
      AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
      scheduler.Schedule((Action) (() =>
      {
        try
        {
          action();
        }
        catch (Exception ex)
        {
          subject.OnError(ex);
          return;
        }
        subject.OnNext(Unit.Default);
        subject.OnCompleted();
      }));
      return ((IObservable<Unit>) subject).AsObservable<Unit>();
    });

    public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action) => this.ToAsync<TSource>(action, SchedulerDefaults.AsyncConversions);

    public virtual Func<TSource, IObservable<Unit>> ToAsync<TSource>(
      Action<TSource> action,
      IScheduler scheduler)
    {
      return (Func<TSource, IObservable<Unit>>) (first =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return ((IObservable<Unit>) subject).AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action) => this.ToAsync<T1, T2>(action, SchedulerDefaults.AsyncConversions);

    public virtual Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(
      Action<T1, T2> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, IObservable<Unit>>) ((first, second) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return ((IObservable<Unit>) subject).AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action) => this.ToAsync<T1, T2, T3>(action, SchedulerDefaults.AsyncConversions);

    public virtual Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(
      Action<T1, T2, T3> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, IObservable<Unit>>) ((first, second, third) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return ((IObservable<Unit>) subject).AsObservable<Unit>();
      });
    }

    public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(
      Action<T1, T2, T3, T4> action)
    {
      return this.ToAsync<T1, T2, T3, T4>(action, SchedulerDefaults.AsyncConversions);
    }

    public virtual Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(
      Action<T1, T2, T3, T4> action,
      IScheduler scheduler)
    {
      return (Func<T1, T2, T3, T4, IObservable<Unit>>) ((first, second, third, fourth) =>
      {
        AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
        scheduler.Schedule((Action) (() =>
        {
          try
          {
            action(first, second, third, fourth);
          }
          catch (Exception ex)
          {
            subject.OnError(ex);
            return;
          }
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
        }));
        return ((IObservable<Unit>) subject).AsObservable<Unit>();
      });
    }

    public virtual IConnectableObservable<TResult> Multicast<TSource, TResult>(
      IObservable<TSource> source,
      ISubject<TSource, TResult> subject)
    {
      return (IConnectableObservable<TResult>) new ConnectableObservable<TSource, TResult>(source, subject);
    }

    public virtual IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(
      IObservable<TSource> source,
      Func<ISubject<TSource, TIntermediate>> subjectSelector,
      Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Multicast<TSource, TIntermediate, TResult>(source, subjectSelector, selector);
    }

    public virtual IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source) => source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new Subject<TSource>());

    public virtual IObservable<TResult> Publish<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new Subject<TSource>()), selector);
    }

    public virtual IConnectableObservable<TSource> Publish<TSource>(
      IObservable<TSource> source,
      TSource initialValue)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new BehaviorSubject<TSource>(initialValue));
    }

    public virtual IObservable<TResult> Publish<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TSource initialValue)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new BehaviorSubject<TSource>(initialValue)), selector);
    }

    public virtual IConnectableObservable<TSource> PublishLast<TSource>(IObservable<TSource> source) => source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new AsyncSubject<TSource>());

    public virtual IObservable<TResult> PublishLast<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new AsyncSubject<TSource>()), selector);
    }

    public virtual IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.RefCount<TSource>(source);

    public virtual IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source) => source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>());

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>()), selector);
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(window));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(window)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(window, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(window, scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, scheduler)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window)), selector);
    }

    public virtual IConnectableObservable<TSource> Replay<TSource>(
      IObservable<TSource> source,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource>((ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window, scheduler));
    }

    public virtual IObservable<TResult> Replay<TSource, TResult>(
      IObservable<TSource> source,
      Func<IObservable<TSource>, IObservable<TResult>> selector,
      int bufferSize,
      TimeSpan window,
      IScheduler scheduler)
    {
      return source.Multicast<TSource, TSource, TResult>((Func<ISubject<TSource, TSource>>) (() => (ISubject<TSource, TSource>) new ReplaySubject<TSource>(bufferSize, window, scheduler)), selector);
    }

    public virtual IEnumerable<IList<TSource>> Chunkify<TSource>(IObservable<TSource> source) => source.Collect<TSource, IList<TSource>>((Func<IList<TSource>>) (() => (IList<TSource>) new List<TSource>()), (Func<IList<TSource>, TSource, IList<TSource>>) ((lst, x) =>
    {
      lst.Add(x);
      return lst;
    }), (Func<IList<TSource>, IList<TSource>>) (_ => (IList<TSource>) new List<TSource>()));

    public virtual IEnumerable<TResult> Collect<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> newCollector,
      Func<TResult, TSource, TResult> merge)
    {
      return QueryLanguage.Collect_<TSource, TResult>(source, newCollector, merge, (Func<TResult, TResult>) (_ => newCollector()));
    }

    public virtual IEnumerable<TResult> Collect<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
    {
      return QueryLanguage.Collect_<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
    }

    private static IEnumerable<TResult> Collect_<TSource, TResult>(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
    {
      return (IEnumerable<TResult>) new System.Reactive.Linq.Observαble.Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
    }

    public virtual TSource First<TSource>(IObservable<TSource> source) => QueryLanguage.FirstOrDefaultInternal<TSource>(source, true);

    public virtual TSource First<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.First<TSource>(this.Where<TSource>(source, predicate));
    }

    public virtual TSource FirstOrDefault<TSource>(IObservable<TSource> source) => QueryLanguage.FirstOrDefaultInternal<TSource>(source, false);

    public virtual TSource FirstOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.FirstOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource FirstOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      ManualResetEvent evt = new ManualResetEvent(false);
      using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
      {
        if (!seenValue)
          value = v;
        seenValue = true;
        evt.Set();
      }), (Action<Exception>) (e =>
      {
        ex = e;
        evt.Set();
      }), (Action) (() => evt.Set()))))
        evt.WaitOne();
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource> onNext)
    {
      ManualResetEvent evt = new ManualResetEvent(false);
      System.Reactive.Linq.Observαble.ForEach<TSource>._ observer = new System.Reactive.Linq.Observαble.ForEach<TSource>._(onNext, (Action) (() => evt.Set()));
      using (source.SubscribeSafe<TSource>((IObserver<TSource>) observer))
        evt.WaitOne();
      observer.Error.ThrowIfNotNull();
    }

    public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource, int> onNext)
    {
      ManualResetEvent evt = new ManualResetEvent(false);
      System.Reactive.Linq.Observαble.ForEach<TSource>.τ observer = new System.Reactive.Linq.Observαble.ForEach<TSource>.τ(onNext, (Action) (() => evt.Set()));
      using (source.SubscribeSafe<TSource>((IObserver<TSource>) observer))
        evt.WaitOne();
      observer.Error.ThrowIfNotNull();
    }

    public virtual IEnumerator<TSource> GetEnumerator<TSource>(IObservable<TSource> source)
    {
      Queue<Notification<TSource>> q = new Queue<Notification<TSource>>();
      System.Reactive.Threading.Semaphore s = new System.Reactive.Threading.Semaphore(0, int.MaxValue);
      return QueryLanguage.PushToPull<TSource, TSource>(source, (Action<Notification<TSource>>) (x =>
      {
        lock (q)
          q.Enqueue(x);
        s.Release();
      }), (Func<Notification<TSource>>) (() =>
      {
        s.WaitOne();
        lock (q)
          return q.Dequeue();
      }));
    }

    public virtual TSource Last<TSource>(IObservable<TSource> source) => QueryLanguage.LastOrDefaultInternal<TSource>(source, true);

    public virtual TSource Last<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate) => this.Last<TSource>(this.Where<TSource>(source, predicate));

    public virtual TSource LastOrDefault<TSource>(IObservable<TSource> source) => QueryLanguage.LastOrDefaultInternal<TSource>(source, false);

    public virtual TSource LastOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.LastOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource LastOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      ManualResetEvent evt = new ManualResetEvent(false);
      using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
      {
        seenValue = true;
        value = v;
      }), (Action<Exception>) (e =>
      {
        ex = e;
        evt.Set();
      }), (Action) (() => evt.Set()))))
        evt.WaitOne();
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual IEnumerable<TSource> Latest<TSource>(IObservable<TSource> source) => (IEnumerable<TSource>) new System.Reactive.Linq.Observαble.Latest<TSource>(source);

    public virtual IEnumerable<TSource> MostRecent<TSource>(
      IObservable<TSource> source,
      TSource initialValue)
    {
      return (IEnumerable<TSource>) new System.Reactive.Linq.Observαble.MostRecent<TSource>(source, initialValue);
    }

    public virtual IEnumerable<TSource> Next<TSource>(IObservable<TSource> source) => (IEnumerable<TSource>) new System.Reactive.Linq.Observαble.Next<TSource>(source);

    public virtual TSource Single<TSource>(IObservable<TSource> source) => QueryLanguage.SingleOrDefaultInternal<TSource>(source, true);

    public virtual TSource Single<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.Single<TSource>(this.Where<TSource>(source, predicate));
    }

    public virtual TSource SingleOrDefault<TSource>(IObservable<TSource> source) => QueryLanguage.SingleOrDefaultInternal<TSource>(source, false);

    public virtual TSource SingleOrDefault<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return this.SingleOrDefault<TSource>(this.Where<TSource>(source, predicate));
    }

    private static TSource SingleOrDefaultInternal<TSource>(
      IObservable<TSource> source,
      bool throwOnEmpty)
    {
      TSource value = default (TSource);
      bool seenValue = false;
      Exception ex = (Exception) null;
      ManualResetEvent evt = new ManualResetEvent(false);
      using (source.Subscribe((IObserver<TSource>) new AnonymousObserver<TSource>((Action<TSource>) (v =>
      {
        if (seenValue)
        {
          ex = (Exception) new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT);
          evt.Set();
        }
        value = v;
        seenValue = true;
      }), (Action<Exception>) (e =>
      {
        ex = e;
        evt.Set();
      }), (Action) (() => evt.Set()))))
        evt.WaitOne();
      ex.ThrowIfNotNull();
      if (throwOnEmpty && !seenValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return value;
    }

    public virtual TSource Wait<TSource>(IObservable<TSource> source) => QueryLanguage.LastOrDefaultInternal<TSource>(source, true);

    private static IEnumerator<TResult> PushToPull<TSource, TResult>(
      IObservable<TSource> source,
      Action<Notification<TSource>> push,
      Func<Notification<TResult>> pull)
    {
      SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
      PushPullAdapter<TSource, TResult> observer = new PushPullAdapter<TSource, TResult>(push, pull, new Action(assignmentDisposable.Dispose));
      assignmentDisposable.Disposable = source.SubscribeSafe<TSource>((IObserver<TSource>) observer);
      return (IEnumerator<TResult>) observer;
    }

    public virtual IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return Synchronization.ObserveOn<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      return Synchronization.ObserveOn<TSource>(source, context);
    }

    public virtual IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      return Synchronization.SubscribeOn<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      return Synchronization.SubscribeOn<TSource>(source, context);
    }

    public virtual IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source) => Synchronization.Synchronize<TSource>(source);

    public virtual IObservable<TSource> Synchronize<TSource>(
      IObservable<TSource> source,
      object gate)
    {
      return Synchronization.Synchronize<TSource>(source, gate);
    }

    public virtual IDisposable Subscribe<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer)
    {
      return QueryLanguage.Subscribe_<TSource>(source, observer, SchedulerDefaults.Iteration);
    }

    public virtual IDisposable Subscribe<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer,
      IScheduler scheduler)
    {
      return QueryLanguage.Subscribe_<TSource>(source, observer, scheduler);
    }

    private static IDisposable Subscribe_<TSource>(
      IEnumerable<TSource> source,
      IObserver<TSource> observer,
      IScheduler scheduler)
    {
      return new System.Reactive.Linq.Observαble.ToObservable<TSource>(source, scheduler).Subscribe(observer);
    }

    public virtual IEnumerable<TSource> ToEnumerable<TSource>(IObservable<TSource> source) => (IEnumerable<TSource>) new AnonymousEnumerable<TSource>((Func<IEnumerator<TSource>>) (() => source.GetEnumerator<TSource>()));

    public virtual IEventSource<Unit> ToEvent(IObservable<Unit> source) => (IEventSource<Unit>) new EventSource<Unit>(source, (Action<Action<Unit>, Unit>) ((h, _) => h(Unit.Default)));

    public virtual IEventSource<TSource> ToEvent<TSource>(IObservable<TSource> source) => (IEventSource<TSource>) new EventSource<TSource>(source, (Action<Action<TSource>, TSource>) ((h, value) => h(value)));

    public virtual IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
      IObservable<EventPattern<TEventArgs>> source)
      where TEventArgs : EventArgs
    {
      return (IEventPatternSource<TEventArgs>) new EventPatternSource<TEventArgs>(source.Select<EventPattern<TEventArgs>, EventPattern<object, TEventArgs>>((Func<EventPattern<TEventArgs>, EventPattern<object, TEventArgs>>) (x => (EventPattern<object, TEventArgs>) x)), (Action<Action<object, TEventArgs>, EventPattern<object, TEventArgs>>) ((h, evt) => h(evt.Sender, evt.EventArgs)));
    }

    public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.ToObservable<TSource>(source, SchedulerDefaults.Iteration);

    public virtual IObservable<TSource> ToObservable<TSource>(
      IEnumerable<TSource> source,
      IScheduler scheduler)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.ToObservable<TSource>(source, scheduler);
    }

    public virtual IObservable<TSource> Create<TSource>(
      Func<IObserver<TSource>, IDisposable> subscribe)
    {
      return (IObservable<TSource>) new AnonymousObservable<TSource>(subscribe);
    }

    public virtual IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, Action> subscribe) => (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (o =>
    {
      Action dispose = subscribe(o);
      return dispose == null ? Disposable.Empty : Disposable.Create(dispose);
    }));

    public virtual IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory) => (IObservable<TValue>) new System.Reactive.Linq.Observαble.Defer<TValue>(observableFactory);

    public virtual IObservable<TResult> Empty<TResult>() => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Empty<TResult>(SchedulerDefaults.ConstantTimeOperations);

    public virtual IObservable<TResult> Empty<TResult>(IScheduler scheduler) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Empty<TResult>(scheduler);

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, SchedulerDefaults.Iteration);
    }

    public virtual IObservable<TResult> Generate<TState, TResult>(
      TState initialState,
      Func<TState, bool> condition,
      Func<TState, TState> iterate,
      Func<TState, TResult> resultSelector,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, scheduler);
    }

    public virtual IObservable<TResult> Never<TResult>() => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Never<TResult>();

    public virtual IObservable<int> Range(int start, int count) => QueryLanguage.Range_(start, count, SchedulerDefaults.Iteration);

    public virtual IObservable<int> Range(int start, int count, IScheduler scheduler) => QueryLanguage.Range_(start, count, scheduler);

    private static IObservable<int> Range_(int start, int count, IScheduler scheduler) => (IObservable<int>) new System.Reactive.Linq.Observαble.Range(start, count, scheduler);

    public virtual IObservable<TResult> Repeat<TResult>(TResult value) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Repeat<TResult>(value, new int?(), SchedulerDefaults.Iteration);

    public virtual IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Repeat<TResult>(value, new int?(), scheduler);

    public virtual IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Repeat<TResult>(value, new int?(repeatCount), SchedulerDefaults.Iteration);

    public virtual IObservable<TResult> Repeat<TResult>(
      TResult value,
      int repeatCount,
      IScheduler scheduler)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Repeat<TResult>(value, new int?(repeatCount), scheduler);
    }

    public virtual IObservable<TResult> Return<TResult>(TResult value) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Return<TResult>(value, SchedulerDefaults.ConstantTimeOperations);

    public virtual IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Return<TResult>(value, scheduler);

    public virtual IObservable<TResult> Throw<TResult>(Exception exception) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Throw<TResult>(exception, SchedulerDefaults.ConstantTimeOperations);

    public virtual IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler) => (IObservable<TResult>) new System.Reactive.Linq.Observαble.Throw<TResult>(exception, scheduler);

    public virtual IObservable<TSource> Using<TSource, TResource>(
      Func<TResource> resourceFactory,
      Func<TResource, IObservable<TSource>> observableFactory)
      where TResource : IDisposable
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Using<TSource, TResource>(resourceFactory, observableFactory);
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler)
    {
      return QueryLanguage.FromEventPattern_(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<EventArgs>> FromEventPattern_(
      Action<EventHandler> addHandler,
      Action<EventHandler> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<EventPattern<EventArgs>>) new System.Reactive.Linq.Observαble.FromEventPattern.τ<EventHandler, EventArgs>((Func<EventHandler<EventArgs>, EventHandler>) (e => new EventHandler(e.Invoke)), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.Observαble.FromEventPattern.τ<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TDelegate, TEventArgs>(
      Func<EventHandler<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.Observαble.FromEventPattern.τ<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return (IObservable<EventPattern<TSender, TEventArgs>>) new System.Reactive.Linq.Observαble.FromEventPattern.τ<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      Action<EventHandler<TEventArgs>> addHandler,
      Action<EventHandler<TEventArgs>> removeHandler,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return (IObservable<EventPattern<TEventArgs>>) new System.Reactive.Linq.Observαble.FromEventPattern.τ<EventHandler<TEventArgs>, TEventArgs>((Func<EventHandler<TEventArgs>, EventHandler<TEventArgs>>) (handler => handler), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      object target,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<EventArgs>> FromEventPattern_(
      object target,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, EventArgs, EventPattern<EventArgs>>(target.GetType(), target, eventName, (Func<object, EventArgs, EventPattern<EventArgs>>) ((sender, args) => new EventPattern<EventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(target.GetType(), target, eventName, (Func<object, TEventArgs, EventPattern<TEventArgs>>) ((sender, args) => new EventPattern<TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(target, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(target, eventName, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(
      object target,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(target.GetType(), target, eventName, (Func<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>) ((sender, args) => new EventPattern<TSender, TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      Type type,
      string eventName)
    {
      return QueryLanguage.FromEventPattern_(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<EventArgs>> FromEventPattern(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<EventArgs>> FromEventPattern_(
      Type type,
      string eventName,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEventPattern_<object, EventArgs, EventPattern<EventArgs>>(type, (object) null, eventName, (Func<object, EventArgs, EventPattern<EventArgs>>) ((sender, args) => new EventPattern<EventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TEventArgs>(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<TEventArgs>> FromEventPattern_<TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<object, TEventArgs, EventPattern<TEventArgs>>(type, (object) null, eventName, (Func<object, TEventArgs, EventPattern<TEventArgs>>) ((sender, args) => new EventPattern<TEventArgs>(sender, args)), scheduler);
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(type, eventName, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs>(type, eventName, scheduler);
    }

    private static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern_<TSender, TEventArgs>(
      Type type,
      string eventName,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      return QueryLanguage.FromEventPattern_<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>(type, (object) null, eventName, (Func<TSender, TEventArgs, EventPattern<TSender, TEventArgs>>) ((sender, args) => new EventPattern<TSender, TEventArgs>(sender, args)), scheduler);
    }

    private static IObservable<TResult> FromEventPattern_<TSender, TEventArgs, TResult>(
      Type targetType,
      object target,
      string eventName,
      Func<TSender, TEventArgs, TResult> getResult,
      IScheduler scheduler)
      where TEventArgs : EventArgs
    {
      MethodInfo addMethod = (MethodInfo) null;
      MethodInfo removeMethod = (MethodInfo) null;
      Type delegateType = (Type) null;
      bool isWinRT = false;
      ReflectionUtils.GetEventMethods<TSender, TEventArgs>(targetType, target, eventName, out addMethod, out removeMethod, out delegateType, out isWinRT);
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.FromEventPattern.ρ<TSender, TEventArgs, TResult>(target, delegateType, addMethod, removeMethod, getResult, false, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.Observαble.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TDelegate, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.Observαble.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler)
    {
      return QueryLanguage.FromEvent_<TEventArgs>(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<TEventArgs> FromEvent<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_<TEventArgs>(addHandler, removeHandler, scheduler);
    }

    private static IObservable<TEventArgs> FromEvent_<TEventArgs>(
      Action<Action<TEventArgs>> addHandler,
      Action<Action<TEventArgs>> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<TEventArgs>) new System.Reactive.Linq.Observαble.FromEvent<Action<TEventArgs>, TEventArgs>((Func<Action<TEventArgs>, Action<TEventArgs>>) (h => h), addHandler, removeHandler, scheduler);
    }

    public virtual IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler)
    {
      return QueryLanguage.FromEvent_(addHandler, removeHandler, QueryLanguage.GetSchedulerForCurrentContext());
    }

    public virtual IObservable<Unit> FromEvent(
      Action<Action> addHandler,
      Action<Action> removeHandler,
      IScheduler scheduler)
    {
      return QueryLanguage.FromEvent_(addHandler, removeHandler, scheduler);
    }

    private static IObservable<Unit> FromEvent_(
      Action<Action> addHandler,
      Action<Action> removeHandler,
      IScheduler scheduler)
    {
      return (IObservable<Unit>) new System.Reactive.Linq.Observαble.FromEvent<Action, Unit>((Func<Action<Unit>, Action>) (h => (Action) (() => h(new Unit()))), addHandler, removeHandler, scheduler);
    }

    private static IScheduler GetSchedulerForCurrentContext()
    {
      SynchronizationContext current = SynchronizationContext.Current;
      return current != null ? (IScheduler) new SynchronizationContextScheduler(current, false) : SchedulerDefaults.ConstantTimeOperations;
    }

    public virtual IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      return (IObservable<TAccumulate>) new System.Reactive.Linq.Observαble.Aggregate<TSource, TAccumulate, TAccumulate>(source, seed, accumulator, Stubs<TAccumulate>.I);
    }

    public virtual IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator,
      Func<TAccumulate, TResult> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Aggregate<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
    }

    public virtual IObservable<TSource> Aggregate<TSource>(
      IObservable<TSource> source,
      Func<TSource, TSource, TSource> accumulator)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Aggregate<TSource>(source, accumulator);
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Average(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Average(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Average(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Average(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<double> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Average(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Average(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Average(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Average(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Average(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<double?> Average<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Average(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<bool> All<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.All<TSource>(source, predicate);
    }

    public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source) => (IObservable<bool>) new System.Reactive.Linq.Observαble.Any<TSource>(source);

    public virtual IObservable<bool> Any<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.Any<TSource>(source, predicate);
    }

    public virtual IObservable<double> Average(IObservable<double> source) => (IObservable<double>) new AverageDouble(source);

    public virtual IObservable<float> Average(IObservable<float> source) => (IObservable<float>) new AverageSingle(source);

    public virtual IObservable<Decimal> Average(IObservable<Decimal> source) => (IObservable<Decimal>) new AverageDecimal(source);

    public virtual IObservable<double> Average(IObservable<int> source) => (IObservable<double>) new AverageInt32(source);

    public virtual IObservable<double> Average(IObservable<long> source) => (IObservable<double>) new AverageInt64(source);

    public virtual IObservable<double?> Average(IObservable<double?> source) => (IObservable<double?>) new AverageDoubleNullable(source);

    public virtual IObservable<float?> Average(IObservable<float?> source) => (IObservable<float?>) new AverageSingleNullable(source);

    public virtual IObservable<Decimal?> Average(IObservable<Decimal?> source) => (IObservable<Decimal?>) new AverageDecimalNullable(source);

    public virtual IObservable<double?> Average(IObservable<int?> source) => (IObservable<double?>) new AverageInt32Nullable(source);

    public virtual IObservable<double?> Average(IObservable<long?> source) => (IObservable<double?>) new AverageInt64Nullable(source);

    public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value) => (IObservable<bool>) new System.Reactive.Linq.Observαble.Contains<TSource>(source, value, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);

    public virtual IObservable<bool> Contains<TSource>(
      IObservable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.Contains<TSource>(source, value, comparer);
    }

    public virtual IObservable<int> Count<TSource>(IObservable<TSource> source) => (IObservable<int>) new System.Reactive.Linq.Observαble.Count<TSource>(source);

    public virtual IObservable<int> Count<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<int>) new System.Reactive.Linq.Observαble.Count<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> ElementAt<TSource>(IObservable<TSource> source, int index) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.ElementAt<TSource>(source, index, true);

    public virtual IObservable<TSource> ElementAtOrDefault<TSource>(
      IObservable<TSource> source,
      int index)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.ElementAt<TSource>(source, index, false);
    }

    public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.FirstAsync<TSource>(source, (Func<TSource, bool>) null, true);

    public virtual IObservable<TSource> FirstAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.FirstAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.FirstAsync<TSource>(source, (Func<TSource, bool>) null, false);

    public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.FirstAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<bool> IsEmpty<TSource>(IObservable<TSource> source) => (IObservable<bool>) new System.Reactive.Linq.Observαble.IsEmpty<TSource>(source);

    public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.LastAsync<TSource>(source, (Func<TSource, bool>) null, true);

    public virtual IObservable<TSource> LastAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.LastAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.LastAsync<TSource>(source, (Func<TSource, bool>) null, false);

    public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.LastAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source) => (IObservable<long>) new System.Reactive.Linq.Observαble.LongCount<TSource>(source);

    public virtual IObservable<long> LongCount<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<long>) new System.Reactive.Linq.Observαble.LongCount<TSource>(source, predicate);
    }

    public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Max<TSource>(source, (IComparer<TSource>) Comparer<TSource>.Default);

    public virtual IObservable<TSource> Max<TSource>(
      IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Max<TSource>(source, comparer);
    }

    public virtual IObservable<double> Max(IObservable<double> source) => (IObservable<double>) new MaxDouble(source);

    public virtual IObservable<float> Max(IObservable<float> source) => (IObservable<float>) new MaxSingle(source);

    public virtual IObservable<Decimal> Max(IObservable<Decimal> source) => (IObservable<Decimal>) new MaxDecimal(source);

    public virtual IObservable<int> Max(IObservable<int> source) => (IObservable<int>) new MaxInt32(source);

    public virtual IObservable<long> Max(IObservable<long> source) => (IObservable<long>) new MaxInt64(source);

    public virtual IObservable<double?> Max(IObservable<double?> source) => (IObservable<double?>) new MaxDoubleNullable(source);

    public virtual IObservable<float?> Max(IObservable<float?> source) => (IObservable<float?>) new MaxSingleNullable(source);

    public virtual IObservable<Decimal?> Max(IObservable<Decimal?> source) => (IObservable<Decimal?>) new MaxDecimalNullable(source);

    public virtual IObservable<int?> Max(IObservable<int?> source) => (IObservable<int?>) new MaxInt32Nullable(source);

    public virtual IObservable<long?> Max(IObservable<long?> source) => (IObservable<long?>) new MaxInt64Nullable(source);

    public virtual IObservable<TResult> Max<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return this.Max<TResult>(this.Select<TSource, TResult>(source, selector));
    }

    public virtual IObservable<TResult> Max<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      return this.Max<TResult>(this.Select<TSource, TResult>(source, selector), comparer);
    }

    public virtual IObservable<double> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Max(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Max(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Max(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Max(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Max(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Max(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Max(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Max(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Max(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Max<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Max(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.MaxBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) Comparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.MaxBy<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.Min<TSource>(source, (IComparer<TSource>) Comparer<TSource>.Default);

    public virtual IObservable<TSource> Min<TSource>(
      IObservable<TSource> source,
      IComparer<TSource> comparer)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.Min<TSource>(source, comparer);
    }

    public virtual IObservable<double> Min(IObservable<double> source) => (IObservable<double>) new MinDouble(source);

    public virtual IObservable<float> Min(IObservable<float> source) => (IObservable<float>) new MinSingle(source);

    public virtual IObservable<Decimal> Min(IObservable<Decimal> source) => (IObservable<Decimal>) new MinDecimal(source);

    public virtual IObservable<int> Min(IObservable<int> source) => (IObservable<int>) new MinInt32(source);

    public virtual IObservable<long> Min(IObservable<long> source) => (IObservable<long>) new MinInt64(source);

    public virtual IObservable<double?> Min(IObservable<double?> source) => (IObservable<double?>) new MinDoubleNullable(source);

    public virtual IObservable<float?> Min(IObservable<float?> source) => (IObservable<float?>) new MinSingleNullable(source);

    public virtual IObservable<Decimal?> Min(IObservable<Decimal?> source) => (IObservable<Decimal?>) new MinDecimalNullable(source);

    public virtual IObservable<int?> Min(IObservable<int?> source) => (IObservable<int?>) new MinInt32Nullable(source);

    public virtual IObservable<long?> Min(IObservable<long?> source) => (IObservable<long?>) new MinInt64Nullable(source);

    public virtual IObservable<TResult> Min<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector)
    {
      return this.Min<TResult>(this.Select<TSource, TResult>(source, selector));
    }

    public virtual IObservable<TResult> Min<TSource, TResult>(
      IObservable<TSource> source,
      Func<TSource, TResult> selector,
      IComparer<TResult> comparer)
    {
      return this.Min<TResult>(this.Select<TSource, TResult>(source, selector), comparer);
    }

    public virtual IObservable<double> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Min(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Min(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Min(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Min(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Min(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Min(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Min(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Min(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Min(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Min<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Min(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.MinBy<TSource, TKey>(source, keySelector, (IComparer<TKey>) Comparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      return (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.MinBy<TSource, TKey>(source, keySelector, comparer);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IObservable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.SequenceEqual<TSource>(first, second, comparer);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IEnumerable<TSource> second)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);
    }

    public virtual IObservable<bool> SequenceEqual<TSource>(
      IObservable<TSource> first,
      IEnumerable<TSource> second,
      IEqualityComparer<TSource> comparer)
    {
      return (IObservable<bool>) new System.Reactive.Linq.Observαble.SequenceEqual<TSource>(first, second, comparer);
    }

    public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.SingleAsync<TSource>(source, (Func<TSource, bool>) null, true);

    public virtual IObservable<TSource> SingleAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SingleAsync<TSource>(source, predicate, true);
    }

    public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source) => (IObservable<TSource>) new System.Reactive.Linq.Observαble.SingleAsync<TSource>(source, (Func<TSource, bool>) null, false);

    public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(
      IObservable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.SingleAsync<TSource>(source, predicate, false);
    }

    public virtual IObservable<double> Sum(IObservable<double> source) => (IObservable<double>) new SumDouble(source);

    public virtual IObservable<float> Sum(IObservable<float> source) => (IObservable<float>) new SumSingle(source);

    public virtual IObservable<Decimal> Sum(IObservable<Decimal> source) => (IObservable<Decimal>) new SumDecimal(source);

    public virtual IObservable<int> Sum(IObservable<int> source) => (IObservable<int>) new SumInt32(source);

    public virtual IObservable<long> Sum(IObservable<long> source) => (IObservable<long>) new SumInt64(source);

    public virtual IObservable<double?> Sum(IObservable<double?> source) => (IObservable<double?>) new SumDoubleNullable(source);

    public virtual IObservable<float?> Sum(IObservable<float?> source) => (IObservable<float?>) new SumSingleNullable(source);

    public virtual IObservable<Decimal?> Sum(IObservable<Decimal?> source) => (IObservable<Decimal?>) new SumDecimalNullable(source);

    public virtual IObservable<int?> Sum(IObservable<int?> source) => (IObservable<int?>) new SumInt32Nullable(source);

    public virtual IObservable<long?> Sum(IObservable<long?> source) => (IObservable<long?>) new SumInt64Nullable(source);

    public virtual IObservable<double> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, double> selector)
    {
      return this.Sum(this.Select<TSource, double>(source, selector));
    }

    public virtual IObservable<float> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, float> selector)
    {
      return this.Sum(this.Select<TSource, float>(source, selector));
    }

    public virtual IObservable<Decimal> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal> selector)
    {
      return this.Sum(this.Select<TSource, Decimal>(source, selector));
    }

    public virtual IObservable<int> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, int> selector)
    {
      return this.Sum(this.Select<TSource, int>(source, selector));
    }

    public virtual IObservable<long> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, long> selector)
    {
      return this.Sum(this.Select<TSource, long>(source, selector));
    }

    public virtual IObservable<double?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, double?> selector)
    {
      return this.Sum(this.Select<TSource, double?>(source, selector));
    }

    public virtual IObservable<float?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, float?> selector)
    {
      return this.Sum(this.Select<TSource, float?>(source, selector));
    }

    public virtual IObservable<Decimal?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, Decimal?> selector)
    {
      return this.Sum(this.Select<TSource, Decimal?>(source, selector));
    }

    public virtual IObservable<int?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, int?> selector)
    {
      return this.Sum(this.Select<TSource, int?>(source, selector));
    }

    public virtual IObservable<long?> Sum<TSource>(
      IObservable<TSource> source,
      Func<TSource, long?> selector)
    {
      return this.Sum(this.Select<TSource, long?>(source, selector));
    }

    public virtual IObservable<TSource[]> ToArray<TSource>(IObservable<TSource> source) => (IObservable<TSource[]>) new System.Reactive.Linq.Observαble.ToArray<TSource>(source);

    public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IDictionary<TKey, TElement>>) new System.Reactive.Linq.Observαble.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return (IObservable<IDictionary<TKey, TElement>>) new System.Reactive.Linq.Observαble.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<IDictionary<TKey, TSource>>) new System.Reactive.Linq.Observαble.ToDictionary<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<IDictionary<TKey, TSource>>) new System.Reactive.Linq.Observαble.ToDictionary<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<IList<TSource>> ToList<TSource>(IObservable<TSource> source) => (IObservable<IList<TSource>>) new System.Reactive.Linq.Observαble.ToList<TSource>(source);

    public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<ILookup<TKey, TElement>>) new System.Reactive.Linq.Observαble.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
    }

    public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return (IObservable<ILookup<TKey, TSource>>) new System.Reactive.Linq.Observαble.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), comparer);
    }

    public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return (IObservable<ILookup<TKey, TElement>>) new System.Reactive.Linq.Observαble.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return (IObservable<ILookup<TKey, TSource>>) new System.Reactive.Linq.Observαble.ToLookup<TSource, TKey, TSource>(source, keySelector, (Func<TSource, TSource>) (x => x), (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources)
    {
      return this.Case<TValue, TResult>(selector, sources, this.Empty<TResult>());
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IScheduler scheduler)
    {
      return this.Case<TValue, TResult>(selector, sources, this.Empty<TResult>(scheduler));
    }

    public virtual IObservable<TResult> Case<TValue, TResult>(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IObservable<TResult> defaultSource)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.Case<TValue, TResult>(selector, sources, defaultSource);
    }

    public virtual IObservable<TSource> DoWhile<TSource>(
      IObservable<TSource> source,
      Func<bool> condition)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.DoWhile<TSource>(source, condition);
    }

    public virtual IObservable<TResult> For<TSource, TResult>(
      IEnumerable<TSource> source,
      Func<TSource, IObservable<TResult>> resultSelector)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.For<TSource, TResult>(source, resultSelector);
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource)
    {
      return this.If<TResult>(condition, thenSource, this.Empty<TResult>());
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IScheduler scheduler)
    {
      return this.If<TResult>(condition, thenSource, this.Empty<TResult>(scheduler));
    }

    public virtual IObservable<TResult> If<TResult>(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IObservable<TResult> elseSource)
    {
      return (IObservable<TResult>) new System.Reactive.Linq.Observαble.If<TResult>(condition, thenSource, elseSource);
    }

    public virtual IObservable<TSource> While<TSource>(
      Func<bool> condition,
      IObservable<TSource> source)
    {
      return (IObservable<TSource>) new System.Reactive.Linq.Observαble.While<TSource>(condition, source);
    }
  }
}
