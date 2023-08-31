// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.AppListManager
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Collections.Generic;
using Yandex.App.Information.AppList.DataAdapters;
using Yandex.App.Information.AppList.Dto;
using Yandex.App.Information.Models;
using Yandex.WebUtils.Events;
using Yandex.WebUtils.Interfaces;

namespace Yandex.App.Information.AppList
{
  public class AppListManager : IDataManager<AppListRequestParameters, IList<AppItem>>
  {
    private readonly IAppListCommunicator _appListCommunicator;
    private readonly IAppListDataAdapter _appListDataAdapter;
    private RequestCompletedEventArgs<AppListRequestParameters, IList<AppItem>> _appList;

    public event RequestCompletedEventHandler<AppListRequestParameters, IList<AppItem>> RequestCompleted;

    private void OnRequestCompleted(
      RequestCompletedEventArgs<AppListRequestParameters, IList<AppItem>> e)
    {
      RequestCompletedEventHandler<AppListRequestParameters, IList<AppItem>> requestCompleted = this.RequestCompleted;
      if (requestCompleted == null)
        return;
      requestCompleted((object) this, e);
    }

    public AppListManager(
      IAppListCommunicator appListCommunicator,
      IAppListDataAdapter appListDataAdapter)
    {
      this._appListCommunicator = appListCommunicator;
      this._appListDataAdapter = appListDataAdapter;
      this._appListCommunicator.RequestCompleted += new RequestCompletedEventHandler<AppListRequestParameters, Apps>(this.AppListCommunicatorRequestCompleted);
    }

    private void AppListCommunicatorRequestCompleted(
      object sender,
      RequestCompletedEventArgs<AppListRequestParameters, Apps> e)
    {
      if (e.RequestResults == null)
        return;
      this._appList = new RequestCompletedEventArgs<AppListRequestParameters, IList<AppItem>>(e.Parameters, this._appListDataAdapter.ReadAppList(e.RequestResults));
      this.OnRequestCompleted(this._appList);
    }

    public void Query(AppListRequestParameters parameters) => this._appListCommunicator.Request(parameters);

    public event EventHandler<RequestFailedEventArgs<AppListRequestParameters>> RequestFailed
    {
      add => this._appListCommunicator.RequestFailed += value;
      remove => this._appListCommunicator.RequestFailed -= value;
    }
  }
}
