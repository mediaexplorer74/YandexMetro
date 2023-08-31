// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.FocusBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Yandex.Controls
{
  internal class FocusBehavior : Behavior<Control>
  {
    public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof (IsFocused), typeof (bool), typeof (FocusBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(FocusBehavior.IsFocusedPropertyChangedCallback)));

    public bool IsFocused
    {
      get => (bool) this.GetValue(FocusBehavior.IsFocusedProperty);
      set => this.SetValue(FocusBehavior.IsFocusedProperty, (object) value);
    }

    private static void IsFocusedPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      ((FocusBehavior) dependencyObject).OnIsFocusedChanged(e);
    }

    private void OnIsFocusedChanged(DependencyPropertyChangedEventArgs eventArgs)
    {
      bool flag = FocusManager.GetFocusedElement() == this.AssociatedObject;
      bool newValue = (bool) eventArgs.NewValue;
      if (newValue && !flag)
      {
        this.AssociatedObject.Focus();
      }
      else
      {
        if (newValue || !flag)
          return;
        this.AssociatedObject.IsEnabled = false;
        this.AssociatedObject.IsEnabled = true;
      }
    }

    protected override void OnAttached()
    {
      ((UIElement) this.AssociatedObject).GotFocus += new RoutedEventHandler(this.AssociatedObjectGotFocus);
      ((UIElement) this.AssociatedObject).LostFocus += new RoutedEventHandler(this.AssociatedObjectLostFocus);
      base.OnAttached();
    }

    private void AssociatedObjectLostFocus(object sender, RoutedEventArgs e) => this.IsFocused = false;

    private void AssociatedObjectGotFocus(object sender, RoutedEventArgs e) => this.IsFocused = true;

    protected override void OnDetaching()
    {
      ((UIElement) this.AssociatedObject).GotFocus -= new RoutedEventHandler(this.AssociatedObjectGotFocus);
      ((UIElement) this.AssociatedObject).LostFocus -= new RoutedEventHandler(this.AssociatedObjectLostFocus);
      base.OnDetaching();
    }
  }
}
