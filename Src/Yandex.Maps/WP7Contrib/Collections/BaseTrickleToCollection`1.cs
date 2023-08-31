// Decompiled with JetBrains decompiler
// Type: WP7Contrib.Collections.BaseTrickleToCollection`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace WP7Contrib.Collections
{
  internal abstract class BaseTrickleToCollection<T> : ITrickleToCollection<T>
  {
    protected readonly Action<T> AddedAction;
    protected readonly Action CompletedAction;
    protected readonly Action ResumedAction;
    protected readonly Action StartedAction;
    protected readonly Action StoppedAction;
    protected readonly Action SuspendedAction;
    protected int Count;
    protected TimeSpan Delay;
    protected IList<T> Destination;
    protected DispatcherTimer DispatcherTimer;
    protected Queue<T> SourceQueue = new Queue<T>();
    protected TimeSpan StartupDelay = TimeSpan.FromMilliseconds(150.0);

    protected BaseTrickleToCollection(Action<T> addedAction, Action completedAction)
      : this(addedAction, (Action) (() => { }), (Action) (() => { }), (Action) (() => { }), (Action) (() => { }), completedAction)
    {
    }

    protected BaseTrickleToCollection(
      Action<T> addedAction,
      Action startedAction,
      Action stoppedAction,
      Action suspendedAction,
      Action resumedAction,
      Action completedAction)
    {
      this.AddedAction = addedAction;
      this.StartedAction = startedAction;
      this.StoppedAction = stoppedAction;
      this.SuspendedAction = suspendedAction;
      this.ResumedAction = resumedAction;
      this.CompletedAction = completedAction;
    }

    public bool Pending => this.SourceQueue.Count != 0;

    public Queue<T> Source => this.SourceQueue;

    public bool IsTrickling => this.DispatcherTimer.IsEnabled;

    public void Start(
      int trickleDelay,
      IEnumerable<T> sourceCollection,
      IList<T> destinationCollection)
    {
      sourceCollection.ToList<T>().ForEach((Action<T>) (i => this.SourceQueue.Enqueue(i)));
      if (this.DispatcherTimer.IsEnabled)
        return;
      this.Count = 0;
      this.Destination = destinationCollection;
      this.Delay = TimeSpan.FromMilliseconds((double) trickleDelay);
      this.DispatcherTimer.Start();
      this.StartedAction();
    }

    public void Stop()
    {
      if (!this.DispatcherTimer.IsEnabled)
        return;
      this.DispatcherTimer.Stop();
      this.Count = 0;
      this.SourceQueue.Clear();
      this.StoppedAction();
    }

    public void Suspend()
    {
      if (!this.DispatcherTimer.IsEnabled)
        return;
      this.DispatcherTimer.Stop();
      this.SuspendedAction();
    }

    public void Resume()
    {
      if (this.DispatcherTimer.IsEnabled)
        return;
      this.DispatcherTimer.Start();
      this.ResumedAction();
    }
  }
}
