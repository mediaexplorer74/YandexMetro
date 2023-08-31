// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterTileFetcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Yandex.Common;
using Yandex.ItemsCounter;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.PrinterClient.Response;
using Yandex.Maps.PrinterClient.Tiles;
using Yandex.Maps.Repository.Events;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Patterns;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  [ComVisible(false)]
  public class PrinterTileFetcher : ITileFetcher<ITile>, IStateService
  {
    private readonly uint _protocolVersion;
    private readonly IPrinterQueryBuilder _printerQueryBuilder;
    private readonly IPrinterTileHeaderReader _printerTileHeaderReader;
    private readonly IPrinterTileReader _printerTileReader;
    private readonly IPrinterTileRequestSizeConverter _printerTileRequestSizeConverter;
    private readonly ITileFactory<ITile> _tileFactory;
    private readonly IMapWebClientFactory _webClientFactory;
    private readonly ITileSize _tileSize;
    private readonly IPrinterManager _printerManager;
    private readonly INetworkInterfaceWrapper _networkInterfaceWrapper;
    private readonly IThreadPool _threadPool;
    private readonly IItemCounter _itemCounter;

    public event EventHandler<TilesReadyEventArgs<ITile>> TilesReady;

    public event EventHandler<TileRequestFailedEventArgs> RequestFailed;

    public PrinterTileFetcher(
      uint maxTilesInQuery,
      IPrinterQueryBuilder printerQueryBuilder,
      uint protocolVersion,
      IPrinterTileHeaderReader printerTileHeaderReader,
      IPrinterTileReader printerTileReader,
      IPrinterTileRequestSizeConverter printerTileRequestSizeConverter,
      ITileFactory<ITile> tileFactory,
      IMapWebClientFactory webClientFactory,
      ITileSize tileSize,
      IItemCounter itemCounter,
      IPrinterManager printerManager,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper,
      [NotNull] IThreadPool threadPool)
    {
      if (maxTilesInQuery == 0U)
        throw new ArgumentException("maxTilesInQuery is zero");
      if (printerQueryBuilder == null)
        throw new ArgumentNullException(nameof (printerQueryBuilder));
      if (printerTileHeaderReader == null)
        throw new ArgumentNullException(nameof (printerTileHeaderReader));
      if (printerTileReader == null)
        throw new ArgumentNullException(nameof (printerTileReader));
      if (printerTileRequestSizeConverter == null)
        throw new ArgumentNullException(nameof (printerTileRequestSizeConverter));
      if (tileFactory == null)
        throw new ArgumentNullException(nameof (tileFactory));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (tileSize == null)
        throw new ArgumentNullException(nameof (tileSize));
      if (itemCounter == null)
        throw new ArgumentNullException(nameof (itemCounter));
      if (printerManager == null)
        throw new ArgumentNullException(nameof (printerManager));
      if (networkInterfaceWrapper == null)
        throw new ArgumentNullException(nameof (networkInterfaceWrapper));
      if (threadPool == null)
        throw new ArgumentNullException(nameof (threadPool));
      this.MaxTilesInSingleQuery = maxTilesInQuery;
      this._itemCounter = itemCounter;
      this._printerQueryBuilder = printerQueryBuilder;
      this._protocolVersion = protocolVersion;
      this._printerTileHeaderReader = printerTileHeaderReader;
      this._printerTileReader = printerTileReader;
      this._printerTileRequestSizeConverter = printerTileRequestSizeConverter;
      this._tileFactory = tileFactory;
      this._webClientFactory = webClientFactory;
      this._tileSize = tileSize;
      this._printerManager = printerManager;
      this._networkInterfaceWrapper = networkInterfaceWrapper;
      this._threadPool = threadPool;
    }

    public uint MaxTilesInSingleQuery { get; private set; }

    public void QueryTilesAsync(IEnumerable<ITileInfo> tiles)
    {
      IList<ITileInfo> tilesList = tiles != null ? (IList<ITileInfo>) tiles.ToList<ITileInfo>() : throw new ArgumentNullException(nameof (tiles));
      if (this.State == ServiceState.Ready && this._networkInterfaceWrapper.GetIsNetworkAvailable())
      {
        Uri printerQuery = this._printerQueryBuilder.GetPrinterQuery(this._protocolVersion);
        string boundary = "***" + DateTime.Now.Ticks.ToString("x") + "***";
        IHttpWebRequest postHttpWebRequest = this._webClientFactory.CreatePostHttpWebRequest(printerQuery, boundary);
        TilesRequest tilesRequest = new TilesRequest();
        foreach (ITileInfo ti in (IEnumerable<ITileInfo>) tilesList)
          tilesRequest.Tiles.Add(new TileRequest(ti)
          {
            Size = (int) this._printerTileRequestSizeConverter.ConvertSize((int) this._tileSize.Height)
          });
        RequestState state = new RequestState()
        {
          WebRequest = postHttpWebRequest,
          TilesRequest = tilesRequest,
          Boundary = boundary
        };
        this._itemCounter.Increment();
        postHttpWebRequest.BeginGetRequestStream(new AsyncCallback(this.GetRequestStreamCallback), (object) state);
      }
      else
        this._threadPool.QueueUserWorkItem((Yandex.Threading.WaitCallback) (nothing => this.OnRequestFailed(new TileRequestFailedEventArgs(tilesList))), (object) null);
    }

    private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
    {
      RequestState asyncState = (RequestState) asynchronousResult.AsyncState;
      using (Stream requestStream = asyncState.WebRequest.EndGetRequestStream(asynchronousResult))
        this._printerQueryBuilder.BuildPostData(requestStream, asyncState, this._protocolVersion);
      asyncState.WebRequest.BeginGetResponse(new AsyncCallback(this.WebResponseCallback), (object) asyncState);
    }

    private void WebResponseCallback(IAsyncResult ar)
    {
      RequestState asyncState = (RequestState) ar.AsyncState;
      IHttpWebRequest webRequest = asyncState.WebRequest;
      TileStatus tileStatus = TileStatus.NeedsReload;
      try
      {
        using (IWebResponse response = webRequest.EndGetResponse(ar))
        {
          switch (response.StatusCode)
          {
            case Yandex.Net.HttpStatusCode.NotModified:
              tileStatus = TileStatus.NotModified;
              break;
            case Yandex.Net.HttpStatusCode.NotFound:
              tileStatus = TileStatus.NotFound;
              break;
            case Yandex.Net.HttpStatusCode.RequestTimeout:
            case Yandex.Net.HttpStatusCode.InternalServerError:
            case Yandex.Net.HttpStatusCode.ServiceUnavailable:
              tileStatus = TileStatus.NeedsReload;
              break;
            default:
              using (Stream responseStream = response.GetResponseStream())
              {
                using (BinaryReader binaryReader = new BinaryReader(responseStream))
                {
                  this.ProcessTiles(asyncState, binaryReader);
                  return;
                }
              }
          }
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (WebException ex)
      {
        tileStatus = PrinterTileFetcher.ProcessWebException(ex, tileStatus);
      }
      catch (Exception ex)
      {
        PrinterTileFetcher.ProcessUnkonownException(ex);
      }
      finally
      {
        this._itemCounter.Decrement();
      }
      this.ReportTiles(asyncState, tileStatus);
    }

    private static void ProcessUnkonownException(Exception ex) => Logger.TrackException(ex);

    private static TileStatus ProcessWebException(WebException ex, TileStatus status)
    {
      if (ex.Response == null)
        return status;
      using (HttpWebResponse response = (HttpWebResponse) ex.Response)
      {
        switch (response.StatusCode)
        {
          case System.Net.HttpStatusCode.NotFound:
            Uri responseUri = response.ResponseUri;
            status = responseUri == (Uri) null || string.IsNullOrEmpty(responseUri.OriginalString) ? TileStatus.CannotLoad : TileStatus.NotFound;
            break;
          case System.Net.HttpStatusCode.RequestTimeout:
          case System.Net.HttpStatusCode.InternalServerError:
          case System.Net.HttpStatusCode.ServiceUnavailable:
            status = TileStatus.NeedsReload;
            break;
          default:
            status = TileStatus.CannotLoad;
            break;
        }
      }
      return status;
    }

    private void ProcessTiles(RequestState requestState, BinaryReader binaryReader)
    {
      foreach (PrinterTileInfo tile1 in this._printerTileHeaderReader.ReadPrinterTileResponseHeader(binaryReader, (IList<TileRequest>) requestState.TilesRequest.Tiles, this._protocolVersion).Tiles)
      {
        ITile tile2 = this._printerTileReader.ReadTile(binaryReader, tile1);
        ITile tile3;
        if (tile2 != null)
        {
          tile3 = tile2;
        }
        else
        {
          TileStatus tileStatus = tile1.StatusCode == (ushort) 404 ? TileStatus.NotFound : TileStatus.CannotLoad;
          tile3 = this._tileFactory.CreateTile((ITileInfo) tile1, tileStatus);
        }
        this.OnTilesReady(new TilesReadyEventArgs<ITile>((IList<ITileInfo>) new ITileInfo[1]
        {
          tile3.TileInfo
        }, (IList<ITile>) new ITile[1]{ tile3 }));
      }
    }

    private void ReportTiles(RequestState requestState, TileStatus tileStatus) => this.OnTilesReady(new TilesReadyEventArgs<ITile>((IList<ITileInfo>) new List<ITileInfo>(), (IList<ITile>) requestState.TilesRequest.Tiles.Select<TileRequest, ITile>((Func<TileRequest, ITile>) (tile => this._tileFactory.CreateTile((ITileInfo) tile, tileStatus))).ToList<ITile>()));

    public void OnTilesReady(TilesReadyEventArgs<ITile> e)
    {
      EventHandler<TilesReadyEventArgs<ITile>> tilesReady = this.TilesReady;
      if (tilesReady == null)
        return;
      tilesReady((object) this, e);
    }

    public void OnRequestFailed(TileRequestFailedEventArgs e)
    {
      EventHandler<TileRequestFailedEventArgs> requestFailed = this.RequestFailed;
      if (requestFailed == null)
        return;
      requestFailed((object) this, e);
    }

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._printerManager.StateChanged += value;
      remove => this._printerManager.StateChanged -= value;
    }

    public ServiceState State => this._printerManager.State;
  }
}
