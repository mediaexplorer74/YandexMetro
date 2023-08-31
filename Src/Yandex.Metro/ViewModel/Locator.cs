// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.Locator
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

namespace Yandex.Metro.ViewModel
{
  public class Locator
  {
    private static MainViewModel _main;
    private static ProgressViewModel _progress;
    private static SettingsViewModel _settings;
    private static LocalizationViewModel _localization;

    public Locator() => Locator._main = new MainViewModel();

    public static MainViewModel MainStatic => Locator._main;

    public static ProgressViewModel ProgressStatic
    {
      get
      {
        if (Locator._progress == null)
          Locator.CreateProgress();
        return Locator._progress;
      }
    }

    public static SettingsViewModel SettingsStatic
    {
      get
      {
        if (Locator._settings == null)
          Locator.CreateSettings();
        return Locator._settings;
      }
    }

    public static LocalizationViewModel LocalizationStatic
    {
      get
      {
        if (Locator._localization == null)
          Locator.CreateLocalization();
        return Locator._localization;
      }
    }

    public MainViewModel Main => Locator.MainStatic;

    public ProgressViewModel Progress => Locator.ProgressStatic;

    public SettingsViewModel Settings => Locator.SettingsStatic;

    public LocalizationViewModel Localization => Locator.LocalizationStatic;

    private static void CreateSettings()
    {
      if (Locator._settings != null)
        return;
      Locator._settings = new SettingsViewModel();
    }

    private static void CreateLocalization()
    {
      if (Locator._localization != null)
        return;
      Locator._localization = new LocalizationViewModel();
    }

    private static void CreateProgress()
    {
      if (Locator._progress != null)
        return;
      Locator._progress = new ProgressViewModel();
    }
  }
}
