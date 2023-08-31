// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocInjectionProperty
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.Ioc
{
  public class IocInjectionProperty : IIocInjectionMember
  {
    public IocInjectionProperty(string propertyName, object propertyValue)
    {
      this.PropertyName = propertyName;
      this.PropertyValue = propertyValue;
    }

    public string PropertyName { get; set; }

    public object PropertyValue { get; set; }
  }
}
