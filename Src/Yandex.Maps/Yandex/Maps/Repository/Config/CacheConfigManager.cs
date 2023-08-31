// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Config.CacheConfigManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Yandex.Maps.API;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Repository.Adapters;
using Yandex.Serialization.Interfaces;

namespace Yandex.Maps.Repository.Config
{
  [ComVisible(true)]
  public class CacheConfigManager : ICacheConfigManager
  {
    private readonly IDataRepository<MapLayers> _cacheConfigDataRepository;
    private readonly IConfigMediator _config;

    public CacheConfigManager(
      [NotNull] IDataRepository<MapLayers> cacheConfigDataRepository,
      [NotNull] IConfigMediator config)
    {
      if (cacheConfigDataRepository == null)
        throw new ArgumentNullException(nameof (cacheConfigDataRepository));
      if (config == null)
        throw new ArgumentNullException(nameof (config));
      this._cacheConfigDataRepository = cacheConfigDataRepository;
      this._config = config;
    }

    public Dictionary<BaseLayers, MapLayer> SystemLayers { get; set; }

    public int GetLayerIdByType(BaseLayers layer) => this.SystemLayers[layer].Id;

    public BaseLayers GetLayerTypeById(int layerId) => this.SystemLayers.Values.Where<MapLayer>((Func<MapLayer, bool>) (l => l.Id == layerId)).Select<MapLayer, BaseLayers>((Func<MapLayer, BaseLayers>) (l => l.Layer)).FirstOrDefault<BaseLayers>();

    public void SaveConfig()
    {
      if (this.SystemLayers == null)
        return;
      this._cacheConfigDataRepository.Data.Layers = this.SystemLayers.Values.ToArray<MapLayer>();
      this._cacheConfigDataRepository.Save();
    }

    public void InitializeConfig()
    {
      IDictionary<BaseLayers, Yandex.Maps.Config.MapLayer> mapLayers = this._config.MapLayers;
      if (mapLayers != null && mapLayers.Any<KeyValuePair<BaseLayers, Yandex.Maps.Config.MapLayer>>())
      {
        this.SystemLayers = MapLayersAdapter.Convert((IEnumerable<Yandex.Maps.Config.MapLayer>) mapLayers.Values);
      }
      else
      {
        MapLayer[] layers = this._cacheConfigDataRepository.Data.Layers;
        if (layers != null)
          this.SystemLayers = ((IEnumerable<MapLayer>) layers).ToDictionary<MapLayer, BaseLayers, MapLayer>((Func<MapLayer, BaseLayers>) (item => item.Layer), (Func<MapLayer, MapLayer>) (item => item));
      }
      if (this.SystemLayers != null && this.SystemLayers.Any<KeyValuePair<BaseLayers, MapLayer>>())
        return;
      this.SystemLayers = new Dictionary<BaseLayers, MapLayer>()
      {
        {
          BaseLayers.map,
          new MapLayer()
          {
            Id = 1,
            Name = "Schema",
            Service = 0,
            Request = "map",
            TilePixelSize = 128,
            Version = 1
          }
        },
        {
          BaseLayers.sat | BaseLayers.skl,
          new MapLayer()
          {
            Id = 2,
            Name = "Satellite",
            Service = 0,
            Request = "sat,skl",
            TilePixelSize = 128,
            Version = 1
          }
        },
        {
          BaseLayers.pmap,
          new MapLayer()
          {
            Id = 3,
            Name = "People's map",
            Service = 0,
            Request = "pmap",
            TilePixelSize = 128,
            Version = 1
          }
        },
        {
          BaseLayers.meta,
          new MapLayer()
          {
            Id = 4,
            Name = "",
            Service = 1,
            Request = "meta",
            Version = 1
          }
        }
      };
    }
  }
}
