// Decompiled with JetBrains decompiler
// Type: boardPositions
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[DebuggerStepThrough]
[XmlRoot(Namespace = "", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(AnonymousType = true)]
public class boardPositions
{
  private boardPositionsPos[] posField;
  private string toStationField;
  private string nextStationField;
  private string prevStationField;

  [XmlElement("pos", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
  public boardPositionsPos[] pos
  {
    get => this.posField;
    set => this.posField = value;
  }

  [XmlAttribute]
  public string toStation
  {
    get => this.toStationField;
    set => this.toStationField = value;
  }

  [XmlAttribute]
  public string nextStation
  {
    get => this.nextStationField;
    set => this.nextStationField = value;
  }

  [XmlAttribute]
  public string prevStation
  {
    get => this.prevStationField;
    set => this.prevStationField = value;
  }
}
