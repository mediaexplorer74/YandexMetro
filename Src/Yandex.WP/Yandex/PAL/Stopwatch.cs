// Decompiled with JetBrains decompiler
// Type: Yandex.PAL.Stopwatch
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using Yandex.PAL.Interfaces;

namespace Yandex.PAL
{
  public class Stopwatch : IStopwatch
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
