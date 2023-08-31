// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.WebUtils.MapKitWebClientFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.WebUtils;

namespace Yandex.Maps.WebUtils
{
  internal class MapKitWebClientFactory : MapWebClientFactory
  {
    private const string ApiKeyParameterName = "api_key";
    private readonly IConfigMediator _configMediator;

    public MapKitWebClientFactory([NotNull] IConfigMediator configMediator) => this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));

    protected override void UpdateUriGet(ref Uri uri)
    {
      base.UpdateUriGet(ref uri);
      this.AddApiKeyParameter(ref uri);
    }

    protected override void UpdateUriPost(ref Uri uri)
    {
      base.UpdateUriPost(ref uri);
      this.AddApiKeyParameter(ref uri);
    }

    private void AddApiKeyParameter(ref Uri uri)
    {
      string apiKey = this._configMediator.ApiKey;
      if (string.IsNullOrEmpty(apiKey))
        return;
      string queryParameter = "api_key=" + apiKey;
      WebClientFactory.AddUriQueryParameter(ref uri, queryParameter);
    }
  }
}
