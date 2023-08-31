// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.BaseTypeRequiredAttribute
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections.Generic;

namespace JetBrains.Annotations
{
  [BaseTypeRequired(typeof (Attribute))]
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  public sealed class BaseTypeRequiredAttribute : Attribute
  {
    private readonly Type[] myBaseTypes;

    public BaseTypeRequiredAttribute(Type baseType) => this.myBaseTypes = new Type[1]
    {
      baseType
    };

    public IEnumerable<Type> BaseTypes => (IEnumerable<Type>) this.myBaseTypes;
  }
}
