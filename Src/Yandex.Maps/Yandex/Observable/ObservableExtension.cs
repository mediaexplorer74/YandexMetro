// Decompiled with JetBrains decompiler
// Type: Yandex.Observable.ObservableExtension
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Yandex.Patterns;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Observable
{
  internal static class ObservableExtension
  {
    private static readonly object _NetworkInterfaceWrapperLock = new object();
    private static WeakReference _networkInterfaceWrapperWeakReference;
    private static readonly TimeSpan DefaultTrickleInterval = TimeSpan.FromMilliseconds(50.0);

    public static IObservable<T> TrickleOnDispatcher<T>(
      this IObservable<T> source,
      byte groupSize,
      TimeSpan? trickleInterval = null)
    {
      return source.Trickle<T>(groupSize, trickleInterval).ObserveOnDispatcher<IList<T>>().SelectMany<IList<T>, T>((Func<IList<T>, IEnumerable<T>>) (item => (IEnumerable<T>) item));
    }

    public static IObservable<IList<T>> Trickle<T>(
      this IObservable<T> source,
      byte groupSize,
      TimeSpan? trickleInterval = null)
    {
      if (!trickleInterval.HasValue)
        trickleInterval = new TimeSpan?(ObservableExtension.DefaultTrickleInterval);
      return source.Buffer<T>((int) groupSize).Zip<IList<T>, long, IList<T>>(System.Reactive.Linq.Observable.Interval(trickleInterval.Value, (IScheduler) Scheduler.Default), (Func<IList<T>, long, IList<T>>) ((group, time) => group));
    }

    private static INetworkInterfaceWrapper GetNetworkInterfaceWrapper()
    {
      lock (ObservableExtension._NetworkInterfaceWrapperLock)
      {
        INetworkInterfaceWrapper target = ObservableExtension._networkInterfaceWrapperWeakReference == null ? (INetworkInterfaceWrapper) null : (INetworkInterfaceWrapper) ObservableExtension._networkInterfaceWrapperWeakReference.Target;
        if (target == null)
        {
          target = (INetworkInterfaceWrapper) new NetworkInterfaceWrapper();
          ObservableExtension._networkInterfaceWrapperWeakReference = new WeakReference((object) target);
        }
        return target;
      }
    }

    public static IObservable<T> WaitForNetwork<T>([NotNull] this IObservable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      INetworkInterfaceWrapper networkInterfaceWrapper = ObservableExtension.GetNetworkInterfaceWrapper();
      return source.WaitForCondition<EventHandler, T>(new Func<bool>(networkInterfaceWrapper.GetIsNetworkAvailable), (Action<EventHandler>) (handler => networkInterfaceWrapper.IsNetworkAvailableChanged += handler), (Action<EventHandler>) (handler => networkInterfaceWrapper.IsNetworkAvailableChanged -= handler));
    }

    public static IObservable<T> Retry<T>(
      [NotNull] this IObservable<T> source,
      TimeSpan interval,
      int retryCount)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (interval <= TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (interval));
      if (retryCount <= 0)
        throw new ArgumentOutOfRangeException(nameof (retryCount));
      return source.Catch<T>(System.Reactive.Linq.Observable.Interval(interval).Take<long>(1).SelectMany<long, T>(source).Retry<T>(retryCount));
    }

    public static IObservable<T> WaitForReadyState<T>(
      [NotNull] this IObservable<T> source,
      [NotNull] IStateService stateService,
      Action initialAction = null)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (stateService == null)
        throw new ArgumentNullException(nameof (stateService));
      return source.WaitForCondition<EventHandler<StateChangedEventArgs>, T>((Func<bool>) (() => stateService.State == ServiceState.Ready), (Action<EventHandler<StateChangedEventArgs>>) (handler => stateService.StateChanged += handler), (Action<EventHandler<StateChangedEventArgs>>) (handler => stateService.StateChanged -= handler), initialAction);
    }

    public static IObservable<T> WaitForCondition<TDelegate, T>(
      [NotNull] this IObservable<T> source,
      [NotNull] Func<bool> isConditionTrue,
      [NotNull] Action<TDelegate> addConditionChangedHandler,
      [NotNull] Action<TDelegate> removeConditionChangedHandler,
      Action initialAction = null)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (isConditionTrue == null)
        throw new ArgumentNullException(nameof (isConditionTrue));
      if (addConditionChangedHandler == null)
        throw new ArgumentNullException(nameof (addConditionChangedHandler));
      if (removeConditionChangedHandler == null)
        throw new ArgumentNullException(nameof (removeConditionChangedHandler));
      IObservable<Unit> second = initialAction == null ? System.Reactive.Linq.Observable.Return<Unit>(Unit.Default) : System.Reactive.Linq.Observable.Defer<Unit>((Func<IObservable<Unit>>) (() => System.Reactive.Linq.Observable.Start(initialAction)));
      return System.Reactive.Linq.Observable.FromEventPattern<TDelegate, EventArgs>(addConditionChangedHandler, removeConditionChangedHandler).Select<EventPattern<EventArgs>, Unit>((Func<EventPattern<EventArgs>, Unit>) (x => Unit.Default)).Merge<Unit>(second).Where<Unit>((Func<Unit, bool>) (x => isConditionTrue())).Take<Unit>(1).SelectMany<Unit, T>((Func<Unit, IObservable<T>>) (x => source));
    }
  }
}
