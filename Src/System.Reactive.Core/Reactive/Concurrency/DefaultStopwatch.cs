// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DefaultStopwatch
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Diagnostics;

namespace System.Reactive.Concurrency
{
  internal class DefaultStopwatch : IStopwatch
  {
    private readonly Stopwatch _sw;

    public DefaultStopwatch() => this._sw = Stopwatch.StartNew();

    public TimeSpan Elapsed => this._sw.Elapsed;
  }
}
