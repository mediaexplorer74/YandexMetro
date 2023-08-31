// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.VisualTreeHelperExtensions
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Controls
{
  public static class VisualTreeHelperExtensions
  {
    public static FrameworkElement FindVisualChild(this FrameworkElement root, string name)
    {
      if (root.FindName(name) is FrameworkElement name1)
        return name1;
      foreach (FrameworkElement visualChild in root.GetVisualChildren())
      {
        if (visualChild.FindName(name) is FrameworkElement name2)
          return name2;
      }
      return (FrameworkElement) null;
    }

    public static FrameworkElement GetVisualParent(this FrameworkElement node) => VisualTreeHelper.GetParent((DependencyObject) node) as FrameworkElement;

    public static FrameworkElement GetVisualChild(this FrameworkElement node, int index) => VisualTreeHelper.GetChild((DependencyObject) node, index) as FrameworkElement;

    public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount((DependencyObject) root); ++i)
        yield return VisualTreeHelper.GetChild((DependencyObject) root, i) as FrameworkElement;
    }

    public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
    {
      for (FrameworkElement parent = node.GetVisualParent(); parent != null; parent = parent.GetVisualParent())
        yield return parent;
    }
  }
}
