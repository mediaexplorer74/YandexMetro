// Decompiled with JetBrains decompiler
// Type: System.Reactive.PriorityQueue`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Threading;

namespace System.Reactive
{
  internal class PriorityQueue<T> where T : IComparable<T>
  {
    private static int _count = int.MinValue;
    private PriorityQueue<T>.IndexedItem[] _items;
    private int _size;

    public PriorityQueue()
      : this(16)
    {
    }

    public PriorityQueue(int capacity)
    {
      this._items = new PriorityQueue<T>.IndexedItem[capacity];
      this._size = 0;
    }

    private bool IsHigherPriority(int left, int right) => this._items[left].CompareTo(this._items[right]) < 0;

    private void Percolate(int index)
    {
      if (index >= this._size || index < 0)
        return;
      int index1 = (index - 1) / 2;
      if (index1 < 0 || index1 == index || !this.IsHigherPriority(index, index1))
        return;
      PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
      this._items[index] = this._items[index1];
      this._items[index1] = indexedItem;
      this.Percolate(index1);
    }

    private void Heapify() => this.Heapify(0);

    private void Heapify(int index)
    {
      if (index >= this._size || index < 0)
        return;
      int left1 = 2 * index + 1;
      int left2 = 2 * index + 2;
      int index1 = index;
      if (left1 < this._size && this.IsHigherPriority(left1, index1))
        index1 = left1;
      if (left2 < this._size && this.IsHigherPriority(left2, index1))
        index1 = left2;
      if (index1 == index)
        return;
      PriorityQueue<T>.IndexedItem indexedItem = this._items[index];
      this._items[index] = this._items[index1];
      this._items[index1] = indexedItem;
      this.Heapify(index1);
    }

    public int Count => this._size;

    public T Peek()
    {
      if (this._size == 0)
        throw new InvalidOperationException(Strings_Core.HEAP_EMPTY);
      return this._items[0].Value;
    }

    private void RemoveAt(int index)
    {
      this._items[index] = this._items[--this._size];
      this._items[this._size] = new PriorityQueue<T>.IndexedItem();
      this.Heapify();
      if (this._size >= this._items.Length / 4)
        return;
      PriorityQueue<T>.IndexedItem[] items = this._items;
      this._items = new PriorityQueue<T>.IndexedItem[this._items.Length / 2];
      Array.Copy((Array) items, 0, (Array) this._items, 0, this._size);
    }

    public T Dequeue()
    {
      T obj = this.Peek();
      this.RemoveAt(0);
      return obj;
    }

    public void Enqueue(T item)
    {
      if (this._size >= this._items.Length)
      {
        PriorityQueue<T>.IndexedItem[] items = this._items;
        this._items = new PriorityQueue<T>.IndexedItem[this._items.Length * 2];
        Array.Copy((Array) items, (Array) this._items, items.Length);
      }
      int index = this._size++;
      this._items[index] = new PriorityQueue<T>.IndexedItem()
      {
        Value = item,
        Id = Interlocked.Increment(ref PriorityQueue<T>._count)
      };
      this.Percolate(index);
    }

    public bool Remove(T item)
    {
      for (int index = 0; index < this._size; ++index)
      {
        if (EqualityComparer<T>.Default.Equals(this._items[index].Value, item))
        {
          this.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    private struct IndexedItem : IComparable<PriorityQueue<T>.IndexedItem>
    {
      public T Value;
      public int Id;

      public int CompareTo(PriorityQueue<T>.IndexedItem other)
      {
        int num = this.Value.CompareTo(other.Value);
        if (num == 0)
          num = this.Id.CompareTo(other.Id);
        return num;
      }
    }
  }
}
