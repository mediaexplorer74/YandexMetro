// Decompiled with JetBrains decompiler
// Type: Yandex.Common.Timer
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Common
{
  public class Timer : IDisposable
  {
    private readonly System.Threading.Timer _timer;

    public Timer(TimerCallback callback, object state, int dueTime, int period) => this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), state, dueTime, period);

    public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period) => this._timer = new System.Threading.Timer((System.Threading.TimerCallback) (o => callback(o)), state, dueTime, period);

    public void Dispose() => this._timer.Dispose();

    public bool Change(int dueTime, int period) => this._timer.Change(dueTime, period);

    public bool Change(TimeSpan dueTime, TimeSpan period) => this._timer.Change(dueTime, period);
  }
}
