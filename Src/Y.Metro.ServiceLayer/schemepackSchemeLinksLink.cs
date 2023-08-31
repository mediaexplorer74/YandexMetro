// Decompiled with JetBrains decompiler
// Type: schemepackSchemeLinksLink
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(AnonymousType = true)]
public class schemepackSchemeLinksLink
{
  private string typeField;
  private schemepackSchemeLinksLinkWeightsWeight[] weightsField;
  private schemepackSchemeLinksLinkCustomDrawCustomDrawElement[] customDrawField;
  private int fromStationField;
  private int toStationField;

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }

  [XmlArrayItem("weight", typeof (schemepackSchemeLinksLinkWeightsWeight), Form = XmlSchemaForm.Unqualified)]
  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeLinksLinkWeightsWeight[] weights
  {
    get => this.weightsField;
    set => this.weightsField = value;
  }

  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  [XmlArrayItem("customDrawElement", typeof (schemepackSchemeLinksLinkCustomDrawCustomDrawElement), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
  public schemepackSchemeLinksLinkCustomDrawCustomDrawElement[] customDraw
  {
    get => this.customDrawField;
    set => this.customDrawField = value;
  }

  [XmlAttribute]
  public int fromStation
  {
    get => this.fromStationField;
    set => this.fromStationField = value;
  }

  [XmlAttribute]
  public int toStation
  {
    get => this.toStationField;
    set => this.toStationField = value;
  }
}
