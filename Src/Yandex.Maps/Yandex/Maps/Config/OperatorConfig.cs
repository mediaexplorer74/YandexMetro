// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.OperatorConfig
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.Config
{
  internal class OperatorConfig
  {
    public OperatorName Operator { get; set; }

    public TimeSpan OperatorBrandingTimeout { get; set; }

    public bool ShowOperatorBrandingLogo { get; set; }

    public Uri OperatorPortal { get; set; }
  }
}
