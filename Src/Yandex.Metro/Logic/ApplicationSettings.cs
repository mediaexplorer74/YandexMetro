// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.ApplicationSettings
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using Y.Metro.ServiceLayer.Entities;
using Y.Metro.ServiceLayer.Enums;

namespace Yandex.Metro.Logic
{
  public class ApplicationSettings
  {
    public bool GpsEnabled { get; set; }

    public string StartupUuid { get; set; }

    public bool PrivacyPolicyShown { get; set; }

    public bool IsShemasCopiedToIso { get; set; }

    public List<Scheme> SchemeData { get; set; }

    public Scheme Scheme { get; set; }

    public DateTime LastUpdate { get; set; }

    public string Language { get; set; }

    public string MapLanguage { get; set; }

    public bool IsUserSpecifiedLanguage { get; set; }

    public bool AutoSelectStation { get; set; }

    public List<ShortRoute> History { get; set; }

    public ShortRoute Route { get; set; }

    public List<Y.Metro.ServiceLayer.Entities.Favorites> Favorites { get; set; }

    public TimeType TimeType { get; set; }

    public int RunCount { get; set; }

    public int TrackCount { get; set; }

    public bool IsProdVersion { get; set; }
  }
}
