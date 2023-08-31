// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ObservableList`1
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Yandex.Collections
{
  public class ObservableList<T> : 
    IList<T>,
    ICollection<T>,
    IObservableEnumerable<T>,
    INotifyCollectionChanged,
    IEnumerable<T>,
    IEnumerable
  {
    private readonly List<T> _list;

    public ObservableList() => this._list = new List<T>();

    public ObservableList(int capacity) => this._list = new List<T>(capacity);

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, e);
    }

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void AddRange(IEnumerable<T> collection)
    {
      int count = this._list.Count;
      foreach (T changedItem in collection)
      {
        this._list.Add(changedItem);
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) changedItem, count++));
      }
    }

    public void Add(T item)
    {
      this._list.Add(item);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) item, this._list.Count - 1));
    }

    public void Clear()
    {
      this._list.Clear();
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(T item) => this._list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this._list.CopyTo(array, arrayIndex);

    public bool Remove(T item)
    {
      bool flag = this._list.Remove(item);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      return flag;
    }

    public int Count => this._list.Count;

    public bool IsReadOnly => false;

    public int IndexOf(T item) => this._list.IndexOf(item);

    public void Insert(int index, T item)
    {
      this._list.Insert(index, item);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) item, index));
    }

    public void RemoveAt(int index)
    {
      this._list.RemoveAt(index);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object) null, index));
    }

    public T this[int index]
    {
      get => this._list[index];
      set
      {
        this._list[index] = value;
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object) value, (object) null, index));
      }
    }
  }
}
