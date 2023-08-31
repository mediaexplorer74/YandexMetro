// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Helpers.MapLayerHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using System.Windows;
using Yandex.Positioning;

namespace Yandex.Maps.Helpers
{
  internal static class MapLayerHelper
  {
    public static GeoCoordinate GetLocationRecursively(FrameworkElement element)
    {
      GeoCoordinate location = MapLayer.GetLocation((DependencyObject) element);
      if (location != null)
        return location;
      FrameworkElement visualParent = element.GetVisualParent();
      return visualParent == null ? (GeoCoordinate) null : MapLayerHelper.GetLocationRecursively(visualParent);
    }
  }
}
