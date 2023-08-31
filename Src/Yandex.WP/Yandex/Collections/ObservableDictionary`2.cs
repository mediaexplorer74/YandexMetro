// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ObservableDictionary`2
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Yandex.Collections
{
  public class ObservableDictionary<TKey, TValue> : 
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
