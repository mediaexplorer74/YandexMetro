// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.PageNavigationService
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Y.UI.Common
{
  public static class PageNavigationService
  {
    private static readonly object LifetimeObject = new object();
    private static bool _isNavigationInProgress = false;
    private static PhoneApplicationFrame _frame = (PhoneApplicationFrame) null;
    private static string _nextUri = string.Empty;

    public static void Initialize(PhoneApplicationFrame phoneApplicationFrame)
    {
      PageNavigationService._frame = phoneApplicationFrame;
      ((Frame) PageNavigationService._frame).Navigating += new NavigatingCancelEventHandler(PageNavigationService._frame_Navigating);
      ((Frame) PageNavigationService._frame).NavigationFailed += new NavigationFailedEventHandler(PageNavigationService._frame_NavigationFailed);
      ((Frame) PageNavigationService._frame).NavigationStopped += new NavigationStoppedEventHandler(PageNavigationService._frame_NavigationStopped);
      ((Frame) PageNavigationService._frame).Navigated += new NavigatedEventHandler(PageNavigationService._frame_Navigated);
    }

    private static void _frame_Navigated(object sender, NavigationEventArgs e) => PageNavigationService._isNavigationInProgress = false;

    private static void _frame_NavigationStopped(object sender, NavigationEventArgs e) => PageNavigationService._isNavigationInProgress = false;

    private static void _frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => PageNavigationService._isNavigationInProgress = false;

    private static void _frame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      PageNavigationService._nextUri = ((object) e.Uri).ToString();
      PageNavigationService._isNavigationInProgress = true;
    }

    public static bool IsNavigationInProgress => PageNavigationService._isNavigationInProgress;

    public static string NextUriSource => PageNavigationService._nextUri;

    static PageNavigationService() => Messenger.Default.Register<Y.UI.Common.Navigate>(PageNavigationService.LifetimeObject, new Action<Y.UI.Common.Navigate>(PageNavigationService.OnNavigateMessageReceived));

    private static void OnNavigateMessageReceived(Y.UI.Common.Navigate message) => PageNavigationService.Navigate(message);

    public static void Navigate(Y.UI.Common.Navigate message, params object[] arguments) => PageNavigationService.NavigateInFrame(new Uri(string.Format(message.UriTemplate, arguments), UriKind.RelativeOrAbsolute));

    public static void Navigate(string url) => PageNavigationService.NavigateInFrame(new Uri(url, UriKind.RelativeOrAbsolute));

    public static void Navigate(Y.UI.Common.Navigate message) => PageNavigationService.NavigateInFrame(message.UriToNavigate);

    private static void NavigateInFrame(Uri uri)
    {
      if (PageNavigationService._frame == null)
        return;
      ((Frame) PageNavigationService._frame).Navigate(uri);
    }

    public static bool CanGoBack => ((Frame) PageNavigationService._frame).CanGoBack;

    public static void GoBack()
    {
      if (!((Frame) PageNavigationService._frame).CanGoBack)
        return;
      ((Frame) PageNavigationService._frame).GoBack();
    }

    public static Uri GetCurrentPage => PageNavigationService._frame == null ? (Uri) null : ((Frame) PageNavigationService._frame).CurrentSource;

    public static void RemoveSplashScreen()
    {
      foreach (JournalEntry journalEntry in Enumerable.ToList<JournalEntry>(PageNavigationService._frame.BackStack))
        PageNavigationService._frame.RemoveBackEntry();
    }

    public static void NavigateBack(Y.UI.Common.Navigate toPage)
    {
      if (((Frame) PageNavigationService._frame).CurrentSource == toPage.UriToNavigate)
        return;
      bool flag = false;
      while (((Frame) PageNavigationService._frame).CanGoBack)
      {
        JournalEntry journalEntry = Enumerable.FirstOrDefault<JournalEntry>(PageNavigationService._frame.BackStack);
        if (journalEntry != null)
        {
          string str = Enumerable.FirstOrDefault<string>((IEnumerable<string>) journalEntry.Source.OriginalString.Split(new string[1]
          {
            "?"
          }, StringSplitOptions.RemoveEmptyEntries));
          if (!string.IsNullOrWhiteSpace(str))
          {
            if (str != toPage.UriToNavigate.OriginalString)
            {
              PageNavigationService._frame.RemoveBackEntry();
            }
            else
            {
              flag = true;
              ((Frame) PageNavigationService._frame).GoBack();
              break;
            }
          }
          else
            break;
        }
        else
          break;
      }
      if (flag)
        return;
      string originalString = toPage.UriToNavigate.OriginalString;
      if (!originalString.Contains("forgetPreviousPage"))
      {
        StringBuilder stringBuilder = new StringBuilder(originalString);
        stringBuilder.Append(originalString.Contains("?") ? "&" : "?");
        stringBuilder.AppendFormat("&{0}={1}", (object) "forgetPreviousPage", (object) true);
        PageNavigationService.Navigate(new Y.UI.Common.Navigate(stringBuilder.ToString()));
      }
      PageNavigationService.Navigate(toPage);
    }

    public static void ForgetPreviousPage()
    {
      if (Enumerable.FirstOrDefault<JournalEntry>(PageNavigationService._frame.BackStack) == null)
        return;
      PageNavigationService._frame.RemoveBackEntry();
    }
  }
}
