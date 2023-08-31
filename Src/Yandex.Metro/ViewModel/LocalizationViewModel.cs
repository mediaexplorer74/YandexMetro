// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.LocalizationViewModel
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using GalaSoft.MvvmLight;
using System;
using Y.UI.Common;
using Yandex.Metro.Logic;
using Yandex.Metro.Resources;

namespace Yandex.Metro.ViewModel
{
  public class LocalizationViewModel : ViewModelBase
  {
    private static Localization _resources = new Localization();
    private string _selectedLanguage = string.Empty;

    public Localization Resources => LocalizationViewModel._resources;

    public WeakDelegateCommand<string> ChangeLanguage { get; set; }

    public string SelectedLanguage
    {
      get => this._selectedLanguage;
      set
      {
        if (this._selectedLanguage == value)
          return;
        this._selectedLanguage = value;
        this.RaisePropertyChanged(nameof (SelectedLanguage));
        this.ChangeLanguage.Execute(this._selectedLanguage ?? string.Empty);
      }
    }

    public LocalizationViewModel()
    {
      this.ChangeLanguage = new WeakDelegateCommand<string>(new Action<string>(this.OnChangeLanguageExecuted));
      this.SelectedLanguage = MetroService.Instance.AppSettings.Language;
    }

    private void OnChangeLanguageExecuted(string language)
    {
      if (string.IsNullOrEmpty(language))
        return;
      MetroService.Instance.ApplyLanguage(language);
      this.RaisePropertyChanged("Resources");
    }
  }
}
