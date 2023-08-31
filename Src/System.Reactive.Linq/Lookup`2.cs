// Decompiled with JetBrains decompiler
// Type: System.Reactive.Lookup`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive
{
  internal class Lookup<K, E> : ILookup<K, E>, IEnumerable<IGrouping<K, E>>, IEnumerable
  {
    private Dictionary<K, List<E>> d;

    public Lookup(IEqualityComparer<K> comparer) => this.d = new Dictionary<K, List<E>>(comparer);

    public void Add(K key, E element)
    {
      List<E> eList = (List<E>) null;
      if (!this.d.TryGetValue(key, out eList))
        this.d[key] = eList = new List<E>();
      eList.Add(element);
    }

    public bool Contains(K key) => this.d.ContainsKey(key);

    public int Count => this.d.Count;

    public IEnumerable<E> this[K key] => this.Hide(this.d[key]);

    private IEnumerable<E> Hide(List<E> elements)
    {
      foreach (E x in elements)
        yield return x;
    }

    public IEnumerator<IGrouping<K, E>> GetEnumerator()
    {
      foreach (KeyValuePair<K, List<E>> kv in this.d)
        yield return (IGrouping<K, E>) new Lookup<K, E>.Grouping(kv);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private class Grouping : IGrouping<K, E>, IEnumerable<E>, IEnumerable
    {
      private KeyValuePair<K, List<E>> kv;

      public Grouping(KeyValuePair<K, List<E>> kv) => this.kv = kv;

      public K Key => this.kv.Key;

      public IEnumerator<E> GetEnumerator() => (IEnumerator<E>) this.kv.Value.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
