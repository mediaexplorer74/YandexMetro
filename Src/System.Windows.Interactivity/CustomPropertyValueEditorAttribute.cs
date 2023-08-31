// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.CustomPropertyValueEditorAttribute
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class CustomPropertyValueEditorAttribute : Attribute
  {
    public CustomPropertyValueEditor CustomPropertyValueEditor { get; private set; }

    public CustomPropertyValueEditorAttribute(
      CustomPropertyValueEditor customPropertyValueEditor)
    {
      this.CustomPropertyValueEditor = customPropertyValueEditor;
    }
  }
}
