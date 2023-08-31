// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.WebUtils.MapWebClientFactory
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Globalization;
using Yandex.Globalization;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.WebUtils
{
  internal class MapWebClientFactory : WebClientFactory, IMapWebClientFactory, IWebClientFactory
  {
    private const string RandomParameterName = "rndwp";
    private const string LangParameterName = "lang";

    protected override void UpdateUriGet(ref Uri uri)
    {
      base.UpdateUriGet(ref uri);
      this.AddLangParameter(ref uri);
      this.RandomizeUri(ref uri);
    }

    protected override void UpdateUriPost(ref Uri uri)
    {
      base.UpdateUriPost(ref uri);
      this.RandomizeUri(ref uri);
    }

    private void AddLangParameter(ref Uri uri)
    {
      string queryParameter = "lang=" + CultureInfo.CurrentUICulture.GetSpecificName();
      WebClientFactory.AddUriQueryParameter(ref uri, queryParameter);
    }

    private void RandomizeUri(ref Uri uri)
    {
      string queryParameter = "rndwp=" + (object) Guid.NewGuid();
      WebClientFactory.AddUriQueryParameter(ref uri, queryParameter);
    }
  }
}
