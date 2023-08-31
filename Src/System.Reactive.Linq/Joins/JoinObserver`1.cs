// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.JoinObserver`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace System.Reactive.Joins
{
  internal sealed class JoinObserver<T> : ObserverBase<Notification<T>>, IJoinObserver, IDisposable
  {
    private object gate;
    private readonly IObservable<T> source;
    private readonly Action<Exception> onError;
    private List<ActivePlan> activePlans;
    private readonly SingleAssignmentDisposable subscription;
    private bool isDisposed;

    public System.Collections.Generic.Queue<Notification<T>> Queue { get; private set; }

    public JoinObserver(IObservable<T> source, Action<Exception> onError)
    {
      this.source = source;
      this.onError = onError;
      this.Queue = new System.Collections.Generic.Queue<Notification<T>>();
      this.subscription = new SingleAssignmentDisposable();
      this.activePlans = new List<ActivePlan>();
    }

    public void AddActivePlan(ActivePlan activePlan) => this.activePlans.Add(activePlan);

    public void Subscribe(object gate)
    {
      this.gate = gate;
      this.subscription.Disposable = this.source.Materialize<T>().SubscribeSafe<Notification<T>>((IObserver<Notification<T>>) this);
    }

    public void Dequeue() => this.Queue.Dequeue();

    protected override void OnNextCore(Notification<T> notification)
    {
      lock (this.gate)
      {
        if (this.isDisposed)
          return;
        if (notification.Kind == NotificationKind.OnError)
        {
          this.onError(notification.Exception);
        }
        else
        {
          this.Queue.Enqueue(notification);
          foreach (ActivePlan activePlan in this.activePlans.ToArray())
            activePlan.Match();
        }
      }
    }

    protected override void OnErrorCore(Exception exception)
    {
    }

    protected override void OnCompletedCore()
    {
    }

    internal void RemoveActivePlan(ActivePlan activePlan)
    {
      this.activePlans.Remove(activePlan);
      if (this.activePlans.Count != 0)
        return;
      this.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (this.isDisposed)
        return;
      if (disposing)
        this.subscription.Dispose();
      this.isDisposed = true;
    }
  }
}
