// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TransitionService
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public static class TransitionService
  {
    public static readonly DependencyProperty NavigationInTransitionProperty = DependencyProperty.RegisterAttached("NavigationInTransition", typeof (NavigationInTransition), typeof (TransitionService), (PropertyMetadata) null);
    public static readonly DependencyProperty NavigationOutTransitionProperty = DependencyProperty.RegisterAttached("NavigationOutTransition", typeof (NavigationOutTransition), typeof (TransitionService), (PropertyMetadata) null);

    public static NavigationInTransition GetNavigationInTransition(UIElement element) => element != null ? (NavigationInTransition) ((DependencyObject) element).GetValue(TransitionService.NavigationInTransitionProperty) : throw new ArgumentNullException(nameof (element));

    public static NavigationOutTransition GetNavigationOutTransition(UIElement element) => element != null ? (NavigationOutTransition) ((DependencyObject) element).GetValue(TransitionService.NavigationOutTransitionProperty) : throw new ArgumentNullException(nameof (element));

    public static void SetNavigationInTransition(UIElement element, NavigationInTransition value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      ((DependencyObject) element).SetValue(TransitionService.NavigationInTransitionProperty, (object) value);
    }

    public static void SetNavigationOutTransition(UIElement element, NavigationOutTransition value)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      ((DependencyObject) element).SetValue(TransitionService.NavigationOutTransitionProperty, (object) value);
    }
  }
}
