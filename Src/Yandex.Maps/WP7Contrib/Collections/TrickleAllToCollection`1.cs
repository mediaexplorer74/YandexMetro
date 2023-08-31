// Decompiled with JetBrains decompiler
// Type: WP7Contrib.Collections.TrickleAllToCollection`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Reactive.Concurrency;
using System.Windows.Threading;

namespace WP7Contrib.Collections
{
  internal sealed class TrickleAllToCollection<T> : BaseTrickleToCollection<T>
  {
    public TrickleAllToCollection(Action<T> addedAction, Action completedAction)
      : this(addedAction, (Action) (() => { }), (Action) (() => { }), (Action) (() => { }), (Action) (() => { }), completedAction)
    {
    }

    public TrickleAllToCollection(
      Action<T> addedAction,
      Action startedAction,
      Action stoppedAction,
      Action suspendedAction,
      Action resumedAction,
      Action completedAction)
      : base(addedAction, startedAction, stoppedAction, suspendedAction, resumedAction, completedAction)
    {
      this.DispatcherTimer = new DispatcherTimer()
      {
        Interval = this.StartupDelay
      };
      this.DispatcherTimer.Tick += (EventHandler) ((param0, param1) =>
      {
        if (!this.DispatcherTimer.IsEnabled)
          return;
        if (this.SourceQueue.Count != 0)
        {
          T item = this.SourceQueue.Dequeue();
          this.Destination.Add(item);
          Scheduler.CurrentThread.Schedule((Action) (() => this.AddedAction(item)));
          if (this.Count == 0)
            this.DispatcherTimer.Interval = this.Delay;
          ++this.Count;
        }
        else
        {
          this.DispatcherTimer.Stop();
          this.CompletedAction();
        }
      });
    }
  }
}
