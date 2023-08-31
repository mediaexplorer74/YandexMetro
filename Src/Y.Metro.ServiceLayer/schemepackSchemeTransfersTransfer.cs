// Decompiled with JetBrains decompiler
// Type: schemepackSchemeTransfersTransfer
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
public class schemepackSchemeTransfersTransfer
{
  private schemepackSchemeTransfersTransferStation[] stationsField;

  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  [XmlArrayItem("station", typeof (schemepackSchemeTransfersTransferStation), Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeTransfersTransferStation[] station
  {
    get => this.stationsField;
    set => this.stationsField = value;
  }

  public override string ToString() => this.stationsField != null ? "T:" + string.Join<schemepackSchemeTransfersTransferStation>(",", (IEnumerable<schemepackSchemeTransfersTransferStation>) this.stationsField) : base.ToString();
}
