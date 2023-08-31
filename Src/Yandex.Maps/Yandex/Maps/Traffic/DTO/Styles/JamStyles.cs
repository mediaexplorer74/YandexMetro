// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Styles.JamStyles
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Serialization;

namespace Yandex.Maps.Traffic.DTO.Styles
{
  [XmlRoot("tjam_styles")]
  [ComVisible(false)]
  public class JamStyles : 
    Dictionary<int, IJamStyle>,
    IXmlSerializable,
    IJamStyles,
    IDictionary<int, IJamStyle>,
    ICollection<KeyValuePair<int, IJamStyle>>,
    IEnumerable<KeyValuePair<int, IJamStyle>>,
    IEnumerable
  {
    public XmlSchema GetSchema() => (XmlSchema) null;

    public void ReadXml(XmlReader reader)
    {
      this.Clear();
      GenericXmlSerializer<JamStyle> genericXmlSerializer = new GenericXmlSerializer<JamStyle>();
      reader.Read();
      while (reader.NodeType == XmlNodeType.Element)
      {
        JamStyle jamStyle = genericXmlSerializer.Deserialize(reader);
        if (jamStyle != null)
          this[jamStyle.Id] = (IJamStyle) jamStyle;
      }
      if (reader.NodeType != XmlNodeType.EndElement)
        return;
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add("", "");
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (JamStyle));
      foreach (KeyValuePair<int, IJamStyle> keyValuePair in (Dictionary<int, IJamStyle>) this)
        xmlSerializer.Serialize(writer, (object) keyValuePair.Value, namespaces);
    }
  }
}
