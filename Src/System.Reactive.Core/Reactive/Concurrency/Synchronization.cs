// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.Synchronization
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static class Synchronization
  {
    public static IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        SerialDisposable d = new SerialDisposable();
        d.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = scheduler.Schedule((Action) (() => d.Disposable = (IDisposable) new ScheduledDisposable(scheduler, source.SubscribeSafe<TSource>(observer))));
        return (IDisposable) d;
      }));
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
        context.PostWithStartComplete((Action) (() =>
        {
          if (subscription.IsDisposed)
            return;
          subscription.Disposable = (IDisposable) new ContextDisposable(context, source.SubscribeSafe<TSource>(observer));
        }));
        return (IDisposable) subscription;
      }));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return scheduler != null ? (IObservable<TSource>) new System.Reactive.Concurrency.ObserveOn<TSource>(source, scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return context != null ? (IObservable<TSource>) new System.Reactive.Concurrency.ObserveOn<TSource>(source, context) : throw new ArgumentNullException(nameof (context));
    }

    public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source) => source != null ? (IObservable<TSource>) new System.Reactive.Concurrency.Synchronize<TSource>(source) : throw new ArgumentNullException(nameof (source));

    public static IObservable<TSource> Synchronize<TSource>(
      IObservable<TSource> source,
      object gate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return gate != null ? (IObservable<TSource>) new System.Reactive.Concurrency.Synchronize<TSource>(source, gate) : throw new ArgumentNullException(nameof (gate));
    }
  }
}
