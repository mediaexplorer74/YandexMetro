// Decompiled with JetBrains decompiler
// Type: Yandex.Common.Timer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Common
{
  internal class Timer : IDisposable
  {
    private static readonly TimeSpan _TimeoutMax = TimeSpan.FromMilliseconds(4294967294.0);
    private readonly System.Threading.Timer _timer;

    private static void LimitTimeSpan(ref TimeSpan time1, ref TimeSpan time2)
    {
      if (time1 > Timer._TimeoutMax)
        time1 = Timer._TimeoutMax;
      if (!(time2 > Timer._TimeoutMax))
        return;
      time2 = Timer._TimeoutMax;
    }

    public Timer(TimerCallback callback, object state, int dueTime, int period) => this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), state, dueTime, period);

    public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
    {
      Timer.LimitTimeSpan(ref dueTime, ref period);
      this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), state, dueTime, period);
    }

    public Timer(TimerCallback callback, object state) => this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), state, -1, -1);

    public Timer(TimerCallback callback) => this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), (object) null, -1, -1);

    public void Dispose() => this._timer.Dispose();

    public bool Change(int dueTime, int period) => this._timer.Change(dueTime, period);

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
      Timer.LimitTimeSpan(ref dueTime, ref period);
      return this._timer.Change(dueTime, period);
    }

    public bool Change(int dueTime) => this.Change(dueTime, -1);

    public bool Change(TimeSpan dueTime) => this.Change(dueTime, Timer._TimeoutMax);

    public void Stop() => this._timer.Change(-1, -1);
  }
}
