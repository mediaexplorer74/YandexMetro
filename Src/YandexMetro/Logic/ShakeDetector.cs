// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.ShakeDetector
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Devices.Sensors;
using System;

namespace Yandex.Metro.Logic
{
  public class ShakeDetector
  {
    private const double MinimumAccelerationMagnitude = 1.2;
    private const double MinimumAccelerationMagnitudeSquared = 1.44;
    private Accelerometer _accelerometer;
    private readonly object _syncRoot = new object();
    private int _minimumShakes;
    private ShakeDetector.ShakeRecord[] _shakeRecordList;
    private int _shakeRecordIndex;
    private static readonly TimeSpan MinimumShakeTime = TimeSpan.FromMilliseconds(500.0);

    public event EventHandler<EventArgs> ShakeEvent;

    protected void OnShakeEvent()
    {
      if (this.ShakeEvent == null)
        return;
      this.ShakeEvent((object) this, new EventArgs());
    }

    public ShakeDetector()
      : this(2)
    {
    }

    public ShakeDetector(int minShakes)
    {
      this._minimumShakes = minShakes;
      this._shakeRecordList = new ShakeDetector.ShakeRecord[minShakes];
    }

    public void Start()
    {
      lock (this._syncRoot)
      {
        if (this._accelerometer != null)
          return;
        this._accelerometer = new Accelerometer();
        this._accelerometer.ReadingChanged += new EventHandler<AccelerometerReadingEventArgs>(this._accelerometer_ReadingChanged);
        this._accelerometer.Start();
      }
    }

    public void Stop()
    {
      lock (this._syncRoot)
      {
        if (this._accelerometer == null)
          return;
        this._accelerometer.Stop();
        this._accelerometer.ReadingChanged -= new EventHandler<AccelerometerReadingEventArgs>(this._accelerometer_ReadingChanged);
        this._accelerometer = (Accelerometer) null;
      }
    }

    private ShakeDetector.Direction DegreesToDirection(double direction)
    {
      if (direction >= 337.5 || direction <= 22.5)
        return ShakeDetector.Direction.North;
      if (direction <= 67.5)
        return ShakeDetector.Direction.NorthEast;
      if (direction <= 112.5)
        return ShakeDetector.Direction.East;
      if (direction <= 157.5)
        return ShakeDetector.Direction.SouthEast;
      if (direction <= 202.5)
        return ShakeDetector.Direction.South;
      if (direction <= 247.5)
        return ShakeDetector.Direction.SouthWest;
      return direction <= 292.5 ? ShakeDetector.Direction.West : ShakeDetector.Direction.NorthWest;
    }

    private void CheckForShakes()
    {
      int index = this._shakeRecordIndex - 1;
      if (index < 0)
        index = this._minimumShakes - 1;
      if (!(this._shakeRecordList[this._shakeRecordIndex].EventTime.Subtract(this._shakeRecordList[index].EventTime) <= ShakeDetector.MinimumShakeTime))
        return;
      this.OnShakeEvent();
    }

    private void _accelerometer_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
    {
      if (e.X * e.X + e.Y * e.Y <= 1.44)
        return;
      ShakeDetector.Direction direction = this.DegreesToDirection(180.0 * Math.Atan2(e.Y, e.X) / Math.PI);
      if ((direction & this._shakeRecordList[this._shakeRecordIndex].ShakeDirection) != ShakeDetector.Direction.None)
        return;
      ShakeDetector.ShakeRecord shakeRecord = new ShakeDetector.ShakeRecord();
      shakeRecord.EventTime = DateTime.Now;
      shakeRecord.ShakeDirection = direction;
      this._shakeRecordIndex = (this._shakeRecordIndex + 1) % this._minimumShakes;
      this._shakeRecordList[this._shakeRecordIndex] = shakeRecord;
      this.CheckForShakes();
    }

    [Flags]
    public enum Direction
    {
      None = 0,
      North = 1,
      South = 2,
      East = 8,
      West = 4,
      NorthWest = West | North, // 0x00000005
      SouthWest = West | South, // 0x00000006
      SouthEast = East | South, // 0x0000000A
      NorthEast = East | North, // 0x00000009
    }

    public struct ShakeRecord
    {
      public ShakeDetector.Direction ShakeDirection;
      public DateTime EventTime;
    }
  }
}
