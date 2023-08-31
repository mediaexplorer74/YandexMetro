// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Joins
{
  internal abstract class ActivePlan
  {
    private Dictionary<IJoinObserver, IJoinObserver> joinObservers = new Dictionary<IJoinObserver, IJoinObserver>();

    internal abstract void Match();

    protected void AddJoinObserver(IJoinObserver joinObserver) => this.joinObservers.Add(joinObserver, joinObserver);

    protected void Dequeue()
    {
      foreach (IJoinObserver joinObserver in this.joinObservers.Values)
        joinObserver.Dequeue();
    }
  }
}
