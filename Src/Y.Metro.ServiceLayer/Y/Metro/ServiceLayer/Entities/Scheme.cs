// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.Entities.Scheme
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Xml.Serialization;
using Y.Common;
using Y.Metro.ServiceLayer.Enums;

namespace Y.Metro.ServiceLayer.Entities
{
  public class Scheme
  {
    public int Id { get; set; }

    public string VersionId { get; set; }

    public bool IsProdVersion { get; set; }

    public List<LanguageCode> LanguageCodes { get; set; }

    public string Coordinate { get; set; }

    public SchemeMatrix Matrix { get; set; }

    [XmlIgnore]
    public SchemeType Type => (SchemeType) this.Id;

    [XmlIgnore]
    public GeoCoordinate GeoCoordinate
    {
      get
      {
        string[] strArray = this.Coordinate.Split(';');
        return new GeoCoordinate(double.Parse(strArray[0], (IFormatProvider) CultureHelper.EnUs), double.Parse(strArray[1], (IFormatProvider) CultureHelper.EnUs));
      }
    }
  }
}
