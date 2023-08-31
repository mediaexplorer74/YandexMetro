// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JContainer
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Linq
{
  public abstract class JContainer : 
    JToken,
    IList<JToken>,
    ICollection<JToken>,
    IEnumerable<JToken>,
    IList,
    ICollection,
    IEnumerable,
    INotifyCollectionChanged
  {
    private object _syncRoot;
    private bool _busy;

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    protected abstract IList<JToken> ChildrenTokens { get; }

    internal JContainer()
    {
    }

    internal JContainer(JContainer other)
    {
      ValidationUtils.ArgumentNotNull((object) other, "c");
      foreach (object content in (IEnumerable<JToken>) other)
        this.Add(content);
    }

    internal void CheckReentrancy()
    {
      if (this._busy)
        throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
      if (collectionChanged == null)
        return;
      this._busy = true;
      try
      {
        collectionChanged((object) this, e);
      }
      finally
      {
        this._busy = false;
      }
    }

    public override bool HasValues => this.ChildrenTokens.Count > 0;

    internal bool ContentsEqual(JContainer container)
    {
      JToken jtoken = this.First;
      JToken node = container.First;
      if (jtoken == node)
        return true;
      for (; jtoken != null || node != null; node = node != container.Last ? node.Next : (JToken) null)
      {
        if (jtoken == null || node == null || !jtoken.DeepEquals(node))
          return false;
        jtoken = jtoken != this.Last ? jtoken.Next : (JToken) null;
      }
      return true;
    }

    public override JToken First => this.ChildrenTokens.FirstOrDefault<JToken>();

    public override JToken Last => this.ChildrenTokens.LastOrDefault<JToken>();

    public override JEnumerable<JToken> Children() => new JEnumerable<JToken>((IEnumerable<JToken>) this.ChildrenTokens);

    public override IEnumerable<T> Values<T>() => this.ChildrenTokens.Convert<JToken, T>();

    public IEnumerable<JToken> Descendants()
    {
      foreach (JToken o in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        yield return o;
        if (o is JContainer c)
        {
          foreach (JToken d in c.Descendants())
            yield return d;
        }
      }
    }

    internal bool IsMultiContent(object content) => content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);

    internal JToken EnsureParentToken(JToken item)
    {
      if (item == null)
        return (JToken) new JValue((object) null);
      if (item.Parent != null)
      {
        item = item.CloneToken();
      }
      else
      {
        JContainer jcontainer = this;
        while (jcontainer.Parent != null)
          jcontainer = jcontainer.Parent;
        if (item == jcontainer)
          item = item.CloneToken();
      }
      return item;
    }

    internal int IndexOfItem(JToken item) => this.ChildrenTokens.IndexOf<JToken>(item, (IEqualityComparer<JToken>) JContainer.JTokenReferenceEqualityComparer.Instance);

    internal virtual void InsertItem(int index, JToken item)
    {
      if (index > this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index must be within the bounds of the List.");
      this.CheckReentrancy();
      item = this.EnsureParentToken(item);
      JToken childrenToken1 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken childrenToken2 = index == this.ChildrenTokens.Count ? (JToken) null : this.ChildrenTokens[index];
      this.ValidateToken(item, (JToken) null);
      item.Parent = this;
      item.Previous = childrenToken1;
      if (childrenToken1 != null)
        childrenToken1.Next = item;
      item.Next = childrenToken2;
      if (childrenToken2 != null)
        childrenToken2.Previous = item;
      this.ChildrenTokens.Insert(index, item);
      if (this.CollectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) item, index));
    }

    internal virtual void RemoveItemAt(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      this.CheckReentrancy();
      JToken childrenToken1 = this.ChildrenTokens[index];
      JToken childrenToken2 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken childrenToken3 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      if (childrenToken2 != null)
        childrenToken2.Next = childrenToken3;
      if (childrenToken3 != null)
        childrenToken3.Previous = childrenToken2;
      childrenToken1.Parent = (JContainer) null;
      childrenToken1.Previous = (JToken) null;
      childrenToken1.Next = (JToken) null;
      this.ChildrenTokens.RemoveAt(index);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object) childrenToken1, index));
    }

    internal virtual bool RemoveItem(JToken item)
    {
      int index = this.IndexOfItem(item);
      if (index < 0)
        return false;
      this.RemoveItemAt(index);
      return true;
    }

    internal virtual JToken GetItem(int index) => this.ChildrenTokens[index];

    internal virtual void SetItem(int index, JToken item)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      JToken childrenToken1 = this.ChildrenTokens[index];
      if (JContainer.IsTokenUnchanged(childrenToken1, item))
        return;
      this.CheckReentrancy();
      item = this.EnsureParentToken(item);
      this.ValidateToken(item, childrenToken1);
      JToken childrenToken2 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken childrenToken3 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      item.Parent = this;
      item.Previous = childrenToken2;
      if (childrenToken2 != null)
        childrenToken2.Next = item;
      item.Next = childrenToken3;
      if (childrenToken3 != null)
        childrenToken3.Previous = item;
      this.ChildrenTokens[index] = item;
      childrenToken1.Parent = (JContainer) null;
      childrenToken1.Previous = (JToken) null;
      childrenToken1.Next = (JToken) null;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object) item, (object) childrenToken1, index));
    }

    internal virtual void ClearItems()
    {
      this.CheckReentrancy();
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        childrenToken.Parent = (JContainer) null;
        childrenToken.Previous = (JToken) null;
        childrenToken.Next = (JToken) null;
      }
      this.ChildrenTokens.Clear();
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    internal virtual void ReplaceItem(JToken existing, JToken replacement)
    {
      if (existing == null || existing.Parent != this)
        return;
      this.SetItem(this.IndexOfItem(existing), replacement);
    }

    internal virtual bool ContainsItem(JToken item) => this.IndexOfItem(item) != -1;

    internal virtual void CopyItemsTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        array.SetValue((object) childrenToken, arrayIndex + num);
        ++num;
      }
    }

    internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
    {
      if (!(currentValue is JValue jvalue))
        return false;
      return jvalue.Type == JTokenType.Null && newValue == null || jvalue.Equals((object) newValue);
    }

    internal virtual void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      if (o.Type == JTokenType.Property)
        throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
    }

    public virtual void Add(object content) => this.AddInternal(this.ChildrenTokens.Count, content);

    public void AddFirst(object content) => this.AddInternal(0, content);

    internal void AddInternal(int index, object content)
    {
      if (this.IsMultiContent(content))
      {
        IEnumerable enumerable = (IEnumerable) content;
        int index1 = index;
        foreach (object content1 in enumerable)
        {
          this.AddInternal(index1, content1);
          ++index1;
        }
      }
      else
      {
        JToken fromContent = this.CreateFromContent(content);
        this.InsertItem(index, fromContent);
      }
    }

    internal JToken CreateFromContent(object content) => content is JToken ? (JToken) content : (JToken) new JValue(content);

    public JsonWriter CreateWriter() => (JsonWriter) new JTokenWriter(this);

    public void ReplaceAll(object content)
    {
      this.ClearItems();
      this.Add(content);
    }

    public void RemoveAll() => this.ClearItems();

    internal void ReadTokenFrom(JsonReader r)
    {
      int depth = r.Depth;
      if (!r.Read())
        throw new Exception("Error reading {0} from JsonReader.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
      this.ReadContentFrom(r);
      if (r.Depth > depth)
        throw new Exception("Unexpected end of content while loading {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
    }

    internal void ReadContentFrom(JsonReader r)
    {
      ValidationUtils.ArgumentNotNull((object) r, nameof (r));
      IJsonLineInfo lineInfo = r as IJsonLineInfo;
      JContainer jcontainer = this;
      do
      {
        if (jcontainer is JProperty && ((JProperty) jcontainer).Value != null)
        {
          if (jcontainer == this)
            break;
          jcontainer = jcontainer.Parent;
        }
        switch (r.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            JObject content1 = new JObject();
            content1.SetLineInfo(lineInfo);
            jcontainer.Add((object) content1);
            jcontainer = (JContainer) content1;
            goto case JsonToken.None;
          case JsonToken.StartArray:
            JArray content2 = new JArray();
            content2.SetLineInfo(lineInfo);
            jcontainer.Add((object) content2);
            jcontainer = (JContainer) content2;
            goto case JsonToken.None;
          case JsonToken.StartConstructor:
            JConstructor jconstructor = new JConstructor(r.Value.ToString());
            jconstructor.SetLineInfo((IJsonLineInfo) jconstructor);
            jcontainer.Add((object) jconstructor);
            jcontainer = (JContainer) jconstructor;
            goto case JsonToken.None;
          case JsonToken.PropertyName:
            string name = r.Value.ToString();
            JProperty content3 = new JProperty(name);
            content3.SetLineInfo(lineInfo);
            JProperty jproperty = ((JObject) jcontainer).Property(name);
            if (jproperty == null)
              jcontainer.Add((object) content3);
            else
              jproperty.Replace((JToken) content3);
            jcontainer = (JContainer) content3;
            goto case JsonToken.None;
          case JsonToken.Comment:
            JValue comment = JValue.CreateComment(r.Value.ToString());
            comment.SetLineInfo(lineInfo);
            jcontainer.Add((object) comment);
            goto case JsonToken.None;
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.String:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            JValue content4 = new JValue(r.Value);
            content4.SetLineInfo(lineInfo);
            jcontainer.Add((object) content4);
            goto case JsonToken.None;
          case JsonToken.Null:
            JValue content5 = new JValue((object) null, JTokenType.Null);
            content5.SetLineInfo(lineInfo);
            jcontainer.Add((object) content5);
            goto case JsonToken.None;
          case JsonToken.Undefined:
            JValue content6 = new JValue((object) null, JTokenType.Undefined);
            content6.SetLineInfo(lineInfo);
            jcontainer.Add((object) content6);
            goto case JsonToken.None;
          case JsonToken.EndObject:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndArray:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndConstructor:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          default:
            throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) r.TokenType));
        }
      }
      while (r.Read());
    }

    internal int ContentsHashCode()
    {
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
        num ^= childrenToken.GetDeepHashCode();
      return num;
    }

    int IList<JToken>.IndexOf(JToken item) => this.IndexOfItem(item);

    void IList<JToken>.Insert(int index, JToken item) => this.InsertItem(index, item);

    void IList<JToken>.RemoveAt(int index) => this.RemoveItemAt(index);

    JToken IList<JToken>.this[int index]
    {
      get => this.GetItem(index);
      set => this.SetItem(index, value);
    }

    void ICollection<JToken>.Add(JToken item) => this.Add((object) item);

    void ICollection<JToken>.Clear() => this.ClearItems();

    bool ICollection<JToken>.Contains(JToken item) => this.ContainsItem(item);

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex) => this.CopyItemsTo((Array) array, arrayIndex);

    bool ICollection<JToken>.IsReadOnly => false;

    bool ICollection<JToken>.Remove(JToken item) => this.RemoveItem(item);

    private JToken EnsureValue(object value)
    {
      if (value == null)
        return (JToken) null;
      return value is JToken ? (JToken) value : throw new ArgumentException("Argument is not a JToken.");
    }

    int IList.Add(object value)
    {
      this.Add((object) this.EnsureValue(value));
      return this.Count - 1;
    }

    void IList.Clear() => this.ClearItems();

    bool IList.Contains(object value) => this.ContainsItem(this.EnsureValue(value));

    int IList.IndexOf(object value) => this.IndexOfItem(this.EnsureValue(value));

    void IList.Insert(int index, object value) => this.InsertItem(index, this.EnsureValue(value));

    bool IList.IsFixedSize => false;

    bool IList.IsReadOnly => false;

    void IList.Remove(object value) => this.RemoveItem(this.EnsureValue(value));

    void IList.RemoveAt(int index) => this.RemoveItemAt(index);

    object IList.this[int index]
    {
      get => (object) this.GetItem(index);
      set => this.SetItem(index, this.EnsureValue(value));
    }

    void ICollection.CopyTo(Array array, int index) => this.CopyItemsTo(array, index);

    public int Count => this.ChildrenTokens.Count;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
    {
      public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();

      public bool Equals(JToken x, JToken y) => object.ReferenceEquals((object) x, (object) y);

      public int GetHashCode(JToken obj) => obj == null ? 0 : obj.GetHashCode();
    }
  }
}
