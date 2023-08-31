// Decompiled with JetBrains decompiler
// Type: schemepackSchemeStationsStationName
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
public class schemepackSchemeStationsStationName
{
  private string textField;
  private string alignmentField;
  private schemepackSchemeStationsStationNameTextLinesTextLine[] textLinesField;
  private schemepackSchemeStationsStationNameCustomSchemePosition customSchemePositionField;
  private string sameAsForStationField;

  [XmlIgnore]
  public string Title { get; set; }

  [XmlIgnore]
  public string Name => this.text;

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string text
  {
    get => this.textField;
    set => this.textField = value;
  }

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string alignment
  {
    get => this.alignmentField;
    set => this.alignmentField = value;
  }

  [XmlArrayItem("textLine", typeof (schemepackSchemeStationsStationNameTextLinesTextLine), Form = XmlSchemaForm.Unqualified)]
  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationNameTextLinesTextLine[] textLines
  {
    get => this.textLinesField;
    set => this.textLinesField = value;
  }

  [XmlElement("customSchemePosition", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationNameCustomSchemePosition customSchemePosition
  {
    get => this.customSchemePositionField;
    set => this.customSchemePositionField = value;
  }

  [XmlAttribute]
  public string sameAsForStation
  {
    get => this.sameAsForStationField;
    set => this.sameAsForStationField = value;
  }
}
