// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.IoC.ControlsIocInitializer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Controls;
using Yandex.App;
using Yandex.App.Information;
using Yandex.App.Interfaces;
using Yandex.Collections;
using Yandex.Collections.Interfaces;
using Yandex.DevUtils;
using Yandex.Hardware;
using Yandex.Input;
using Yandex.Input.Interfaces;
using Yandex.IO;
using Yandex.Ioc;
using Yandex.ItemsCounter;
using Yandex.Maps.API;
using Yandex.Maps.API.Converters;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ApplicationIdentity;
using Yandex.Maps.Config;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Geocoding;
using Yandex.Maps.Geocoding.Dto;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Maps.Helpers;
using Yandex.Maps.Imaging;
using Yandex.Maps.Interfaces;
using Yandex.Maps.PositionManager;
using Yandex.Maps.PositionManager.Interfaces;
using Yandex.Maps.PrinterClient;
using Yandex.Maps.PrinterClient.Config;
using Yandex.Maps.PrinterClient.DataAdapters;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.PrinterClient.Tiles;
using Yandex.Maps.Repository;
using Yandex.Maps.Repository.Config;
using Yandex.Maps.Repository.Dto;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Maps.Repository.MapLoader;
using Yandex.Maps.TileWeighting;
using Yandex.Maps.Traffic;
using Yandex.Maps.Traffic.DataAdapters;
using Yandex.Maps.Traffic.DTO.Collect;
using Yandex.Maps.Traffic.DTO.Styles;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Maps.TwilightMode;
using Yandex.Maps.WebUtils;
using Yandex.Media.Animation;
using Yandex.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Media.Transformations;
using Yandex.Media.Transformations.Interfaces;
using Yandex.Memory;
using Yandex.PAL;
using Yandex.PAL.Interfaces;
using Yandex.Positioning;
using Yandex.Positioning.Interfaces;
using Yandex.Serialization;
using Yandex.Serialization.Interfaces;
using Yandex.StringUtils;
using Yandex.StringUtils.Interfaces;
using Yandex.Threading;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.IoC
{
  internal class ControlsIocInitializer : IocInitializer
  {
    private const string RootPath = "IsolatedStore\\cache";
    private const uint CacheFormatVersion = 2;
    private const string PlatformId = "wp";

    protected override void Initialize()
    {
      this.Container.Register<IMemoryUsageMonitor, MemoryUsageMonitor>((string) null, false).Register<IMemoryPlanner, MemoryPlanner>(isSingleton: true).Register<ITileScaleAdapter>((Func<IIocContainer, ITileScaleAdapter>) (i => !LowMemoryHelper.IsLowMemDevice ? (ITileScaleAdapter) new Printer1And2TileScaleAdapter() : (ITileScaleAdapter) new Printer1TileScaleAdapter())).Register<ICacheTileScaleAdapter, CacheTileScaleAdapter>((string) null, false).Register<IMemoryCache<ITileInfo, ITile>>((Func<IIocContainer, IMemoryCache<ITileInfo, ITile>>) (container => (IMemoryCache<ITileInfo, ITile>) new MemoryCache<ITileInfo, ITile>(128))).Register<IPath, Path>(isSingleton: true).Register<IDictionary<string, bool>, Dictionary<string, bool>>((Expression<Func<Dictionary<string, bool>>>) (() => new Dictionary<string, bool>())).Register<IFileStorage, CachedDirectoryIsolatedStorage>(isSingleton: true);
      uint num = 128;
      this.Container.Register<IPositionWatcher>((Func<IIocContainer, IPositionWatcher>) (container => WpHighAccuracyPositionWatcher.Instance));
      this.Container.Register<IPositionWatcherManager, PositionWatcherManager>(isSingleton: true).Register<IVelocityCalculator, WeightedSumVelocityCalculator>(isSingleton: true).Register<ITouchHandler>((Func<IIocContainer, ITouchHandler>) (container => (ITouchHandler) new InertialTouchManipulationSource(TimeSpan.FromMilliseconds(500.0), TimeSpan.FromMilliseconds(500.0), container.Resolve<IVelocityCalculator>(), container.Resolve<IStopwatch>(), (IEasingFunction) new ExponentialEase()
      {
        Exponent = -2
      }))).Register<IPositionManagerRestictions>((Func<IIocContainer, IPositionManagerRestictions>) (container => (IPositionManagerRestictions) new PositionManagerRestictions(container.Resolve<IViewportPointConveter>(), container.Resolve<IZoomInfo>(), 5.0))).Register<IInterimPointsHelper>((Func<IIocContainer, IInterimPointsHelper>) (container => (IInterimPointsHelper) new InterimPointsHelper(container.Resolve<IViewportPointConveter>(), 50.0, 800.0, 10, 500, 5, -0.016))).Register<IPositionManager, KineticPositionManager>((string) null, false).Register<IPositionDispatcher, PositionDispatcher>((string) null, false).Register<IPositionDispatcherBase, StaticPositionDispatcher>((string) null, false).Register<IManipulationWrapper, ManipulationWrapper>((string) null, false).RegisterInstance<IDeviceInfo>((IDeviceInfo) new Wp7DeviceInfo()).RegisterInstance<ITileSize>((ITileSize) new TileSize(num, num)).Register<DraggableControlBehaviorHelper>((Func<IIocContainer, DraggableControlBehaviorHelper>) (container => new DraggableControlBehaviorHelper(1, 15, 6)), isSingleton: true).Register<IDictionary<string, HostTypes>, Dictionary<string, HostTypes>>((string) null, false).Register<QueryHosts>((string) null, false).Register<IConfigMediator>((Func<IIocContainer, IConfigMediator>) (container =>
      {
        ConfigMediator configMediator = container.Resolve<ConfigMediator>();
        configMediator.ZoomLevelsToDisplaySimultaneouslyDefault = !LowMemoryHelper.IsLowMemDevice ? (byte) 1 : (byte) 0;
        return (IConfigMediator) configMediator;
      }), isSingleton: true).Register<IOperatorInfoDataAdapter, OperatorInfoIgnoreBrandingTimeoutDataAdapter>(isSingleton: true).Register<IMapWebClientFactory, MapKitWebClientFactory>(isSingleton: true).Register<IWebClientFactory, WebClientFactory>(isSingleton: true).Register<INetworkInterfaceWrapper, NetworkInterfaceWrapper>(isSingleton: true).Register<IJamManager, JamManager>(isSingleton: true).Register<ITrafficCommunicator, TrafficCommunicator>(isSingleton: true).Register<IJamInformerManager, JamInformerManager>(isSingleton: true).Register<IJamCollectManager, JamCollectManager>(isSingleton: true).Register<IJamQueryBuilder>((Func<IIocContainer, IJamQueryBuilder>) (container => (IJamQueryBuilder) new JamQueryBuilder(container.Resolve<IPrinterUrlProvider>(), true)), isSingleton: true).Register<IJamCollectQueryBuilder, JamCollectQueryBuilder>(isSingleton: true).Register<IJamCollectPostDataBuilder, JamCollectPostDataBuilder>(isSingleton: true).Register<IJamCollectSender, JamCollectCommunicator>(isSingleton: true).Register<IJamStylesManager, JamStylesManager>(isSingleton: true).Register<IGenericXmlSerializer<JamStyles>, JamStylesSerializer>((string) null, false).Register<IGenericXmlSerializer<JamTracks>, JamsvecBinarySerializer>((string) null, false).Register<IGenericXmlSerializer<JamCollectPoints>, GenericCompressedXmlSerializer<JamCollectPoints>>((string) null, false).Register<ITileFetcher<IJamTile>, JamTilesFetcher>((string) null, false).Register<IJamRender, JamRender>((string) null, false).Register<IJamTileBuilder, JamTileBuilder>(isSingleton: true).Register<ITrafficContentLayer, TrafficContentLayer>(isSingleton: true).Register<IMapContentLayer, MapContentLayer>((string) null, false).Register<ITrackAdapter, TrackAdapter>(isSingleton: true).Register<IThread, Thread>(isSingleton: true).Register<IThreadPool, ThreadPool>(isSingleton: true).Register<IMonitor, Monitor>(isSingleton: true).Register<IPrinterTileRequestSizeConverter, PrinterTileRequestSizeConverter>((string) null, false).Register<IPrinterTileHeaderReader, PrinterTileHeaderReader>((string) null, false).Register<IPrinterTileReader, PrinterTileReader>((string) null, false).Register<IPrinterQueryBuilder, PrinterQueryBuilder>((string) null, false).Register<IApplicationSettings, IsolatedStorageApplicationSettings>(isSingleton: true).Register<IManifestInformationProvider, ManifestInformationProvider>((string) null, false).Register<IStartupQueryBuilder, MapKitStartupQueryBuilder>(isSingleton: true).Register<IPrinterUrlProvider, MapKitPrinterUrlProvider>(isSingleton: true);
      this.Container.Register<IPrinterStartupManager, PrinterStartupManager>((string) null, false).Register<IPrinterManager, PrinterManager>(isSingleton: true).Register<IGenericXmlSerializer<StartupParameters>, GenericXmlSerializer<StartupParameters>>((string) null, false).Register<IGenericXmlSerializer<TilesRequest>, GenericXmlSerializer<TilesRequest>>((string) null, false).Register<IContentCompressor, ContentCompressor>((string) null, false).Register<IUiDispatcher, UiDispatcher>(isSingleton: true).Register<IBitmapFactory, BitmapFactory>((string) null, false).Register<Panel, Canvas>((string) null, false).Register<ITileFactory<ITile>, TileFactory>(isSingleton: true).Register<ITileFactory<IJamTile>, JamTileBuilder>(isSingleton: true).Register<IZoomInfo>((Func<IIocContainer, IZoomInfo>) (container => (IZoomInfo) new MobileYandexMapsZoomInfo(container.Resolve<ITileSize>(), container.Resolve<IConfigMediator>(), (byte) 23))).Register<ITileRectangleBreaker, TileRectangleBreaker>((string) null, false).Register<IViewportPointConveter, ViewportPointConverter>(isSingleton: true).Register<ITileInfoNormalizer, TileInfoNormalizer>(isSingleton: true).Register<IZoomLevelConverter, ZoomLevelConverter>(isSingleton: true).Register<IGeoTileConverter, GeoTileConverter>(isSingleton: true).Register<IGeoPixelConverter, GeoPixelConverter>(isSingleton: true).Register<IViewportTileConverter, ViewportTileConverter>(isSingleton: true).Register<IDictionary<ITileInfo, TileQueueEntry>, Dictionary<ITileInfo, TileQueueEntry>>((Expression<Func<Dictionary<ITileInfo, TileQueueEntry>>>) (() => new Dictionary<ITileInfo, TileQueueEntry>())).Register<IMappingTileQueue, MappingTileQueue>((string) null, false).Register<ICacheFolderNameConstructor>((Func<IIocContainer, ICacheFolderNameConstructor>) (container => (ICacheFolderNameConstructor) new CacheFolderNameConstructor("IsolatedStore\\cache", 2U, container.Resolve<IConfigMediator>(), container.Resolve<ICacheTileScaleAdapter>(), container.Resolve<IPath>()))).Register<IHeaderBlockSerializer, HeaderBlockSerializer>((string) null, false).Register<ITileFileNameConstructor, CacheFileNameConstructor>((string) null, false).Register<IGenericXmlSerializer<MapLayers>, GenericXmlSerializer<MapLayers>>(isSingleton: true).Register<IDataRepository<MapLayers>, CacheConfigDataRepository>(isSingleton: true).Register<ICacheConfigManager, CacheConfigManager>(isSingleton: true).Register<IGenericXmlSerializer<CacheMetaData>, GenericXmlSerializer<CacheMetaData>>(isSingleton: true).Register<IDataRepository<CacheMetaData>, CacheMetaDataRepository>(isSingleton: true).Register<ICacheStorageUpdater, CacheStorageUpdater>(isSingleton: true).Register<IZoomTreeHeightCalculator, ZoomTreeHeightCalculator>(isSingleton: true).Register<IQueue<ITileInfo>, Yandex.Collections.Queue<ITileInfo>>((string) null, false).Register<IEnumerableQueue<ITileInfo>, EnumerableQueue<ITileInfo>>((string) null, false).Register<IItemCounter, ItemCounter>(isSingleton: true).Register<IQueryQueueManager<ITile>>((Func<IIocContainer, IQueryQueueManager<ITile>>) (container =>
      {
        QueryQueueManager queryQueueManager = container.Resolve<QueryQueueManager>();
        queryQueueManager.MaxRequestsCount = 6U;
        return (IQueryQueueManager<ITile>) queryQueueManager;
      })).Register<IQueryQueueManager<IJamTile>>((Func<IIocContainer, IQueryQueueManager<IJamTile>>) (container =>
      {
        JamQueryQueueManager queryQueueManager = container.Resolve<JamQueryQueueManager>();
        queryQueueManager.MaxRequestsCount = 6U;
        return (IQueryQueueManager<IJamTile>) queryQueueManager;
      })).Register<ITileFetcher<ITile>>((Func<IIocContainer, ITileFetcher<ITile>>) (container => (ITileFetcher<ITile>) new PrinterTileFetcher(10U, container.Resolve<IPrinterQueryBuilder>(), 2U, container.Resolve<IPrinterTileHeaderReader>(), container.Resolve<IPrinterTileReader>(), container.Resolve<IPrinterTileRequestSizeConverter>(), container.Resolve<ITileFactory<ITile>>(), container.Resolve<IMapWebClientFactory>(), container.Resolve<ITileSize>(), container.Resolve<IItemCounter>(), container.Resolve<IPrinterManager>(), container.Resolve<INetworkInterfaceWrapper>(), container.Resolve<IThreadPool>()))).Register<ITileMemoryCache<ITile>, TileMemoryCache<ITile>>(isSingleton: true).Register<ITileMemoryCache<IJamTile>, TileMemoryCache<IJamTile>>(isSingleton: true).Register<IJamTileMemoryCache, JamTileMemoryCache>(isSingleton: true).Register<ITileStorage<ITile>, CacheStorage>(isSingleton: true).Register<IDictionary<ITileInfo, ITile>, WeakDictionary<ITileInfo, ITile>>((Expression<Func<WeakDictionary<ITileInfo, ITile>>>) (() => new WeakDictionary<ITileInfo, ITile>())).Register<IDictionary<ITileInfo, IJamTile>, WeakDictionary<ITileInfo, IJamTile>>((Expression<Func<WeakDictionary<ITileInfo, IJamTile>>>) (() => new WeakDictionary<ITileInfo, IJamTile>()));
      if (DesignerProperties.IsInDesignMode)
        this.Container.Register<ITileFileCache<ITile>, DummyFileCache<ITile>>(isSingleton: true).Register<IPositionWatcherWrapper, DummyPositionWatcherWraper>(isSingleton: true);
      else
        this.Container.Register<ITileFileCache<ITile>, CacheStorageAsync>(isSingleton: true).Register<IPositionWatcherWrapper, PositionWatcherWrapper>(isSingleton: true);
      this.Container.Register<IStopwatch, Stopwatch>(isSingleton: true).Register<ITileRepository<ITile>, TileRepository>(isSingleton: true);
      if (LowMemoryHelper.IsLowMemDevice)
        this.Container.Register<ITileRepository<IJamTile>, DummyJamTilesRepository>(isSingleton: true);
      else
        this.Container.Register<ITileRepository<IJamTile>, JamTilesRepository>(isSingleton: true);
      this.Container.Register<ITileManager<ITile>, TileManager>((string) null, false).Register<IVisibleTileLayer<ITile>, VisibleTileLayer<ITile>>((string) null, false).Register<ITileZoomLayer<ITile>, BitmapTileZoomLayer<ITile>>((string) null, false).Register<ITileZoomLayer<IJamTile>, TrafficTileZoomLayer>((string) null, false).Register<IVisibleTileInfoLayer, VisibleTileInfoLayer>((string) null, false).Register<ITileSerializer, TileReader>(isSingleton: true).Register<ITileMetadataReader, TileMetadataReader>(isSingleton: true).Register<IGenericXmlSerializer<TileMetadataDto>, GenericXmlSerializer<TileMetadataDto>>((string) null, false).Register<ITileManager<IJamTile>, JamTileManager>((string) null, false).Register<IMetaContentLayer, MetaContentLayer>((string) null, false).Register<ITileZoomLayerBuilder<ITile>, TileZoomLayerBuilder<ITile>>(isSingleton: true).Register<ITileZoomLayerBuilder<IJamTile>, TileZoomLayerBuilder<IJamTile>>(isSingleton: true).Register<ITrafficTileZoomLayerBuilder, TrafficTileZoomLayerBuilder>(isSingleton: true).Register<IDictionary<byte, ITileZoomLayer<ITile>>, ConcurrentDictionary<byte, ITileZoomLayer<ITile>>>((Expression<Func<ConcurrentDictionary<byte, ITileZoomLayer<ITile>>>>) (() => new ConcurrentDictionary<byte, ITileZoomLayer<ITile>>())).Register<IDictionary<byte, ITileZoomLayer<IJamTile>>, ConcurrentDictionary<byte, ITileZoomLayer<IJamTile>>>((Expression<Func<ConcurrentDictionary<byte, ITileZoomLayer<IJamTile>>>>) (() => new ConcurrentDictionary<byte, ITileZoomLayer<IJamTile>>())).Register<IDictionary<ITileInfo, bool>, ConcurrentDictionary<ITileInfo, bool>>((Expression<Func<ConcurrentDictionary<ITileInfo, bool>>>) (() => new ConcurrentDictionary<ITileInfo, bool>())).Register<IQueue<ILayerCommand>, Yandex.Collections.Queue<ILayerCommand>>((string) null, false).Register<IPool<Image>, TileImagePool>((string) null, false).Register<ITileDrawManager, TileDrawManager>((string) null, false).Register<IWriteableBitmapBuilder, WriteableBitmapBuilder>(isSingleton: true).Register<IBulkWriteableBitmapBuilder, BulkTileWriteableBitmapBuilder>(isSingleton: true).Register<IBitmapRenderer, BitmapRenderer>((string) null, false).Register<IAffineTransform, AffineTransform>((string) null, false).Register<IMapPresenterFactory, MapPresenterFactory>((string) null, false).Register<IPushPinManager, PushPinManager>((string) null, false).Register<IGeocodeUrlProvider, GeocodeUrlProvider>((string) null, false).Register<IGeocodeQueryBuilder, GeocodeQueryBuilder>((string) null, false).Register<IGeocodeManager, GeocodeManager>((string) null, false).Register<ICommunicator<GeocodeRequestParameters, GeocodeResult>, GeocodeCommunicator>((string) null, false).Register<IDictionary<GeoCoordinate, GeocodeResult>, Dictionary<GeoCoordinate, GeocodeResult>>((Expression<Func<Dictionary<GeoCoordinate, GeocodeResult>>>) (() => new Dictionary<GeoCoordinate, GeocodeResult>())).Register<IGenericXmlSerializer<GeocodeResult>, GenericCompressedXmlSerializer<GeocodeResult>>((string) null, false).Register<IVersionStringAdapter, VersionStringAdapter>((string) null, false);
      if (DesignerProperties.IsInDesignMode)
        this.Container.Register<IApplicationIdentity, DummyApplicationIdentity>((string) null, false).Register<ITileRenderPreprocessor, DummyTileRenderPreprocessor>((string) null, false);
      else
        this.Container.Register<IApplicationIdentity>((Func<IIocContainer, IApplicationIdentity>) (container => (IApplicationIdentity) new MapkitAssemblyIdentity("wp")), isSingleton: true).Register<ITileRenderPreprocessor, TwilightModeTileRenderPreprocessor>((string) null, false);
      this.Container.Register<IDistanceFormatterUtil, MetricDistanceFormatterUtil>(isSingleton: true).Register<ITileWeightCalculator, TileWeightCalculator>((string) null, false);
    }
  }
}
