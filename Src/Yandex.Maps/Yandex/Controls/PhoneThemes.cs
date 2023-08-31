// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.PhoneThemes
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;

namespace Yandex.Controls
{
  internal class PhoneThemes
  {
    public static PhoneTheme GetCurrentTheme() => (Visibility) Application.Current.Resources[(object) "PhoneLightThemeVisibility"] != null ? PhoneTheme.Dark : PhoneTheme.Light;
  }
}
