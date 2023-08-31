// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocSingleton`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Ioc
{
  internal abstract class IocSingleton<TIoc> where TIoc : IocInitializer, new()
  {
    protected static readonly object LockObj = new object();
    protected static TIoc _singleInstance = new TIoc();

    private static TIoc GetInstance() => IocSingleton<TIoc>._singleInstance;

    public static IIocContainer Container => IocSingleton<TIoc>.GetInstance().Container;

    public static T Resolve<T>() where T : class => IocSingleton<TIoc>.Container.Resolve<T>();

    public static T Resolve<T>(string key) where T : class => IocSingleton<TIoc>.Container.Resolve<T>(key);
  }
}
