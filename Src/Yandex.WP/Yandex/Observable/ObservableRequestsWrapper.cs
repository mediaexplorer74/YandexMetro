// Decompiled with JetBrains decompiler
// Type: Yandex.Observable.ObservableRequestsWrapper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Reactive;
using System.Reactive.Subjects;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Observable
{
  public abstract class ObservableRequestsWrapper
  {
    protected IObservable<TResponce> ExecuteObservableOperation<TParameters, TResponce, TError>(
      TParameters parameters,
      IDiversityCommunicator<TParameters, TResponce, TError> communicator)
      where TParameters : class
      where TResponce : class
      where TError : class
    {
      IObservable<TResponce> first = System.Reactive.Linq.Observable.FromEventPattern<EventHandler<RequestFailedEventArgs<TParameters>>, RequestFailedEventArgs<TParameters>>((Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => ((ICommunicator<TParameters, TResponce>) communicator).RequestFailed += ev), (Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => ((ICommunicator<TParameters, TResponce>) communicator).RequestFailed -= ev)).Where<EventPattern<RequestFailedEventArgs<TParameters>>>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestFailedEventArgs<TParameters>>, TResponce>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, TResponce>) (e =>
      {
        throw new WrappedObservableException("Error", e.EventArgs.Exception);
      }));
      IObservable<TResponce> second1 = System.Reactive.Linq.Observable.FromEventPattern<RequestCompletedEventHandler<TParameters, TError>, RequestCompletedEventArgs<TParameters, TError>>((Action<RequestCompletedEventHandler<TParameters, TError>>) (ev => communicator.RequestCompletedWithErrorResult += ev), (Action<RequestCompletedEventHandler<TParameters, TError>>) (ev => communicator.RequestCompletedWithErrorResult -= ev)).Where<EventPattern<RequestCompletedEventArgs<TParameters, TError>>>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, TResponce>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, TResponce>) (e =>
      {
        throw new WrappedObservableException(e.EventArgs.RequestResults.ToString());
      }));
      IObservable<TResponce> second2 = System.Reactive.Linq.Observable.FromEventPattern<RequestCompletedEventHandler<TParameters, TResponce>, RequestCompletedEventArgs<TParameters, TResponce>>((Action<RequestCompletedEventHandler<TParameters, TResponce>>) (ev => ((ICommunicator<TParameters, TResponce>) communicator).RequestCompleted += ev), (Action<RequestCompletedEventHandler<TParameters, TResponce>>) (ev => ((ICommunicator<TParameters, TResponce>) communicator).RequestCompleted -= ev)).Where<EventPattern<RequestCompletedEventArgs<TParameters, TResponce>>>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResponce>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestCompletedEventArgs<TParameters, TResponce>>, TResponce>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResponce>>, TResponce>) (e => e.EventArgs.RequestResults));
      ReplaySubject<TResponce> replaySubject = new ReplaySubject<TResponce>();
      first.Merge<TResponce>(second1).Merge<TResponce>(second2).Take<TResponce>(1).Subscribe((IObserver<TResponce>) replaySubject);
      communicator.Request(parameters);
      return (IObservable<TResponce>) replaySubject;
    }

    protected IObservable<TResult> ExecuteObservableOperation<TParameters, TResult>(
      TParameters parameters,
      ICommunicator<TParameters, TResult> communicator)
      where TParameters : class
      where TResult : class
    {
      IObservable<TResult> second = System.Reactive.Linq.Observable.FromEventPattern<EventHandler<RequestFailedEventArgs<TParameters>>, RequestFailedEventArgs<TParameters>>((Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => communicator.RequestFailed += ev), (Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => communicator.RequestFailed -= ev)).Where<EventPattern<RequestFailedEventArgs<TParameters>>>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestFailedEventArgs<TParameters>>, TResult>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, TResult>) (e =>
      {
        throw new WrappedObservableException("Error", e.EventArgs.Exception);
      }));
      IObservable<TResult> first = System.Reactive.Linq.Observable.FromEventPattern<RequestCompletedEventHandler<TParameters, TResult>, RequestCompletedEventArgs<TParameters, TResult>>((Action<RequestCompletedEventHandler<TParameters, TResult>>) (ev => communicator.RequestCompleted += ev), (Action<RequestCompletedEventHandler<TParameters, TResult>>) (ev => communicator.RequestCompleted -= ev)).Where<EventPattern<RequestCompletedEventArgs<TParameters, TResult>>>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResult>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestCompletedEventArgs<TParameters, TResult>>, TResult>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResult>>, TResult>) (e => e.EventArgs.RequestResults));
      ReplaySubject<TResult> replaySubject = new ReplaySubject<TResult>();
      first.Merge<TResult>(second).Take<TResult>(1).Subscribe((IObserver<TResult>) replaySubject);
      communicator.Request(parameters);
      return (IObservable<TResult>) replaySubject;
    }
  }
}
