// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.StaticMap
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Windows.Controls;
using Yandex.Ioc;
using Yandex.Maps.ContentLayers;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class StaticMap : MapBase, IMapViewBase, IMapState, ICompositeTransformation
  {
    private LayerManager _layerManager;

    public StaticMap() => ((Control) this).DefaultStyleKey = (object) typeof (StaticMap);

    public override void OnApplyTemplate()
    {
      this._layerManager = ((Control) this).GetTemplateChild("LayerManager") as LayerManager;
      this.MapContentLayer = (IMapContentLayer) (((Control) this).GetTemplateChild("MapContentLayer") as Yandex.Maps.ContentLayers.MapContentLayer);
      this.MapPresenter = IocSingleton<ControlsIocInitializer>.Resolve<IMapPresenterFactory>().Get(MapPresenterType.Base, (IMapViewBase) this);
      base.OnApplyTemplate();
    }

    public IMapContentLayer MapContentLayer { get; private set; }

    public override LayerManager LayerManager => this._layerManager;
  }
}
