// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Views.ViewModels.AppListViewModel
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Yandex.App.Information.AppList;
using Yandex.App.Information.AppList.DataAdapters;
using Yandex.App.Information.AppList.Dto;
using Yandex.App.Information.Models;
using Yandex.ItemsCounter;
using Yandex.Serialization;
using Yandex.Serialization.Interfaces;
using Yandex.Threading;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.App.Information.Views.ViewModels
{
  public class AppListViewModel : IAppListViewModel, INotifyPropertyChanged
  {
    private const string AppItemsPropertyName = "AppItems";
    private const string ParametersPropertyName = "Parameters";
    private const string LoadingOfAppItemsHasFailedPropertyName = "LoadingHasFailed";
    private const string IsLoadingPropertyName = "IsLoading";
    private readonly IDataManager<AppListRequestParameters, IList<AppItem>> _appListManager;
    private readonly IUiDispatcher _uiDispatcher;
    private bool _isLoading;
    private bool _loadingOfAppItemsHasFailed;
    private IEnumerable<AppItem> _appItems;
    private AppListParameters _parameters;

    public AppListViewModel(
      [NotNull] IDataManager<AppListRequestParameters, IList<AppItem>> appListManager,
      [NotNull] IUiDispatcher uiDispatcher)
    {
      if (appListManager == null)
        throw new ArgumentNullException(nameof (appListManager));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._appListManager = appListManager;
      this._appListManager.RequestCompleted += new RequestCompletedEventHandler<AppListRequestParameters, IList<AppItem>>(this.AppListManagerRequestCompleted);
      this._appListManager.RequestFailed += new EventHandler<RequestFailedEventArgs<AppListRequestParameters>>(this.AppListManagerRequestFailed);
      this._uiDispatcher = uiDispatcher;
    }

    public bool IsLoading
    {
      get => this._isLoading;
      private set
      {
        if (value == this._isLoading)
          return;
        this._isLoading = value;
        this.OnPropertyChanged(nameof (IsLoading));
      }
    }

    public bool LoadingHasFailed
    {
      get => this._loadingOfAppItemsHasFailed;
      private set
      {
        if (value == this._loadingOfAppItemsHasFailed)
          return;
        this._loadingOfAppItemsHasFailed = value;
        this.OnPropertyChanged(nameof (LoadingHasFailed));
      }
    }

    private void AppListManagerRequestFailed(
      object sender,
      RequestFailedEventArgs<AppListRequestParameters> e)
    {
      this.IsLoading = false;
      this.LoadingHasFailed = true;
    }

    public AppListViewModel()
      : this((IDataManager<AppListRequestParameters, IList<AppItem>>) new AppListManager((IAppListCommunicator) new AppListCommunicator(new AppListQueryBuilder((IAppListUrlProvider) new ProductionAppListUrlProvider()), (IGenericXmlSerializer<Apps>) new GenericXmlSerializer<Apps>(), (IWebClientFactory) new WebClientFactory(), (IItemCounter) new ItemCounter()), (IAppListDataAdapter) new AppListDataAdapter()), (IUiDispatcher) new UiDispatcher())
    {
    }

    private void AppListManagerRequestCompleted(
      object sender,
      RequestCompletedEventArgs<AppListRequestParameters, IList<AppItem>> e)
    {
      this.IsLoading = false;
      this.AppItems = (IEnumerable<AppItem>) e.RequestResults;
    }

    public IEnumerable<AppItem> AppItems
    {
      get => this._appItems;
      private set
      {
        this._appItems = value;
        this.OnPropertyChanged(nameof (AppItems));
      }
    }

    public AppListParameters Parameters
    {
      get => this._parameters;
      set
      {
        this._parameters = value;
        this.ReloadAppItems();
        this.OnPropertyChanged(nameof (Parameters));
      }
    }

    private void ReloadAppItems()
    {
      if (this._parameters == null)
        return;
      this.LoadingHasFailed = false;
      this.IsLoading = true;
      this._appListManager.Query(new AppListRequestParameters(this._parameters.GeoCode, this._parameters.ApplicationId, this._parameters.Platform, CultureInfo.CurrentCulture));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler == null)
        return;
      this._uiDispatcher.BeginInvoke((Action) (() => handler((object) this, new PropertyChangedEventArgs(propertyName))));
    }
  }
}
