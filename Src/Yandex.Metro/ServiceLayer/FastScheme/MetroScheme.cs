// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.MetroScheme
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class MetroScheme
  {
    [DataMember]
    public int CityId;
    [DataMember]
    public string Language;
    [DataMember]
    public Dictionary<int, MetroStation> Stations;
    [DataMember]
    public MetroLine[] Lines;
    [DataMember]
    public string Version;
    [DataMember]
    public MetroLink[] Links;
    [DataMember]
    public int[][] Transfers;
    [DataMember]
    public Graph Graph;
    [DataMember]
    public Graph SuperGraph;
    [DataMember]
    public List<GroupEdge> RawSuperEdges;
    [DataMember]
    public double Width;
    [DataMember]
    public double Height;
    [DataMember]
    public WorkTime WorkTime;
    public List<KeyValuePair<int, int>> RemovedEdges = new List<KeyValuePair<int, int>>();
    public List<GroupEdge> RemovedSuperEdges = new List<GroupEdge>();
    public List<KeyValuePair<int, int>> AddedEdges = new List<KeyValuePair<int, int>>();
    public List<int> AddedNodes = new List<int>();
  }
}
