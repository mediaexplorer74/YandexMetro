// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.AppSettingsProvider
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.IO.IsolatedStorage;

namespace Y.UI.Common.Utility
{
  public static class AppSettingsProvider
  {
    private static IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;

    public static void Set(string settingName, string value) => AppSettingsProvider.Set<string>(settingName, value);

    public static void Set<TValue>(string settingName, TValue value)
    {
      if (!AppSettingsProvider.Settings.Contains(settingName))
        AppSettingsProvider.Settings.Add(settingName, (object) value);
      else
        AppSettingsProvider.Settings[settingName] = (object) value;
    }

    public static bool TryGet<TValue>(string settingName, out TValue value, bool throwOnGetError = false)
    {
      if (AppSettingsProvider.Settings.Contains(settingName))
      {
        try
        {
          value = (TValue) AppSettingsProvider.Settings[settingName];
          return true;
        }
        catch (Exception ex)
        {
          if (throwOnGetError)
            throw;
        }
      }
      value = default (TValue);
      return false;
    }

    public static T Get<T>(string settingName, T defaultValue)
    {
      T obj;
      return AppSettingsProvider.TryGet<T>(settingName, out obj) ? obj : defaultValue;
    }

    public static void Save() => AppSettingsProvider.Settings.Save();
  }
}
