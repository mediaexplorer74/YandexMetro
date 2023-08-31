// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.EnlightenmentProvider
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

namespace System.Reactive.PlatformServices
{
  public static class EnlightenmentProvider
  {
    public static bool EnsureLoaded() => PlatformEnlightenmentProvider.Current is CurrentPlatformEnlightenmentProvider;
  }
}
