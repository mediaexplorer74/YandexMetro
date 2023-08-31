// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.PrinterStartupManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Net;
using System.Threading;
using Yandex.Common;
using Yandex.Hardware;
using Yandex.Maps.PrinterClient.Config;
using Yandex.Maps.PrinterClient.EventArgs;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  [UsedImplicitly]
  internal class PrinterStartupManager : IPrinterStartupManager, IDisposable
  {
    private const int MinRetryTimeoutInMilliseconds = 1000;
    private const int MaxRetryTimeoutInMilliseconds = 60000;
    private readonly IDeviceInfo _deviceInfo;
    private readonly IStartupQueryBuilder _startupQueryBuilder;
    private readonly IGenericXmlSerializer<StartupParameters> _printerStartupParametersSerializer;
    private readonly IMapWebClientFactory _webClientFactory;
    private readonly INetworkInterfaceWrapper _networkInterfaceWrapper;
    private readonly Yandex.Common.Timer _timer;
    private int _currentRetryTimeoutInMilliseconds = 1000;
    private string _queryString;
    private readonly AutoResetEvent _startupAutoReset;
    private bool _disposed;

    public event EventHandler<PrinterStartupEventArgs> StartupCompleted;

    public event EventHandler StartupFailed;

    public PrinterStartupManager(
      [NotNull] IDeviceInfo deviceInfo,
      [NotNull] IStartupQueryBuilder startupQueryBuilder,
      [NotNull] IGenericXmlSerializer<StartupParameters> printerStartupParametersSerializer,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] INetworkInterfaceWrapper networkInterfaceWrapper)
    {
      if (deviceInfo == null)
        throw new ArgumentNullException(nameof (deviceInfo));
      if (startupQueryBuilder == null)
        throw new ArgumentNullException(nameof (startupQueryBuilder));
      if (printerStartupParametersSerializer == null)
        throw new ArgumentNullException(nameof (printerStartupParametersSerializer));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (networkInterfaceWrapper == null)
        throw new ArgumentNullException(nameof (networkInterfaceWrapper));
      this._deviceInfo = deviceInfo;
      this._startupQueryBuilder = startupQueryBuilder;
      this._printerStartupParametersSerializer = printerStartupParametersSerializer;
      this._webClientFactory = webClientFactory;
      this._networkInterfaceWrapper = networkInterfaceWrapper;
      this._timer = new Yandex.Common.Timer(new Yandex.Common.TimerCallback(this.TimerCallback));
      this._startupAutoReset = new AutoResetEvent(false);
    }

    public void Startup(Version appVersion, string appPlatform, string uuid)
    {
      this._queryString = this._startupQueryBuilder.GetStartupQuery(appVersion, appPlatform, (double) this._deviceInfo.GetScreenWidth(), (double) this._deviceInfo.GetScreenHeight(), uuid, this._deviceInfo.GetDeviceManufacturer(), this._deviceInfo.GetDeviceName(), this._deviceInfo.Platform, this._deviceInfo.OSVersion, true);
      this.SendStartupRequest();
    }

    private void SendStartupRequest()
    {
      if (!this._startupAutoReset.Set())
        return;
      this._networkInterfaceWrapper.IsNetworkAvailableChanged -= new EventHandler(this.NetworkInterfaceWrapperIsNetworkAvailableChanged);
      this._timer.Stop();
      IHttpWebRequest webRequest = this._webClientFactory.CreateGetHttpWebRequest(new Uri(this._queryString));
      webRequest.BeginGetResponse((AsyncCallback) (asyncResult =>
      {
        try
        {
          using (IWebResponse response = webRequest.EndGetResponse(asyncResult))
          {
            using (Stream responseStream = response.GetResponseStream())
              this.OnStartupCompleted(this._printerStartupParametersSerializer.Deserialize(responseStream));
          }
        }
        catch (WebException ex)
        {
          this.OnStartupFailed();
          this.RetrySendStartupRequest();
        }
        catch (Exception ex)
        {
          Logger.TrackException(ex);
          this.OnStartupFailed();
          this.RetrySendStartupRequest();
        }
        finally
        {
          this._startupAutoReset.Reset();
        }
      }), (object) null);
    }

    private void RetrySendStartupRequest()
    {
      this._networkInterfaceWrapper.IsNetworkAvailableChanged += new EventHandler(this.NetworkInterfaceWrapperIsNetworkAvailableChanged);
      this._timer.Change(this._currentRetryTimeoutInMilliseconds);
    }

    private void NetworkInterfaceWrapperIsNetworkAvailableChanged(object sender, System.EventArgs e)
    {
      if (!this._networkInterfaceWrapper.GetIsNetworkAvailable())
        return;
      this._currentRetryTimeoutInMilliseconds = 1000;
      this.SendStartupRequest();
    }

    private void TimerCallback(object state)
    {
      this._currentRetryTimeoutInMilliseconds = Math.Min(this._currentRetryTimeoutInMilliseconds * 2, 60000);
      this.SendStartupRequest();
    }

    protected virtual void OnStartupCompleted(StartupParameters config)
    {
      if (this.StartupCompleted == null)
        return;
      this.StartupCompleted((object) this, new PrinterStartupEventArgs(config));
    }

    protected virtual void OnStartupFailed()
    {
      EventHandler startupFailed = this.StartupFailed;
      if (startupFailed == null)
        return;
      startupFailed((object) this, System.EventArgs.Empty);
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      if (this._startupAutoReset == null)
        return;
      this._startupAutoReset.Dispose();
    }
  }
}
