// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.ServiceProvider
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.Net;
using Y.Common;
using Y.Common.Extensions;
using Y.Metro.ServiceLayer.Common;
using Y.Metro.ServiceLayer.Generated;

namespace Y.Metro.ServiceLayer
{
  public class ServiceProvider
  {
    private const string BaseUrl = "http://mobile.metro.{0}/metro/startup?app_platform={1}&screen_w={2}&screen_h={3}&schemes&utf&gzip";

    public string AppVersion { get; set; }

    public void BeginStartup(int runs, int tracks, string serverUrl)
    {
      HttpWebRequest httpRequest = ServicesHelper.CreateHttpRequest(this.GetStartupUrl(runs, tracks, serverUrl));
      httpRequest.BeginGetResponse(new AsyncCallback(this.OnGetStartupResponseCompleted), (object) httpRequest);
    }

    public void BeginDownloadScheme(string url, int id)
    {
      HttpWebRequest httpRequest = ServicesHelper.CreateHttpRequest(url);
      httpRequest.BeginGetResponse(new AsyncCallback(this.OnGetDownloadSchemeResponseCompleted), (object) new GetState(httpRequest, (object) id));
    }

    private string GetStartupUrl(int runs, int tracks, string serverUrl)
    {
      Dictionary<string, string> source = new Dictionary<string, string>()
      {
        {
          "app_version",
          this.AppVersion
        },
        {
          "manufacturer",
          DeviceStatus.DeviceManufacturer
        },
        {
          "model",
          DeviceStatus.DeviceName
        },
        {
          "osVersion",
          Environment.OSVersion.Version.ToString()
        },
        {
          "platformtype",
          Environment.OSVersion.Platform.ToString()
        },
        {
          nameof (runs),
          runs.ToString()
        },
        {
          nameof (tracks),
          tracks.ToString()
        }
      };
      if (!string.IsNullOrEmpty(ServicesHelper.Uuid))
        source.Add("uuid", ServicesHelper.Uuid);
      return source.ToQueryString(string.Format("http://mobile.metro.{0}/metro/startup?app_platform={1}&screen_w={2}&screen_h={3}&schemes&utf&gzip", (object) serverUrl, (object) "wp", (object) 480, (object) 800));
    }

    private void OnGetDownloadSchemeResponseCompleted(IAsyncResult ar)
    {
      SchemeEventArgs e = new SchemeEventArgs();
      GetState asyncState = (GetState) ar.AsyncState;
      HttpWebRequest request = asyncState.Request;
      int userInputData = (int) asyncState.UserInputData;
      try
      {
        HttpWebResponse response = (HttpWebResponse) request.EndGetResponse(ar);
        if (response.StatusCode == HttpStatusCode.OK)
        {
          string str = HttpWebRequestExtensions.GetUnGzippedStream(response).UnzipData();
          e.SchemePack = str;
          e.SchemeId = userInputData;
        }
      }
      catch (Exception ex)
      {
        e.Error = ex;
      }
      if (this.DownloadSchemeCompleted == null)
        return;
      this.DownloadSchemeCompleted((object) this, e);
    }

    private void OnGetStartupResponseCompleted(IAsyncResult ar)
    {
      RegistrationEventArgs e = new RegistrationEventArgs();
      HttpWebRequest asyncState = (HttpWebRequest) ar.AsyncState;
      try
      {
        HttpWebResponse response = (HttpWebResponse) asyncState.EndGetResponse(ar);
        if (response.StatusCode == HttpStatusCode.OK)
        {
          startup startup = SerializeHelper.Deserialize<startup>(HttpWebRequestExtensions.GetUnGzippedStream(response));
          e.Uuid = startup.uuid;
          e.Schemes = startup.schemes;
        }
      }
      catch (Exception ex)
      {
        e.Error = ex;
      }
      if (this.RegistrationCompleted == null)
        return;
      this.RegistrationCompleted((object) this, e);
    }

    public event EventHandler<RegistrationEventArgs> RegistrationCompleted;

    public event EventHandler<SchemeEventArgs> DownloadSchemeCompleted;

    public class BaseEventArgs : EventArgs
    {
      public Exception Error { get; set; }
    }

    public class ResponseEventArgs<TResult> : ServiceProvider.BaseEventArgs
    {
      public object RequestState { get; internal set; }

      public TResult Result { get; set; }
    }
  }
}
