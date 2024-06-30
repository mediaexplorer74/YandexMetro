// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.IMetroMap
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Windows;
using Y.Metro.ServiceLayer.FastScheme;

namespace Yandex.Metro.ViewModel
{
  public interface IMetroMap
  {
    void UpdateAppBar(bool isRouteExist, bool isStationSelect = false);

    void GenerateMap();

    void FocusStation(MetroStation station, bool chooseStation = false);

    void SetDefaultZoom(Route route);

    void SetSearch(bool isActive, bool skipAnimation = false);

    void AnimateNearestStation(bool isDragAnimation = false);

    void AnimateSelectStation(FrameworkElement element, MetroColor color, bool direction);
  }
}
