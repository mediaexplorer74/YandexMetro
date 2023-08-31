// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.AssertionConditionAttribute
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
  public sealed class AssertionConditionAttribute : Attribute
  {
    private readonly AssertionConditionType myConditionType;

    public AssertionConditionAttribute(AssertionConditionType conditionType) => this.myConditionType = conditionType;

    public AssertionConditionType ConditionType => this.myConditionType;
  }
}
