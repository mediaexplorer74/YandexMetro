// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.AppList.AppListCommunicator
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using JetBrains.Annotations;
using Yandex.App.Information.AppList.Dto;
using Yandex.ItemsCounter;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.App.Information.AppList
{
  public class AppListCommunicator : 
    DefaultCommunicatorBase<AppListQueryBuilder, AppListRequestParameters, Apps>,
    IAppListCommunicator,
    ICommunicator<AppListRequestParameters, Apps>
  {
    public AppListCommunicator(
      [NotNull] AppListQueryBuilder queryBuilder,
      [NotNull] IGenericXmlSerializer<Apps> serializer,
      [NotNull] IWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(queryBuilder, serializer, webClientFactory, itemCounter)
    {
    }

    public override void Request(AppListRequestParameters parameters)
    {
      string query = this._queryBuilder.GetQuery(parameters);
      this.Execute(parameters, query);
    }

    protected override void AfterRequestExecuted(
      AppListRequestParameters requestParameters,
      Apps result)
    {
    }
  }
}
