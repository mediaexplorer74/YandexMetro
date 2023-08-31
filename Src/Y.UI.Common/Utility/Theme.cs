// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.Theme
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;

namespace Y.UI.Common.Utility
{
  public static class Theme
  {
    private static bool _isDark = true;

    public static bool IsDark
    {
      get => Theme._isDark;
      private set
      {
        if (Theme._isDark == value)
          return;
        Theme._isDark = value;
      }
    }

    public static void Refresh() => Theme.IsDark = (Visibility) Application.Current.Resources[(object) "PhoneLightThemeVisibility"] == 1;
  }
}
