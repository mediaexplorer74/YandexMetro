// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapItemsControlEx
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using Yandex.Maps.ViewModels;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class MapItemsControlEx : MapItemsControl
  {
    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      if (!(element is FrameworkElement frameworkElement) || !(item is MapItemViewModel))
        return;
      frameworkElement.SetBinding(MapLayer.LocationProperty, new Binding("Position"));
      frameworkElement.SetBinding(MapLayer.AlignmentProperty, new Binding("Alignment"));
    }
  }
}
