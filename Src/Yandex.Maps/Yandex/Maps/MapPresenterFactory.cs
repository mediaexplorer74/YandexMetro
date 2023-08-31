// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapPresenterFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class MapPresenterFactory : IMapPresenterFactory
  {
    public IMapPresenterBase Get(MapPresenterType type, IMapViewBase view)
    {
      IMapPresenterBase mapPresenterBase;
      switch (type)
      {
        case MapPresenterType.Common:
          mapPresenterBase = (IMapPresenterBase) new MapPresenter((IMapView) view);
          break;
        case MapPresenterType.Base:
          mapPresenterBase = (IMapPresenterBase) new MapPresenterBase(view);
          break;
        default:
          throw new ArgumentException(nameof (type));
      }
      mapPresenterBase.Connect();
      return mapPresenterBase;
    }
  }
}
