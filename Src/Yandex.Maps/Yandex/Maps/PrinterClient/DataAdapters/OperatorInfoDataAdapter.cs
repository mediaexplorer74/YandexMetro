// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.DataAdapters.OperatorInfoDataAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config;
using Yandex.Maps.PrinterClient.Config;

namespace Yandex.Maps.PrinterClient.DataAdapters
{
  internal class OperatorInfoDataAdapter : IOperatorInfoDataAdapter
  {
    private OperatorName ParseName(string value)
    {
      if (string.IsNullOrEmpty(value))
        return OperatorName.Unknown;
      try
      {
        return (OperatorName) Enum.Parse(typeof (OperatorName), value, true);
      }
      catch (ArgumentException ex)
      {
        return OperatorName.Unknown;
      }
    }

    private TimeSpan InterpretBrandingTimeout(int timeout) => timeout >= 0 ? TimeSpan.FromSeconds((double) timeout) : TimeSpan.MaxValue;

    private Uri CreatePortalUrl(string text)
    {
      Uri result;
      return Uri.TryCreate(text, UriKind.Absolute, out result) ? result : (Uri) null;
    }

    public OperatorConfig ReadOperatorConfig([NotNull] StartupParameters startupParameters)
    {
      if (startupParameters == null)
        throw new ArgumentNullException(nameof (startupParameters));
      string str = (string) null;
      if (startupParameters.Operator != null)
        str = startupParameters.Operator.Name;
      int timeout = 0;
      string text = (string) null;
      ProviderFeatures providerFeatures = startupParameters.ProviderFeatures;
      if (providerFeatures != null)
      {
        if (providerFeatures.OperatorBranding != null)
          timeout = providerFeatures.OperatorBranding.Timeout;
        if (providerFeatures.OperatorPortal != null)
          text = providerFeatures.OperatorPortal.Url;
      }
      return new OperatorConfig()
      {
        Operator = this.ParseName(str),
        OperatorBrandingTimeout = this.InterpretBrandingTimeout(timeout),
        ShowOperatorBrandingLogo = providerFeatures != null && providerFeatures.OperatorBranding != null,
        OperatorPortal = this.CreatePortalUrl(text)
      };
    }
  }
}
