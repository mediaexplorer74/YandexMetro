// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IIocContainer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Yandex.Ioc
{
  internal interface IIocContainer
  {
    IIocContainer Unbind<T>() where T : class;

    IIocContainer Register<T>(string key = null, bool isSingleton = false) where T : class;

    IIocContainer Register<T>(params IIocInjectionMember[] injectionMembers);

    IIocContainer Register<T>(string key, params IIocInjectionMember[] injectionMembers);

    IIocContainer Register<T>(
      string key,
      bool isSingleton,
      params IIocInjectionMember[] injectionMembers);

    IIocContainer Register<T>(Func<IIocContainer, T> factory, string key = null, bool isSingleton = false) where T : class;

    IIocContainer Register<TFrom, TTo>(string key = null, bool isSingleton = false)
      where TFrom : class
      where TTo : class, TFrom;

    IIocContainer Register<TFrom, TTo>(params IIocInjectionMember[] injectionMembers) where TTo : TFrom;

    IIocContainer Register<TFrom, TTo>(string key, params IIocInjectionMember[] injectionMembers) where TTo : TFrom;

    IIocContainer Register<TFrom, TTo>(
      string key,
      bool isSingleton,
      params IIocInjectionMember[] injectionMembers)
      where TTo : TFrom;

    IIocContainer Register<TFrom, TTo>(
      Expression<Func<TTo>> constructor,
      string key = null,
      bool isSingleton = false)
      where TFrom : class
      where TTo : class, TFrom;

    IIocContainer RegisterInstance<TInterface>(TInterface instance, string key = null) where TInterface : class;

    IIocContainer RegisterInstance(Type type, object instance, string key);

    T Resolve<T>(string key = null) where T : class;

    object Resolve(Type type, string key = null);

    T Resolve<T>([NotNull] IDictionary<string, object> parameters, string key = null) where T : class;

    object Resolve(Type type, [NotNull] IDictionary<string, object> parameters, string key = null);
  }
}
