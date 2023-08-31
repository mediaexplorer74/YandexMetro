// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Constants
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Windows;
using Y.UI.Common;
using Y.UI.Common.Utility;

namespace Yandex.Metro.Logic
{
  public static class Constants
  {
    private const string ServerTestUrl = "heroism.yandex.ru";
    private const string ServerProdUrl = "yandex.net";
    internal const string XmlnsPrefix = "ym";
    internal const string XmlnsNamespace = "we://love.windowsphone/metro.yandex.ru/";
    public const int UpdateAtemptCount = 3;
    public const int UpdateInterval = 300000;
    public const int FlickGestureAngleDelta = 50;
    public const string TestModeString = "heroism@test";
    public const string ProdModeString = "production1metro";
    public const int LinePathZIndex = 100;
    public const int StationZIndex = 300;
    public const int TranzitBorderZIndex = 200;
    public const int SelectStationZIndex = 500;
    public const int BorderZIndex = 400;
    public const int GpsZIndex = 9999;
    public const string ApplicationName = "Яндекс.Метро";
    public const string SchemeDefaultType = "default";
    public const string LanguageRu = "ru";
    public const string LanguageBel = "be";
    public const string LanguageEn = "en";
    public const string LanguageUkr = "uk";
    public const string LanguageEnCode = "en-US";
    public const string LanguageTrCode = "tr-TR";
    public const double MaxScaleForRoute = 3.0;
    public const double MaxMapScale = 3.2;
    public const double MaxWidth = 3200.0;
    public const double MaxHeight = 5300.0;
    public const double MinHeight = 767.0;
    public const double MinWidth = 460.0;
    public const double MinPinchScale = 0.5;
    public const double MaxPinchScale = 3.0;
    public const double ScreenSizeX = 480.0;
    public const double ScreenSizeY = 622.0;
    public const double VerticalPadding = 50.0;
    public const double HorizontalPadding = 50.0;
    public const double Threshold = 0.01;
    public const double ScreenWidthLimit = 470.0;
    public const double ScreenHeightLimit = 620.0;
    public static readonly Navigate ToMainPage = new Navigate(string.Format("/Views/MetroMapView.xaml?{0}=true", (object) "forgetPreviousPage"));
    public static readonly Navigate ToOtherApps = new Navigate("/Views/YandexApps.xaml");
    public static readonly Navigate ToSettings = new Navigate("/Views/Settings.xaml");
    public static readonly Navigate ToCities = new Navigate("/Views/Cities.xaml");
    public static readonly Navigate ToCitiesFirstLoad = new Navigate(string.Format("/Views/Cities.xaml?{0}=true", (object) "forgetPreviousPage"));
    public static readonly Navigate ToSelectFavorites = new Navigate("/Views/SelectFavorites.xaml");
    public static readonly Navigate ToStation = new Navigate("/Views/Station.xaml");
    public static readonly Navigate ToPolicy = new Navigate("/Views/Policy.xaml");
    public static readonly Navigate ToFavorites = new Navigate("/Views/Favorites.xaml");
    public static readonly Navigate ToLicenseAgreement = new Navigate(ResourcesHelper.Get<string>("LicenseAgreementUrl"));
    public static readonly Navigate ToStationOnCityMap = new Navigate()
    {
      UriTemplate = string.Format("/Views/CityMapView.xaml?{0}={{0}}", (object) "stationId")
    };
    public static readonly Navigate ToPodorosnik = new Navigate("https://m.money.yandex.ru/shop/4006");
    public static double MinMapScale = 0.46;
    public static readonly Duration AnimationDuration = new Duration(TimeSpan.FromMilliseconds(300.0));
    public static double ScreenRightLimit = 430.0;
    public static double ScreenLeftLimit = 50.0;
    public static double ScreenTopLimit = 50.0;
    public static double ScreenBottomLimit = 572.0;

    public static bool ForceUpdate { get; set; }

    public static string ServerUrl => !Constants.IsProdVersion ? "heroism.yandex.ru" : "yandex.net";

    public static bool IsProdVersion
    {
      get => MetroService.Instance.AppSettings.IsProdVersion;
      set
      {
        if (MetroService.Instance.AppSettings.IsProdVersion == value)
          return;
        MetroService.Instance.AppSettings.IsProdVersion = value;
        Constants.ForceUpdate = true;
      }
    }

    public static class UrlParams
    {
      public const string StationId = "stationId";
    }
  }
}
