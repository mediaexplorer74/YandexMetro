// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.TimerStubs
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Threading;

namespace System.Reactive.Concurrency
{
  internal static class TimerStubs
  {
    public static readonly System.Threading.Timer Never = new System.Threading.Timer((TimerCallback) (_ => { }));
  }
}
