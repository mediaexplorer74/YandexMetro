// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamStylesManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.ItemsCounter;
using Yandex.Maps.Traffic.DTO.Styles;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Serialization.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class JamStylesManager : 
    DefaultCommunicatorBase<IJamQueryBuilder, object, JamStyles>,
    IJamStylesManager,
    ICommunicator<object, JamStyles>
  {
    public JamStylesManager(
      [NotNull] IJamQueryBuilder jamQueryBuilder,
      [NotNull] IGenericXmlSerializer<JamStyles> jamStylesSerializer,
      [NotNull] IMapWebClientFactory webClientFactory,
      [NotNull] IItemCounter itemCounter)
      : base(jamQueryBuilder, jamStylesSerializer, (IWebClientFactory) webClientFactory, itemCounter)
    {
    }

    public JamStyles Styles { get; set; }

    public override void Request(object parameters) => this.Execute((object) null, this._queryBuilder.GetStylesQuery());

    protected override void AfterRequestExecuted(object requestParameters, JamStyles result) => this.Styles = result;
  }
}
