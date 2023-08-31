// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Styles.JamZoom
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Yandex.Maps.Traffic.DTO.Styles
{
  [XmlRoot("zoom")]
  [ComVisible(false)]
  public class JamZoom
  {
    private const string from = "from";
    private const string to = "to";
    private const string line_color = "line_color";
    private const string line_width = "line_width";
    private const string arrows = "arrows";

    [XmlAttribute("from")]
    public int From { get; set; }

    [XmlAttribute("to")]
    public int To { get; set; }

    [XmlIgnore]
    public Color LineColor { get; set; }

    [XmlIgnore]
    public int LineColorInt { get; set; }

    [XmlElement("line_color")]
    public string LineColorArgb
    {
      get => this.LineColor.ToString().Replace("#", "");
      set
      {
        uint result = 0;
        if (!uint.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          return;
        this.LineColorInt = (int) result;
        this.LineColor = Color.FromArgb((byte) (result >> 24), (byte) (result << 8 >> 24), (byte) (result << 16 >> 24), (byte) (result << 24 >> 24));
      }
    }

    [XmlElement("line_width")]
    public double LineWidth { get; set; }

    [XmlElement("arrows", IsNullable = true)]
    public JamArrow Arrows { get; set; }
  }
}
