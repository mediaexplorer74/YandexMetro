// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.Phone.Shell.PhoneApplicationService
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using Microsoft.Phone;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Reactive.PlatformServices.Phone.Shell
{
  internal class PhoneApplicationService
  {
    private static readonly object s_gate = new object();
    internal static readonly Assembly s_phshAsm = typeof (BackgroundAgent).Assembly;
    private static readonly Type s_pasType = PhoneApplicationService.s_phshAsm.GetType("Microsoft.Phone.Shell.PhoneApplicationService");
    private static readonly PropertyInfo s_curProp = PhoneApplicationService.s_pasType.GetProperty(nameof (Current), BindingFlags.Static | BindingFlags.Public);
    private static readonly EventInfo s_actdEvt = PhoneApplicationService.s_curProp.PropertyType.GetEvent("Activated", BindingFlags.Instance | BindingFlags.Public);
    private static readonly EventInfo s_deacEvt = PhoneApplicationService.s_curProp.PropertyType.GetEvent("Deactivated", BindingFlags.Instance | BindingFlags.Public);
    private readonly object _target;
    private static PhoneApplicationService s_current;
    private Dictionary<object, object> _actdHandlers = new Dictionary<object, object>();
    private Dictionary<object, object> _deacHandlers = new Dictionary<object, object>();

    private PhoneApplicationService(object target) => this._target = target;

    public static PhoneApplicationService Current
    {
      get
      {
        lock (PhoneApplicationService.s_gate)
        {
          if (PhoneApplicationService.s_current == null)
          {
            object target = PhoneApplicationService.s_curProp.GetValue((object) null, (object[]) null);
            if (target == null)
              return (PhoneApplicationService) null;
            PhoneApplicationService.s_current = new PhoneApplicationService(target);
          }
        }
        return PhoneApplicationService.s_current;
      }
    }

    public event EventHandler<ActivatedEventArgs> Activated
    {
      add => this.AddHandler<ActivatedEventArgs>(PhoneApplicationService.s_actdEvt, this._actdHandlers, (object) value);
      remove => this.RemoveHandler(PhoneApplicationService.s_actdEvt, this._actdHandlers, (object) value);
    }

    public event EventHandler<DeactivatedEventArgs> Deactivated
    {
      add => this.AddHandler<DeactivatedEventArgs>(PhoneApplicationService.s_deacEvt, this._deacHandlers, (object) value);
      remove => this.RemoveHandler(PhoneApplicationService.s_deacEvt, this._deacHandlers, (object) value);
    }

    private void AddHandler<TEventArgs>(
      EventInfo evt,
      Dictionary<object, object> map,
      object handler)
      where TEventArgs : EventArgs
    {
      object handler1 = PhoneApplicationService.GetHandler<TEventArgs>(evt, handler);
      MethodInfo addMethod = evt.GetAddMethod();
      lock (PhoneApplicationService.s_gate)
      {
        map.Add(handler, handler1);
        addMethod.Invoke(this._target, new object[1]
        {
          handler1
        });
      }
    }

    private void RemoveHandler(EventInfo evt, Dictionary<object, object> map, object handler)
    {
      MethodInfo removeMethod = evt.GetRemoveMethod();
      lock (PhoneApplicationService.s_gate)
      {
        object obj = (object) null;
        if (!map.TryGetValue(handler, out obj))
          return;
        map.Remove(handler);
        removeMethod.Invoke(this._target, new object[1]
        {
          obj
        });
      }
    }

    private static object GetHandler<TEventArgsThunk>(EventInfo evt, object call) where TEventArgsThunk : EventArgs
    {
      Type eventHandlerType = evt.EventHandlerType;
      ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
      ParameterExpression parameterExpression1 = Expression.Parameter(parameters[0].ParameterType, parameters[0].Name);
      ParameterExpression parameterExpression2 = Expression.Parameter(parameters[1].ParameterType, parameters[1].Name);
      return (object) Expression.Lambda(eventHandlerType, (Expression) Expression.Invoke((Expression) Expression.Constant(call), (Expression) parameterExpression1, (Expression) Expression.New(typeof (TEventArgsThunk).GetConstructor(new Type[1]
      {
        typeof (object)
      }), (Expression) parameterExpression2)), parameterExpression1, parameterExpression2).Compile();
    }
  }
}
