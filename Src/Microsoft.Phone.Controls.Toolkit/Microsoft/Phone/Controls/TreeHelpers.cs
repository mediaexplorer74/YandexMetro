// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TreeHelpers
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  internal static class TreeHelpers
  {
    public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
    {
      for (FrameworkElement parent = node.GetVisualParent(); parent != null; parent = parent.GetVisualParent())
        yield return parent;
    }

    public static FrameworkElement GetVisualParent(this FrameworkElement node) => VisualTreeHelper.GetParent((DependencyObject) node) as FrameworkElement;
  }
}
