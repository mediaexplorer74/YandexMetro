// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.GroupEdge
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;
using System.Runtime.Serialization;
using Yandex.Metro.Logic.FastScheme;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class GroupEdge
  {
    [DataMember]
    public PointWeights Weight;
    [DataMember]
    public List<int> SubNodes;
    [DataMember]
    public int Hash;
    public bool IsFake;

    public GroupEdge(int a, int b) => this.Hash = IntPair.Hash(a, b);
  }
}
