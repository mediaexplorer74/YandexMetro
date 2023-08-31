// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.Graph
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Yandex.Metro.Logic.FastScheme;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class Graph
  {
    [DataMember]
    public Dictionary<int, PointWeights> _data = new Dictionary<int, PointWeights>();
    [DataMember]
    public Dictionary<int, List<int>> AdjacentEdges;
    [DataMember]
    public List<MetroStation> Nodes;

    public PointWeights this[int index1, int index2]
    {
      get
      {
        int key = IntPair.Hash(index1, index2);
        return this._data.ContainsKey(key) ? this._data[key] : throw new ArgumentOutOfRangeException();
      }
      set => this._data[IntPair.Hash(index1, index2)] = value;
    }

    internal void Remove(int originalFrom, int originalTo) => this._data.Remove(IntPair.Hash(originalFrom, originalTo));

    internal void MakeAdjacentNodesUnique()
    {
      foreach (int key in this.AdjacentEdges.Keys.ToList<int>())
      {
        List<int> adjacentEdge = this.AdjacentEdges[key];
        this.AdjacentEdges[key] = adjacentEdge.Distinct<int>().ToList<int>();
      }
    }
  }
}
