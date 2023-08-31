// Decompiled with JetBrains decompiler
// Type: schemepackSchemeOptions
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class schemepackSchemeOptions
{
  private string nameField;
  private schemepackSchemeOptionsSize[] sizeField;
  private schemepackSchemeOptionsWorkTime[] workTimeField;
  private schemepackSchemeOptionsDefaultRegion[] defaultRegionField;
  private schemepackSchemeOptionsInitialPosition[] initialPositionField;

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string name
  {
    get => this.nameField;
    set => this.nameField = value;
  }

  [XmlElement("workTime", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeOptionsWorkTime[] workTime
  {
    get => this.workTimeField;
    set => this.workTimeField = value;
  }

  [XmlElement("size", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeOptionsSize[] size
  {
    get => this.sizeField;
    set => this.sizeField = value;
  }

  [XmlElement("defaultRegion", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeOptionsDefaultRegion[] defaultRegion
  {
    get => this.defaultRegionField;
    set => this.defaultRegionField = value;
  }

  [XmlElement("initialPosition", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeOptionsInitialPosition[] initialPosition
  {
    get => this.initialPositionField;
    set => this.initialPositionField = value;
  }
}
