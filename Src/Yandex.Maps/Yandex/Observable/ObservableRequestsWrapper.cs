// Decompiled with JetBrains decompiler
// Type: Yandex.Observable.ObservableRequestsWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Reactive;
using System.Reactive.Subjects;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Observable
{
  internal abstract class ObservableRequestsWrapper
  {
    protected IObservable<TResponse> ExecuteObservableOperation<TParameters, TResponse, TError>(
      TParameters parameters,
      IDiversityCommunicator<TParameters, TResponse, TError> communicator)
      where TParameters : class
      where TResponse : class
      where TError : class
    {
      IObservable<TResponse> first = System.Reactive.Linq.Observable.FromEventPattern<EventHandler<RequestFailedEventArgs<TParameters>>, RequestFailedEventArgs<TParameters>>((Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => ((ICommunicator<TParameters, TResponse>) communicator).RequestFailed += ev), (Action<EventHandler<RequestFailedEventArgs<TParameters>>>) (ev => ((ICommunicator<TParameters, TResponse>) communicator).RequestFailed -= ev)).Where<EventPattern<RequestFailedEventArgs<TParameters>>>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestFailedEventArgs<TParameters>>, TResponse>((Func<EventPattern<RequestFailedEventArgs<TParameters>>, TResponse>) (e =>
      {
        throw new WrappedObservableException("Error", e.EventArgs.Exception);
      }));
      IObservable<TResponse> second1 = System.Reactive.Linq.Observable.FromEventPattern<RequestCompletedEventHandler<TParameters, TError>, RequestCompletedEventArgs<TParameters, TError>>((Action<RequestCompletedEventHandler<TParameters, TError>>) (ev => communicator.RequestCompletedWithErrorResult += ev), (Action<RequestCompletedEventHandler<TParameters, TError>>) (ev => communicator.RequestCompletedWithErrorResult -= ev)).Where<EventPattern<RequestCompletedEventArgs<TParameters, TError>>>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, TResponse>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TError>>, TResponse>) (e =>
      {
        throw new WrappedObservableException(e.EventArgs.RequestResults.ToString());
      }));
      IObservable<TResponse> second2 = System.Reactive.Linq.Observable.FromEventPattern<RequestCompletedEventHandler<TParameters, TResponse>, RequestCompletedEventArgs<TParameters, TResponse>>((Action<RequestCompletedEventHandler<TParameters, TResponse>>) (ev => ((ICommunicator<TParameters, TResponse>) communicator).RequestCompleted += ev), (Action<RequestCompletedEventHandler<TParameters, TResponse>>) (ev => ((ICommunicator<TParameters, TResponse>) communicator).RequestCompleted -= ev)).Where<EventPattern<RequestCompletedEventArgs<TParameters, TResponse>>>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResponse>>, bool>) (e => (object) e.EventArgs.Parameters == (object) (TParameters) parameters)).Select<EventPattern<RequestCompletedEventArgs<TParameters, TResponse>>, TResponse>((Func<EventPattern<RequestCompletedEventArgs<TParameters, TResponse>>, TResponse>) (e => e.EventArgs.RequestResults));
      ReplaySubject<TResponse> replaySubject = new ReplaySubject<TResponse>();
      first.Merge<TResponse>(second1).Merge<TResponse>(second2).Take<TResponse>(1).Subscribe((IObserver<TResponse>) replaySubject);
      communicator.Request(parameters);
      return (IObservable<TResponse>) replaySubject;
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
