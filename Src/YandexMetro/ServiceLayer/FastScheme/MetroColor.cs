// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.MetroColor
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Runtime.Serialization;
using System.Windows.Media;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public struct MetroColor
  {
    [DataMember]
    public byte R;
    [DataMember]
    public byte G;
    [DataMember]
    public byte B;

    public Color ToColor() => Color.FromArgb(byte.MaxValue, this.R, this.G, this.B);
  }
}
