// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.DefaultTriggerAttribute
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Collections;
using System.Globalization;

namespace System.Windows.Interactivity
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
  [CLSCompliant(false)]
  public sealed class DefaultTriggerAttribute : Attribute
  {
    private Type targetType;
    private Type triggerType;
    private object[] parameters;

    public Type TargetType => this.targetType;

    public Type TriggerType => this.triggerType;

    public IEnumerable Parameters => (IEnumerable) this.parameters;

    public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter)
      : this(targetType, triggerType, new object[1]
      {
        parameter
      })
    {
    }

    public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
    {
      if (!typeof (TriggerBase).IsAssignableFrom(triggerType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, new object[1]
        {
          (object) triggerType.Name
        }));
      this.targetType = targetType;
      this.triggerType = triggerType;
      this.parameters = parameters;
    }

    public TriggerBase Instantiate()
    {
      object obj = (object) null;
      try
      {
        obj = Activator.CreateInstance(this.TriggerType, this.parameters);
      }
      catch
      {
      }
      return (TriggerBase) obj;
    }
  }
}
