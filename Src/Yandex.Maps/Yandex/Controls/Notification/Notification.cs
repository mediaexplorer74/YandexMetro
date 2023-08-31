// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Notification.Notification
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Yandex.Controls.Notification
{
  internal static class Notification
  {
    private const int DefaultMargin = 24;

    private static PhoneApplicationFrame RootFrame => Application.Current.RootVisual as PhoneApplicationFrame;

    private static PhoneApplicationPage CurrentPage
    {
      get
      {
        PhoneApplicationPage currentPage = (PhoneApplicationPage) null;
        if (Yandex.Controls.Notification.Notification.RootFrame != null)
          currentPage = ((ContentControl) Yandex.Controls.Notification.Notification.RootFrame).Content as PhoneApplicationPage;
        return currentPage;
      }
    }

    private static double ApplicationBarSize
    {
      get
      {
        if (Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar == null || !Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar.IsVisible || Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar.Opacity >= 1.0)
          return 0.0;
        return Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar.Mode != null ? Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar.MiniSize : Yandex.Controls.Notification.Notification.CurrentPage.ApplicationBar.DefaultSize;
      }
    }

    private static NotificationControl NotificationControl => ((IEnumerable) ((Panel) Yandex.Controls.Notification.Notification.GetLayoutRootGrid()).Children).OfType<NotificationControl>().FirstOrDefault<NotificationControl>();

    public static void ShowNotification(string message)
    {
      NotificationControl notificationControl = Yandex.Controls.Notification.Notification.NotificationControl;
      if (notificationControl == null)
      {
        notificationControl = new NotificationControl();
        Grid layoutRootGrid = Yandex.Controls.Notification.Notification.GetLayoutRootGrid();
        if (layoutRootGrid == null)
          return;
        ((PresentationFrameworkCollection<UIElement>) ((Panel) layoutRootGrid).Children).Add((UIElement) notificationControl);
        if (((PresentationFrameworkCollection<ColumnDefinition>) layoutRootGrid.ColumnDefinitions).Count > 1)
          Grid.SetColumnSpan((FrameworkElement) notificationControl, ((PresentationFrameworkCollection<ColumnDefinition>) layoutRootGrid.ColumnDefinitions).Count);
        if (((PresentationFrameworkCollection<RowDefinition>) layoutRootGrid.RowDefinitions).Count > 1)
          Grid.SetRowSpan((FrameworkElement) notificationControl, ((PresentationFrameworkCollection<RowDefinition>) layoutRootGrid.RowDefinitions).Count);
        ((FrameworkElement) notificationControl).HorizontalAlignment = (HorizontalAlignment) 1;
        ((FrameworkElement) notificationControl).VerticalAlignment = (VerticalAlignment) 2;
        Canvas.SetZIndex((UIElement) notificationControl, (int) ushort.MaxValue);
        ((UIElement) Yandex.Controls.Notification.Notification.CurrentPage).ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(Yandex.Controls.Notification.Notification.CurrentPageManipulationStarted);
      }
      ((UIElement) notificationControl).Visibility = (Visibility) 0;
      Thickness thickness = new Thickness(24.0);
      PageOrientation orientation = Yandex.Controls.Notification.Notification.CurrentPage.Orientation;
      if (orientation <= 5)
      {
        if (orientation == 1 || orientation == 5)
          thickness.Bottom += Yandex.Controls.Notification.Notification.ApplicationBarSize;
      }
      else if (orientation != 18)
      {
        if (orientation == 34)
          thickness.Left += Yandex.Controls.Notification.Notification.ApplicationBarSize;
      }
      else
        thickness.Right += Yandex.Controls.Notification.Notification.ApplicationBarSize;
      ((FrameworkElement) notificationControl).Margin = thickness;
      notificationControl.Text = message;
      notificationControl.BeginStoryboard();
    }

    private static Grid GetLayoutRootGrid() => VisualTreeHelper.GetChildrenCount((DependencyObject) Yandex.Controls.Notification.Notification.CurrentPage) != 1 ? (Grid) null : VisualTreeHelper.GetChild((DependencyObject) Yandex.Controls.Notification.Notification.CurrentPage, 0) as Grid;

    private static void CurrentPageManipulationStarted(
      object sender,
      ManipulationStartedEventArgs e)
    {
      if (Yandex.Controls.Notification.Notification.NotificationControl == null)
        return;
      ((UIElement) Yandex.Controls.Notification.Notification.NotificationControl).Visibility = (Visibility) 1;
    }
  }
}
