// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.Interfaces.IGeocodeManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.Geocoding.Dto;
using Yandex.WebUtils.Events;

namespace Yandex.Maps.Geocoding.Interfaces
{
  [ComVisible(true)]
  public interface IGeocodeManager
  {
    event RequestCompletedEventHandler<GeocodeRequestParameters, GeocodeResult> GeocodeCompleted;

    event EventHandler<RequestFailedEventArgs<GeocodeRequestParameters>> GeocodeFailed;

    void Query(GeocodeRequestParameters request);
  }
}
