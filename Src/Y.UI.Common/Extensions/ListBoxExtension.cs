// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Extensions.ListBoxExtension
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Y.UI.Common.Extensions
{
  public static class ListBoxExtension
  {
    public static void ScrollToTheTop(this ListBox target)
    {
      if (target == null)
        return;
      ScrollViewer scrollViewer = Enumerable.FirstOrDefault<ScrollViewer>(Enumerable.OfType<ScrollViewer>((IEnumerable) ((FrameworkElement) target).GetVisualChildren()));
      if (scrollViewer == null)
        return;
      ((UIElement) scrollViewer).UpdateLayout();
      scrollViewer.ScrollToVerticalOffset(0.0);
    }
  }
}
