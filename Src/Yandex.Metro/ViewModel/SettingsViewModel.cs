// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.SettingsViewModel
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Y.Common.Extensions;
using Y.Metro.ServiceLayer.Entities;
using Y.UI.Common;
using Y.UI.Common.Converters;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;
using Yandex.Metro.Resources;

namespace Yandex.Metro.ViewModel
{
  public class SettingsViewModel : ViewModelBase
  {
    private readonly ApplicationSettings _shortCut;
    private bool _touchRotationEnabled;
    private SchemeMeta _actualMeta;
    private ObservableCollection<Language> _languages;
    private int _selectedIndex;

    public WeakDelegateCommand ShowCities { get; set; }

    public WeakDelegateCommand ForceUpdate { get; set; }

    public WeakDelegateCommand<Scheme> SelectionChanged { get; set; }

    public SettingsViewModel()
    {
      this._shortCut = MetroService.Instance.AppSettings;
      this.SelectionChanged = new WeakDelegateCommand<Scheme>(new Action<Scheme>(this.OnSelectionChangedExecuted));
      this.ShowCities = new WeakDelegateCommand((Action) (() => PageNavigationService.Navigate(Constants.ToCities)));
      this.ForceUpdate = new WeakDelegateCommand(new Action(this.OnForceUpdateExecuted));
    }

    public bool AutoSelectStation
    {
      get => this._shortCut.AutoSelectStation;
      set
      {
        if (this._shortCut.AutoSelectStation == value)
          return;
        this._shortCut.AutoSelectStation = value;
        this.RaisePropertyChanged(nameof (AutoSelectStation));
      }
    }

    public bool GpsEnabled
    {
      get => this._shortCut.GpsEnabled;
      set
      {
        if (this._shortCut.GpsEnabled == value)
          return;
        this._shortCut.GpsEnabled = value;
        MetroService.Instance.UpdateServiceHelperSettings();
        Locator.MainStatic.UpdateNearestStation();
        this.RaisePropertyChanged(nameof (GpsEnabled));
      }
    }

    public bool TouchRotationEnabled
    {
      get => this._touchRotationEnabled;
      set
      {
        if (this._touchRotationEnabled == value)
          return;
        this._touchRotationEnabled = value;
        this.RaisePropertyChanged(nameof (TouchRotationEnabled));
      }
    }

    public SchemeMeta ActualMeta
    {
      get => this._actualMeta;
      set
      {
        if (this._actualMeta == value)
          return;
        this._actualMeta = value;
        this.RaisePropertyChanged(nameof (ActualMeta));
      }
    }

    public Language MapLanguage
    {
      get => !this._shortCut.IsUserSpecifiedLanguage || string.IsNullOrWhiteSpace(this._shortCut.MapLanguage) ? ((IEnumerable<Language>) this.Languages).FirstOrDefault<Language>((Func<Language, bool>) (r => r.IsDefault)) : ((IEnumerable<Language>) this.Languages).FirstOrDefault<Language>((Func<Language, bool>) (r => r.Code == this._shortCut.MapLanguage)) ?? ((IEnumerable<Language>) this.Languages).FirstOrDefault<Language>((Func<Language, bool>) (r => r.IsDefault));
      set
      {
        if (this._shortCut.MapLanguage == value.Code)
          return;
        this._shortCut.IsUserSpecifiedLanguage = true;
        this._shortCut.MapLanguage = value.Code;
        this.RaisePropertyChanged(nameof (MapLanguage));
      }
    }

    public ObservableCollection<Language> Languages
    {
      get
      {
        if (this._languages == null)
        {
          this._languages = new ObservableCollection<Language>();
          foreach (LanguageCode languageCode in this._shortCut.Scheme.LanguageCodes)
            ((Collection<Language>) this._languages).Add(new Language(this.GetLanguageNameByCode(languageCode.Code), languageCode.Code, languageCode.IsDefault));
        }
        return this._languages;
      }
      set
      {
        if (this._languages == value)
          return;
        this._languages = value;
        this.RaisePropertyChanged(nameof (Languages));
      }
    }

    public int SelectedIndex
    {
      get => this._selectedIndex;
      set
      {
        if (this._selectedIndex == value)
          return;
        this._selectedIndex = value;
        this.RaisePropertyChanged(nameof (SelectedIndex));
      }
    }

    public Scheme SelectedScheme
    {
      get => this._shortCut.Scheme == null ? (Scheme) null : this._shortCut.SchemeData.FirstOrDefault<Scheme>((Func<Scheme, bool>) (r => r.Id == this._shortCut.Scheme.Id));
      set
      {
        this._shortCut.Scheme = value;
        this._languages = (ObservableCollection<Language>) null;
        this.RaisePropertyChanged("Languages");
        this.RaisePropertyChanged("MapLanguage");
        this.RaisePropertyChanged(nameof (SelectedScheme));
      }
    }

    public ObservableCollection<Scheme> Schemes => this._shortCut.SchemeData.ToObservableCollection<Scheme>();

    public string UpdateText => string.Format(Localization.Settings_UpdateText, new RelativeTimeConverter().Convert((object) this._shortCut.LastUpdate, (Type) null, (object) null, Thread.CurrentThread.CurrentCulture));

    private string GetLanguageNameByCode(string locale)
    {
      switch (locale)
      {
        case "en":
          return Localization.Language_En;
        case "uk":
          return Localization.Language_Ukr;
        case "be":
          return Localization.Language_Bel;
        case "ru":
          return Localization.Language_Rus;
        default:
          return (string) null;
      }
    }

    private void OnForceUpdateExecuted()
    {
      Locator.ProgressStatic.StartJob(Localization.Progress_UpdateScheme);
      Constants.ForceUpdate = true;
      MetroService.Instance.RegisterApplication();
    }

    private void OnSelectionChangedExecuted(Scheme obj)
    {
      if (obj == null)
        return;
      if (MetroService.Instance.IsFirstLoad)
      {
        this._shortCut.Scheme = obj;
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => PageNavigationService.Navigate(Constants.ToMainPage)));
      }
      else
      {
        this.SelectedScheme = obj;
        Locator.MainStatic.ClearRoute();
        PageNavigationService.GoBack();
      }
    }
  }
}
