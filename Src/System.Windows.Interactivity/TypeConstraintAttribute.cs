// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TypeConstraintAttribute
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class TypeConstraintAttribute : Attribute
  {
    public Type Constraint { get; private set; }

    public TypeConstraintAttribute(Type constraint) => this.Constraint = constraint;
  }
}
