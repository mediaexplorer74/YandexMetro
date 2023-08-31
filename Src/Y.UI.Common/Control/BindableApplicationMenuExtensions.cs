// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.BindableApplicationMenuExtensions
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

namespace Y.UI.Common.Control
{
  public static class BindableApplicationMenuExtensions
  {
    public static void SetVisible(
      this BindableApplicationBar bar,
      params BindableApplicationBarMenuItem[] menuItems)
    {
      foreach (BindableApplicationBarMenuItem menuItem in menuItems)
      {
        if (!bar.MenuItems.Contains((object) menuItem))
          bar.MenuItems.Add((object) menuItem);
      }
    }

    public static void SetHidden(
      this BindableApplicationBar bar,
      params BindableApplicationBarMenuItem[] menuItems)
    {
      foreach (BindableApplicationBarMenuItem menuItem in menuItems)
      {
        if (bar.MenuItems.Contains((object) menuItem))
          bar.MenuItems.Remove((object) menuItem);
      }
    }
  }
}
