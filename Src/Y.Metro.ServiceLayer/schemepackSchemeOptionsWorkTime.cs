﻿// Decompiled with JetBrains decompiler
// Type: schemepackSchemeOptionsWorkTime
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

[XmlType(AnonymousType = true)]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class schemepackSchemeOptionsWorkTime
{
  private string openField;
  private string closeField;

  [XmlAttribute]
  public string open
  {
    get => this.openField;
    set => this.openField = value;
  }

  [XmlAttribute]
  public string close
  {
    get => this.closeField;
    set => this.closeField = value;
  }
}
