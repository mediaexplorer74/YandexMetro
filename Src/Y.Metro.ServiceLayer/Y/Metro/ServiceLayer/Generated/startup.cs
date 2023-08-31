// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.Generated.startup
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Y.Metro.ServiceLayer.Generated
{
  [GeneratedCode("xsd", "2.0.50727.3038")]
  [XmlRoot(Namespace = "", IsNullable = false)]
  [DebuggerStepThrough]
  [XmlType(AnonymousType = true)]
  public class startup
  {
    private string uuidField;
    private startupSchemesScheme[] schemesField;

    [XmlElement(Form = XmlSchemaForm.Unqualified)]
    public string uuid
    {
      get => this.uuidField;
      set => this.uuidField = value;
    }

    [XmlArray(Form = XmlSchemaForm.Unqualified)]
    [XmlArrayItem("scheme", typeof (startupSchemesScheme), Form = XmlSchemaForm.Unqualified)]
    public startupSchemesScheme[] schemes
    {
      get => this.schemesField;
      set => this.schemesField = value;
    }
  }
}
