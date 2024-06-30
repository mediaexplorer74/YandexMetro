// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.RouteTimeHelper
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using Y.Metro.ServiceLayer.Enums;
using Y.Metro.ServiceLayer.FastScheme;

namespace Yandex.Metro.Logic
{
  public static class RouteTimeHelper
  {
    public static void UpdateRouteTime(Route route, TimeType type)
    {
      if (type != TimeType.ArrivalTime)
        return;
      DateTime now = DateTime.Now;
      MetroService.Instance.TimeForRoute = now;
      route.EstimatedDuration = route.EstimatedDuration;
      for (int index = 0; index < route.SortStations.Count; ++index)
      {
        int timing = route.Timings[index];
        route.SortStations[index].ArrivalTime = now.AddSeconds((double) timing).ToString("HH:mm");
      }
    }
  }
}
