// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Joins
{
  internal class ActivePlan<T1> : ActivePlan
  {
    private readonly Action<T1> onNext;
    private readonly Action onCompleted;
    private readonly JoinObserver<T1> first;

    internal ActivePlan(JoinObserver<T1> first, Action<T1> onNext, Action onCompleted)
    {
      this.onNext = onNext;
      this.onCompleted = onCompleted;
      this.first = first;
      this.AddJoinObserver((IJoinObserver) first);
    }

    internal override void Match()
    {
      if (this.first.Queue.Count <= 0)
        return;
      Notification<T1> notification = this.first.Queue.Peek();
      if (notification.Kind == NotificationKind.OnCompleted)
      {
        this.onCompleted();
      }
      else
      {
        this.Dequeue();
        this.onNext(notification.Value);
      }
    }
  }
}
