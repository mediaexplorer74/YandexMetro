// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ObservableDictionary`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Yandex.Collections
{
  internal class ObservableDictionary<TKey, TValue> : 
    Dictionary<TKey, TValue>,
    IObservableEnumerable<TValue>,
    INotifyCollectionChanged,
    IEnumerable<TValue>,
    IEnumerable
  {
    public new bool ContainsKey(TKey key) => base.ContainsKey(key);

    public new void Add(TKey key, TValue value)
    {
      base.Add(key, value);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) value, this.Count - 1));
    }

    public new bool Remove(TKey key)
    {
      bool flag = base.Remove(key);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      return flag;
    }

    public new TValue this[TKey key]
    {
      get => base[key];
      set
      {
        base[key] = value;
        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
      }
    }

    public new void Clear()
    {
      base.Clear();
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      collectionChanged((object) this, e);
    }

    public IEnumerator<TValue> GetEnumerator() => (IEnumerator<TValue>) this.Values.GetEnumerator();
  }
}
