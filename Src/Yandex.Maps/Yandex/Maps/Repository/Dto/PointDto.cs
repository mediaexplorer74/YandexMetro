﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Dto.PointDto
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Repository.Dto
{
  [ComVisible(true)]
  public class PointDto
  {
    [XmlElement("pos", Namespace = "http://www.opengis.net/gml")]
    public string Pos { get; set; }
  }
}
