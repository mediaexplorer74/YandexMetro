// Decompiled with JetBrains decompiler
// Type: schemepack
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[XmlRoot("scheme-pack", Namespace = "", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class schemepack
{
  private schemepackScheme[] schemeField;
  private int idField;
  private string verField;

  [XmlElement("scheme", Form = XmlSchemaForm.Unqualified)]
  public schemepackScheme[] scheme
  {
    get => this.schemeField;
    set => this.schemeField = value;
  }

  [XmlAttribute]
  public int id
  {
    get => this.idField;
    set => this.idField = value;
  }

  [XmlAttribute]
  public string ver
  {
    get => this.verField;
    set => this.verField = value;
  }
}
