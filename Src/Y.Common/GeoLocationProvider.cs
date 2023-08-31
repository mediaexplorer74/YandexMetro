// Decompiled with JetBrains decompiler
// Type: Y.Common.GeoLocationProvider
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;
using System.Device.Location;
using System.Threading;

namespace Y.Common
{
  public class GeoLocationProvider
  {
    public GeoCoordinate GetCurrentUserLocationSync(int timeout = 10)
    {
      GeoCoordinate coordinate = (GeoCoordinate) null;
      using (GeoCoordinateWatcher coordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High))
      {
        AutoResetEvent watcherProvidedPositionEvent = (AutoResetEvent) null;
        bool flag = timeout > 0;
        if (flag)
        {
          watcherProvidedPositionEvent = new AutoResetEvent(false);
          coordinateWatcher.StatusChanged += (EventHandler<GeoPositionStatusChangedEventArgs>) ((s, e) =>
          {
            if (e.Status != GeoPositionStatus.Ready)
              return;
            watcherProvidedPositionEvent.Set();
          });
          coordinateWatcher.PositionChanged += (EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>) ((s, e) =>
          {
            coordinate = e.Position.Location;
            if (((DateTimeOffset) DateTime.UtcNow - e.Position.Timestamp).TotalMinutes > 1.0)
              return;
            watcherProvidedPositionEvent.Set();
          });
        }
        coordinateWatcher.Start();
        if (flag)
          watcherProvidedPositionEvent.WaitOne(TimeSpan.FromSeconds((double) timeout));
        if (coordinate == (GeoCoordinate) null && coordinateWatcher.Position != null)
          coordinate = coordinateWatcher.Position.Location;
        coordinateWatcher.Stop();
      }
      return coordinate;
    }
  }
}
