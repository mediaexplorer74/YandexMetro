// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.FocusBehavior
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Y.UI.Common.Behaviors
{
  public class FocusBehavior : Behavior<Control>
  {
    public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(nameof (IsFocused), typeof (bool), typeof (FocusBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(FocusBehavior.IsFocusedChanged)));
    private bool _manualGotFocus;
    private bool _programGotFocus;

    public bool IsFocused
    {
      get => (bool) this.GetValue(FocusBehavior.IsFocusedProperty);
      set => this.SetValue(FocusBehavior.IsFocusedProperty, (object) value);
    }

    private static void IsFocusedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      FocusBehavior focusBehavior = obj as FocusBehavior;
      if ((bool) e.NewValue)
      {
        if (focusBehavior == null)
          return;
        if (focusBehavior._manualGotFocus)
        {
          focusBehavior._manualGotFocus = false;
        }
        else
        {
          focusBehavior._programGotFocus = true;
          focusBehavior.AssociatedObject.Focus();
        }
      }
      else
        ((Control) ((FrameworkElement) focusBehavior.AssociatedObject).FindPageBaseParent())?.Focus();
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      ((UIElement) this.AssociatedObject).GotFocus += new RoutedEventHandler(this.AssociatedObject_GotFocus);
      ((UIElement) this.AssociatedObject).LostFocus += new RoutedEventHandler(this.AssociatedObject_LostFocus);
      ((UIElement) this.AssociatedObject).KeyUp += new KeyEventHandler(this.AssociatedObject_KeyUp);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      ((UIElement) this.AssociatedObject).GotFocus -= new RoutedEventHandler(this.AssociatedObject_GotFocus);
      ((UIElement) this.AssociatedObject).LostFocus -= new RoutedEventHandler(this.AssociatedObject_LostFocus);
      ((UIElement) this.AssociatedObject).KeyUp -= new KeyEventHandler(this.AssociatedObject_KeyUp);
    }

    private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
    {
      if (e.OriginalSource.GetType() != typeof (TextBox))
        return;
      TextBox originalSource = (TextBox) e.OriginalSource;
      if (string.IsNullOrEmpty(originalSource.SelectedText))
        return;
      originalSource.Select(0, 0);
    }

    private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
    {
      if (e.OriginalSource.GetType() != typeof (TextBox))
        return;
      if (!this._programGotFocus)
        this._manualGotFocus = true;
      else
        this._programGotFocus = false;
      this.IsFocused = true;
      ((TextBox) e.OriginalSource).SelectAll();
    }

    private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.PlatformKeyCode != 27 && e.Key != 8)
        return;
      this.IsFocused = false;
      e.Handled = true;
    }
  }
}
