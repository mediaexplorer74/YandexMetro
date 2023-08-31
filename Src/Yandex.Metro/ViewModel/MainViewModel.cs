// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.MainViewModel
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using GalaSoft.MvvmLight;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Y.Common.Extensions;
using Y.Metro.ServiceLayer;
using Y.Metro.ServiceLayer.Common;
using Y.Metro.ServiceLayer.Entities;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common;
using Y.UI.Common.Extensions;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;
using Yandex.Metro.Logic.FastScheme;
using Yandex.Metro.Resources;
using Yandex.Metro.Views;
using Yandex.Metro.Views.Partial;

namespace Yandex.Metro.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private MetroStation _selectedStation;
    private MetroStation _startStation;
    private MetroStation _endStation;
    private MetroStation _nearestStation;
    private bool _isStationPopupVisible;
    private bool _isHoldStation;
    private Thickness _popupMargin = new Thickness(0.0, 0.0, 0.0, 0.0);
    private SolidColorBrush _background = new SolidColorBrush("#D8ffffff".ToColor());
    private SolidColorBrush _backgroundTr = new SolidColorBrush(Colors.Transparent);
    private ObservableCollection<Route> _routes = new ObservableCollection<Route>();
    private Route _index;
    private bool _isFromDirection;
    private ObservableCollection<MetroStation> _favorites;
    private bool _isSelectionEnabled;
    private ObservableCollection<GroupByTitle<MetroStation>> _stationByName;
    private ObservableCollection<GroupByLine<MetroStation>> _stationByLine;
    private bool _searchIsActive;
    private AutoCompleteState _autoCompleteState;
    private ObservableCollection<StationName> _searchStation = new ObservableCollection<StationName>();
    private bool _isPlainMode;
    private int _selectedIndex;
    private List<Route> _routesHistory;
    private bool _isMessageShow;

    public IMetroMap MetroMap { get; set; }

    public IFavorites IFavorites { get; set; }

    public List<KeyValuePair<StationName, string>> OldNameStations { get; set; }

    public Dictionary<StationName, string> SortNameStation { get; set; }

    public bool IsDebugVersion => ServiceConstants.IsDebugVersion;

    public string Uuid => ServicesHelper.Uuid;

    public MetroStation SelectedStation
    {
      get => this._selectedStation;
      set
      {
        if (this._selectedStation == value)
          return;
        this._selectedStation = value;
        this.RaisePropertyChanged(nameof (SelectedStation));
      }
    }

    public MetroStation StartStation
    {
      get => this._startStation;
      set
      {
        if (this._startStation == value)
          return;
        this._startStation = value;
        this.RaisePropertyChanged("IsStartStationAvailable");
        this.RaisePropertyChanged(nameof (StartStation));
      }
    }

    public bool IsStartStationAvailable => this.StartStation != null;

    public MetroStation EndStation
    {
      get => this._endStation;
      set
      {
        if (this._endStation == value)
          return;
        this._endStation = value;
        this.RaisePropertyChanged("IsEndStationAvailable");
        this.RaisePropertyChanged(nameof (EndStation));
      }
    }

    public bool IsEndStationAvailable => this.EndStation != null;

    public MetroStation NearestStation
    {
      get => this._nearestStation;
      set
      {
        if (this._nearestStation == value)
          return;
        this._nearestStation = value;
        this.RaisePropertyChanged(nameof (NearestStation));
      }
    }

    public bool IsStationPopupVisible
    {
      get => this._isStationPopupVisible;
      set
      {
        if (this._isStationPopupVisible == value)
          return;
        this._isStationPopupVisible = value;
        this.RaisePropertyChanged("IsHoldStation");
        this.RaisePropertyChanged(nameof (IsStationPopupVisible));
        this.RaisePropertyChanged("BorderBackground");
      }
    }

    public bool IsHoldStation
    {
      get => this._isHoldStation;
      set
      {
        if (this._isStationPopupVisible == value)
          return;
        this._isHoldStation = value;
        this.RaisePropertyChanged(nameof (IsHoldStation));
      }
    }

    public Thickness PopupMargin
    {
      get => this._popupMargin;
      set
      {
        if (Thickness.op_Equality(this._popupMargin, value))
          return;
        this._popupMargin = value;
        this.RaisePropertyChanged(nameof (PopupMargin));
      }
    }

    public SolidColorBrush BorderBackground
    {
      get => this.IsRouteAvailable ? this._backgroundTr : this._background;
      set
      {
        if (this._background == value)
          return;
        this._background = value;
        this.RaisePropertyChanged("Background");
      }
    }

    public bool IsRouteAvailable => this.Routes != null && ((Collection<Route>) this.Routes).Count > 0;

    public ObservableCollection<Route> Routes
    {
      get => this._routes;
      set
      {
        if (this._routes == value)
          return;
        this._routes = value;
        this.RaisePropertyChanged("IsRouteAvailable");
        this.MetroMap.UpdateAppBar(this.IsRouteAvailable);
        this.RaisePropertyChanged(nameof (Routes));
      }
    }

    public Route SelectRoute
    {
      get => this._index;
      set
      {
        if (this._index == value)
          return;
        this._index = value;
        this.RaisePropertyChanged("RevertButtonText");
        this.RaisePropertyChanged(nameof (SelectRoute));
      }
    }

    public bool IsFromDirection
    {
      get => this._isFromDirection;
      set
      {
        if (this._isFromDirection == value)
          return;
        this._isFromDirection = value;
        this.RaisePropertyChanged(nameof (IsFromDirection));
      }
    }

    public ObservableCollection<MetroStation> Favorites
    {
      get => this._favorites;
      set
      {
        if (this._favorites == value)
          return;
        this._favorites = value;
        this.RaisePropertyChanged(nameof (Favorites));
      }
    }

    public bool FavoritesSelection
    {
      get => this._isSelectionEnabled;
      set
      {
        if (this._isSelectionEnabled == value)
          return;
        this._isSelectionEnabled = value;
        this.IFavorites.UpdateAppBar(this._isSelectionEnabled);
        this.RaisePropertyChanged(nameof (FavoritesSelection));
      }
    }

    public ObservableCollection<GroupByTitle<MetroStation>> StationByName
    {
      get => this._stationByName;
      set
      {
        if (this._stationByName == value)
          return;
        this._stationByName = value;
        this.RaisePropertyChanged(nameof (StationByName));
      }
    }

    public ObservableCollection<GroupByLine<MetroStation>> StationByLine
    {
      get => this._stationByLine;
      set
      {
        if (this._stationByLine == value)
          return;
        this._stationByLine = value;
        this.RaisePropertyChanged(nameof (StationByLine));
      }
    }

    public bool SearchIsActive
    {
      get => this._searchIsActive;
      set
      {
        if (this._searchIsActive == value)
          return;
        this._searchIsActive = value;
        this.RaisePropertyChanged("RoutesHistory");
        this.RaisePropertyChanged("BarIsVisible");
        AutoCompleteState autoCompleteState = this.RoutesHistory == null || this.RoutesHistory.Count <= 0 ? AutoCompleteState.ShownEmpty : AutoCompleteState.Shown;
        this.AutoCompleteState = this._searchIsActive ? autoCompleteState : AutoCompleteState.Hidden;
        this.RaisePropertyChanged(nameof (SearchIsActive));
      }
    }

    public bool BarIsVisible => !this.SearchIsActive && !this.IsPlainMode;

    public AutoCompleteState AutoCompleteState
    {
      get => this._autoCompleteState;
      set
      {
        if (this._autoCompleteState == value)
          return;
        this._autoCompleteState = value;
        this.RaisePropertyChanged(nameof (AutoCompleteState));
      }
    }

    public ObservableCollection<StationName> SearchStation
    {
      get => this._searchStation;
      set
      {
        if (this._searchStation == value)
          return;
        this._searchStation = value;
        this.RaisePropertyChanged(nameof (SearchStation));
      }
    }

    public bool IsPlainMode
    {
      get => this._isPlainMode;
      set
      {
        if (this._isPlainMode == value)
          return;
        this._isPlainMode = value;
        this.RaisePropertyChanged("BarIsVisible");
        this.RaisePropertyChanged(nameof (IsPlainMode));
      }
    }

    public int SelectedIndexPivot
    {
      get => this._selectedIndex;
      set
      {
        if (this._selectedIndex == value)
          return;
        this._selectedIndex = value;
        this.RaisePropertyChanged(nameof (SelectedIndexPivot));
      }
    }

    public FrameworkElement SelectElement { get; set; }

    public string RevertButtonText => this.SelectRoute != null ? string.Format("{0} — {1}", (object) this.SelectRoute.EndStation.Name.Text, (object) this.SelectRoute.StartStation.Name.Text) : "•••••••";

    public List<Route> RoutesHistory
    {
      get
      {
        if (FastKeeper.Scheme != null && this._routesHistory == null)
        {
          MetroScheme scheme = FastKeeper.Scheme;
          this._routesHistory = MetroService.Instance.AppSettings.History.Where<ShortRoute>((Func<ShortRoute, bool>) (r => r.SchemaId == MetroService.Instance.AppSettings.Scheme.Id)).Select<ShortRoute, Route>((Func<ShortRoute, Route>) (shortRoute => new Route()
          {
            StartStation = scheme.Stations[shortRoute.FromStationId],
            EndStation = scheme.Stations[shortRoute.ToStationId]
          })).ToList<Route>();
        }
        return this._routesHistory;
      }
    }

    public string LogoUrl
    {
      get
      {
        string language = MetroService.Instance.AppSettings.Language;
        return !(language == "en-US") && !(language == "tr-TR") ? "/Images/logo.png" : "/Images/logo_eng.png";
      }
    }

    public WeakDelegateCommand About { get; set; }

    public WeakDelegateCommand OtherApps { get; set; }

    public WeakDelegateCommand ShowFavorites { get; set; }

    public WeakDelegateCommand ClearRouteCommand { get; set; }

    public WeakDelegateCommand SettingsCommand { get; set; }

    public WeakDelegateCommand<MetroStation> StartStationCommand { get; set; }

    public WeakDelegateCommand<MetroStation> EndStationCommand { get; set; }

    public WeakDelegateCommand<bool> OpenStationView { get; set; }

    public WeakDelegateCommand<bool> ClearStationCommand { get; set; }

    public WeakDelegateCommand<MetroStation> SelectionChanged { get; set; }

    public WeakDelegateCommand SwithModeCommand { get; set; }

    public WeakDelegateCommand ViewLicenseAgreement { get; set; }

    public WeakDelegateCommand RevertRoute { get; set; }

    public WeakDelegateCommand<MetroStation> ShowStationAtCityMap { get; set; }

    public WeakDelegateCommand ShowFavoritesSelection { get; set; }

    public WeakDelegateCommand<MetroStation> FavoritesAddCommand { get; set; }

    public WeakDelegateCommand<bool> FavoritesSelectionCommand { get; set; }

    public WeakDelegateCommand<MetroStation> FocusStationCommand { get; set; }

    public WeakDelegateCommand<string> StartAutocompleteCommand { get; set; }

    public WeakDelegateCommand<Route> SetRouteCommand { get; set; }

    public WeakDelegateCommand PodorosnikCommand { get; set; }

    public WeakDelegateCommand SetFirstResultCommand { get; set; }

    public MainViewModel()
    {
      if (this.IsInDesignMode)
        return;
      this.About = new WeakDelegateCommand(new Action(this.OnAboutExecuted));
      this.OtherApps = new WeakDelegateCommand((Action) (() => PageNavigationService.Navigate(Constants.ToOtherApps)));
      this.ShowFavorites = new WeakDelegateCommand((Action) (() => PageNavigationService.Navigate(Constants.ToFavorites)));
      this.ClearRouteCommand = new WeakDelegateCommand(new Action(this.OnClearRouteCommandExecuted));
      this.SettingsCommand = new WeakDelegateCommand(new Action(this.OnSettingsExecuted));
      this.StartStationCommand = new WeakDelegateCommand<MetroStation>(new Action<MetroStation>(this.OnStartStationExecuted));
      this.EndStationCommand = new WeakDelegateCommand<MetroStation>(new Action<MetroStation>(this.OnEndStationExecuted));
      this.OpenStationView = new WeakDelegateCommand<bool>(new Action<bool>(this.OnOpenStationViewExecuted));
      this.ClearStationCommand = new WeakDelegateCommand<bool>(new Action<bool>(this.OnClearStationCommandExecuted));
      this.SelectionChanged = new WeakDelegateCommand<MetroStation>(new Action<MetroStation>(this.OnSelectionChangedExecuted));
      this.SwithModeCommand = new WeakDelegateCommand(new Action(this.OnSwitchModeCommandExecuted));
      this.ViewLicenseAgreement = new WeakDelegateCommand(new Action(this.OnViewLicenseAgreementExecuted));
      this.RevertRoute = new WeakDelegateCommand(new Action(this.OnRevertRouteExecuted));
      this.ShowStationAtCityMap = new WeakDelegateCommand<MetroStation>(new Action<MetroStation>(this.OnShowStationAtCityMap));
      this.FavoritesAddCommand = new WeakDelegateCommand<MetroStation>(new Action<MetroStation>(this.OnFavoritesAddCommandExecuted));
      this.FavoritesSelectionCommand = new WeakDelegateCommand<bool>((Action<bool>) (isSelection => this.FavoritesSelection = isSelection));
      this.ShowFavoritesSelection = new WeakDelegateCommand((Action) (() => PageNavigationService.Navigate(Constants.ToSelectFavorites)));
      this.FocusStationCommand = new WeakDelegateCommand<MetroStation>((Action<MetroStation>) (s =>
      {
        if (this.FavoritesSelection)
          return;
        PageNavigationService.GoBack();
        this.MetroMap.FocusStation(s, true);
      }));
      this.StartAutocompleteCommand = new WeakDelegateCommand<string>(new Action<string>(this.StartAutocompleteCommandExecuted));
      this.SetRouteCommand = new WeakDelegateCommand<Route>(new Action<Route>(this.SetRouteExecuted));
      this.PodorosnikCommand = new WeakDelegateCommand((Action) (() => TaskHelper.SafeExec((Action) (() => new WebBrowserTask()
      {
        Uri = Constants.ToPodorosnik.UriToNavigate
      }.Show()))));
      this.SetFirstResultCommand = new WeakDelegateCommand(new Action(this.OnSetFirstResultCommandExecuted));
    }

    private void OnSetFirstResultCommandExecuted()
    {
      if (!this.SearchIsActive)
        return;
      switch (this.AutoCompleteState)
      {
        case AutoCompleteState.Shown:
        case AutoCompleteState.ShownFast:
          if (this.RoutesHistory == null || this.RoutesHistory.Count <= 0)
            break;
          this.SetRouteCommand.Execute(this.RoutesHistory[0]);
          break;
        case AutoCompleteState.Search:
          if (this.SearchStation == null || ((Collection<StationName>) this.SearchStation).Count <= 0)
            break;
          this.SelectionChanged.Execute(((Collection<StationName>) this.SearchStation)[0].Station);
          break;
        default:
          this.MetroMap.SetSearch(false);
          break;
      }
    }

    private void OnFavoritesAddCommandExecuted(MetroStation station)
    {
      if (this.Favorites == null || ((Collection<MetroStation>) this.Favorites).Contains(station))
        return;
      ApplicationSettings appSettings = MetroService.Instance.AppSettings;
      appSettings.Favorites.Add(new Y.Metro.ServiceLayer.Entities.Favorites()
      {
        StationId = station.Id,
        SchemaId = appSettings.Scheme.Id
      });
      ((Collection<MetroStation>) this.Favorites).Add(station);
      if (this.IFavorites == null)
        return;
      this.IFavorites.UpdateAppBar(false);
    }

    private void OnRevertRouteExecuted()
    {
      MetroStation startStation = this.StartStation;
      this.StartStation = this.EndStation;
      this.EndStation = startStation;
      this.ApplyRoute();
    }

    internal void UpdateNearestStation() => ThreadPool.QueueUserWorkItem((WaitCallback) (s =>
    {
      Thread.Sleep(2000);
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.NearestStation = (MetroStation) null;
        if (!ServicesHelper.GpsEnabled)
          return;
        GeoCoordinate coordinate = ServicesHelper.GetLastLocation(true);
        if (coordinate == (GeoCoordinate) null || coordinate.IsUnknown)
          return;
        var data = FastKeeper.Scheme.Stations.Select(c => new
        {
          c = c,
          coord = c.Value.Coordinates
        }).Select(_param1 => new
        {
          \u003C\u003Eh__TransparentIdentifier16 = _param1,
          distance = new GeoCoordinate(_param1.coord.Lat, _param1.coord.Lon).GetDistanceTo(coordinate)
        }).Where(_param0 => _param0.distance < 5000.0).OrderBy(_param0 => _param0.distance).Select(_param0 => new
        {
          Station = _param0.\u003C\u003Eh__TransparentIdentifier16.c.Value,
          Distance = _param0.distance
        }).FirstOrDefault();
        if (data == null)
          return;
        this.NearestStation = data.Station;
        if (this.StartStation != null)
          return;
        this.OnStartStationExecuted(this.NearestStation);
      }));
    }));

    private void SetRouteExecuted(Route route)
    {
      if (route == null)
        return;
      this.ClearStationVisualsOnScheme();
      this.StartStation = (MetroStation) null;
      this.EndStation = (MetroStation) null;
      this.StartStationCommand.Execute(route.StartStation);
      this.EndStationCommand.Execute(route.EndStation);
      if (!this.SearchIsActive)
        return;
      this.MetroMap.SetSearch(false);
    }

    private void OnViewLicenseAgreementExecuted() => TaskHelper.SafeExec(new Action(new WebBrowserTask()
    {
      Uri = Constants.ToLicenseAgreement.UriToNavigate
    }.Show));

    private void OnSwitchModeCommandExecuted()
    {
      this.HideSelectedStationControl();
      this.IsPlainMode = true;
      this.MetroMap.AnimateNearestStation();
    }

    private void StartAutocompleteCommandExecuted(string query)
    {
      if (!this.SearchIsActive)
        return;
      if (string.IsNullOrWhiteSpace(query))
        this.AutoCompleteState = this.RoutesHistory == null || this.RoutesHistory.Count <= 0 ? AutoCompleteState.ShownEmpty : AutoCompleteState.ShownFast;
      else
        ThreadPool.QueueUserWorkItem((WaitCallback) (g =>
        {
          if (g != MetroService.Instance.UniqObject)
            return;
          string lowerQuery = ReplaceHelper.ReplaceChar(query);
          List<StationName> list1 = this.SortNameStation.Where<KeyValuePair<StationName, string>>((Func<KeyValuePair<StationName, string>, bool>) (r => r.Value.StartsWith(lowerQuery))).Select<KeyValuePair<StationName, string>, StationName>((Func<KeyValuePair<StationName, string>, StationName>) (s => s.Key)).ToList<StationName>();
          List<int> listInt = list1.Select<StationName, int>((Func<StationName, int>) (r => r.Station.Id)).ToList<int>();
          List<StationName> list2 = this.SortNameStation.Where<KeyValuePair<StationName, string>>((Func<KeyValuePair<StationName, string>, bool>) (r => r.Value.Contains(lowerQuery))).Where<KeyValuePair<StationName, string>>((Func<KeyValuePair<StationName, string>, bool>) (r => !listInt.Contains(r.Key.Station.Id))).Select<KeyValuePair<StationName, string>, StationName>((Func<KeyValuePair<StationName, string>, StationName>) (s => s.Key)).ToList<StationName>();
          List<StationName> list3 = this.OldNameStations.Where<KeyValuePair<StationName, string>>((Func<KeyValuePair<StationName, string>, bool>) (r => r.Value.Contains(lowerQuery))).Select<KeyValuePair<StationName, string>, StationName>((Func<KeyValuePair<StationName, string>, StationName>) (s => s.Key)).ToList<StationName>();
          List<StationName> stations = new List<StationName>((IEnumerable<StationName>) list1);
          stations.AddRange((IEnumerable<StationName>) list2);
          stations.AddRange((IEnumerable<StationName>) list3);
          List<StationName> first7Item = stations.Take<StationName>(7).ToList<StationName>();
          List<StationName> next = stations.Skip<StationName>(7).ToList<StationName>();
          if (g != MetroService.Instance.UniqObject)
            return;
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            this.SearchStation = first7Item.ToObservableCollection<StationName>();
            this.AutoCompleteState = stations.Count > 0 ? AutoCompleteState.Search : AutoCompleteState.SearchEmpty;
          }));
          int count = next.Count;
          for (int index = 0; index < next.Count; index += 2)
          {
            if (g != MetroService.Instance.UniqObject)
              break;
            int k = index;
            int j = index + 1;
            if (j >= count)
              j = 0;
            DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
            {
              ((Collection<StationName>) this.SearchStation).Add(next[k]);
              if (j <= 0)
                return;
              ((Collection<StationName>) this.SearchStation).Add(next[j]);
            }));
            Thread.Sleep(70);
          }
        }), MetroService.Instance.UniqObject);
    }

    private void OnSelectionChangedExecuted(MetroStation obj)
    {
      if (obj == null)
        return;
      if (this.IsFromDirection)
        this.OnStartStationExecuted(obj);
      else
        this.OnEndStationExecuted(obj);
      if (this.SearchIsActive)
        this.MetroMap.SetSearch(false);
      else
        PageNavigationService.GoBack();
    }

    private void OnClearStationCommandExecuted(bool isFromStation)
    {
      if (isFromStation)
      {
        MainViewModel.SetSelection(this.StartStation, false);
        this.StartStation = (MetroStation) null;
      }
      else
      {
        MainViewModel.SetSelection(this.EndStation, false);
        this.EndStation = (MetroStation) null;
      }
      this.MetroMap.UpdateAppBar(this.IsRouteAvailable);
    }

    private void OnOpenStationViewExecuted(bool isFromStation)
    {
      this.IsFromDirection = isFromStation;
      PageNavigationService.Navigate(Constants.ToStation);
    }

    private void OnStartStationExecuted(MetroStation station)
    {
      if (this.StartStation != null)
      {
        if (this.StartStation == station)
        {
          this.IsStationPopupVisible = false;
          return;
        }
        if (this.IsRouteAvailable && this.EndStation == station)
        {
          this.IsStationPopupVisible = false;
          this.OnRevertRouteExecuted();
          return;
        }
        MainViewModel.SetSelection(this.StartStation, false);
      }
      else if (this.EndStation == station)
      {
        MainViewModel.SetSelection(this.EndStation, false);
        this.EndStation = (MetroStation) null;
      }
      MainViewModel.SetSelection(station, true);
      this.StartStation = station;
      this.IsStationPopupVisible = false;
      this.ApplyRoute(true);
    }

    private static void SetSelection(MetroStation station, bool isSelect)
    {
      if (station.SameAsId > 0)
      {
        MetroStation station1 = FastKeeper.Scheme.Stations[station.SameAsId];
        station1.SameEllipseBrush = isSelect ? MapGenerator.GetBrush(station.LineReference.Color) : (SolidColorBrush) null;
        station1.IsSelect = isSelect;
      }
      else
        station.IsSelect = isSelect;
    }

    internal void HideSelectedStationControl() => this.IsStationPopupVisible = false;

    private void ApplyRoute(bool isFocusStation = false)
    {
      if (this.StartStation != null && this.EndStation != null)
      {
        Locator.ProgressStatic.StartJob(Localization.Progress_Route);
        this.SelectRoute = (Route) null;
        ThreadPool.QueueUserWorkItem((WaitCallback) (s =>
        {
          List<Route> routes = RouteHelper.ConstructRoutes(this.StartStation, this.EndStation);
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            this.Routes = routes.ToObservableCollection<Route>();
            this.SelectedIndexPivot = 0;
            this.SelectRoute = ((IEnumerable<Route>) this.Routes).FirstOrDefault<Route>();
            this.SetRouteHistory(this.SelectRoute);
            Locator.ProgressStatic.StopJob(Localization.Progress_Route);
          }));
        }));
      }
      else
      {
        this.MetroMap.UpdateAppBar(this.IsRouteAvailable, true);
        if (!isFocusStation)
          return;
        this.MetroMap.FocusStation(this.StartStation ?? this.EndStation);
      }
    }

    private void SetRouteHistory(Route route)
    {
      if (route == null)
        return;
      ApplicationSettings appSettings = MetroService.Instance.AppSettings;
      ++appSettings.TrackCount;
      ShortRoute newRoute = new ShortRoute()
      {
        SchemaId = appSettings.Scheme.Id,
        FromStationId = route.StartStation.Id,
        ToStationId = route.EndStation.Id
      };
      appSettings.Route = newRoute;
      List<ShortRoute> list = appSettings.History.Where<ShortRoute>((Func<ShortRoute, bool>) (r => r.SchemaId == newRoute.SchemaId)).ToList<ShortRoute>();
      foreach (ShortRoute shortRoute in list)
        appSettings.History.Remove(shortRoute);
      ShortRoute shortRoute1 = list.FirstOrDefault<ShortRoute>((Func<ShortRoute, bool>) (r => r.ToStationId == newRoute.ToStationId && r.FromStationId == newRoute.FromStationId));
      if (shortRoute1 != null)
        list.Remove(shortRoute1);
      list.Insert(0, newRoute);
      foreach (ShortRoute shortRoute2 in list.Take<ShortRoute>(5).ToList<ShortRoute>())
        appSettings.History.Add(shortRoute2);
      this._routesHistory = (List<Route>) null;
    }

    private void OnEndStationExecuted(MetroStation station)
    {
      if (this.EndStation != null)
      {
        if (this.EndStation == station)
        {
          this.IsStationPopupVisible = false;
          return;
        }
        if (this.IsRouteAvailable && this.StartStation == station)
        {
          this.IsStationPopupVisible = false;
          this.OnRevertRouteExecuted();
          return;
        }
        MainViewModel.SetSelection(this.EndStation, false);
      }
      else if (this.StartStation == station)
      {
        MainViewModel.SetSelection(this.StartStation, false);
        this.StartStation = (MetroStation) null;
      }
      MainViewModel.SetSelection(station, true);
      this.EndStation = station;
      this.IsStationPopupVisible = false;
      this.ApplyRoute(true);
    }

    private void OnSettingsExecuted()
    {
      Locator.SettingsStatic.SelectedIndex = 0;
      PageNavigationService.Navigate(Constants.ToSettings);
    }

    private void OnAboutExecuted()
    {
      Locator.SettingsStatic.SelectedIndex = 1;
      PageNavigationService.Navigate(Constants.ToSettings);
    }

    private void OnClearRouteCommandExecuted()
    {
      if (this.IsRouteAvailable)
        this.ShowClearRouteMessage();
      else
        this.ClearRoute();
    }

    internal void ShowClearRouteMessage()
    {
      if (this._isMessageShow || !this.IsRouteAvailable)
        return;
      this._isMessageShow = true;
      Guide.BeginShowMessageBox(Localization.Shake_Text, " ", (IEnumerable<string>) new List<string>()
      {
        Localization.ShakeButton_Yes,
        Localization.ShakeButton_No
      }, 0, MessageBoxIcon.None, (AsyncCallback) (asyncResult =>
      {
        if ((Guide.EndShowMessageBox(asyncResult) ?? -1) == 0)
          DispatcherHelper.CheckBeginInvokeOnUI(new Action(this.ClearRoute));
        this._isMessageShow = false;
      }), (object) null);
    }

    internal void ClearRoute()
    {
      this.ClearStationVisualsOnScheme();
      this.HideSelectedStationControl();
      MetroService.Instance.AppSettings.Route = (ShortRoute) null;
      this.StartStation = (MetroStation) null;
      this.EndStation = (MetroStation) null;
      Route selectRoute = this.SelectRoute;
      this.SelectRoute = (Route) null;
      this.IsPlainMode = false;
      this.Routes = new ObservableCollection<Route>();
      if (this.NearestStation != null)
        this.MetroMap.FocusStation(this.NearestStation);
      else
        this.MetroMap.SetDefaultZoom(selectRoute);
    }

    private void ClearStationVisualsOnScheme()
    {
      if (this.StartStation != null)
        MainViewModel.SetSelection(this.StartStation, false);
      if (this.EndStation == null)
        return;
      MainViewModel.SetSelection(this.EndStation, false);
    }

    internal void ChooseStation(
      MetroStation station,
      FrameworkElement element,
      double? verticalChange = null)
    {
      this.SelectElement = element;
      if (!this.IsHoldStation && MetroService.Instance.AppSettings.AutoSelectStation && (this.StartStation == null || this.EndStation == null))
      {
        bool direction = this.StartStation == null;
        this.MetroMap.AnimateSelectStation(this.SelectElement, station.LineReference.Color, direction);
        if (direction)
          this.OnStartStationExecuted(station);
        else
          this.OnEndStationExecuted(station);
      }
      else if (verticalChange.HasValue && this.SelectedStation == station && this.IsStationPopupVisible)
      {
        this.PopupMargin = new Thickness(0.0, this.PopupMargin.Top + verticalChange.Value, 0.0, 0.0);
      }
      else
      {
        GeneralTransform visual = ((UIElement) element).TransformToVisual(Application.Current.RootVisual);
        Point point1 = visual.Transform(new Point(0.0, 0.0));
        Point point2 = visual.Transform(new Point(0.0, ((UIElement) element).RenderSize.Height));
        this.PopupMargin = new Thickness(0.0, point1.Y > 415.0 ? point1.Y - 224.0 : point2.Y + 70.0, 0.0, 0.0);
        this.SelectedStation = station;
        this.IsStationPopupVisible = true;
      }
    }

    private void OnShowStationAtCityMap(MetroStation stationToShow)
    {
      if (stationToShow == null)
        throw new ArgumentNullException(nameof (stationToShow));
      PageNavigationService.Navigate(Constants.ToStationOnCityMap, (object) stationToShow.Id);
    }
  }
}
