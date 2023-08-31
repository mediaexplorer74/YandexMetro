// Decompiled with JetBrains decompiler
// Type: System.Windows.FrameworkElementExtensions
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Y.UI.Common;

namespace System.Windows
{
  public static class FrameworkElementExtensions
  {
    public static BasePage FindPageBaseParent(this FrameworkElement parent)
    {
      if (parent == null)
        return (BasePage) null;
      return parent is BasePage ? (BasePage) parent : (parent.Parent as FrameworkElement).FindPageBaseParent();
    }
  }
}
