// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Themes.LightThemeDispatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Yandex.Controls.Themes
{
  internal class LightThemeDispatcher : IThemeDispatcher
  {
    private const string DefaultLightResourceDictionary = "Themes\\light\\ResourceDictionary.xaml";
    private readonly Uri _lightResourceDictionary;

    public LightThemeDispatcher()
      : this(new Uri("Themes\\light\\ResourceDictionary.xaml", UriKind.Relative))
    {
    }

    public LightThemeDispatcher(Uri lightResourceDictionary) => this._lightResourceDictionary = !(lightResourceDictionary == (Uri) null) ? lightResourceDictionary : throw new ArgumentNullException(nameof (lightResourceDictionary));

    public void LoadTheme()
    {
      if (PhoneThemes.GetCurrentTheme() == PhoneTheme.Dark)
        return;
      using (StreamReader streamReader = new StreamReader(Application.GetResourceStream(this._lightResourceDictionary).Stream))
      {
        if (!(XamlReader.Load(streamReader.ReadToEnd()) is ResourceDictionary resourceDictionary))
          return;
        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
      }
    }
  }
}
