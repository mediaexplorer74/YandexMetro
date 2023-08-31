// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Config.MapLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Maps.API;
using Yandex.Maps.API.Adapters;

namespace Yandex.Maps.Repository.Config
{
  [ComVisible(true)]
  public class MapLayer
  {
    private string _request;

    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("request")]
    public string Request
    {
      get => this._request;
      set
      {
        this._request = value;
        this.Layer = LayerAdapter.StringToLayer(this.Request);
      }
    }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("ver")]
    public int Version { get; set; }

    [XmlAttribute("service")]
    public int Service { get; set; }

    [XmlAttribute("tile_pixel_size")]
    public int TilePixelSize { get; set; }

    [XmlIgnore]
    public BaseLayers Layer { get; private set; }
  }
}
