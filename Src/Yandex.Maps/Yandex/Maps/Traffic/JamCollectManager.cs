// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamCollectManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Common;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Traffic.DTO.Collect;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Positioning;
using Yandex.Positioning.Interfaces;
using Yandex.WebUtils.Events;

namespace Yandex.Maps.Traffic
{
  internal class JamCollectManager : IJamCollectManager
  {
    private const int PositionAccuracityDelta = 70;
    private readonly TimeSpan _speedInterval = TimeSpan.FromSeconds(20.0);
    private readonly IConfigMediator _configMediator;
    private readonly IPositionWatcher _positionWatcher;
    private readonly IJamCollectSender _jamCollectSender;
    private readonly Timer _scanTimer;
    private readonly Timer _sendTimer;
    private readonly List<JamCollectPointData> _jamCollectPointsCahce;
    private List<GeoPosition> _speedPoints;
    private readonly object _speedPointsSync = new object();
    private int _currentSendTimeoutInMilliseconds;

    public JamCollectManager(
      IConfigMediator configMediator,
      IPositionWatcher positionWatcher,
      IJamCollectSender jamCollectSender)
    {
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
      this._positionWatcher = positionWatcher != null ? positionWatcher : throw new ArgumentNullException(nameof (positionWatcher));
      this._jamCollectSender = jamCollectSender != null ? jamCollectSender : throw new ArgumentNullException(nameof (jamCollectSender));
      this._jamCollectSender.RequestFailed += new EventHandler<RequestFailedEventArgs<JamCollectSenderParameters>>(this.JamCollectSenderRequestFailed);
      this._jamCollectSender.RequestCompleted += new RequestCompletedEventHandler<JamCollectSenderParameters, JamCollectPoints>(this.JamCollectSenderRequestCompleted);
      this._jamCollectPointsCahce = new List<JamCollectPointData>();
      this._speedPoints = new List<GeoPosition>();
      this._scanTimer = new Timer(new TimerCallback(this.ScanTimerTick), (object) null, -1, -1);
      this._sendTimer = new Timer(new TimerCallback(this.SendTimerTick), (object) null, -1, -1);
    }

    private void JamCollectSenderRequestCompleted(
      object sender,
      RequestCompletedEventArgs<JamCollectSenderParameters, JamCollectPoints> e)
    {
      this.SetSendTimerToDefaultTimout();
    }

    private void JamCollectSenderRequestFailed(
      object sender,
      RequestFailedEventArgs<JamCollectSenderParameters> e)
    {
      if (this._currentSendTimeoutInMilliseconds < -1)
      {
        this._currentSendTimeoutInMilliseconds *= 2;
        this._sendTimer.Change(this._currentSendTimeoutInMilliseconds, this._currentSendTimeoutInMilliseconds);
      }
      lock (this._jamCollectPointsCahce)
        this._jamCollectPointsCahce.AddRange((IEnumerable<JamCollectPointData>) e.Parameters.JamCollectPoints);
    }

    public void Activate()
    {
      if (this._configMediator.JamCollectConfig == null || !this._configMediator.JamCollectConfig.Enabled)
        return;
      this._scanTimer.Change(0, (int) this._configMediator.JamCollectConfig.ScanTimeout.TotalMilliseconds);
      this.SetSendTimerToDefaultTimout();
      this._positionWatcher.PositionChanged += new EventHandler<PositionChangedEventArgs>(this.PositionChanged);
    }

    private void SetSendTimerToDefaultTimout()
    {
      this._currentSendTimeoutInMilliseconds = (int) this._configMediator.JamCollectConfig.SendTimeout.TotalMilliseconds;
      this._sendTimer.Change(this._currentSendTimeoutInMilliseconds, this._currentSendTimeoutInMilliseconds);
    }

    private static bool IsPositionAccurate(GeoPosition position) => position.HorizontalAccuracy < 70.0 && position.VerticalAccuracy < 70.0;

    private double CalcAverageSpeed()
    {
      lock (this._speedPointsSync)
      {
        long count = 0;
        return this._speedPoints.Aggregate<GeoPosition, double>(0.0, (Func<double, GeoPosition, double>) ((current, x) => current + (x.Speed - current) / (double) ++count));
      }
    }

    private void PositionChanged(object sender, PositionChangedEventArgs e)
    {
      GeoPosition currentPosition = e.GeoPosition;
      lock (this._speedPointsSync)
      {
        if (!double.IsNaN(currentPosition.Speed))
          this._speedPoints.Add(currentPosition);
        this._speedPoints = this._speedPoints.SkipWhile<GeoPosition>((Func<GeoPosition, bool>) (p => currentPosition.TimeStamp.Subtract(p.TimeStamp) > this._speedInterval)).ToList<GeoPosition>();
      }
    }

    private void ScanTimerTick(object sender)
    {
      if (!this._configMediator.CollectJamInformation || !this._positionWatcher.Enabled || this._positionWatcher.Status != GeoPositionStatus.Ready || this._positionWatcher.LastPosition == null || !JamCollectManager.IsPositionAccurate(this._positionWatcher.LastPosition))
        return;
      JamCollectPointData collectPointData = new JamCollectPointData()
      {
        Lat = this._positionWatcher.LastPosition.GeoCoordinate.Latitude,
        Lon = this._positionWatcher.LastPosition.GeoCoordinate.Longitude,
        AvgSpeed = this.CalcAverageSpeed(),
        Direction = this._positionWatcher.LastPosition.Course,
        Time = this._positionWatcher.LastPosition.TimeStamp
      };
      lock (this._jamCollectPointsCahce)
        this._jamCollectPointsCahce.Add(collectPointData);
    }

    private void SendTimerTick(object sender)
    {
      List<JamCollectPointData> list;
      lock (this._jamCollectPointsCahce)
      {
        TimeSpan errorTimeout = this._configMediator.JamCollectConfig.ErrorTimeout;
        list = this._jamCollectPointsCahce.Where<JamCollectPointData>((Func<JamCollectPointData, bool>) (p => DateTimeOffset.Now.Subtract(p.Time) < errorTimeout)).ToList<JamCollectPointData>();
        this._jamCollectPointsCahce.Clear();
      }
      if (!list.Any<JamCollectPointData>())
        return;
      this._jamCollectSender.Request(new JamCollectSenderParameters()
      {
        JamCollectPoints = list
      });
    }
  }
}
