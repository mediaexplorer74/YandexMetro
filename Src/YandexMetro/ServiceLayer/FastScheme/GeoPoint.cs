// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.GeoPoint
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Runtime.Serialization;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class GeoPoint
  {
    [DataMember]
    public double Lat;
    [DataMember]
    public double Lon;

    public GeoPoint(double lat, double lon)
    {
      this.Lat = lat;
      this.Lon = lon;
    }
  }
}
