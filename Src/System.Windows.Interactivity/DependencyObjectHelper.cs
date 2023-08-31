// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.DependencyObjectHelper
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Interactivity
{
  public static class DependencyObjectHelper
  {
    public static IEnumerable<DependencyObject> GetSelfAndAncestors(
      this DependencyObject dependencyObject)
    {
      for (; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
        yield return dependencyObject;
    }
  }
}
