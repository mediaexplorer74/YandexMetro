﻿// Decompiled with JetBrains decompiler
// Type: schemepackSchemeOptionsInitialPosition
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class schemepackSchemeOptionsInitialPosition
{
  private string xField;
  private string yField;

  [XmlAttribute]
  public string x
  {
    get => this.xField;
    set => this.xField = value;
  }

  [XmlAttribute]
  public string y
  {
    get => this.yField;
    set => this.yField = value;
  }
}
