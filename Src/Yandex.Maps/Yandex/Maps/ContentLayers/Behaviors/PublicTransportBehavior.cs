// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.Behaviors.PublicTransportBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.ContentLayers.Behaviors
{
  internal class PublicTransportBehavior : Behavior<MetaContentLayer>
  {
    private const string SupportedStopType = "urban";
    private const int StartingZoomToShowTransportStops = 16;
    private MetaContentLayer _metaContentLayer;

    public string Name
    {
      get => (string) this.GetValue(FrameworkElement.NameProperty);
      set => this.SetValue(FrameworkElement.NameProperty, (object) value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this._metaContentLayer = this.AssociatedObject;
      this.TransportStops = new ObservableCollection<PublicTransportStop>();
      this._metaContentLayer.VisibleTilesChanged += new EventHandler(this.VisibleTilesChanged);
      this.UpdateTransportStops();
    }

    protected override void OnDetaching()
    {
      this._metaContentLayer.VisibleTilesChanged -= new EventHandler(this.VisibleTilesChanged);
      base.OnDetaching();
    }

    private void VisibleTilesChanged(object sender, EventArgs eventArgs) => this.UpdateTransportStops();

    public ObservableCollection<PublicTransportStop> TransportStops { get; private set; }

    private void UpdateTransportStops()
    {
      PublicTransportStop[] array = this._metaContentLayer.VisibleTiles.Where<ITile>((Func<ITile, bool>) (tile => tile.Metadata.TransportStops != null)).SelectMany<ITile, PublicTransportStop>((Func<ITile, IEnumerable<PublicTransportStop>>) (tile => (IEnumerable<PublicTransportStop>) tile.Metadata.TransportStops)).Where<PublicTransportStop>((Func<PublicTransportStop, bool>) (stop => stop.Type == "urban")).ToArray<PublicTransportStop>();
      if (this._metaContentLayer.AreAllVisibleTilesLoaded || this._metaContentLayer.ActiveZoom < (byte) 16)
      {
        foreach (PublicTransportStop publicTransportStop in ((IEnumerable<PublicTransportStop>) this.TransportStops).Except<PublicTransportStop>((IEnumerable<PublicTransportStop>) array).ToArray<PublicTransportStop>())
          ((Collection<PublicTransportStop>) this.TransportStops).Remove(publicTransportStop);
      }
      foreach (PublicTransportStop publicTransportStop in ((IEnumerable<PublicTransportStop>) array).Except<PublicTransportStop>((IEnumerable<PublicTransportStop>) this.TransportStops).ToArray<PublicTransportStop>())
        ((Collection<PublicTransportStop>) this.TransportStops).Add(publicTransportStop);
    }
  }
}
