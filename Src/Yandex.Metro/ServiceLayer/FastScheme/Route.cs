// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.Route
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;

namespace Y.Metro.ServiceLayer.FastScheme
{
  public class Route : BaseObject
  {
    private int _estimatedDuration;

    public MetroStation StartStation { get; set; }

    public MetroStation EndStation { get; set; }

    public MetroScheme RouteScheme { get; set; }

    public List<MetroStation> SortStations { get; set; }

    public int RouteNumber { get; set; }

    public List<int> Timings { get; set; }

    public int EstimatedDuration
    {
      get => this._estimatedDuration;
      set
      {
        this._estimatedDuration = value;
        this.RaisePropertyChanged(nameof (EstimatedDuration));
      }
    }
  }
}
