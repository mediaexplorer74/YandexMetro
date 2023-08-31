// Decompiled with JetBrains decompiler
// Type: schemepackSchemeLinksLinkCustomDrawCustomDrawElement
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
public class schemepackSchemeLinksLinkCustomDrawCustomDrawElement
{
  private string typeField;
  private double xField;
  private double yField;
  private double radiusField;
  private double startAngleField;
  private double endAngleField;

  [XmlAttribute]
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }

  [XmlAttribute]
  public double x
  {
    get => this.xField;
    set => this.xField = value;
  }

  [XmlAttribute]
  public double y
  {
    get => this.yField;
    set => this.yField = value;
  }

  [XmlAttribute]
  public double radius
  {
    get => this.radiusField;
    set => this.radiusField = value;
  }

  [XmlAttribute]
  public double startAngle
  {
    get => this.startAngleField;
    set => this.startAngleField = value;
  }

  [XmlAttribute]
  public double endAngle
  {
    get => this.endAngleField;
    set => this.endAngleField = value;
  }
}
