// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.BaseTypeRequiredAttribute
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  [BaseTypeRequired(typeof (Attribute))]
  internal sealed class BaseTypeRequiredAttribute : Attribute
  {
    private readonly Type[] myBaseTypes;

    public BaseTypeRequiredAttribute(Type baseType) => this.myBaseTypes = new Type[1]
    {
      baseType
    };

    public IEnumerable<Type> BaseTypes => (IEnumerable<Type>) this.myBaseTypes;
  }
}
