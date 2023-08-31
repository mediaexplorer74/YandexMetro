// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCContainer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TinyIoC
{
  internal sealed class TinyIoCContainer : IDisposable
  {
    private static readonly TinyIoCContainer _Current = new TinyIoCContainer();
    private readonly SafeDictionary<TinyIoCContainer.TypeRegistration, TinyIoCContainer.ObjectFactoryBase> _RegisteredTypes;
    private TinyIoCContainer _Parent;
    private readonly object _AutoRegisterLock = new object();
    private bool disposed;

    public TinyIoCContainer GetChildContainer() => new TinyIoCContainer(this);

    public void AutoRegister() => this.AutoRegisterInternal((IEnumerable<Assembly>) new Assembly[1]
    {
      this.GetType().Assembly()
    }, true, (Func<Type, bool>) null);

    public void AutoRegister(Func<Type, bool> registrationPredicate) => this.AutoRegisterInternal((IEnumerable<Assembly>) new Assembly[1]
    {
      this.GetType().Assembly()
    }, true, registrationPredicate);

    public void AutoRegister(bool ignoreDuplicateImplementations) => this.AutoRegisterInternal((IEnumerable<Assembly>) new Assembly[1]
    {
      this.GetType().Assembly()
    }, ignoreDuplicateImplementations, (Func<Type, bool>) null);

    public void AutoRegister(
      bool ignoreDuplicateImplementations,
      Func<Type, bool> registrationPredicate)
    {
      this.AutoRegisterInternal((IEnumerable<Assembly>) new Assembly[1]
      {
        this.GetType().Assembly()
      }, ignoreDuplicateImplementations, registrationPredicate);
    }

    public void AutoRegister(IEnumerable<Assembly> assemblies) => this.AutoRegisterInternal(assemblies, true, (Func<Type, bool>) null);

    public void AutoRegister(
      IEnumerable<Assembly> assemblies,
      Func<Type, bool> registrationPredicate)
    {
      this.AutoRegisterInternal(assemblies, true, registrationPredicate);
    }

    public void AutoRegister(IEnumerable<Assembly> assemblies, bool ignoreDuplicateImplementations) => this.AutoRegisterInternal(assemblies, ignoreDuplicateImplementations, (Func<Type, bool>) null);

    public void AutoRegister(
      IEnumerable<Assembly> assemblies,
      bool ignoreDuplicateImplementations,
      Func<Type, bool> registrationPredicate)
    {
      this.AutoRegisterInternal(assemblies, ignoreDuplicateImplementations, registrationPredicate);
    }

    public TinyIoCContainer.RegisterOptions Register(Type registerType) => this.RegisterInternal(registerType, string.Empty, this.GetDefaultObjectFactory(registerType, registerType));

    public TinyIoCContainer.RegisterOptions Register(Type registerType, string name) => this.RegisterInternal(registerType, name, this.GetDefaultObjectFactory(registerType, registerType));

    public TinyIoCContainer.RegisterOptions Register(Type registerType, Type registerImplementation) => this.RegisterInternal(registerType, string.Empty, this.GetDefaultObjectFactory(registerType, registerImplementation));

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      Type registerImplementation,
      string name)
    {
      return this.RegisterInternal(registerType, name, this.GetDefaultObjectFactory(registerType, registerImplementation));
    }

    public TinyIoCContainer.RegisterOptions Register(Type registerType, object instance) => this.RegisterInternal(registerType, string.Empty, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.InstanceFactory(registerType, registerType, instance));

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      object instance,
      string name)
    {
      return this.RegisterInternal(registerType, name, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.InstanceFactory(registerType, registerType, instance));
    }

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      Type registerImplementation,
      object instance)
    {
      return this.RegisterInternal(registerType, string.Empty, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.InstanceFactory(registerType, registerImplementation, instance));
    }

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      Type registerImplementation,
      object instance,
      string name)
    {
      return this.RegisterInternal(registerType, name, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.InstanceFactory(registerType, registerImplementation, instance));
    }

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
    {
      return this.RegisterInternal(registerType, string.Empty, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.DelegateFactory(registerType, factory));
    }

    public TinyIoCContainer.RegisterOptions Register(
      Type registerType,
      Func<TinyIoCContainer, NamedParameterOverloads, object> factory,
      string name)
    {
      return this.RegisterInternal(registerType, name, (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.DelegateFactory(registerType, factory));
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType>() where RegisterType : class => this.Register(typeof (RegisterType));

    public TinyIoCContainer.RegisterOptions Register<RegisterType>(string name) where RegisterType : class => this.Register(typeof (RegisterType), name);

    public TinyIoCContainer.RegisterOptions Register<RegisterType, RegisterImplementation>()
      where RegisterType : class
      where RegisterImplementation : class, RegisterType
    {
      return this.Register(typeof (RegisterType), typeof (RegisterImplementation));
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType, RegisterImplementation>(
      string name)
      where RegisterType : class
      where RegisterImplementation : class, RegisterType
    {
      return this.Register(typeof (RegisterType), typeof (RegisterImplementation), name);
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType>(RegisterType instance) where RegisterType : class => this.Register(typeof (RegisterType), (object) instance);

    public TinyIoCContainer.RegisterOptions Register<RegisterType>(
      RegisterType instance,
      string name)
      where RegisterType : class
    {
      return this.Register(typeof (RegisterType), (object) instance, name);
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType, RegisterImplementation>(
      RegisterImplementation instance)
      where RegisterType : class
      where RegisterImplementation : class, RegisterType
    {
      return this.Register(typeof (RegisterType), typeof (RegisterImplementation), (object) instance);
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType, RegisterImplementation>(
      RegisterImplementation instance,
      string name)
      where RegisterType : class
      where RegisterImplementation : class, RegisterType
    {
      return this.Register(typeof (RegisterType), typeof (RegisterImplementation), (object) instance, name);
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType>(
      Func<TinyIoCContainer, NamedParameterOverloads, RegisterType> factory)
      where RegisterType : class
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      return this.Register(typeof (RegisterType), (Func<TinyIoCContainer, NamedParameterOverloads, object>) ((c, o) => (object) factory(c, o)));
    }

    public TinyIoCContainer.RegisterOptions Register<RegisterType>(
      Func<TinyIoCContainer, NamedParameterOverloads, RegisterType> factory,
      string name)
      where RegisterType : class
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      return this.Register(typeof (RegisterType), (Func<TinyIoCContainer, NamedParameterOverloads, object>) ((c, o) => (object) factory(c, o)), name);
    }

    public TinyIoCContainer.MultiRegisterOptions RegisterMultiple<RegisterType>(
      IEnumerable<Type> implementationTypes)
    {
      return this.RegisterMultiple(typeof (RegisterType), implementationTypes);
    }

    public TinyIoCContainer.MultiRegisterOptions RegisterMultiple(
      Type registrationType,
      IEnumerable<Type> implementationTypes)
    {
      if (implementationTypes == null)
        throw new ArgumentNullException("types", "types is null.");
      foreach (Type implementationType in implementationTypes)
      {
        if (!registrationType.IsAssignableFrom(implementationType))
          throw new ArgumentException(string.Format("types: The type {0} is not assignable from {1}", (object) registrationType.FullName, (object) implementationType.FullName));
      }
      if (implementationTypes.Count<Type>() != implementationTypes.Distinct<Type>().Count<Type>())
        throw new ArgumentException("types: The same implementation type cannot be specificed multiple times");
      List<TinyIoCContainer.RegisterOptions> registerOptions = new List<TinyIoCContainer.RegisterOptions>();
      foreach (Type implementationType in implementationTypes)
        registerOptions.Add(this.Register(registrationType, implementationType, implementationType.FullName));
      return new TinyIoCContainer.MultiRegisterOptions((IEnumerable<TinyIoCContainer.RegisterOptions>) registerOptions);
    }

    public object Resolve(Type resolveType) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), NamedParameterOverloads.Default, ResolveOptions.Default);

    public object Resolve(Type resolveType, ResolveOptions options) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), NamedParameterOverloads.Default, options);

    public object Resolve(Type resolveType, string name) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), NamedParameterOverloads.Default, ResolveOptions.Default);

    public object Resolve(Type resolveType, string name, ResolveOptions options) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), NamedParameterOverloads.Default, options);

    public object Resolve(Type resolveType, NamedParameterOverloads parameters) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), parameters, ResolveOptions.Default);

    public object Resolve(
      Type resolveType,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      return this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), parameters, options);
    }

    public object Resolve(Type resolveType, string name, NamedParameterOverloads parameters) => this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), parameters, ResolveOptions.Default);

    public object Resolve(
      Type resolveType,
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      return this.ResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), parameters, options);
    }

    public ResolveType Resolve<ResolveType>() where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType));

    public ResolveType Resolve<ResolveType>(ResolveOptions options) where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType), options);

    public ResolveType Resolve<ResolveType>(string name) where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType), name);

    public ResolveType Resolve<ResolveType>(string name, ResolveOptions options) where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType), name, options);

    public ResolveType Resolve<ResolveType>(NamedParameterOverloads parameters) where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType), parameters);

    public ResolveType Resolve<ResolveType>(
      NamedParameterOverloads parameters,
      ResolveOptions options)
      where ResolveType : class
    {
      return (ResolveType) this.Resolve(typeof (ResolveType), parameters, options);
    }

    public ResolveType Resolve<ResolveType>(string name, NamedParameterOverloads parameters) where ResolveType : class => (ResolveType) this.Resolve(typeof (ResolveType), name, parameters);

    public ResolveType Resolve<ResolveType>(
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options)
      where ResolveType : class
    {
      return (ResolveType) this.Resolve(typeof (ResolveType), name, parameters, options);
    }

    public bool CanResolve(Type resolveType) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), NamedParameterOverloads.Default, ResolveOptions.Default);

    private bool CanResolve(Type resolveType, string name) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), NamedParameterOverloads.Default, ResolveOptions.Default);

    public bool CanResolve(Type resolveType, ResolveOptions options) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), NamedParameterOverloads.Default, options);

    public bool CanResolve(Type resolveType, string name, ResolveOptions options) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), NamedParameterOverloads.Default, options);

    public bool CanResolve(Type resolveType, NamedParameterOverloads parameters) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), parameters, ResolveOptions.Default);

    public bool CanResolve(Type resolveType, string name, NamedParameterOverloads parameters) => this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), parameters, ResolveOptions.Default);

    public bool CanResolve(
      Type resolveType,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      return this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType), parameters, options);
    }

    public bool CanResolve(
      Type resolveType,
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      return this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(resolveType, name), parameters, options);
    }

    public bool CanResolve<ResolveType>() where ResolveType : class => this.CanResolve(typeof (ResolveType));

    public bool CanResolve<ResolveType>(string name) where ResolveType : class => this.CanResolve(typeof (ResolveType), name);

    public bool CanResolve<ResolveType>(ResolveOptions options) where ResolveType : class => this.CanResolve(typeof (ResolveType), options);

    public bool CanResolve<ResolveType>(string name, ResolveOptions options) where ResolveType : class => this.CanResolve(typeof (ResolveType), name, options);

    public bool CanResolve<ResolveType>(NamedParameterOverloads parameters) where ResolveType : class => this.CanResolve(typeof (ResolveType), parameters);

    public bool CanResolve<ResolveType>(string name, NamedParameterOverloads parameters) where ResolveType : class => this.CanResolve(typeof (ResolveType), name, parameters);

    public bool CanResolve<ResolveType>(NamedParameterOverloads parameters, ResolveOptions options) where ResolveType : class => this.CanResolve(typeof (ResolveType), parameters, options);

    public bool CanResolve<ResolveType>(
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options)
      where ResolveType : class
    {
      return this.CanResolve(typeof (ResolveType), name, parameters, options);
    }

    public bool TryResolve(Type resolveType, out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(Type resolveType, ResolveOptions options, out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(Type resolveType, string name, out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, name);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(
      Type resolveType,
      string name,
      ResolveOptions options,
      out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, name, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(
      Type resolveType,
      NamedParameterOverloads parameters,
      out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, parameters);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(
      Type resolveType,
      string name,
      NamedParameterOverloads parameters,
      out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, name, parameters);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(
      Type resolveType,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, parameters, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve(
      Type resolveType,
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      out object resolvedType)
    {
      try
      {
        resolvedType = this.Resolve(resolveType, name, parameters, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = (object) null;
        return false;
      }
    }

    public bool TryResolve<ResolveType>(out ResolveType resolvedType) where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>();
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(ResolveOptions options, out ResolveType resolvedType) where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(string name, out ResolveType resolvedType) where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(name);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(
      string name,
      ResolveOptions options,
      out ResolveType resolvedType)
      where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(name, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(
      NamedParameterOverloads parameters,
      out ResolveType resolvedType)
      where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(parameters);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(
      string name,
      NamedParameterOverloads parameters,
      out ResolveType resolvedType)
      where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(name, parameters);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(
      NamedParameterOverloads parameters,
      ResolveOptions options,
      out ResolveType resolvedType)
      where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(parameters, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public bool TryResolve<ResolveType>(
      string name,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      out ResolveType resolvedType)
      where ResolveType : class
    {
      try
      {
        resolvedType = this.Resolve<ResolveType>(name, parameters, options);
        return true;
      }
      catch (TinyIoCResolutionException ex)
      {
        resolvedType = default (ResolveType);
        return false;
      }
    }

    public IEnumerable<object> ResolveAll(Type resolveType, bool includeUnnamed) => this.ResolveAllInternal(resolveType, includeUnnamed);

    public IEnumerable<object> ResolveAll(Type resolveType) => this.ResolveAll(resolveType, false);

    public IEnumerable<ResolveType> ResolveAll<ResolveType>(bool includeUnnamed) where ResolveType : class => this.ResolveAll(typeof (ResolveType), includeUnnamed).Cast<ResolveType>();

    public IEnumerable<ResolveType> ResolveAll<ResolveType>() where ResolveType : class => this.ResolveAll<ResolveType>(true);

    public void BuildUp(object input) => this.BuildUpInternal(input, ResolveOptions.Default);

    public void BuildUp(object input, ResolveOptions resolveOptions) => this.BuildUpInternal(input, resolveOptions);

    public static TinyIoCContainer Current => TinyIoCContainer._Current;

    public TinyIoCContainer()
    {
      this._RegisteredTypes = new SafeDictionary<TinyIoCContainer.TypeRegistration, TinyIoCContainer.ObjectFactoryBase>();
      this.RegisterDefaultTypes();
    }

    private TinyIoCContainer(TinyIoCContainer parent)
      : this()
    {
      this._Parent = parent;
    }

    private void AutoRegisterInternal(
      IEnumerable<Assembly> assemblies,
      bool ignoreDuplicateImplementations,
      Func<Type, bool> registrationPredicate)
    {
      lock (this._AutoRegisterLock)
      {
        List<Type> list = assemblies.SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (a => (IEnumerable<Type>) a.SafeGetTypes())).Where<Type>((Func<Type, bool>) (t => !this.IsIgnoredType(t, registrationPredicate))).ToList<Type>();
        IEnumerable<Type> source = list.Where<Type>((Func<Type, bool>) (type => type.IsClass() && !type.IsAbstract() && type != this.GetType() && type.DeclaringType != this.GetType() && !type.IsGenericTypeDefinition()));
        foreach (Type type in source)
        {
          try
          {
            this.RegisterInternal(type, string.Empty, this.GetDefaultObjectFactory(type, type));
          }
          catch (MemberAccessException ex)
          {
          }
        }
        foreach (Type type1 in list.Where<Type>((Func<Type, bool>) (type => (type.IsInterface() || type.IsAbstract()) && type.DeclaringType != this.GetType() && !type.IsGenericTypeDefinition())))
        {
          Type type = type1;
          IEnumerable<Type> types = source.Where<Type>((Func<Type, bool>) (implementationType => ((IEnumerable<Type>) implementationType.GetInterfaces()).Contains<Type>(type) || implementationType.BaseType() == type));
          Type registerImplementation = ignoreDuplicateImplementations || types.Count<Type>() <= 1 ? types.FirstOrDefault<Type>() : throw new TinyIoCAutoRegistrationException(type, types);
          if (registerImplementation != null)
          {
            try
            {
              this.RegisterInternal(type, string.Empty, this.GetDefaultObjectFactory(type, registerImplementation));
            }
            catch (MemberAccessException ex)
            {
            }
          }
        }
      }
    }

    private bool IsIgnoredAssembly(Assembly assembly)
    {
      foreach (Func<Assembly, bool> func in new List<Func<Assembly, bool>>()
      {
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("Microsoft.", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("System.", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("System,", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("CR_ExtUnitTest", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("mscorlib,", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("CR_VSTest", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("DevExpress.CodeRush", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("IronPython", StringComparison.Ordinal)),
        (Func<Assembly, bool>) (asm => asm.FullName.StartsWith("IronRuby", StringComparison.Ordinal))
      })
      {
        if (func(assembly))
          return true;
      }
      return false;
    }

    private bool IsIgnoredType(Type type, Func<Type, bool> registrationPredicate)
    {
      List<Func<Type, bool>> funcList = new List<Func<Type, bool>>()
      {
        (Func<Type, bool>) (t => t.FullName.StartsWith("System.", StringComparison.Ordinal)),
        (Func<Type, bool>) (t => t.FullName.StartsWith("Microsoft.", StringComparison.Ordinal)),
        (Func<Type, bool>) (t => t.IsPrimitive()),
        (Func<Type, bool>) (t => t.IsGenericTypeDefinition),
        (Func<Type, bool>) (t => t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length == 0 && !t.IsInterface() && !t.IsAbstract())
      };
      if (registrationPredicate != null)
        funcList.Add((Func<Type, bool>) (t => !registrationPredicate(t)));
      foreach (Func<Type, bool> func in funcList)
      {
        if (func(type))
          return true;
      }
      return false;
    }

    private void RegisterDefaultTypes() => this.Register<TinyIoCContainer>(this);

    private TinyIoCContainer.ObjectFactoryBase GetCurrentFactory(
      TinyIoCContainer.TypeRegistration registration)
    {
      TinyIoCContainer.ObjectFactoryBase currentFactory = (TinyIoCContainer.ObjectFactoryBase) null;
      this._RegisteredTypes.TryGetValue(registration, out currentFactory);
      return currentFactory;
    }

    private TinyIoCContainer.RegisterOptions RegisterInternal(
      Type registerType,
      string name,
      TinyIoCContainer.ObjectFactoryBase factory)
    {
      return this.AddUpdateRegistration(new TinyIoCContainer.TypeRegistration(registerType, name), factory);
    }

    private TinyIoCContainer.RegisterOptions AddUpdateRegistration(
      TinyIoCContainer.TypeRegistration typeRegistration,
      TinyIoCContainer.ObjectFactoryBase factory)
    {
      this._RegisteredTypes[typeRegistration] = factory;
      return new TinyIoCContainer.RegisterOptions(this, typeRegistration);
    }

    private void RemoveRegistration(TinyIoCContainer.TypeRegistration typeRegistration) => this._RegisteredTypes.Remove(typeRegistration);

    private TinyIoCContainer.ObjectFactoryBase GetDefaultObjectFactory(
      Type registerType,
      Type registerImplementation)
    {
      return registerType.IsInterface() || registerType.IsAbstract() ? (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.SingletonFactory(registerType, registerImplementation) : (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.MultiInstanceFactory(registerType, registerImplementation);
    }

    private bool CanResolveInternal(
      TinyIoCContainer.TypeRegistration registration,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      IList<Type> circularTypes = null)
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      Type type = registration.Type;
      string name = registration.Name;
      TinyIoCContainer.ObjectFactoryBase objectFactoryBase;
      if (this._RegisteredTypes.TryGetValue(new TinyIoCContainer.TypeRegistration(type, name), out objectFactoryBase))
      {
        if (objectFactoryBase.AssumeConstruction)
          return true;
        return objectFactoryBase.Constructor == null ? this.GetBestConstructor(objectFactoryBase.CreatesType, parameters, options, circularTypes) != null : this.CanConstruct(objectFactoryBase.Constructor, parameters, options);
      }
      if (!string.IsNullOrEmpty(name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.Fail)
        return this._Parent != null && this._Parent.CanResolveInternal(registration, parameters, options);
      if (!string.IsNullOrEmpty(name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.AttemptUnnamedResolution && this._RegisteredTypes.TryGetValue(new TinyIoCContainer.TypeRegistration(type), out objectFactoryBase))
        return objectFactoryBase.AssumeConstruction || this.GetBestConstructor(objectFactoryBase.CreatesType, parameters, options) != null;
      if (this.IsAutomaticLazyFactoryRequest(type) || this.IsIEnumerableRequest(registration.Type))
        return true;
      if (options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve || type.IsGenericType() && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly)
      {
        if (this.GetBestConstructor(type, parameters, options) != null)
          return true;
        return this._Parent != null && this._Parent.CanResolveInternal(registration, parameters, options);
      }
      return this._Parent != null && this._Parent.CanResolveInternal(registration, parameters, options);
    }

    private bool IsIEnumerableRequest(Type type) => type.IsGenericType() && type.GetGenericTypeDefinition() == typeof (IEnumerable<>);

    private bool IsAutomaticLazyFactoryRequest(Type type)
    {
      if (!type.IsGenericType())
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      return genericTypeDefinition == typeof (Func<>) || genericTypeDefinition == typeof (Func<,>) && type.GetGenericArguments()[0] == typeof (string) || genericTypeDefinition == typeof (Func<,,>) && type.GetGenericArguments()[0] == typeof (string) && type.GetGenericArguments()[1] == typeof (IDictionary<string, object>);
    }

    private TinyIoCContainer.ObjectFactoryBase GetParentObjectFactory(
      TinyIoCContainer.TypeRegistration registration)
    {
      if (this._Parent == null)
        return (TinyIoCContainer.ObjectFactoryBase) null;
      TinyIoCContainer.ObjectFactoryBase objectFactoryBase;
      return this._Parent._RegisteredTypes.TryGetValue(registration, out objectFactoryBase) ? objectFactoryBase.GetFactoryForChildContainer(registration.Type, this._Parent, this) : this._Parent.GetParentObjectFactory(registration);
    }

    private object ResolveInternal(
      TinyIoCContainer.TypeRegistration registration,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      TinyIoCContainer.ObjectFactoryBase objectFactoryBase;
      if (this._RegisteredTypes.TryGetValue(registration, out objectFactoryBase))
      {
        try
        {
          return objectFactoryBase.GetObject(registration.Type, this, parameters, options);
        }
        catch (TinyIoCResolutionException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          throw new TinyIoCResolutionException(registration.Type, ex);
        }
      }
      else
      {
        if (registration.Type.IsGenericType())
        {
          if (this._RegisteredTypes.TryGetValue(new TinyIoCContainer.TypeRegistration(registration.Type.GetGenericTypeDefinition(), registration.Name), out objectFactoryBase))
          {
            try
            {
              return objectFactoryBase.GetObject(registration.Type, this, parameters, options);
            }
            catch (TinyIoCResolutionException ex)
            {
              throw;
            }
            catch (Exception ex)
            {
              throw new TinyIoCResolutionException(registration.Type, ex);
            }
          }
        }
        TinyIoCContainer.ObjectFactoryBase parentObjectFactory = this.GetParentObjectFactory(registration);
        if (parentObjectFactory != null)
        {
          try
          {
            return parentObjectFactory.GetObject(registration.Type, this, parameters, options);
          }
          catch (TinyIoCResolutionException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new TinyIoCResolutionException(registration.Type, ex);
          }
        }
        else
        {
          if (!string.IsNullOrEmpty(registration.Name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.Fail)
            throw new TinyIoCResolutionException(registration.Type);
          if (!string.IsNullOrEmpty(registration.Name) && options.NamedResolutionFailureAction == NamedResolutionFailureActions.AttemptUnnamedResolution)
          {
            if (this._RegisteredTypes.TryGetValue(new TinyIoCContainer.TypeRegistration(registration.Type, string.Empty), out objectFactoryBase))
            {
              try
              {
                return objectFactoryBase.GetObject(registration.Type, this, parameters, options);
              }
              catch (TinyIoCResolutionException ex)
              {
                throw;
              }
              catch (Exception ex)
              {
                throw new TinyIoCResolutionException(registration.Type, ex);
              }
            }
          }
          if (this.IsAutomaticLazyFactoryRequest(registration.Type))
            return this.GetLazyAutomaticFactoryRequest(registration.Type);
          if (this.IsIEnumerableRequest(registration.Type))
            return this.GetIEnumerableRequest(registration.Type);
          if ((options.UnregisteredResolutionAction == UnregisteredResolutionActions.AttemptResolve || registration.Type.IsGenericType() && options.UnregisteredResolutionAction == UnregisteredResolutionActions.GenericsOnly) && !registration.Type.IsAbstract() && !registration.Type.IsInterface())
            return this.ConstructType((Type) null, registration.Type, parameters, options);
          throw new TinyIoCResolutionException(registration.Type);
        }
      }
    }

    private object GetLazyAutomaticFactoryRequest(Type type)
    {
      if (!type.IsGenericType())
        return (object) null;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      Type[] genericArguments = type.GetGenericArguments();
      if (genericTypeDefinition == typeof (Func<>))
      {
        Type type1 = genericArguments[0];
        MethodInfo method = typeof (TinyIoCContainer).GetMethod("Resolve", new Type[0]).MakeGenericMethod(type1);
        return (object) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Constant((object) this), method)).Compile();
      }
      if (genericTypeDefinition == typeof (Func<,>) && genericArguments[0] == typeof (string))
      {
        Type type2 = genericArguments[1];
        MethodInfo method = typeof (TinyIoCContainer).GetMethod("Resolve", new Type[1]
        {
          typeof (string)
        }).MakeGenericMethod(type2);
        ParameterExpression[] parameterExpressionArray = new ParameterExpression[1]
        {
          Expression.Parameter(typeof (string), "name")
        };
        return (object) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Constant((object) this), method, (Expression[]) parameterExpressionArray), parameterExpressionArray).Compile();
      }
      if (genericTypeDefinition != typeof (Func<,,>) || type.GetGenericArguments()[0] != typeof (string) || type.GetGenericArguments()[1] != typeof (IDictionary<string, object>))
        throw new TinyIoCResolutionException(type);
      Type type3 = genericArguments[2];
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (string), "name");
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (IDictionary<string, object>), "parameters");
      MethodInfo method1 = typeof (TinyIoCContainer).GetMethod("Resolve", new Type[2]
      {
        typeof (string),
        typeof (NamedParameterOverloads)
      }).MakeGenericMethod(type3);
      return (object) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Constant((object) this), method1, (Expression) parameterExpression1, (Expression) Expression.Call(typeof (NamedParameterOverloads), "FromIDictionary", (Type[]) null, (Expression) parameterExpression2)), parameterExpression1, parameterExpression2).Compile();
    }

    private object GetIEnumerableRequest(Type type) => this.GetType().GetGenericMethod(BindingFlags.Instance | BindingFlags.Public, "ResolveAll", type.GetGenericArguments(), new Type[1]
    {
      typeof (bool)
    }).Invoke((object) this, new object[1]{ (object) false });

    private bool CanConstruct(
      ConstructorInfo ctor,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      IList<Type> circularTypes = null)
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      foreach (ParameterInfo parameter in ctor.GetParameters())
      {
        if (string.IsNullOrEmpty(parameter.Name))
          return false;
        bool flag = parameters.ContainsKey(parameter.Name);
        if (parameter.ParameterType.IsPrimitive() && !flag || !flag && !this.CanResolveInternal(new TinyIoCContainer.TypeRegistration(parameter.ParameterType), NamedParameterOverloads.Default, options, circularTypes))
          return false;
      }
      return true;
    }

    private ConstructorInfo GetBestConstructor(
      Type type,
      NamedParameterOverloads parameters,
      ResolveOptions options,
      IList<Type> circularTypes = null)
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      if (type.IsValueType())
        return (ConstructorInfo) null;
      if (circularTypes != null)
      {
        if (circularTypes.Contains(type))
          return (ConstructorInfo) null;
        circularTypes = (IList<Type>) circularTypes.ToList<Type>();
        circularTypes.Add(type);
      }
      return this.GetTypeConstructors(type).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (ctor => this.CanConstruct(ctor, parameters, options, circularTypes)));
    }

    private IEnumerable<ConstructorInfo> GetTypeConstructors(Type type) => (IEnumerable<ConstructorInfo>) ((IEnumerable<ConstructorInfo>) type.GetConstructors()).OrderByDescending<ConstructorInfo, int>((Func<ConstructorInfo, int>) (ctor => ((IEnumerable<ParameterInfo>) ctor.GetParameters()).Count<ParameterInfo>()));

    private object ConstructType(
      Type requestedType,
      Type implementationType,
      ResolveOptions options)
    {
      return this.ConstructType(requestedType, implementationType, (ConstructorInfo) null, NamedParameterOverloads.Default, options);
    }

    private object ConstructType(
      Type requestedType,
      Type implementationType,
      ConstructorInfo constructor,
      ResolveOptions options)
    {
      return this.ConstructType(requestedType, implementationType, constructor, NamedParameterOverloads.Default, options);
    }

    private object ConstructType(
      Type requestedType,
      Type implementationType,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      return this.ConstructType(requestedType, implementationType, (ConstructorInfo) null, parameters, options);
    }

    private object ConstructType(
      Type requestedType,
      Type implementationType,
      ConstructorInfo constructor,
      NamedParameterOverloads parameters,
      ResolveOptions options)
    {
      Type type = implementationType;
      if (implementationType.IsGenericTypeDefinition())
        type = requestedType != null && requestedType.IsGenericType() && ((IEnumerable<Type>) requestedType.GetGenericArguments()).Any<Type>() ? type.MakeGenericType(requestedType.GetGenericArguments()) : throw new TinyIoCResolutionException(type);
      if (constructor == null)
      {
        IList<Type> circularTypes = (IList<Type>) null;
        constructor = this.GetBestConstructor(type, parameters, options, circularTypes) ?? this.GetTypeConstructors(type).LastOrDefault<ConstructorInfo>();
      }
      ParameterInfo[] source = constructor != null ? constructor.GetParameters() : throw new TinyIoCResolutionException(type);
      object[] parameters1 = new object[((IEnumerable<ParameterInfo>) source).Count<ParameterInfo>()];
      for (int index = 0; index < ((IEnumerable<ParameterInfo>) source).Count<ParameterInfo>(); ++index)
      {
        ParameterInfo parameterInfo = source[index];
        try
        {
          parameters1[index] = parameters.ContainsKey(parameterInfo.Name) ? parameters[parameterInfo.Name] : this.ResolveInternal(new TinyIoCContainer.TypeRegistration(parameterInfo.ParameterType), NamedParameterOverloads.Default, options);
        }
        catch (TinyIoCResolutionException ex)
        {
          throw new TinyIoCResolutionException(type, (Exception) ex);
        }
        catch (Exception ex)
        {
          throw new TinyIoCResolutionException(type, ex);
        }
      }
      try
      {
        return constructor.Invoke(parameters1);
      }
      catch (Exception ex)
      {
        throw new TinyIoCResolutionException(type, ex);
      }
    }

    private void BuildUpInternal(object input, ResolveOptions resolveOptions)
    {
      foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) input.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (property => property.GetGetMethod() != null && property.GetSetMethod() != null && !property.PropertyType.IsValueType())))
      {
        if (propertyInfo.GetValue(input, (object[]) null) == null)
        {
          try
          {
            propertyInfo.SetValue(input, this.ResolveInternal(new TinyIoCContainer.TypeRegistration(propertyInfo.PropertyType), NamedParameterOverloads.Default, resolveOptions), (object[]) null);
          }
          catch (TinyIoCResolutionException ex)
          {
          }
        }
      }
    }

    private IEnumerable<TinyIoCContainer.TypeRegistration> GetParentRegistrationsForType(
      Type resolveType)
    {
      return this._Parent == null ? (IEnumerable<TinyIoCContainer.TypeRegistration>) new TinyIoCContainer.TypeRegistration[0] : this._Parent._RegisteredTypes.Keys.Where<TinyIoCContainer.TypeRegistration>((Func<TinyIoCContainer.TypeRegistration, bool>) (tr => tr.Type == resolveType)).Concat<TinyIoCContainer.TypeRegistration>(this._Parent.GetParentRegistrationsForType(resolveType));
    }

    private IEnumerable<object> ResolveAllInternal(Type resolveType, bool includeUnnamed)
    {
      IEnumerable<TinyIoCContainer.TypeRegistration> source = this._RegisteredTypes.Keys.Where<TinyIoCContainer.TypeRegistration>((Func<TinyIoCContainer.TypeRegistration, bool>) (tr => tr.Type == resolveType)).Concat<TinyIoCContainer.TypeRegistration>(this.GetParentRegistrationsForType(resolveType));
      if (!includeUnnamed)
        source = source.Where<TinyIoCContainer.TypeRegistration>((Func<TinyIoCContainer.TypeRegistration, bool>) (tr => !string.IsNullOrEmpty(tr.Name)));
      return source.Select<TinyIoCContainer.TypeRegistration, object>((Func<TinyIoCContainer.TypeRegistration, object>) (registration => this.ResolveInternal(registration, NamedParameterOverloads.Default, ResolveOptions.Default)));
    }

    private static bool IsValidAssignment(Type registerType, Type registerImplementation)
    {
      if (!registerType.IsGenericTypeDefinition())
      {
        if (!registerType.IsAssignableFrom(registerImplementation))
          return false;
      }
      else if (registerType.IsInterface())
      {
        if (registerImplementation.GetInterface(registerType.Name, false) == null)
          return false;
      }
      else if (registerType.IsAbstract() && registerImplementation.BaseType() != registerType)
        return false;
      return true;
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      this.disposed = true;
      this._RegisteredTypes.Dispose();
      GC.SuppressFinalize((object) this);
    }

    public sealed class RegisterOptions
    {
      private TinyIoCContainer _Container;
      private TinyIoCContainer.TypeRegistration _Registration;

      public RegisterOptions(
        TinyIoCContainer container,
        TinyIoCContainer.TypeRegistration registration)
      {
        this._Container = container;
        this._Registration = registration;
      }

      public TinyIoCContainer.RegisterOptions AsSingleton() => this._Container.AddUpdateRegistration(this._Registration, (this._Container.GetCurrentFactory(this._Registration) ?? throw new TinyIoCRegistrationException(this._Registration.Type, "singleton")).SingletonVariant);

      public TinyIoCContainer.RegisterOptions AsMultiInstance() => this._Container.AddUpdateRegistration(this._Registration, (this._Container.GetCurrentFactory(this._Registration) ?? throw new TinyIoCRegistrationException(this._Registration.Type, "multi-instance")).MultiInstanceVariant);

      public TinyIoCContainer.RegisterOptions WithWeakReference() => this._Container.AddUpdateRegistration(this._Registration, (this._Container.GetCurrentFactory(this._Registration) ?? throw new TinyIoCRegistrationException(this._Registration.Type, "weak reference")).WeakReferenceVariant);

      public TinyIoCContainer.RegisterOptions WithStrongReference() => this._Container.AddUpdateRegistration(this._Registration, (this._Container.GetCurrentFactory(this._Registration) ?? throw new TinyIoCRegistrationException(this._Registration.Type, "strong reference")).StrongReferenceVariant);

      public TinyIoCContainer.RegisterOptions UsingConstructor<RegisterType>(
        Expression<Func<RegisterType>> constructor)
      {
        LambdaExpression lambdaExpression = (LambdaExpression) constructor;
        if (lambdaExpression == null)
          throw new TinyIoCConstructorResolutionException(typeof (RegisterType));
        if (!(lambdaExpression.Body is NewExpression body))
          throw new TinyIoCConstructorResolutionException(typeof (RegisterType));
        ConstructorInfo constructor1 = body.Constructor;
        if (constructor1 == null)
          throw new TinyIoCConstructorResolutionException(typeof (RegisterType));
        (this._Container.GetCurrentFactory(this._Registration) ?? throw new TinyIoCConstructorResolutionException(typeof (RegisterType))).SetConstructor(constructor1);
        return this;
      }

      public static TinyIoCContainer.RegisterOptions ToCustomLifetimeManager(
        TinyIoCContainer.RegisterOptions instance,
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorString)
      {
        if (instance == null)
          throw new ArgumentNullException(nameof (instance), "instance is null.");
        if (lifetimeProvider == null)
          throw new ArgumentNullException(nameof (lifetimeProvider), "lifetimeProvider is null.");
        if (string.IsNullOrEmpty(errorString))
          throw new ArgumentException("errorString is null or empty.", nameof (errorString));
        return instance._Container.AddUpdateRegistration(instance._Registration, (instance._Container.GetCurrentFactory(instance._Registration) ?? throw new TinyIoCRegistrationException(instance._Registration.Type, errorString)).GetCustomObjectLifetimeVariant(lifetimeProvider, errorString));
      }
    }

    public sealed class MultiRegisterOptions
    {
      private IEnumerable<TinyIoCContainer.RegisterOptions> _RegisterOptions;

      public MultiRegisterOptions(
        IEnumerable<TinyIoCContainer.RegisterOptions> registerOptions)
      {
        this._RegisterOptions = registerOptions;
      }

      public TinyIoCContainer.MultiRegisterOptions AsSingleton()
      {
        this._RegisterOptions = this.ExecuteOnAllRegisterOptions((Func<TinyIoCContainer.RegisterOptions, TinyIoCContainer.RegisterOptions>) (ro => ro.AsSingleton()));
        return this;
      }

      public TinyIoCContainer.MultiRegisterOptions AsMultiInstance()
      {
        this._RegisterOptions = this.ExecuteOnAllRegisterOptions((Func<TinyIoCContainer.RegisterOptions, TinyIoCContainer.RegisterOptions>) (ro => ro.AsMultiInstance()));
        return this;
      }

      private IEnumerable<TinyIoCContainer.RegisterOptions> ExecuteOnAllRegisterOptions(
        Func<TinyIoCContainer.RegisterOptions, TinyIoCContainer.RegisterOptions> action)
      {
        List<TinyIoCContainer.RegisterOptions> registerOptionsList = new List<TinyIoCContainer.RegisterOptions>();
        foreach (TinyIoCContainer.RegisterOptions registerOption in this._RegisterOptions)
          registerOptionsList.Add(action(registerOption));
        return (IEnumerable<TinyIoCContainer.RegisterOptions>) registerOptionsList;
      }
    }

    public interface ITinyIoCObjectLifetimeProvider
    {
      object GetObject();

      void SetObject(object value);

      void ReleaseObject();
    }

    private abstract class ObjectFactoryBase
    {
      public virtual bool AssumeConstruction => false;

      public abstract Type CreatesType { get; }

      public ConstructorInfo Constructor { get; protected set; }

      public abstract object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options);

      public virtual TinyIoCContainer.ObjectFactoryBase SingletonVariant => throw new TinyIoCRegistrationException(this.GetType(), "singleton");

      public virtual TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant => throw new TinyIoCRegistrationException(this.GetType(), "multi-instance");

      public virtual TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant => throw new TinyIoCRegistrationException(this.GetType(), "strong reference");

      public virtual TinyIoCContainer.ObjectFactoryBase WeakReferenceVariant => throw new TinyIoCRegistrationException(this.GetType(), "weak reference");

      public virtual TinyIoCContainer.ObjectFactoryBase GetCustomObjectLifetimeVariant(
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorString)
      {
        throw new TinyIoCRegistrationException(this.GetType(), errorString);
      }

      public virtual void SetConstructor(ConstructorInfo constructor) => this.Constructor = constructor;

      public virtual TinyIoCContainer.ObjectFactoryBase GetFactoryForChildContainer(
        Type type,
        TinyIoCContainer parent,
        TinyIoCContainer child)
      {
        return this;
      }
    }

    private class MultiInstanceFactory : TinyIoCContainer.ObjectFactoryBase
    {
      private readonly Type registerType;
      private readonly Type registerImplementation;

      public override Type CreatesType => this.registerImplementation;

      public MultiInstanceFactory(Type registerType, Type registerImplementation)
      {
        if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
          throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (MultiInstanceFactory));
        this.registerType = TinyIoCContainer.IsValidAssignment(registerType, registerImplementation) ? registerType : throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (MultiInstanceFactory));
        this.registerImplementation = registerImplementation;
      }

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        try
        {
          return container.ConstructType(requestedType, this.registerImplementation, this.Constructor, parameters, options);
        }
        catch (TinyIoCResolutionException ex)
        {
          throw new TinyIoCResolutionException(this.registerType, (Exception) ex);
        }
      }

      public override TinyIoCContainer.ObjectFactoryBase SingletonVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.SingletonFactory(this.registerType, this.registerImplementation);

      public override TinyIoCContainer.ObjectFactoryBase GetCustomObjectLifetimeVariant(
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorString)
      {
        return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
      }

      public override TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant => (TinyIoCContainer.ObjectFactoryBase) this;
    }

    private class DelegateFactory : TinyIoCContainer.ObjectFactoryBase
    {
      protected readonly Type RegisterType;
      protected readonly Func<TinyIoCContainer, NamedParameterOverloads, object> Factory;

      public override bool AssumeConstruction => true;

      public override Type CreatesType => this.RegisterType;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        try
        {
          return this.Factory(container, parameters);
        }
        catch (Exception ex)
        {
          throw new TinyIoCResolutionException(this.RegisterType, ex);
        }
      }

      public DelegateFactory(
        Type registerType,
        Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
      {
        this.Factory = factory != null ? factory : throw new ArgumentNullException(nameof (factory));
        this.RegisterType = registerType;
      }

      public override TinyIoCContainer.ObjectFactoryBase WeakReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.WeakDelegateFactory(this.RegisterType, this.Factory);

      public override TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public override TinyIoCContainer.ObjectFactoryBase SingletonVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.SingletonDelegateFactory(this.RegisterType, this.Factory);

      public override void SetConstructor(ConstructorInfo constructor) => throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for delegate factory registrations");
    }

    private class SingletonDelegateFactory : TinyIoCContainer.DelegateFactory, IDisposable
    {
      private readonly object _singletonLock = new object();
      private object _current;

      public SingletonDelegateFactory(
        Type registerType,
        Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
        : base(registerType, factory)
      {
      }

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        lock (this._singletonLock)
          return this._current ?? (this._current = base.GetObject(requestedType, container, parameters, options));
      }

      public override TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.DelegateFactory(this.RegisterType, this.Factory);

      public override TinyIoCContainer.ObjectFactoryBase SingletonVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public void Dispose()
      {
        if (this._current == null || !(this._current is IDisposable current))
          return;
        current.Dispose();
      }
    }

    private class WeakDelegateFactory : TinyIoCContainer.ObjectFactoryBase
    {
      private readonly Type registerType;
      private WeakReference _factory;

      public override bool AssumeConstruction => true;

      public override Type CreatesType => this.registerType;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        if (!(this._factory.Target is Func<TinyIoCContainer, NamedParameterOverloads, object> target))
          throw new TinyIoCWeakReferenceException(this.registerType);
        try
        {
          return target(container, parameters);
        }
        catch (Exception ex)
        {
          throw new TinyIoCResolutionException(this.registerType, ex);
        }
      }

      public WeakDelegateFactory(
        Type registerType,
        Func<TinyIoCContainer, NamedParameterOverloads, object> factory)
      {
        this._factory = factory != null ? new WeakReference((object) factory) : throw new ArgumentNullException(nameof (factory));
        this.registerType = registerType;
      }

      public override TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant
      {
        get
        {
          if (!(this._factory.Target is Func<TinyIoCContainer, NamedParameterOverloads, object> target))
            throw new TinyIoCWeakReferenceException(this.registerType);
          return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.DelegateFactory(this.registerType, target);
        }
      }

      public override TinyIoCContainer.ObjectFactoryBase WeakReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public override void SetConstructor(ConstructorInfo constructor) => throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for delegate factory registrations");
    }

    private class InstanceFactory : TinyIoCContainer.ObjectFactoryBase, IDisposable
    {
      private readonly Type registerType;
      private readonly Type registerImplementation;
      private object _instance;

      public override bool AssumeConstruction => true;

      public InstanceFactory(Type registerType, Type registerImplementation, object instance)
      {
        this.registerType = TinyIoCContainer.IsValidAssignment(registerType, registerImplementation) ? registerType : throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (InstanceFactory));
        this.registerImplementation = registerImplementation;
        this._instance = instance;
      }

      public override Type CreatesType => this.registerImplementation;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        return this._instance;
      }

      public override TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.MultiInstanceFactory(this.registerType, this.registerImplementation);

      public override TinyIoCContainer.ObjectFactoryBase WeakReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.WeakInstanceFactory(this.registerType, this.registerImplementation, this._instance);

      public override TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public override void SetConstructor(ConstructorInfo constructor) => throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for instance factory registrations");

      public void Dispose()
      {
        if (!(this._instance is IDisposable instance))
          return;
        instance.Dispose();
      }
    }

    private class WeakInstanceFactory : TinyIoCContainer.ObjectFactoryBase, IDisposable
    {
      private readonly Type registerType;
      private readonly Type registerImplementation;
      private readonly WeakReference _instance;

      public WeakInstanceFactory(Type registerType, Type registerImplementation, object instance)
      {
        this.registerType = TinyIoCContainer.IsValidAssignment(registerType, registerImplementation) ? registerType : throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (WeakInstanceFactory));
        this.registerImplementation = registerImplementation;
        this._instance = new WeakReference(instance);
      }

      public override Type CreatesType => this.registerImplementation;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        return this._instance.Target ?? throw new TinyIoCWeakReferenceException(this.registerType);
      }

      public override TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.MultiInstanceFactory(this.registerType, this.registerImplementation);

      public override TinyIoCContainer.ObjectFactoryBase WeakReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public override TinyIoCContainer.ObjectFactoryBase StrongReferenceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.InstanceFactory(this.registerType, this.registerImplementation, this._instance.Target ?? throw new TinyIoCWeakReferenceException(this.registerType));

      public override void SetConstructor(ConstructorInfo constructor) => throw new TinyIoCConstructorResolutionException("Constructor selection is not possible for instance factory registrations");

      public void Dispose()
      {
        if (!(this._instance.Target is IDisposable target))
          return;
        target.Dispose();
      }
    }

    private class SingletonFactory : TinyIoCContainer.ObjectFactoryBase, IDisposable
    {
      private readonly Type registerType;
      private readonly Type registerImplementation;
      private readonly object SingletonLock = new object();
      private object _Current;

      public SingletonFactory(Type registerType, Type registerImplementation)
      {
        if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
          throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (SingletonFactory));
        this.registerType = TinyIoCContainer.IsValidAssignment(registerType, registerImplementation) ? registerType : throw new TinyIoCRegistrationTypeException(registerImplementation, nameof (SingletonFactory));
        this.registerImplementation = registerImplementation;
      }

      public override Type CreatesType => this.registerImplementation;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        if (parameters.Count != 0)
          throw new ArgumentException("Cannot specify parameters for singleton types");
        lock (this.SingletonLock)
        {
          if (this._Current == null)
            this._Current = container.ConstructType(requestedType, this.registerImplementation, this.Constructor, options);
        }
        return this._Current;
      }

      public override TinyIoCContainer.ObjectFactoryBase SingletonVariant => (TinyIoCContainer.ObjectFactoryBase) this;

      public override TinyIoCContainer.ObjectFactoryBase GetCustomObjectLifetimeVariant(
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorString)
      {
        return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
      }

      public override TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant => (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.MultiInstanceFactory(this.registerType, this.registerImplementation);

      public override TinyIoCContainer.ObjectFactoryBase GetFactoryForChildContainer(
        Type type,
        TinyIoCContainer parent,
        TinyIoCContainer child)
      {
        this.GetObject(type, parent, NamedParameterOverloads.Default, ResolveOptions.Default);
        return (TinyIoCContainer.ObjectFactoryBase) this;
      }

      public void Dispose()
      {
        if (this._Current == null || !(this._Current is IDisposable current))
          return;
        current.Dispose();
      }
    }

    private class CustomObjectLifetimeFactory : TinyIoCContainer.ObjectFactoryBase, IDisposable
    {
      private readonly object SingletonLock = new object();
      private readonly Type registerType;
      private readonly Type registerImplementation;
      private readonly TinyIoCContainer.ITinyIoCObjectLifetimeProvider _LifetimeProvider;

      public CustomObjectLifetimeFactory(
        Type registerType,
        Type registerImplementation,
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorMessage)
      {
        if (lifetimeProvider == null)
          throw new ArgumentNullException(nameof (lifetimeProvider), "lifetimeProvider is null.");
        if (!TinyIoCContainer.IsValidAssignment(registerType, registerImplementation))
          throw new TinyIoCRegistrationTypeException(registerImplementation, "SingletonFactory");
        if (registerImplementation.IsAbstract() || registerImplementation.IsInterface())
          throw new TinyIoCRegistrationTypeException(registerImplementation, errorMessage);
        this.registerType = registerType;
        this.registerImplementation = registerImplementation;
        this._LifetimeProvider = lifetimeProvider;
      }

      public override Type CreatesType => this.registerImplementation;

      public override object GetObject(
        Type requestedType,
        TinyIoCContainer container,
        NamedParameterOverloads parameters,
        ResolveOptions options)
      {
        object obj;
        lock (this.SingletonLock)
        {
          obj = this._LifetimeProvider.GetObject();
          if (obj == null)
          {
            obj = container.ConstructType(requestedType, this.registerImplementation, this.Constructor, options);
            this._LifetimeProvider.SetObject(obj);
          }
        }
        return obj;
      }

      public override TinyIoCContainer.ObjectFactoryBase SingletonVariant
      {
        get
        {
          this._LifetimeProvider.ReleaseObject();
          return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.SingletonFactory(this.registerType, this.registerImplementation);
        }
      }

      public override TinyIoCContainer.ObjectFactoryBase MultiInstanceVariant
      {
        get
        {
          this._LifetimeProvider.ReleaseObject();
          return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.MultiInstanceFactory(this.registerType, this.registerImplementation);
        }
      }

      public override TinyIoCContainer.ObjectFactoryBase GetCustomObjectLifetimeVariant(
        TinyIoCContainer.ITinyIoCObjectLifetimeProvider lifetimeProvider,
        string errorString)
      {
        this._LifetimeProvider.ReleaseObject();
        return (TinyIoCContainer.ObjectFactoryBase) new TinyIoCContainer.CustomObjectLifetimeFactory(this.registerType, this.registerImplementation, lifetimeProvider, errorString);
      }

      public override TinyIoCContainer.ObjectFactoryBase GetFactoryForChildContainer(
        Type type,
        TinyIoCContainer parent,
        TinyIoCContainer child)
      {
        this.GetObject(type, parent, NamedParameterOverloads.Default, ResolveOptions.Default);
        return (TinyIoCContainer.ObjectFactoryBase) this;
      }

      public void Dispose() => this._LifetimeProvider.ReleaseObject();
    }

    public sealed class TypeRegistration
    {
      private int _hashCode;

      public Type Type { get; private set; }

      public string Name { get; private set; }

      public TypeRegistration(Type type)
        : this(type, string.Empty)
      {
      }

      public TypeRegistration(Type type, string name)
      {
        this.Type = type;
        this.Name = name;
        this._hashCode = (this.Type.FullName + "|" + this.Name).GetHashCode();
      }

      public override bool Equals(object obj) => obj is TinyIoCContainer.TypeRegistration typeRegistration && this.Type == typeRegistration.Type && (string.IsNullOrEmpty(this.Name) && string.IsNullOrEmpty(typeRegistration.Name) || string.Compare(this.Name, typeRegistration.Name, StringComparison.Ordinal) == 0);

      public override int GetHashCode() => this._hashCode;
    }
  }
}
