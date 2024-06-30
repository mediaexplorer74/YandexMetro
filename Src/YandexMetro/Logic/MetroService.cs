// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.MetroService
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Microsoft.Devices;
using Microsoft.Phone.Controls.LocalizedResources;
using Microsoft.Phone.Info;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Y.Common;
using Y.Metro.ServiceLayer;
using Y.Metro.ServiceLayer.Common;
using Y.Metro.ServiceLayer.Entities;
using Y.Metro.ServiceLayer.Enums;
using Y.Metro.ServiceLayer.Generated;
using Y.UI.Common;
using Y.UI.Common.Extensions;
using Y.UI.Common.Utility;
using Yandex.Metro.Resources;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Logic
{
  public class MetroService : BaseApplicationService<ApplicationSettings, ApplicationState>
  {
    private int _updateCount;
    private static readonly MetroService _instance = new MetroService();

    public object UniqObject { get; set; }

    public DateTime TimeForRoute { get; set; }

    public bool IsFirstLoad => this.AppSettings.Scheme == null;

    public static MetroService Instance => MetroService._instance;

    public override string AppVersion => ResourcesHelper.Get<string>("ApplicationVersion");

    protected override void SaveSettings()
    {
      AppSettingsProvider.Set<bool>("GpsEnabled", this.AppSettings.GpsEnabled);
      AppSettingsProvider.Set("StartupUuid", this.AppSettings.StartupUuid);
      AppSettingsProvider.Set<bool>("PrivacyPolicyShown", this.AppSettings.PrivacyPolicyShown);
      AppSettingsProvider.Set<bool>("IsShemasCopiedToIso", this.AppSettings.IsShemasCopiedToIso);
      AppSettingsProvider.Set<List<Scheme>>("SchemeData", this.AppSettings.SchemeData);
      AppSettingsProvider.Set<Scheme>("Scheme", this.AppSettings.Scheme);
      AppSettingsProvider.Set<DateTime>("LastUpdate", this.AppSettings.LastUpdate);
      AppSettingsProvider.Set<bool>("IsUserSpecifiedLanguage", this.AppSettings.IsUserSpecifiedLanguage);
      AppSettingsProvider.Set<bool>("AutoselectStation", this.AppSettings.AutoSelectStation);
      AppSettingsProvider.Set("Language", this.AppSettings.Language);
      AppSettingsProvider.Set("MapLanguage", this.AppSettings.MapLanguage);
      AppSettingsProvider.Set<List<ShortRoute>>("History", this.AppSettings.History);
      AppSettingsProvider.Set<ShortRoute>("Route", this.AppSettings.Route);
      AppSettingsProvider.Set<List<Favorites>>("Favorites", this.AppSettings.Favorites);
      AppSettingsProvider.Set<TimeType>("TimeType", this.AppSettings.TimeType);
      AppSettingsProvider.Set<int>("RunCount", this.AppSettings.RunCount);
      AppSettingsProvider.Set<int>("TrackCount", this.AppSettings.TrackCount);
      AppSettingsProvider.Set<bool>("IsProdVersion", this.AppSettings.IsProdVersion);
    }

    protected override void LoadSettings()
    {
      this.AppSettings.GpsEnabled = AppSettingsProvider.Get<bool>("GpsEnabled", true);
      this.AppSettings.StartupUuid = AppSettingsProvider.Get<string>("StartupUuid", (string) null);
      this.AppSettings.PrivacyPolicyShown = AppSettingsProvider.Get<bool>("PrivacyPolicyShown", false);
      this.AppSettings.IsShemasCopiedToIso = AppSettingsProvider.Get<bool>("IsShemasCopiedToIso", false);
      this.AppSettings.SchemeData = AppSettingsProvider.Get<List<Scheme>>("SchemeData", MetroService.GetDefaultSchemes());
      this.AppSettings.Scheme = AppSettingsProvider.Get<Scheme>("Scheme", (Scheme) null);
      this.AppSettings.LastUpdate = AppSettingsProvider.Get<DateTime>("LastUpdate", DateTime.Now.AddDays(-1.0).Date);
      this.AppSettings.IsUserSpecifiedLanguage = AppSettingsProvider.Get<bool>("IsUserSpecifiedLanguage", false);
      this.AppSettings.AutoSelectStation = AppSettingsProvider.Get<bool>("AutoselectStation", true);
      this.AppSettings.History = AppSettingsProvider.Get<List<ShortRoute>>("History", new List<ShortRoute>());
      this.AppSettings.Route = AppSettingsProvider.Get<ShortRoute>("Route", (ShortRoute) null);
      this.AppSettings.Favorites = AppSettingsProvider.Get<List<Favorites>>("Favorites", new List<Favorites>());
      this.AppSettings.TimeType = AppSettingsProvider.Get<TimeType>("TimeType", TimeType.ArrivalTime);
      this.AppSettings.RunCount = AppSettingsProvider.Get<int>("RunCount", 0);
      this.AppSettings.TrackCount = AppSettingsProvider.Get<int>("TrackCount", 0);
      this.AppSettings.IsProdVersion = AppSettingsProvider.Get<bool>("IsProdVersion", true);
      this.LoadLanguageSettings();
      this.UpdateServiceHelperSettings();
      IsolatedStorageHelper.EnsureShemasCopied();
    }

    private void LoadLanguageSettings()
    {
      string defaultLanguageCode = CultureInfo.CurrentCulture.Name;
      string[] strArray = defaultLanguageCode.Split(new char[1]
      {
        '-'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length > 0)
        defaultLanguageCode = strArray[0].ToLower();
      Language language = Language.Languages.FirstOrDefault<Language>((Func<Language, bool>) (r => r.Code == defaultLanguageCode));
      defaultLanguageCode = language == null ? "en-US" : language.UseCode;
      this.AppSettings.MapLanguage = this.AppSettings.IsUserSpecifiedLanguage ? AppSettingsProvider.Get<string>("MapLanguage", defaultLanguageCode) : (string) null;
      this.AppSettings.Language = defaultLanguageCode;
      this.ApplyLanguage(defaultLanguageCode);
    }

    public void ApplyLanguage(string defaultLanguageCode)
    {
      Thread currentThread = Thread.CurrentThread;
      ControlResources.Culture = Localization.Culture = currentThread.CurrentCulture = currentThread.CurrentUICulture = new CultureInfo(defaultLanguageCode);
      this.AppSettings.Language = defaultLanguageCode;
    }

    private static List<Scheme> GetDefaultSchemes()
    {
      List<LanguageCode> languageCodeList1 = new List<LanguageCode>()
      {
        new LanguageCode("ru", true),
        new LanguageCode("en")
      };
      List<LanguageCode> languageCodeList2 = new List<LanguageCode>()
      {
        new LanguageCode("uk", true),
        new LanguageCode("ru"),
        new LanguageCode("en")
      };
      List<LanguageCode> languageCodeList3 = new List<LanguageCode>()
      {
        new LanguageCode("be", true),
        new LanguageCode("ru"),
        new LanguageCode("en")
      };
      SchemeMatrix schemeMatrix1 = new SchemeMatrix()
      {
        M11 = 1.095,
        M22 = 1.095,
        OffsetX = 10.0
      };
      SchemeMatrix schemeMatrix2 = new SchemeMatrix()
      {
        M11 = 0.86,
        M22 = 0.86,
        OffsetX = 20.0
      };
      SchemeMatrix schemeMatrix3 = new SchemeMatrix()
      {
        M11 = 0.54,
        M22 = 0.54,
        OffsetX = 10.0
      };
      SchemeMatrix schemeMatrix4 = new SchemeMatrix()
      {
        M11 = 0.58,
        M22 = 0.58,
        OffsetX = 10.0
      };
      SchemeMatrix schemeMatrix5 = new SchemeMatrix()
      {
        M11 = 0.461,
        M22 = 0.461,
        OffsetX = 10.0
      };
      return new List<Scheme>()
      {
        new Scheme()
        {
          Id = 1,
          VersionId = "7",
          IsProdVersion = true,
          LanguageCodes = languageCodeList1,
          Coordinate = "55.75;37.616667",
          Matrix = schemeMatrix5
        },
        new Scheme()
        {
          Id = 2,
          VersionId = "2",
          IsProdVersion = true,
          LanguageCodes = languageCodeList1,
          Coordinate = "59.95;30.316667",
          Matrix = schemeMatrix4
        },
        new Scheme()
        {
          Id = 8,
          VersionId = "5",
          IsProdVersion = true,
          LanguageCodes = languageCodeList2,
          Coordinate = "50.45;30.523333",
          Matrix = schemeMatrix3
        },
        new Scheme()
        {
          Id = 9,
          VersionId = "1",
          IsProdVersion = true,
          LanguageCodes = languageCodeList2,
          Coordinate = "49.916667;36.316667",
          Matrix = schemeMatrix2
        },
        new Scheme()
        {
          Id = 13,
          VersionId = "2",
          IsProdVersion = true,
          LanguageCodes = languageCodeList3,
          Coordinate = "53.9;27.566667",
          Matrix = schemeMatrix1
        }
      };
    }

    public void DetectSchemasAndRedirect()
    {
      if (this.AppSettings.Scheme == null)
      {
        int defaultSchemeId = this.GetDefaultSchemeBylanguage();
        Scheme defaultScheme = this.AppSettings.SchemeData.FirstOrDefault<Scheme>((Func<Scheme, bool>) (r => r.Id == defaultSchemeId));
        if (this.AppSettings.GpsEnabled)
        {
          Locator.ProgressStatic.StartJob("Progress_DetectingYourPlace");
          Guide.BeginShowMessageBox(Localization.DetectCity_Title, Localization.DetectCity_Text, (IEnumerable<string>) new List<string>()
          {
            Localization.ShakeButton_Yes,
            Localization.ShakeButton_No
          }, 0, MessageBoxIcon.None, (AsyncCallback) (asyncResult =>
          {
            if ((Guide.EndShowMessageBox(asyncResult) ?? -1) == 0)
            {
              GeoCoordinate lastLocation = ServicesHelper.GetLastLocation(this.AppSettings.GpsEnabled);
              if (lastLocation != (GeoCoordinate) null && !lastLocation.IsUnknown)
                this.AppSettings.Scheme = this.DetectNearestCity(lastLocation) ?? defaultScheme;
              else
                this.AppSettings.Scheme = defaultScheme;
              DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => PageNavigationService.Navigate(Constants.ToMainPage)));
            }
            else
              DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => PageNavigationService.Navigate(Constants.ToCitiesFirstLoad)));
          }), (object) null);
          Locator.ProgressStatic.StopJob("Progress_DetectingYourPlace");
        }
        else
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => PageNavigationService.Navigate(Constants.ToCitiesFirstLoad)));
      }
      else
        PageNavigationService.Navigate(Constants.ToMainPage);
    }

    private int GetDefaultSchemeBylanguage()
    {
      switch (this.AppSettings.Language)
      {
        case "be":
          return 13;
        case "uk":
          return 8;
        default:
          return 1;
      }
    }

    private Scheme DetectNearestCity(GeoCoordinate coordinate)
    {
      if (coordinate == (GeoCoordinate) null || coordinate.IsUnknown)
        return (Scheme) null;
      var data = this.AppSettings.SchemeData.Select(c => new
      {
        c = c,
        distance = c.GeoCoordinate.GetDistanceTo(coordinate)
      }).Where(_param0 => _param0.distance < 150000.0).OrderBy(_param0 => _param0.distance).Select(_param0 => new
      {
        Schema = _param0.c,
        Distance = _param0.distance
      }).FirstOrDefault();
      return data?.Schema;
    }

    protected override bool OnHandleException(Exception exception)
    {
      if (exception is WebException)
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => Locator.ProgressStatic.StopAllJobs()));
        return true;
      }
      bool handled = false;
      if (DispatcherHelper.UIDispatcher != null && !DispatcherHelper.UIDispatcher.CheckAccess())
      {
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        DispatcherHelper.UIDispatcher.BeginInvoke((Action) (() =>
        {
          handled = this.ProcessException(exception);
          syncEvent.Set();
        }));
        syncEvent.WaitOne();
      }
      else
        handled = this.ProcessException(exception);
      return handled;
    }

    private bool ProcessException(Exception exception)
    {
      string email = ResourcesHelper.Get<string>("SupportEmail");
      string applicationFailedTitle = Localization.ApplicationFailed_Title;
      if (MessageBox.Show(Localization.ApplicationFailed_MessageLine1 + Environment.NewLine + Environment.NewLine + Localization.ApplicationFailed_MessageLine2, applicationFailedTitle, (MessageBoxButton) 0) == 1)
      {
        if (Environment.DeviceType == 1)
          MessageBox.Show(exception.ToString(), applicationFailedTitle, (MessageBoxButton) 0);
        else
          this.SubmitError(exception, email);
        return true;
      }
      this.OnApplicationClosing();
      return false;
    }

    private void SubmitError(Exception exception, string email)
    {
      string deviceManufacturer = DeviceStatus.DeviceManufacturer;
      string deviceName = DeviceStatus.DeviceName;
      string str1 = Environment.OSVersion.Version.ToString();
      string str2 = PageNavigationService.GetCurrentPage == (Uri) null ? Localization.ErrorReport_NoPage : PageNavigationService.GetCurrentPage.ToString();
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendFormat("{0}\r\n", (object) Localization.ErrorReport_Line1);
      stringBuilder1.AppendFormat("{0}\r\n", (object) Localization.ErrorReport_Line3);
      stringBuilder1.AppendFormat("{2}: {0} {1}\r\n", (object) deviceManufacturer, (object) deviceName, (object) Localization.ErrorReport_Model);
      stringBuilder1.AppendFormat("{1}: {0}\r\n", (object) this.AppVersion, (object) Localization.ErrorReport_AppVersion);
      stringBuilder1.AppendFormat("{1}: {0}\r\n", (object) str1, (object) Localization.ErrorReport_OSVersion);
      stringBuilder1.AppendFormat("{1}: {0}\r\n", (object) str2, (object) Localization.ErrorReport_CurrentPage);
      stringBuilder1.AppendFormat("Время: {0}\r\n", (object) DateTime.Now);
      stringBuilder1.AppendFormat("Маршрут: {0}\r\n", (object) RouteHelper.CurrentRouteAsString);
      stringBuilder1.AppendFormat("\r\n\r\n{0}\r\n=====\r\n", (object) exception);
      string applicationFailedSubject = Localization.ApplicationFailed_Subject;
      StringBuilder stringBuilder2 = new StringBuilder();
      for (Exception exception1 = exception; exception1 != null; exception1 = exception1.InnerException)
        stringBuilder2.AppendFormat("[{0}] {1} | ", (object) exception1.GetType().Name, (object) exception1.Message);
      TaskHelper.SafeExec(new Action(new EmailComposeTask()
      {
        To = email,
        Subject = string.Format("{0}: v{1} - {2}", (object) applicationFailedSubject, (object) this.AppVersion, (object) stringBuilder2),
        Body = stringBuilder1.ToString()
      }.Show));
    }

    protected override void OnApplicationLoading()
    {
      base.OnApplicationLoading();
      ++this.AppSettings.RunCount;
      this.RegisterApplication();
    }

    public void UpdateServiceHelperSettings()
    {
      ServicesHelper.Uuid = this.AppSettings.StartupUuid;
      ServicesHelper.GpsEnabled = this.AppSettings.GpsEnabled;
    }

    public void RegisterApplication()
    {
      ServiceProvider startupService = new ServiceProvider()
      {
        AppVersion = this.AppVersion
      };
      bool runDoubleRegistration = string.IsNullOrEmpty(ServicesHelper.Uuid);
      startupService.RegistrationCompleted += (EventHandler<RegistrationEventArgs>) ((s, e) =>
      {
        if (e.Error == null)
        {
          ServicesHelper.Uuid = this.AppSettings.StartupUuid = e.Uuid;
          if (!string.IsNullOrEmpty(ServicesHelper.Uuid) && runDoubleRegistration)
          {
            runDoubleRegistration = false;
            startupService.BeginStartup(this.AppSettings.RunCount, this.AppSettings.TrackCount, Constants.ServerUrl);
          }
          else
            ThreadPool.QueueUserWorkItem((WaitCallback) (unused =>
            {
              Thread.Sleep(5000);
              this.UpdateSchemePacks(e.Schemes);
              Locator.ProgressStatic.StopJob(Localization.Progress_UpdateScheme);
            }));
        }
        else
          Locator.ProgressStatic.StopJob(Localization.Progress_UpdateScheme);
      });
      startupService.BeginStartup(this.AppSettings.RunCount, this.AppSettings.TrackCount, Constants.ServerUrl);
    }

    private void UpdateSchemePacks(startupSchemesScheme[] schemePacks)
    {
      if (schemePacks == null || schemePacks.Length == 0 || !(this.AppSettings.LastUpdate < DateTime.Now.Date) && !Constants.ForceUpdate || this._updateCount >= 3)
        return;
      List<Scheme> schemeData = this.AppSettings.SchemeData;
      Dictionary<startupSchemesScheme, Scheme> source = new Dictionary<startupSchemesScheme, Scheme>();
      foreach (startupSchemesScheme schemePack in schemePacks)
      {
        startupSchemesScheme startupScheme = schemePack;
        Scheme scheme = schemeData.FirstOrDefault<Scheme>((Func<Scheme, bool>) (r => r.Id == startupScheme.id));
        if (scheme != null && (startupScheme.verID != scheme.VersionId || scheme.IsProdVersion != Constants.IsProdVersion))
          source.Add(startupScheme, scheme);
      }
      if (Constants.ForceUpdate && source.Count == 0)
      {
        this.AppSettings.LastUpdate = DateTime.Now.Date;
        Constants.ForceUpdate = false;
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => MessageBox.Show(Localization.UpdateScheme_UpToDate, Localization.UpdateScheme_Header, (MessageBoxButton) 0)));
      }
      if (source.Count <= 0)
        return;
      AutoResetEvent syncContextEvent = new AutoResetEvent(false);
      int itemsToLoad = source.Count;
      ServiceProvider service = new ServiceProvider();
      Dictionary<int, string> downloadItems = new Dictionary<int, string>();
      service.DownloadSchemeCompleted += (EventHandler<SchemeEventArgs>) ((s, e) =>
      {
        if (e.Error == null && e.SchemePack != null)
        {
          string str = new Regex("version=\".*?\"", RegexOptions.Singleline).Replace(e.SchemePack, "version=\"1.0\"");
          downloadItems.Add(e.SchemeId, str);
        }
        if (--itemsToLoad > 0)
          return;
        syncContextEvent.Set();
      });
      foreach (KeyValuePair<startupSchemesScheme, Scheme> keyValuePair in source)
      {
        KeyValuePair<startupSchemesScheme, Scheme> localItem = keyValuePair;
        ThreadPool.QueueUserWorkItem((WaitCallback) (unused => service.BeginDownloadScheme(localItem.Key.Value, localItem.Key.id)));
      }
      syncContextEvent.WaitOne();
      bool flag1 = source.Count == downloadItems.Count;
      foreach (KeyValuePair<int, string> keyValuePair in downloadItems)
      {
        bool flag2 = SerializeHelper.CanDeserialize<schemepack>(keyValuePair.Value);
        flag1 = flag1 && flag2;
      }
      if (flag1)
      {
        try
        {
          foreach (KeyValuePair<int, string> keyValuePair1 in downloadItems)
          {
            KeyValuePair<int, string> downloadItem = keyValuePair1;
            IsolatedStorageHelper.UpdateScheme(downloadItem.Value, downloadItem.Key);
            KeyValuePair<startupSchemesScheme, Scheme> keyValuePair2 = source.FirstOrDefault<KeyValuePair<startupSchemesScheme, Scheme>>((Func<KeyValuePair<startupSchemesScheme, Scheme>, bool>) (r => r.Value.Id == downloadItem.Key));
            Scheme scheme = keyValuePair2.Value;
            startupSchemesScheme key = keyValuePair2.Key;
            scheme.VersionId = key.verID;
            scheme.IsProdVersion = Constants.IsProdVersion;
          }
          this.AppSettings.LastUpdate = DateTime.Now.Date;
          Constants.ForceUpdate = false;
          if (Locator.MainStatic.MetroMap == null)
            return;
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            Locator.MainStatic.MetroMap.GenerateMap();
            MessageBox.Show(Localization.UpdateScheme_Complete, Localization.UpdateScheme_Header, (MessageBoxButton) 0);
          }));
        }
        catch (Exception ex)
        {
          this.TryUpdateSchemePacks(schemePacks);
        }
      }
      else
        this.TryUpdateSchemePacks(schemePacks);
    }

    private void TryUpdateSchemePacks(startupSchemesScheme[] schemePacks)
    {
      ++this._updateCount;
      ThreadPool.QueueUserWorkItem((WaitCallback) (unused =>
      {
        Thread.Sleep(300000);
        this.UpdateSchemePacks(schemePacks);
      }));
    }
  }
}
