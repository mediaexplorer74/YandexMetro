// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.TinyIoCContainer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TinyIoC;

namespace Yandex.Ioc
{
  internal class TinyIoCContainer : IIocContainer
  {
    private readonly TinyIoC.TinyIoCContainer _container = new TinyIoC.TinyIoCContainer();

    public IIocContainer Unbind<T>() where T : class
    {
      this._container.Register<T>(default (T));
      return (IIocContainer) this;
    }

    public IIocContainer Register<T>(string key = null, bool isSingleton = false) where T : class
    {
      if (isSingleton)
        this._container.Register<T>(key).AsSingleton();
      else
        this._container.Register<T>(key).AsMultiInstance();
      return (IIocContainer) this;
    }

    public IIocContainer Register<T>(params IIocInjectionMember[] injectionMembers) => throw new NotSupportedException();

    public IIocContainer Register<T>(string key, params IIocInjectionMember[] injectionMembers) => throw new NotSupportedException();

    public IIocContainer Register<T>(
      string key,
      bool isSingleton,
      params IIocInjectionMember[] injectionMembers)
    {
      throw new NotSupportedException();
    }

    public IIocContainer Register<TFrom, TTo>(string key = null, bool isSingleton = false)
      where TFrom : class
      where TTo : class, TFrom
    {
      if (isSingleton)
        this._container.Register<TFrom, TTo>(key).AsSingleton();
      else
        this._container.Register<TFrom, TTo>(key).AsMultiInstance();
      return (IIocContainer) this;
    }

    public IIocContainer Register<TFrom, TTo>(params IIocInjectionMember[] injectionMembers) where TTo : TFrom => this.Register<TFrom, TTo>((string) null, injectionMembers);

    public IIocContainer Register<TFrom, TTo>(
      string key,
      params IIocInjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      return this.Register<TFrom, TTo>((string) null, false, injectionMembers);
    }

    public IIocContainer Register<TFrom, TTo>(
      string key,
      bool isSingleton,
      params IIocInjectionMember[] injectionMembers)
      where TTo : TFrom
    {
      throw new NotSupportedException();
    }

    public IIocContainer RegisterInstance<TInterface>(TInterface instance, string key = null) where TInterface : class
    {
      this._container.Register<TInterface>(instance, key);
      return (IIocContainer) this;
    }

    public IIocContainer RegisterInstance(Type type, object instance, string key)
    {
      this._container.Register(type, instance, key);
      return (IIocContainer) this;
    }

    public T Resolve<T>(string key = null) where T : class => this._container.Resolve<T>(key);

    public object Resolve(Type type, string key = null) => this._container.Resolve(type, key);

    public IIocContainer Register<T>(Func<IIocContainer, T> factory, string key = null, bool isSingleton = false) where T : class
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      TinyIoC.TinyIoCContainer.RegisterOptions registerOptions = this._container.Register<T>((Func<TinyIoC.TinyIoCContainer, NamedParameterOverloads, T>) ((container, x) => factory((IIocContainer) this)), key);
      if (isSingleton)
        registerOptions.AsSingleton();
      return (IIocContainer) this;
    }

    public IIocContainer Register<TFrom, TTo>(
      Expression<Func<TTo>> constructor,
      string key = null,
      bool isSingleton = false)
      where TFrom : class
      where TTo : class, TFrom
    {
      TinyIoC.TinyIoCContainer.RegisterOptions registerOptions = this._container.Register<TFrom, TTo>(key);
      if (isSingleton)
        registerOptions.AsSingleton();
      else
        registerOptions.AsMultiInstance();
      registerOptions.UsingConstructor<TTo>(constructor);
      return (IIocContainer) this;
    }

    public T Resolve<T>(IDictionary<string, object> parameters, string key = null) where T : class
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      return this._container.Resolve<T>(key, NamedParameterOverloads.FromIDictionary(parameters));
    }

    public object Resolve(Type type, IDictionary<string, object> parameters, string key = null)
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      return this._container.Resolve(type, key, NamedParameterOverloads.FromIDictionary(parameters));
    }
  }
}
