// Decompiled with JetBrains decompiler
// Type: Yandex.Ioc.IocInjectionConstructor
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System.Collections.Generic;

namespace Yandex.Ioc
{
  public class IocInjectionConstructor : IIocInjectionMember
  {
    public IocInjectionConstructor(Dictionary<string, object> parameterValues) => this.Parameters = parameterValues;

    public Dictionary<string, object> Parameters { get; set; }
  }
}
