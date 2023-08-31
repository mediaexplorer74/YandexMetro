// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheMetaData
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Repository.MapLoader
{
  [XmlRoot("cache")]
  [ComVisible(true)]
  public class CacheMetaData
  {
    [XmlAttribute("version")]
    public int Version { get; set; }

    [XmlAttribute("localVersion")]
    public int LocalCacheVersion { get; set; }

    [XmlElement("generator")]
    public CacheMetaDataGenerator Generator { get; set; }

    public CacheMetaData()
    {
      this.Version = 2;
      this.LocalCacheVersion = 130;
      this.Generator = new CacheMetaDataGenerator()
      {
        Platform = "wp"
      };
    }
  }
}
