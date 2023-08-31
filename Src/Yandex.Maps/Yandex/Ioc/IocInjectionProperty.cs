// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocInjectionProperty
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Ioc
{
  internal class IocInjectionProperty : IIocInjectionMember
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
