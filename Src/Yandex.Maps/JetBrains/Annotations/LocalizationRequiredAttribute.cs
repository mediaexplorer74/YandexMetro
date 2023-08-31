// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.LocalizationRequiredAttribute
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  internal sealed class LocalizationRequiredAttribute : Attribute
  {
    public LocalizationRequiredAttribute(bool required) => this.Required = required;

    public bool Required { get; set; }

    public override bool Equals(object obj) => obj is LocalizationRequiredAttribute requiredAttribute && requiredAttribute.Required == this.Required;

    public override int GetHashCode() => base.GetHashCode();
  }
}
