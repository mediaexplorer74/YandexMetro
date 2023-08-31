// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Helpers.VisualTreeHelperEx
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Media;

namespace Yandex.Controls.Helpers
{
  internal static class VisualTreeHelperEx
  {
    public static bool IsChild(DependencyObject parentElement, DependencyObject childElement)
    {
      DependencyObject parent;
      for (DependencyObject dependencyObject = childElement; (parent = VisualTreeHelper.GetParent(dependencyObject)) != null; dependencyObject = parent)
      {
        if (parentElement == parent)
          return true;
      }
      return false;
    }
  }
}
