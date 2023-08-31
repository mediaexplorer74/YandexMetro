// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Styles.JamStyle
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Serialization;

namespace Yandex.Maps.Traffic.DTO.Styles
{
  [XmlRoot("style")]
  [ComVisible(false)]
  public class JamStyle : IXmlSerializable, IJamStyle
  {
    private const string id = "id";
    private const string zorder = "zorder";

    public JamStyle() => this.Zooms = new List<JamZoom>();

    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("zorder")]
    public int Zorder { get; set; }

    public List<JamZoom> Zooms { get; set; }

    public XmlSchema GetSchema() => (XmlSchema) null;

    public void ReadXml(XmlReader reader)
    {
      this.Id = int.Parse(reader.GetAttribute("id"));
      this.Zorder = int.Parse(reader.GetAttribute("zorder"));
      this.Zooms.Clear();
      GenericXmlSerializer<JamZoom> genericXmlSerializer = new GenericXmlSerializer<JamZoom>();
      reader.Read();
      while (reader.NodeType == XmlNodeType.Element)
        this.Zooms.Add(genericXmlSerializer.Deserialize(reader));
      if (reader.NodeType != XmlNodeType.EndElement)
        return;
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add("", "");
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (JamZoom));
      writer.WriteAttributeString("id", this.Id.ToString());
      writer.WriteAttributeString("zorder", this.Zorder.ToString());
      foreach (JamZoom zoom in this.Zooms)
        xmlSerializer.Serialize(writer, (object) zoom, namespaces);
    }
  }
}
