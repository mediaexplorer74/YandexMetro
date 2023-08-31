// Decompiled with JetBrains decompiler
// Type: System.Reactive.ExceptionHelpers
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.PlatformServices;

namespace System.Reactive
{
  internal static class ExceptionHelpers
  {
    private static Lazy<IExceptionServices> s_services = new Lazy<IExceptionServices>(new Func<IExceptionServices>(ExceptionHelpers.Initialize));

    public static void Throw(this Exception exception) => ExceptionHelpers.s_services.Value.Rethrow(exception);

    public static void ThrowIfNotNull(this Exception exception)
    {
      if (exception == null)
        return;
      ExceptionHelpers.s_services.Value.Rethrow(exception);
    }

    private static IExceptionServices Initialize() => PlatformEnlightenmentProvider.Current.GetService<IExceptionServices>() ?? (IExceptionServices) new DefaultExceptionServices();
  }
}
