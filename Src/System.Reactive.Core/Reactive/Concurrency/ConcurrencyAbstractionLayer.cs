// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.PlatformServices;

namespace System.Reactive.Concurrency
{
  internal static class ConcurrencyAbstractionLayer
  {
    private static Lazy<IConcurrencyAbstractionLayer> s_current = new Lazy<IConcurrencyAbstractionLayer>(new Func<IConcurrencyAbstractionLayer>(ConcurrencyAbstractionLayer.Initialize));

    public static IConcurrencyAbstractionLayer Current => ConcurrencyAbstractionLayer.s_current.Value;

    private static IConcurrencyAbstractionLayer Initialize() => PlatformEnlightenmentProvider.Current.GetService<IConcurrencyAbstractionLayer>() ?? (IConcurrencyAbstractionLayer) new DefaultConcurrencyAbstractionLayer();
  }
}
