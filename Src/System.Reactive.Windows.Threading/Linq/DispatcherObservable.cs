// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.DispatcherObservable
// Assembly: System.Reactive.Windows.Threading, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: D0078FA9-FF17-4C3C-823C-96AD815797E4
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Windows.Threading.dll

using System.Reactive.Concurrency;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace System.Reactive.Linq
{
  public static class DispatcherObservable
  {
    public static IObservable<TSource> ObserveOn<TSource>(
      this IObservable<TSource> source,
      Dispatcher dispatcher)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return dispatcher != null ? DispatcherObservable.ObserveOn_<TSource>(source, dispatcher) : throw new ArgumentNullException(nameof (dispatcher));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      this IObservable<TSource> source,
      DispatcherScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return DispatcherObservable.ObserveOn_<TSource>(source, scheduler.Dispatcher);
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      this IObservable<TSource> source,
      DependencyObject dependencyObject)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dependencyObject == null)
        throw new ArgumentNullException(nameof (dependencyObject));
      return DispatcherObservable.ObserveOn_<TSource>(source, dependencyObject.Dispatcher);
    }

    public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return DispatcherObservable.ObserveOn_<TSource>(source, ((DependencyObject) Deployment.Current).Dispatcher);
    }

    private static IObservable<TSource> ObserveOn_<TSource>(
      IObservable<TSource> source,
      Dispatcher dispatcher)
    {
      return Synchronization.ObserveOn<TSource>(source, (SynchronizationContext) new DispatcherSynchronizationContext(dispatcher));
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      this IObservable<TSource> source,
      Dispatcher dispatcher)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return dispatcher != null ? DispatcherObservable.SubscribeOn_<TSource>(source, dispatcher) : throw new ArgumentNullException(nameof (dispatcher));
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      this IObservable<TSource> source,
      DispatcherScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return DispatcherObservable.SubscribeOn_<TSource>(source, scheduler.Dispatcher);
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      this IObservable<TSource> source,
      DependencyObject dependencyObject)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (dependencyObject == null)
        throw new ArgumentNullException(nameof (dependencyObject));
      return DispatcherObservable.SubscribeOn_<TSource>(source, dependencyObject.Dispatcher);
    }

    public static IObservable<TSource> SubscribeOnDispatcher<TSource>(
      this IObservable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return DispatcherObservable.SubscribeOn_<TSource>(source, ((DependencyObject) Deployment.Current).Dispatcher);
    }

    private static IObservable<TSource> SubscribeOn_<TSource>(
      IObservable<TSource> source,
      Dispatcher dispatcher)
    {
      return Synchronization.SubscribeOn<TSource>(source, (SynchronizationContext) new DispatcherSynchronizationContext(dispatcher));
    }
  }
}
