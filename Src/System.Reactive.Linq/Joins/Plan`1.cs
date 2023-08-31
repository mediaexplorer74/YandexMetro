// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Plan`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Joins
{
  public abstract class Plan<TResult>
  {
    internal Plan()
    {
    }

    internal abstract ActivePlan Activate(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObserver<TResult> observer,
      Action<ActivePlan> deactivate);

    internal static JoinObserver<TSource> CreateObserver<TSource>(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObservable<TSource> observable,
      Action<Exception> onError)
    {
      IJoinObserver joinObserver = (IJoinObserver) null;
      JoinObserver<TSource> observer;
      if (!externalSubscriptions.TryGetValue((object) observable, out joinObserver))
      {
        observer = new JoinObserver<TSource>(observable, onError);
        externalSubscriptions.Add((object) observable, (IJoinObserver) observer);
      }
      else
        observer = (JoinObserver<TSource>) joinObserver;
      return observer;
    }
  }
}
