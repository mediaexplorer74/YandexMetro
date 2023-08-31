// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocInitializer
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

namespace Yandex.Ioc
{
  public abstract class IocInitializer
  {
    private readonly IIocContainer _container = (IIocContainer) new TinyIoCContainer();

    public IocInitializer() => this.Initialize();

    protected abstract void Initialize();

    public IIocContainer Container => this._container;
  }
}
