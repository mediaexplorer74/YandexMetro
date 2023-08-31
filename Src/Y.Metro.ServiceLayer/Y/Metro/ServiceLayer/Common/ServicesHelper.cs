// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.Common.ServicesHelper
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.Device.Location;
using System.Net;
using System.Threading;
using Y.Common;

namespace Y.Metro.ServiceLayer.Common
{
  public static class ServicesHelper
  {
    private static GeoCoordinate _location;
    private static bool _gpsEnabled;

    public static string Uuid { get; set; }

    public static bool GpsEnabled
    {
      get => ServicesHelper._gpsEnabled;
      set
      {
        if (ServicesHelper._gpsEnabled == value)
          return;
        ServicesHelper._gpsEnabled = value;
        if (ServicesHelper._gpsEnabled)
          ThreadPool.QueueUserWorkItem(new WaitCallback(ServicesHelper.UpdateCurrentGpsLocation));
        else
          ServicesHelper._location = (GeoCoordinate) null;
      }
    }

    public static GeoCoordinate UserLocation => ServicesHelper._location;

    public static GeoCoordinate GetLastLocation(bool gpsEnabled)
    {
      GeoCoordinate lastLocation = (GeoCoordinate) null;
      if (gpsEnabled)
        lastLocation = new GeoLocationProvider().GetCurrentUserLocationSync(0);
      return lastLocation;
    }

    public static void UpdateCurrentGpsLocation(object state)
    {
      GeoCoordinate userLocationSync = new GeoLocationProvider().GetCurrentUserLocationSync();
      if (!(userLocationSync != (GeoCoordinate) null) || userLocationSync.IsUnknown)
        return;
      ServicesHelper._location = userLocationSync;
    }

    internal static HttpWebRequest CreateHttpRequest(string url)
    {
      HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(url);
      httpRequest.Headers["Accept-Encoding"] = "gzip, deflate";
      return httpRequest;
    }
  }
}
