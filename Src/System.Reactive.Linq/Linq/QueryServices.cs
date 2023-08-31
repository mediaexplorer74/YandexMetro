// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.QueryServices
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.PlatformServices;

namespace System.Reactive.Linq
{
  internal static class QueryServices
  {
    private static Lazy<IQueryServices> s_services = new Lazy<IQueryServices>(new Func<IQueryServices>(QueryServices.Initialize));

    public static T GetQueryImpl<T>(T defaultInstance) => QueryServices.s_services.Value.Extend<T>(defaultInstance);

    private static IQueryServices Initialize() => PlatformEnlightenmentProvider.Current.GetService<IQueryServices>() ?? (IQueryServices) new DefaultQueryServices();
  }
}
