// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.BindableApplicationBarExtensions
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

namespace Y.UI.Common.Control
{
  public static class BindableApplicationBarExtensions
  {
    public static void SetVisible(
      this BindableApplicationBar bar,
      params BindableApplicationBarIconButton[] buttons)
    {
      foreach (BindableApplicationBarIconButton button in buttons)
      {
        if (!bar.Buttons.Contains((object) button))
          bar.Buttons.Add((object) button);
      }
    }

    public static void SetHidden(
      this BindableApplicationBar bar,
      params BindableApplicationBarIconButton[] buttons)
    {
      foreach (BindableApplicationBarIconButton button in buttons)
      {
        if (bar.Buttons.Contains((object) button))
          bar.Buttons.Remove((object) button);
      }
    }
  }
}
