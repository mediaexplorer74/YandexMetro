// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocSingleton`1
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

namespace Yandex.Ioc
{
  public abstract class IocSingleton<TIoc> where TIoc : IocInitializer, new()
  {
    protected static readonly object LockObj = new object();
    protected static TIoc _singleInstance = new TIoc();

    private static TIoc GetInstance() => IocSingleton<TIoc>._singleInstance;

    public static IIocContainer Container => IocSingleton<TIoc>.GetInstance().Container;

    public static T Resolve<T>() where T : class => IocSingleton<TIoc>.Container.Resolve<T>();

    public static T Resolve<T>(string key) where T : class => IocSingleton<TIoc>.Container.Resolve<T>(key);
  }
}
