// Decompiled with JetBrains decompiler
// Type: schemepackScheme
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
public class schemepackScheme
{
  private schemepackSchemeOptions[] optionsField;
  private schemepackSchemeLinesLine[] linesField;
  private schemepackSchemeStationsStation[] stationsField;
  private schemepackSchemeLinksLink[] linksField;
  private schemepackSchemeTransfersTransferStation[][] transfersField;
  private string localeField;
  private string typeField;

  [XmlElement("options", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeOptions[] options
  {
    get => this.optionsField;
    set => this.optionsField = value;
  }

  [XmlArrayItem("line", typeof (schemepackSchemeLinesLine), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeLinesLine[] lines
  {
    get => this.linesField;
    set => this.linesField = value;
  }

  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  [XmlArrayItem("station", typeof (schemepackSchemeStationsStation), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
  public schemepackSchemeStationsStation[] stations
  {
    get => this.stationsField;
    set => this.stationsField = value;
  }

  [XmlArrayItem("link", typeof (schemepackSchemeLinksLink), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeLinksLink[] links
  {
    get => this.linksField;
    set => this.linksField = value;
  }

  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  [XmlArrayItem("transfer", typeof (schemepackSchemeTransfersTransferStation[]), Form = XmlSchemaForm.Unqualified, IsNullable = false)]
  [XmlArrayItem("station", typeof (schemepackSchemeTransfersTransferStation), Form = XmlSchemaForm.Unqualified, NestingLevel = 1)]
  public schemepackSchemeTransfersTransferStation[][] transfers
  {
    get => this.transfersField;
    set => this.transfersField = value;
  }

  [XmlAttribute]
  public string locale
  {
    get => this.localeField;
    set => this.localeField = value;
  }

  [XmlAttribute]
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }
}
