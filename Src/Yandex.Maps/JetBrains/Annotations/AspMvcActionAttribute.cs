﻿// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.AspMvcActionAttribute
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
  internal sealed class AspMvcActionAttribute : Attribute
  {
    public string AnonymousProperty { get; private set; }

    public AspMvcActionAttribute()
    {
    }

    public AspMvcActionAttribute(string anonymousProperty) => this.AnonymousProperty = anonymousProperty;
  }
}
