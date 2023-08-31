// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapShapeObservable
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps
{
  [ContentProperty("Locations")]
  [ComVisible(false)]
  public abstract class MapShapeObservable : MapShape
  {
    public static readonly DependencyProperty LocationsProperty = DependencyProperty.Register(nameof (Locations), typeof (Collection<GeoCoordinate>), typeof (MapShapeObservable), new PropertyMetadata(new PropertyChangedCallback(MapShapeObservable.LocationsChanged)));

    public MapShapeObservable() => this.Locations = (Collection<GeoCoordinate>) new ObservableCollection<GeoCoordinate>();

    private static void LocationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShapeObservable mapShapeObservable))
        return;
      mapShapeObservable.OnLocationsChanged(e.OldValue as Collection<GeoCoordinate>, e.NewValue as Collection<GeoCoordinate>);
    }

    [UsedImplicitly]
    public Collection<GeoCoordinate> Locations
    {
      get => (Collection<GeoCoordinate>) ((DependencyObject) this).GetValue(MapShapeObservable.LocationsProperty);
      set => ((DependencyObject) this).SetValue(MapShapeObservable.LocationsProperty, (object) value);
    }

    [UsedImplicitly]
    public void SetLocations(Collection<GeoCoordinate> value) => this.Locations = value;

    [CanBeNull]
    [UsedImplicitly]
    public Collection<GeoCoordinate> GetLocations() => this.Locations;

    private void OnLocationsChanged(
      [CanBeNull] Collection<GeoCoordinate> oldValue,
      [CanBeNull] Collection<GeoCoordinate> newValue)
    {
      if (oldValue is INotifyCollectionChanged collectionChanged1)
        collectionChanged1.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.LocationsCollectionChanged);
      if (newValue is INotifyCollectionChanged collectionChanged2)
        collectionChanged2.CollectionChanged += new NotifyCollectionChangedEventHandler(this.LocationsCollectionChanged);
      this.Invalidate();
    }

    private void LocationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => this.Invalidate();

    protected override IList<Point> GetViewportPointsList(double zoomLevel)
    {
      Collection<GeoCoordinate> locations = this.Locations;
      return locations != null ? (IList<Point>) locations.Select<GeoCoordinate, Point>((Func<GeoCoordinate, Point>) (location => this.ViewportPointConveter.CoordinatesToViewportPoint(location, zoomLevel))).ToArray<Point>() : (IList<Point>) null;
    }
  }
}
