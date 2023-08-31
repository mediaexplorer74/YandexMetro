// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.CityMapView
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common.Utility;
using Yandex.Maps;
using Yandex.Metro.Logic;
using Yandex.Metro.Logic.FastScheme;
using Yandex.Positioning;

namespace Yandex.Metro.Views
{
  public class CityMapView : MetroPage
  {
    private bool _showStationOnLoad;
    internal Grid ContentPanel;
    internal Map map;
    internal MapItemsControl mapItemsControl;
    private bool _contentLoaded;

    public GeoCoordinate StationPosition { get; set; }

    public CityMapView()
    {
      this.InitializeComponent();
      this.map.UseLocation = MetroService.Instance.AppSettings.GpsEnabled;
      this.map.ApiKey = ResourcesHelper.Get<string>("MapkitApiKey");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode == null)
        this.DefinePositionToShowOnMapLoad();
      base.OnNavigatedTo(e);
    }

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e.NavigationMode == 1 && this.map != null)
        this.map.Dispose();
      ((Page) this).OnNavigatedFrom(e);
    }

    private void PhoneApplicationPageLoaded(object sender, RoutedEventArgs e)
    {
      if (this._showStationOnLoad && this.StationPosition != null)
      {
        this.map.Center = this.StationPosition;
        this.mapItemsControl.ItemsSource = (IEnumerable) new List<PushpinDummyViewModel>()
        {
          new PushpinDummyViewModel()
          {
            Position = this.StationPosition,
            State = PushPinState.Expanded
          }
        };
      }
      this._showStationOnLoad = false;
    }

    private void DefinePositionToShowOnMapLoad()
    {
      this._showStationOnLoad = true;
      if (!((Page) this).NavigationContext.QueryString.ContainsKey("stationId"))
        return;
      int stationIdToShow = Convert.ToInt32(((Page) this).NavigationContext.QueryString["stationId"]);
      if (FastKeeper.Scheme == null)
        return;
      KeyValuePair<int, MetroStation> keyValuePair = FastKeeper.Scheme.Stations.SingleOrDefault<KeyValuePair<int, MetroStation>>((Func<KeyValuePair<int, MetroStation>, bool>) (s => s.Key == stationIdToShow));
      if (keyValuePair.Value.Coordinates == null)
        return;
      GeoPoint coordinates = keyValuePair.Value.Coordinates;
      this.StationPosition = new GeoCoordinate(coordinates.Lat, coordinates.Lon);
      this._showStationOnLoad = true;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/CityMapView.xaml", UriKind.Relative));
      this.ContentPanel = (Grid) ((FrameworkElement) this).FindName("ContentPanel");
      this.map = (Map) ((FrameworkElement) this).FindName("map");
      this.mapItemsControl = (MapItemsControl) ((FrameworkElement) this).FindName("mapItemsControl");
    }
  }
}
