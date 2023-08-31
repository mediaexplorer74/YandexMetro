// Decompiled with JetBrains decompiler
// Type: Yandex.ShellHelper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using Microsoft.Phone.Tasks;
using System;
using Yandex.Common;

namespace Yandex
{
  public static class ShellHelper
  {
    public static bool TryOpenLinkInWebBrowser(Uri link)
    {
      try
      {
        new WebBrowserTask() { Uri = link }.Show();
        return true;
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
        return false;
      }
    }

    public static bool TryOpenLinkInWebBrowser(string link)
    {
      try
      {
        return ShellHelper.TryOpenLinkInWebBrowser(new Uri(link, UriKind.Absolute));
      }
      catch (UriFormatException ex)
      {
        Logger.TrackException((Exception) ex);
        return false;
      }
    }
  }
}
