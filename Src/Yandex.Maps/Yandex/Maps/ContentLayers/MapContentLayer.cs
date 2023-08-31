// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.MapContentLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Yandex.Collections;
using Yandex.Collections.Interfaces;
using Yandex.Ioc;
using Yandex.Maps.API;
using Yandex.Maps.API.Extensions;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Maps.Properties;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.TileWeighting;
using Yandex.Media;
using Yandex.Memory;
using Yandex.Threading;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public class MapContentLayer : 
    TileContentLayerBase<ITile>,
    IMapContentLayer,
    ITileContentLayer,
    IContentLayer,
    IContentContainer
  {
    private readonly IMemoryCache<ITileInfo, ITile> _unavailableTileCache;
    private readonly ITileWeightCalculator _tileWeightCalculator;
    private readonly IThreadPool _threadPool;
    private readonly IDisplayTileSize _displayTileSize;
    private readonly IDictionary<byte, ViewportRect> _savedExpandedViewPorts = (IDictionary<byte, ViewportRect>) new ConcurrentDictionary<byte, ViewportRect>();
    private readonly ITileManager<ITile> _tileManager;
    private readonly IDictionary<ITileInfo, bool> _tilesToLoad;
    private readonly object _tilesToLoadSync = new object();
    private readonly IMemoryPlanner _memoryPlanner;
    private BitmapSource _notFoundImage;
    private readonly object _unavailableTileLock = new object();

    public MapContentLayer()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<ITileZoomLayerBuilder<ITile>>(), IocSingleton<ControlsIocInitializer>.Resolve<IDictionary<byte, ITileZoomLayer<ITile>>>(), IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>(), IocSingleton<ControlsIocInitializer>.Resolve<IZoomLevelConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IViewportTileConverter>(), IocSingleton<ControlsIocInitializer>.Resolve<IQueue<ILayerCommand>>(), IocSingleton<ControlsIocInitializer>.Resolve<IThread>(), IocSingleton<ControlsIocInitializer>.Resolve<ITileManager<ITile>>(), IocSingleton<ControlsIocInitializer>.Resolve<IDictionary<ITileInfo, bool>>(), IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>(), IocSingleton<ControlsIocInitializer>.Resolve<INetworkInterfaceWrapper>(), IocSingleton<ControlsIocInitializer>.Resolve<IMemoryPlanner>(), IocSingleton<ControlsIocInitializer>.Resolve<IMemoryCache<ITileInfo, ITile>>(), IocSingleton<ControlsIocInitializer>.Resolve<ITileWeightCalculator>(), IocSingleton<ControlsIocInitializer>.Resolve<IThreadPool>())
    {
    }

    [UsedImplicitly]
    public MapContentLayer(
      ITileZoomLayerBuilder<ITile> layerBuilder,
      IDictionary<byte, ITileZoomLayer<ITile>> layerRepository,
      IUiDispatcher uiDispatcher,
      IZoomLevelConverter zoomLevelConverter,
      IViewportTileConverter viewportTileConverter,
      IQueue<ILayerCommand> commandQueue,
      IThread thread,
      ITileManager<ITile> tileManager,
      IDictionary<ITileInfo, bool> tilesToLoad,
      [NotNull] IConfigMediator configMediator,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper,
      [NotNull] IMemoryPlanner memoryPlanner,
      [NotNull] IMemoryCache<ITileInfo, ITile> unavailableTileCache,
      [NotNull] ITileWeightCalculator tileWeightCalculator,
      [NotNull] IThreadPool threadPool)
      : base(layerBuilder, layerRepository, zoomLevelConverter, uiDispatcher, viewportTileConverter, commandQueue, thread, configMediator, networkInterfaceWrapper)
    {
      if (tileManager == null)
        throw new ArgumentNullException(nameof (tileManager));
      if (unavailableTileCache == null)
        throw new ArgumentNullException(nameof (unavailableTileCache));
      if (tileWeightCalculator == null)
        throw new ArgumentNullException(nameof (tileWeightCalculator));
      if (threadPool == null)
        throw new ArgumentNullException(nameof (threadPool));
      if (memoryPlanner == null)
        throw new ArgumentNullException(nameof (memoryPlanner));
      this._tileManager = tileManager;
      this._tilesToLoad = tilesToLoad;
      this._unavailableTileCache = unavailableTileCache;
      this._tileWeightCalculator = tileWeightCalculator;
      this._threadPool = threadPool;
      this._memoryPlanner = memoryPlanner;
      this._tileManager.TilesReady += new EventHandler<TilesReadyEventArgs<ITile>>(((TileContentLayerBase<ITile>) this).TileManagerTilesReady);
      this._uiDispatcher.BeginInvoke((Action) (() => this._notFoundImage = (BitmapSource) new BitmapImage(new Uri(Resources.NoMapImageResource, UriKind.Relative))
      {
        CreateOptions = (BitmapCreateOptions) 18
      }));
      this._displayTileSize = configMediator.DisplayTileSize;
      if (this._displayTileSize == null)
        throw new InvalidOperationException("DisplayTileSize is null");
    }

    public BaseLayers Layers { get; set; }

    protected override void OnDispose()
    {
      this._tileManager.TilesReady -= new EventHandler<TilesReadyEventArgs<ITile>>(((TileContentLayerBase<ITile>) this).TileManagerTilesReady);
      lock (this._unavailableTileLock)
        this._unavailableTileCache.Clear();
      base.OnDispose();
      this.DisposeContentInternal(true);
    }

    protected override void OnDisposeContent() => this.DisposeContentInternal(false);

    private void DisposeContentInternal(bool deferTileManagerFlush)
    {
      base.OnDisposeContent();
      if (deferTileManagerFlush)
        this._threadPool.QueueUserWorkItem((WaitCallback) (state => this._tileManager.Flush()), (object) null);
      else
        this._tileManager.Flush();
    }

    protected override void OnOperationModeChanged(OperationMode newValue)
    {
      base.OnOperationModeChanged(newValue);
      this._tileManager.OperationMode = newValue;
    }

    protected override void OnReset()
    {
      base.OnReset();
      this._tileManager.Flush();
    }

    private void ProcessNotFoundTiles([NotNull] IList<ITileInfo> tiles)
    {
      if (tiles == null)
        throw new ArgumentNullException(nameof (tiles));
      ITileInfo[] array = tiles.Where<ITileInfo>((Func<ITileInfo, bool>) (tile => tile.Layer == BaseLayers.map)).ToArray<ITileInfo>();
      this.QueryTiles(((IEnumerable<ITileInfo>) array).Select<ITileInfo, ITileInfo>((Func<ITileInfo, ITileInfo>) (tile => (ITileInfo) new TileInfo(tile)
      {
        Layer = BaseLayers.pmap
      })).ToArray<ITileInfo>());
      List<ITile> list;
      lock (this._unavailableTileLock)
      {
        byte activeZoom = ContentLayerBase.ZoomLevelToZoom(this.ActiveViewportRect.ZoomLevel);
        list = tiles.Except<ITileInfo>((IEnumerable<ITileInfo>) array).Select<ITileInfo, ITile>((Func<ITileInfo, ITile>) (tileInfo =>
        {
          ITile tile;
          if (!this._unavailableTileCache.TryGetValue(tileInfo, out tile))
          {
            tile = (ITile) new UnavailableTile(this._notFoundImage, tileInfo);
            this._unavailableTileCache.Add(tileInfo, tile);
          }
          return tile;
        })).Where<ITile>((Func<ITile, bool>) (tile => (int) tile.TileInfo.Zoom == (int) activeZoom)).ToList<ITile>();
      }
      this.DisplayReadyTiles((IList<ITile>) list);
    }

    private ITileInfo[] GetRequiredTiles(
      ViewportRect viewportRect,
      ITileZoomLayer<ITile> layer,
      bool excludeCoveredTiles)
    {
      return this._viewportTileConverter.ViewportRectToTiles(viewportRect, layer.Zoom).Select<ITileInfo, ITileInfo>((Func<ITileInfo, ITileInfo>) (tileInfo => (ITileInfo) new TileInfo(tileInfo.X, tileInfo.Y, tileInfo.Zoom, this.Layers))).Where<ITileInfo>((Func<ITileInfo, bool>) (t =>
      {
        ITile tile;
        layer.TryGetValue(t, out tile);
        if (tile != null && tile.Status != TileStatus.NeedsReload)
          return false;
        return !excludeCoveredTiles || !this.TileIsCoveredByUpperTiles(t);
      })).ToArray<ITileInfo>();
    }

    public event EventHandler AutoPeoplesMapIsDisplayed;

    protected virtual void OnAutoPeoplesMapIsDisplayed(EventArgs e)
    {
      EventHandler peoplesMapIsDisplayed = this.AutoPeoplesMapIsDisplayed;
      if (peoplesMapIsDisplayed == null)
        return;
      peoplesMapIsDisplayed((object) this, e);
    }

    protected override void OnDisposeContentOutsideArea(ViewportRect viewportRect)
    {
      byte zoom = ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel);
      lock (this.LayerRepository)
      {
        foreach (byte zoom1 in this.LayerRepository.Values.Where<ITileZoomLayer<ITile>>((Func<ITileZoomLayer<ITile>, bool>) (l => (int) l.Zoom > (int) zoom || (int) l.Zoom < (int) zoom - this.ZoomLevelsToDisplaySimultaneously)).Select<ITileZoomLayer<ITile>, byte>((Func<ITileZoomLayer<ITile>, byte>) (l => l.Zoom)).ToArray<byte>())
          this.RemoveLayer(zoom1);
      }
      List<ITileInfo> infiniteTiles = new List<ITileInfo>((IEnumerable<ITileInfo>) this._tilesToLoad.Keys);
      ViewportRect viewportRect1 = viewportRect.Expand(2.0);
      foreach (KeyValuePair<byte, ITileZoomLayer<ITile>> keyValuePair in (IEnumerable<KeyValuePair<byte, ITileZoomLayer<ITile>>>) this.LayerRepository)
      {
        ViewportRect viewportRect2;
        if (!this._savedExpandedViewPorts.TryGetValue(keyValuePair.Key, out viewportRect2))
          viewportRect2 = viewportRect1;
        byte key = keyValuePair.Key;
        ITileZoomLayer<ITile> tileZoomLayer = keyValuePair.Value;
        List<ITileInfo> list = this._viewportTileConverter.ViewportRectToTiles(viewportRect2, key).ToList<ITileInfo>();
        tileZoomLayer.DisposeTilesExcept((IList<ITileInfo>) list);
        infiniteTiles.AddRange((IEnumerable<ITileInfo>) list);
      }
      this._tileManager.CancelQueryingTilesExcept((IList<ITileInfo>) infiniteTiles);
    }

    protected override void OnCommand(ILayerCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      if (command.Type == LayerCommandTypes.Custom && this.OperationMode == OperationMode.Full && command is TilesReadyLayerCommand<ITile> readyLayerCommand)
        this.OnTilesReady(readyLayerCommand.ReadyTiles);
      base.OnCommand(command);
    }

    protected virtual void OnTilesReady(IList<ITile> readyTiles)
    {
      ITile[] array1;
      ITileInfo[] array2;
      lock (this._tilesToLoadSync)
      {
        array1 = readyTiles.Where<ITile>((Func<ITile, bool>) (tile => this._tilesToLoad.ContainsKey(tile.TileInfo) && (tile.Status & TileStatus.Error) != TileStatus.Error)).Union<ITile>(readyTiles.Where<ITile>((Func<ITile, bool>) (tile => tile.Status == TileStatus.CannotLoad && tile.TileInfo.Layer == this.Layers))).ToArray<ITile>();
        array2 = readyTiles.Where<ITile>((Func<ITile, bool>) (tile => tile.Status == TileStatus.NotFound)).Select<ITile, ITileInfo>((Func<ITile, ITileInfo>) (tile => tile.TileInfo)).ToArray<ITileInfo>();
      }
      this.DisplayReadyTiles((IList<ITile>) array1);
      this.ProcessNotFoundTiles((IList<ITileInfo>) array2);
    }

    private void DisplayReadyTiles(IList<ITile> requiredTiles)
    {
      if (this.Layers == BaseLayers.map && requiredTiles.Any<ITile>((Func<ITile, bool>) (tile => tile.TileInfo.Layer == BaseLayers.pmap && (tile.Status & TileStatus.Error) != TileStatus.Error)))
        this.OnAutoPeoplesMapIsDisplayed(new EventArgs());
      foreach (IVisibleTileLayer<ITile> visibleTileLayer in (IEnumerable<ITileZoomLayer<ITile>>) this.LayerRepository.Values)
        visibleTileLayer.DisplayTiles(requiredTiles);
      lock (this._tilesToLoadSync)
      {
        foreach (ITile requiredTile in (IEnumerable<ITile>) requiredTiles)
          this._tilesToLoad.Remove(requiredTile.TileInfo);
        this.DisposeUnderlyingTiles();
        this.DisposeUnderlyingLevels();
      }
    }

    private void DisposeUnderlyingLevels()
    {
      byte? lastNecessaryZoomLevel = this.GetLastNecessaryZoomLevel();
      if (!lastNecessaryZoomLevel.HasValue || this._tilesToLoad.Any<KeyValuePair<ITileInfo, bool>>((Func<KeyValuePair<ITileInfo, bool>, bool>) (t => (int) t.Key.Zoom == (int) lastNecessaryZoomLevel.Value)))
        return;
      foreach (byte lowerZoomLevel in (IEnumerable<byte>) MapContentLayer.GetLowerZoomLevels(this.LayerRepository, lastNecessaryZoomLevel.Value))
        this.LayerRepository[lowerZoomLevel].DisposeTiles();
    }

    [CanBeNull]
    private byte? GetLastNecessaryZoomLevel()
    {
      ViewportRect viewportRect1;
      if (this._savedExpandedViewPorts.TryGetValue(this._activeZoom, out viewportRect1))
      {
        ViewportRect viewportRect2 = viewportRect1.Expand(2.0);
        foreach (KeyValuePair<byte, ViewportRect> expandedViewPort in (IEnumerable<KeyValuePair<byte, ViewportRect>>) this._savedExpandedViewPorts)
        {
          Rect tilesArea = this._viewportTileConverter.GetTilesArea(expandedViewPort.Value, expandedViewPort.Key);
          if (tilesArea.Width >= viewportRect2.Rect.Width && tilesArea.Height >= viewportRect2.Rect.Height)
            return new byte?(expandedViewPort.Key);
        }
      }
      return new byte?();
    }

    private void DisposeUnderlyingTiles()
    {
      List<ITileInfo> list = this._tilesToLoad.Keys.ToList<ITileInfo>();
      List<ITileInfo> tileInfoList = new List<ITileInfo>();
      foreach (ITileInfo tileInfo in list)
      {
        if (this.TileIsCoveredByUpperTiles(tileInfo))
          tileInfoList.Add(tileInfo);
      }
      foreach (ITileInfo key in tileInfoList)
      {
        if (this._tilesToLoad.ContainsKey(key))
          this._tilesToLoad.Remove(key);
      }
    }

    private bool TileIsCoveredByUpperTiles(ITileInfo tileInfo) => this.TileIsCoveredByUpperTiles((IEnumerable<byte>) MapContentLayer.GetUppperZoomLevels(this.LayerRepository, tileInfo.Zoom), tileInfo);

    private bool TileIsCoveredByUpperTiles(IEnumerable<byte> upperZoomLevels, ITileInfo tileInfo)
    {
      Rect currentTileRect = this.GetCurrentTileRect(tileInfo);
      foreach (byte upperZoomLevel in upperZoomLevels)
      {
        if (this.RectIsCoveredByUpperTiles(currentTileRect, tileInfo.Zoom, upperZoomLevel))
          return true;
      }
      return false;
    }

    private bool RectIsCoveredByUpperTiles(
      Rect currentTileRect,
      byte zoomLevel,
      byte upperZoomLevel)
    {
      IList<ITileInfo> visibleTileInfos = (IList<ITileInfo>) this.GetLayer(upperZoomLevel).VisibleTileInfos.ToList<ITileInfo>();
      return this.GetUpperTiles(zoomLevel, upperZoomLevel, currentTileRect).All<ITileInfo>((Func<ITileInfo, bool>) (t => visibleTileInfos.Any<ITileInfo>((Func<ITileInfo, bool>) (t1 => t1.EqualsCoordinates(t)))));
    }

    private IEnumerable<ITileInfo> GetUpperTiles(
      byte zoomLevel,
      byte upperZoomLevel,
      Rect currentTileRect)
    {
      return this._viewportTileConverter.GetTilesByArea(this.GetTileRectOnCurrentZoom(currentTileRect, zoomLevel, upperZoomLevel), upperZoomLevel);
    }

    private Rect GetTileRectOnCurrentZoom(Rect currentTileRect, byte zoom, byte upperZoomLevel)
    {
      double stretchFactor = this._zoomLevelConverter.ZoomLevelToStretchFactor((double) upperZoomLevel, zoom);
      return new Rect(currentTileRect.X * stretchFactor, currentTileRect.Y * stretchFactor, currentTileRect.Width * stretchFactor, currentTileRect.Height * stretchFactor);
    }

    private Rect GetCurrentTileRect(ITileInfo tileInfo)
    {
      Point tileTopLeftPoint = this._viewportTileConverter.GetTileTopLeftPoint(tileInfo);
      return new Rect(tileTopLeftPoint.X, tileTopLeftPoint.Y, (double) this._displayTileSize.Width, (double) this._displayTileSize.Height);
    }

    private static IList<byte> GetUppperZoomLevels(
      IDictionary<byte, ITileZoomLayer<ITile>> layerRepository,
      byte zoomLevel)
    {
      return (IList<byte>) layerRepository.Keys.Where<byte>((Func<byte, bool>) (z => (int) z > (int) zoomLevel)).ToList<byte>();
    }

    private static IList<byte> GetLowerZoomLevels(
      IDictionary<byte, ITileZoomLayer<ITile>> layerRepository,
      byte zoomLevel)
    {
      return (IList<byte>) layerRepository.Keys.Where<byte>((Func<byte, bool>) (z => (int) z < (int) zoomLevel)).ToList<byte>();
    }

    protected override void OnReload(ViewportRect viewportRect)
    {
      base.OnReload(viewportRect);
      byte activeZoom = ContentLayerBase.ZoomLevelToZoom(viewportRect.ZoomLevel);
      int num = Math.Max(0, (int) activeZoom - this.GetZoomLevelsToLoadSimultaneously());
      double extendRatio = 1.2;
      this._savedExpandedViewPorts.Clear();
      lock (this._tilesToLoadSync)
        this._tilesToLoad.Clear();
      IEnumerable<ITileInfo> tileInfos = Enumerable.Empty<ITileInfo>();
      for (int index = (int) activeZoom; index >= num; --index)
      {
        ITileZoomLayer<ITile> layer = this.GetLayer((byte) index);
        ViewportRect viewportRect1 = viewportRect.Expand(extendRatio);
        this._savedExpandedViewPorts[(byte) index] = viewportRect1;
        ITileInfo[] requiredTiles1 = this.GetRequiredTiles(viewportRect1, layer, (int) this._activeZoom <= (int) activeZoom);
        extendRatio *= 1.2;
        List<ITile> requiredTiles2 = (List<ITile>) null;
        lock (this._unavailableTileLock)
        {
          foreach (IGrouping<bool, ITileInfo> grouping in ((IEnumerable<ITileInfo>) requiredTiles1).GroupBy<ITileInfo, bool>((Func<ITileInfo, bool>) (tileInfo => this._unavailableTileCache.ContainsKey(tileInfo))).ToList<IGrouping<bool, ITileInfo>>())
          {
            if (grouping.Key)
            {
              if (index == (int) activeZoom)
                requiredTiles2 = grouping.Where<ITileInfo>((Func<ITileInfo, bool>) (tileInfo => (int) tileInfo.Zoom == (int) activeZoom)).Select<ITileInfo, ITile>((Func<ITileInfo, ITile>) (tileInfo => this._unavailableTileCache[tileInfo])).ToList<ITile>();
            }
            else
              tileInfos = tileInfos.Concat<ITileInfo>((IEnumerable<ITileInfo>) grouping);
          }
        }
        if (requiredTiles2 != null)
          this.DisplayReadyTiles((IList<ITile>) requiredTiles2);
      }
      this.QueryTiles(tileInfos.OrderByDescending<ITileInfo, ITileWeight>((Func<ITileInfo, ITileWeight>) (tile => this._tileWeightCalculator.GetTileWeight(tile, viewportRect))).ToArray<ITileInfo>());
      this.DoZoom(this.GetLayer(activeZoom));
    }

    private int GetZoomLevelsToLoadSimultaneously() => this._memoryPlanner.CurrentMemoryPressure != MemoryPressure.Heavy ? this.ZoomLevelsToDisplaySimultaneously : 0;

    private void QueryTiles([NotNull] ITileInfo[] requestMapTiles)
    {
      lock (this._tilesToLoadSync)
      {
        foreach (ITileInfo requestMapTile in requestMapTiles)
          this._tilesToLoad[requestMapTile] = true;
      }
      if (requestMapTiles.Length <= 0)
        return;
      this._tileManager.QueryTiles((IEnumerable<ITileInfo>) requestMapTiles);
    }
  }
}
