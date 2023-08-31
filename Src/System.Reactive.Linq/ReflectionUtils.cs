// Decompiled with JetBrains decompiler
// Type: System.Reactive.ReflectionUtils
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Globalization;
using System.Reflection;

namespace System.Reactive
{
  internal static class ReflectionUtils
  {
    public static TDelegate CreateDelegate<TDelegate>(object o, MethodInfo method) => (TDelegate) Delegate.CreateDelegate(typeof (TDelegate), o, method);

    public static Delegate CreateDelegate(Type delegateType, object o, MethodInfo method) => Delegate.CreateDelegate(delegateType, o, method);

    public static void GetEventMethods<TSender, TEventArgs>(
      Type targetType,
      object target,
      string eventName,
      out MethodInfo addMethod,
      out MethodInfo removeMethod,
      out Type delegateType,
      out bool isWinRT)
      where TEventArgs : EventArgs
    {
      EventInfo eventEx;
      if (target == null)
      {
        eventEx = targetType.GetEventEx(eventName, true);
        if ((object) eventEx == null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_STATIC_EVENT, new object[2]
          {
            (object) eventName,
            (object) targetType.FullName
          }));
      }
      else
      {
        eventEx = targetType.GetEventEx(eventName, false);
        if ((object) eventEx == null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.COULD_NOT_FIND_INSTANCE_EVENT, new object[2]
          {
            (object) eventName,
            (object) targetType.FullName
          }));
      }
      addMethod = eventEx.GetAddMethod();
      removeMethod = eventEx.GetRemoveMethod();
      if ((object) addMethod == null)
        throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_ADD_METHOD);
      if ((object) removeMethod == null)
        throw new InvalidOperationException(Strings_Linq.EVENT_MISSING_REMOVE_METHOD);
      ParameterInfo[] parameters1 = addMethod.GetParameters();
      if (parameters1.Length != 1)
        throw new InvalidOperationException(Strings_Linq.EVENT_ADD_METHOD_SHOULD_TAKE_ONE_PARAMETER);
      if (removeMethod.GetParameters().Length != 1)
        throw new InvalidOperationException(Strings_Linq.EVENT_REMOVE_METHOD_SHOULD_TAKE_ONE_PARAMETER);
      isWinRT = false;
      delegateType = parameters1[0].ParameterType;
      MethodInfo method = delegateType.GetMethod("Invoke");
      ParameterInfo[] parameters2 = method.GetParameters();
      if (parameters2.Length != 2)
        throw new InvalidOperationException(Strings_Linq.EVENT_PATTERN_REQUIRES_TWO_PARAMETERS);
      if (!typeof (TSender).IsAssignableFrom(parameters2[0].ParameterType))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.EVENT_SENDER_NOT_ASSIGNABLE, new object[1]
        {
          (object) typeof (TSender).FullName
        }));
      if (!typeof (TEventArgs).IsAssignableFrom(parameters2[1].ParameterType))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.EVENT_ARGS_NOT_ASSIGNABLE, new object[1]
        {
          (object) typeof (TEventArgs).FullName
        }));
      if ((object) method.ReturnType != (object) typeof (void))
        throw new InvalidOperationException(Strings_Linq.EVENT_MUST_RETURN_VOID);
    }

    public static EventInfo GetEventEx(this Type type, string name, bool isStatic) => type.GetEvent(name, isStatic ? BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public);
  }
}
