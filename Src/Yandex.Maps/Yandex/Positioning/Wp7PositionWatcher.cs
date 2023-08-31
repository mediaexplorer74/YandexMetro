// Decompiled with JetBrains decompiler
// Type: Yandex.Positioning.Wp7PositionWatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Shell;
using System;
using System.Device.Location;
using Yandex.Positioning.Events;
using Yandex.Positioning.Interfaces;

namespace Yandex.Positioning
{
  internal abstract class Wp7PositionWatcher : IPositionWatcher
  {
    private readonly IGeoPositionWatcher<System.Device.Location.GeoCoordinate> _geoCoordinateWatcher;
    private bool _enabled;
    private GeoPositionStatus _status;

    protected Wp7PositionWatcher(
      IGeoPositionWatcher<System.Device.Location.GeoCoordinate> geoPositionWatcher)
    {
      this._geoCoordinateWatcher = geoPositionWatcher != null ? geoPositionWatcher : throw new ArgumentNullException(nameof (geoPositionWatcher));
      this._geoCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate>>(this.GeoCoordinateWatcherPositionChanged);
      this._geoCoordinateWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(this.GeoCoordinateWatcherStatusChanged);
      this.Enabled = false;
      this.LastPosition = new GeoPosition();
      PhoneApplicationService current = PhoneApplicationService.Current;
      if (current == null)
        return;
      current.Closing += (EventHandler<ClosingEventArgs>) ((param0, param1) => this._geoCoordinateWatcher.Stop());
      current.Deactivated += (EventHandler<DeactivatedEventArgs>) ((param0, param1) => this._geoCoordinateWatcher.Stop());
      current.Activated += (EventHandler<ActivatedEventArgs>) ((param0, param1) =>
      {
        if (!this.Enabled)
          return;
        this._geoCoordinateWatcher.Start();
      });
    }

    public event EventHandler<PositionChangedEventArgs> PositionChanged;

    public event EventHandler<PositionWatcherStatusChangedEventArgs> StatusUpdated;

    public bool Enabled
    {
      get => this._enabled;
      set
      {
        if (this._enabled == value)
          return;
        this._enabled = value;
        if (value)
        {
          this._geoCoordinateWatcher.Start();
        }
        else
        {
          this._geoCoordinateWatcher.Stop();
          this.Status = GeoPositionStatus.Disabled;
          this.LastPosition = new GeoPosition();
        }
      }
    }

    public GeoPosition LastPosition { get; private set; }

    public GeoPositionStatus Status
    {
      get => this._status;
      private set
      {
        if (this._status == value)
          return;
        this._status = value;
        this.OnStatusUpdated(value);
      }
    }

    private void GeoCoordinateWatcherStatusChanged(
      object sender,
      GeoPositionStatusChangedEventArgs e)
    {
      this.Status = (GeoPositionStatus) e.Status;
    }

    private void GeoCoordinateWatcherPositionChanged(
      object sender,
      GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
    {
      GeoPosition geoPosition = new GeoPosition(new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude), e.Position.Timestamp, e.Position.Location.HorizontalAccuracy, e.Position.Location.VerticalAccuracy, e.Position.Location.Altitude, e.Position.Location.Speed, e.Position.Location.Course);
      this.LastPosition = geoPosition;
      if (!this.Enabled)
        return;
      this.OnLocationChanged(geoPosition);
    }

    protected virtual void OnLocationChanged(GeoPosition geoPosition)
    {
      if (this.PositionChanged == null)
        return;
      this.PositionChanged((object) this, new PositionChangedEventArgs(geoPosition));
    }

    protected virtual void OnStatusUpdated(GeoPositionStatus status)
    {
      if (this.StatusUpdated == null)
        return;
      this.StatusUpdated((object) this, new PositionWatcherStatusChangedEventArgs(status));
    }
  }
}
