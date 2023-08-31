// Decompiled with JetBrains decompiler
// Type: schemepackSchemeOptionsDefaultRegion
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class schemepackSchemeOptionsDefaultRegion
{
  private string latitudeField;
  private string longitudeField;
  private string latitudeDeltaField;
  private string longitudeDeltaField;

  [XmlAttribute]
  public string latitude
  {
    get => this.latitudeField;
    set => this.latitudeField = value;
  }

  [XmlAttribute]
  public string longitude
  {
    get => this.longitudeField;
    set => this.longitudeField = value;
  }

  [XmlAttribute]
  public string latitudeDelta
  {
    get => this.latitudeDeltaField;
    set => this.latitudeDeltaField = value;
  }

  [XmlAttribute]
  public string longitudeDelta
  {
    get => this.longitudeDeltaField;
    set => this.longitudeDeltaField = value;
  }
}
