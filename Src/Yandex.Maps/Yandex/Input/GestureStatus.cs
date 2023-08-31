// Decompiled with JetBrains decompiler
// Type: Yandex.Input.GestureStatus
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Input.Events;
using Yandex.Media;

namespace Yandex.Input
{
  internal struct GestureStatus
  {
    private const int ManipulationVelocitiesCount = 3;
    private object _manipulationVelocitiesLock;

    public bool IsPending { get; set; }

    public Point StartPoint { get; set; }

    public double StartTimestamp { get; set; }

    public double LastFrameTimestamp { get; set; }

    public bool TapIsPending { get; set; }

    public bool DoubleTapIsPending { get; set; }

    public double DoubleTapStartTimestamp { get; set; }

    public Point DoubleTapStartPoint { get; set; }

    public bool MultiFingerTapIsPending { get; set; }

    public Point? LastPoint { get; set; }

    public double LastScaleDistance { get; set; }

    public double FirstScaleDistance { get; set; }

    public Queue<TouchEventArgs> TapFramesQueue { get; set; }

    public bool MultiTapIsPossible { get; set; }

    public int LastPointsCount { get; set; }

    public Point CumulativeTranslation { get; set; }

    public double CumulativeScaleDelta { get; set; }

    public Queue<Point> ManipulationVelocities { get; private set; }

    public void Initialize(TouchEventArgs e)
    {
      this.IsPending = true;
      this.StartTimestamp = e.TimestampMilliseconds;
      this.StartPoint = e.PrimaryTouchPoint.Position;
      this.TapIsPending = true;
      this.DoubleTapIsPending = false;
      if (this.TapFramesQueue == null)
        this.TapFramesQueue = new Queue<TouchEventArgs>();
      this.TapFramesQueue.Clear();
      this.LastFrameTimestamp = 0.0;
      this.MultiFingerTapIsPending = false;
      this.MultiTapIsPossible = true;
      this.LastPoint = new Point?();
      this.CumulativeTranslation = new Point();
      this.CumulativeScaleDelta = 1.0;
      this.FirstScaleDistance = 0.0;
      this.LastScaleDistance = 0.0;
      this.LastPointsCount = 0;
      this._manipulationVelocitiesLock = new object();
      this.ManipulationVelocities = new Queue<Point>();
    }

    public void EnqueueVelocity(Point velocity)
    {
      lock (this._manipulationVelocitiesLock)
      {
        this.ManipulationVelocities.Enqueue(velocity);
        if (this.ManipulationVelocities.Count <= 3)
          return;
        this.ManipulationVelocities.Dequeue();
      }
    }
  }
}
