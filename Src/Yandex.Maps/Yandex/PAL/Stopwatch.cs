// Decompiled with JetBrains decompiler
// Type: Yandex.PAL.Stopwatch
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.PAL.Interfaces;

namespace Yandex.PAL
{
  internal class Stopwatch : IStopwatch
  {
    private System.Diagnostics.Stopwatch _systemStopWatch;

    public Stopwatch() => this._systemStopWatch = new System.Diagnostics.Stopwatch();

    public void Start() => this._systemStopWatch.Start();

    public void Stop() => this._systemStopWatch.Stop();

    public void Reset() => this._systemStopWatch.Reset();

    public void Restart()
    {
      this._systemStopWatch.Reset();
      this._systemStopWatch.Start();
    }

    public double ElapsedMilliseconds => (double) this._systemStopWatch.ElapsedMilliseconds;

    public TimeSpan Elapsed => this._systemStopWatch.Elapsed;
  }
}
