// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Plan`5
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Joins
{
  internal class Plan<T1, T2, T3, T4, TResult> : Plan<TResult>
  {
    internal Pattern<T1, T2, T3, T4> Expression { get; private set; }

    internal Func<T1, T2, T3, T4, TResult> Selector { get; private set; }

    internal Plan(Pattern<T1, T2, T3, T4> expression, Func<T1, T2, T3, T4, TResult> selector)
    {
      this.Expression = expression;
      this.Selector = selector;
    }

    internal override ActivePlan Activate(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObserver<TResult> observer,
      Action<ActivePlan> deactivate)
    {
      Action<Exception> onError = new Action<Exception>(observer.OnError);
      JoinObserver<T1> firstJoinObserver = Plan<TResult>.CreateObserver<T1>(externalSubscriptions, this.Expression.First, onError);
      JoinObserver<T2> secondJoinObserver = Plan<TResult>.CreateObserver<T2>(externalSubscriptions, this.Expression.Second, onError);
      JoinObserver<T3> thirdJoinObserver = Plan<TResult>.CreateObserver<T3>(externalSubscriptions, this.Expression.Third, onError);
      JoinObserver<T4> fourthJoinObserver = Plan<TResult>.CreateObserver<T4>(externalSubscriptions, this.Expression.Fourth, onError);
      ActivePlan<T1, T2, T3, T4> activePlan = (ActivePlan<T1, T2, T3, T4>) null;
      activePlan = new ActivePlan<T1, T2, T3, T4>(firstJoinObserver, secondJoinObserver, thirdJoinObserver, fourthJoinObserver, (Action<T1, T2, T3, T4>) ((first, second, third, fourth) =>
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this.Selector(first, second, third, fourth);
        }
        catch (Exception ex)
        {
          observer.OnError(ex);
          return;
        }
        observer.OnNext(result2);
      }), (Action) (() =>
      {
        firstJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        secondJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        thirdJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        fourthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        deactivate((ActivePlan) activePlan);
      }));
      firstJoinObserver.AddActivePlan((ActivePlan) activePlan);
      secondJoinObserver.AddActivePlan((ActivePlan) activePlan);
      thirdJoinObserver.AddActivePlan((ActivePlan) activePlan);
      fourthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      return (ActivePlan) activePlan;
    }
  }
}
