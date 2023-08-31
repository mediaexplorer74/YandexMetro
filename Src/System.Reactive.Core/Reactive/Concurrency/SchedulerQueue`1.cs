// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerQueue`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Concurrency
{
  public class SchedulerQueue<TAbsolute> where TAbsolute : IComparable<TAbsolute>
  {
    private readonly PriorityQueue<ScheduledItem<TAbsolute>> _queue;

    public SchedulerQueue()
      : this(1024)
    {
    }

    public SchedulerQueue(int capacity) => this._queue = capacity >= 0 ? new PriorityQueue<ScheduledItem<TAbsolute>>(capacity) : throw new ArgumentOutOfRangeException(nameof (capacity));

    public int Count => this._queue.Count;

    public void Enqueue(ScheduledItem<TAbsolute> scheduledItem) => this._queue.Enqueue(scheduledItem);

    public bool Remove(ScheduledItem<TAbsolute> scheduledItem) => this._queue.Remove(scheduledItem);

    public ScheduledItem<TAbsolute> Dequeue() => this._queue.Dequeue();

    public ScheduledItem<TAbsolute> Peek() => this._queue.Peek();
  }
}
