// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Touch
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Input.Interfaces;

namespace Yandex.Input
{
  [ComVisible(false)]
  public class Touch
  {
    public static readonly DependencyProperty TouchWrapperProperty = DependencyProperty.RegisterAttached("TouchWrapper", typeof (ITouchWrapper), typeof (Touch), new PropertyMetadata((object) null, new PropertyChangedCallback(Touch.TouchWrapperChanged)));

    private static void TouchWrapperChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement frameworkElement) || !(e.NewValue is TouchWrapper newValue))
        return;
      newValue.Control = frameworkElement;
    }

    public static void SetTouchWrapper(UIElement uiElement, ITouchWrapper value) => ((DependencyObject) uiElement).SetValue(Touch.TouchWrapperProperty, (object) value);

    public static ITouchWrapper GetTouchWrapper(UIElement uiElement) => ((DependencyObject) uiElement).GetValue(Touch.TouchWrapperProperty) as ITouchWrapper;
  }
}
