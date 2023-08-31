// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.Tiles.TileRequest
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Maps.API;
using Yandex.Maps.API.Adapters;
using Yandex.Maps.API.Interfaces;
using Yandex.StringUtils;

namespace Yandex.Maps.PrinterClient.Tiles
{
  [XmlRoot("tile")]
  [ComVisible(true)]
  public class TileRequest : TileInfo
  {
    private string _layerText;
    private BaseLayers _layer;

    public TileRequest()
      : base(0, 0, (byte) 0, BaseLayers.none)
    {
    }

    public TileRequest(ITileInfo ti)
      : base(ti)
    {
      this.Checksum = ti.Checksum;
      this.MapVersion = ti.MapVersion;
      this.UpdateLayerText(ti.Layer);
    }

    [XmlAttribute("layer")]
    public string LayerText
    {
      get => this._layerText;
      set
      {
        this._layerText = value;
        this.Layer = LayerAdapter.StringToLayer(value);
      }
    }

    private void UpdateLayerText(BaseLayers value)
    {
      this._layerText = LayerAdapter.LayerToString(value);
      this._layer = value;
    }

    [XmlAttribute("size")]
    public int Size { get; set; }

    [XmlAttribute("checksum")]
    public string HexChecksum
    {
      get => this.Checksum == null ? (string) null : HexString.HexStr(this.Checksum);
      set
      {
      }
    }
  }
}
