// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocInitializer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Ioc
{
  internal abstract class IocInitializer
  {
    private readonly IIocContainer _container = (IIocContainer) new TinyIoCContainer();

    public IocInitializer() => this.Initialize();

    protected abstract void Initialize();

    public IIocContainer Container => this._container;
  }
}
