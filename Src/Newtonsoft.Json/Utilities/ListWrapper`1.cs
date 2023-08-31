// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ListWrapper`1
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  internal class ListWrapper<T> : 
    CollectionWrapper<T>,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IWrappedList,
    IList,
    ICollection,
    IEnumerable
  {
    private readonly IList<T> _genericList;

    public ListWrapper(IList list)
      : base(list)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      if (!(list is IList<T>))
        return;
      this._genericList = (IList<T>) list;
    }

    public ListWrapper(IList<T> list)
      : base((ICollection<T>) list)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      this._genericList = list;
    }

    public int IndexOf(T item) => this._genericList != null ? this._genericList.IndexOf(item) : ((IList) this).IndexOf((object) item);

    public void Insert(int index, T item)
    {
      if (this._genericList != null)
        this._genericList.Insert(index, item);
      else
        ((IList) this).Insert(index, (object) item);
    }

    public void RemoveAt(int index)
    {
      if (this._genericList != null)
        this._genericList.RemoveAt(index);
      else
        ((IList) this).RemoveAt(index);
    }

    public T this[int index]
    {
      get => this._genericList != null ? this._genericList[index] : (T) ((IList) this)[index];
      set
      {
        if (this._genericList != null)
          this._genericList[index] = value;
        else
          ((IList) this)[index] = (object) value;
      }
    }

    public override void Add(T item)
    {
      if (this._genericList != null)
        this._genericList.Add(item);
      else
        base.Add(item);
    }

    public override void Clear()
    {
      if (this._genericList != null)
        this._genericList.Clear();
      else
        base.Clear();
    }

    public override bool Contains(T item) => this._genericList != null ? this._genericList.Contains(item) : base.Contains(item);

    public override void CopyTo(T[] array, int arrayIndex)
    {
      if (this._genericList != null)
        this._genericList.CopyTo(array, arrayIndex);
      else
        base.CopyTo(array, arrayIndex);
    }

    public override int Count => this._genericList != null ? this._genericList.Count : base.Count;

    public override bool IsReadOnly => this._genericList != null ? this._genericList.IsReadOnly : base.IsReadOnly;

    public override bool Remove(T item)
    {
      if (this._genericList != null)
        return this._genericList.Remove(item);
      bool flag = base.Contains(item);
      if (flag)
        base.Remove(item);
      return flag;
    }

    public override IEnumerator<T> GetEnumerator() => this._genericList != null ? this._genericList.GetEnumerator() : base.GetEnumerator();

    public object UnderlyingList => this._genericList != null ? (object) this._genericList : this.UnderlyingCollection;
  }
}
