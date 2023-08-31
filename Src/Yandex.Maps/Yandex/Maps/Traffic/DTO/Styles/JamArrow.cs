// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Styles.JamArrow
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Yandex.Maps.Traffic.DTO.Styles
{
  [XmlRoot("arrow")]
  [ComVisible(false)]
  public class JamArrow
  {
    private const string color = "color";
    private const string dash = "dash";
    private const string space = "space";
    private const string thickness = "thickness";
    private const string arrow_height = "arrow_height";
    private const string arrow_length = "arrow_length";

    [XmlIgnore]
    public Color Color { get; set; }

    [XmlIgnore]
    public int ColorInt { get; set; }

    [XmlElement("color")]
    public string ColorArgb
    {
      get => this.Color.ToString().Replace("#", "");
      set
      {
        uint num = uint.Parse(value, NumberStyles.HexNumber);
        this.ColorInt = (int) num;
        this.Color = Color.FromArgb((byte) (num >> 24), (byte) (num << 8 >> 24), (byte) (num << 16 >> 24), (byte) (num << 24 >> 24));
      }
    }

    [XmlElement("dash")]
    public double Dash { get; set; }

    [XmlElement("space")]
    public double Space { get; set; }

    [XmlElement("thickness")]
    public double Thickness { get; set; }

    [XmlElement("arrow_height")]
    public double ArrowHeight { get; set; }

    [XmlElement("arrow_length")]
    public double ArrowLength { get; set; }
  }
}
