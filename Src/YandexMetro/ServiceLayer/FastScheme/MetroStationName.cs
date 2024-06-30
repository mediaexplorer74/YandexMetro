// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.MetroStationName
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Runtime.Serialization;
using System.Windows;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class MetroStationName
  {
    [DataMember]
    public NameAlignment Alignment;
    [DataMember]
    public string[] TextLines;
    [DataMember]
    public Point? CustomPosition;

    [DataMember]
    public string Text { get; set; }

    public MetroStationName Copy()
    {
      MetroStationName metroStationName = new MetroStationName()
      {
        Alignment = this.Alignment,
        Text = this.Text
      };
      if (this.TextLines != null)
      {
        int length = this.TextLines.Length;
        metroStationName.TextLines = new string[length];
        Array.Copy((Array) this.TextLines, (Array) metroStationName.TextLines, length);
      }
      if (this.CustomPosition.HasValue)
      {
        Point point = this.CustomPosition.Value;
        metroStationName.CustomPosition = new Point?(new Point(point.X, point.Y));
      }
      return metroStationName;
    }
  }
}
