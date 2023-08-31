// Decompiled with JetBrains decompiler
// Type: Yandex.ShellHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Tasks;
using System;
using Yandex.Common;

namespace Yandex
{
  internal static class ShellHelper
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
