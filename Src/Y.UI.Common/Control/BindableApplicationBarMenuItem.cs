// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.BindableApplicationBarMenuItem
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Input;

namespace Y.UI.Common.Control
{
  public class BindableApplicationBarMenuItem : FrameworkElement, IApplicationBarMenuItem
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(nameof (Command), typeof (ICommand), typeof (BindableApplicationBarMenuItem), (PropertyMetadata) null);
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(nameof (CommandParameter), typeof (object), typeof (BindableApplicationBarMenuItem), (PropertyMetadata) null);
    public static readonly DependencyProperty CommandParameterValueProperty = DependencyProperty.RegisterAttached(nameof (CommandParameterValue), typeof (object), typeof (BindableApplicationBarMenuItem), (PropertyMetadata) null);
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(nameof (IsEnabled), typeof (bool), typeof (BindableApplicationBarMenuItem), new PropertyMetadata((object) true, new PropertyChangedCallback(BindableApplicationBarMenuItem.OnEnabledChanged)));
    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(nameof (Text), typeof (string), typeof (BindableApplicationBarMenuItem), new PropertyMetadata(new PropertyChangedCallback(BindableApplicationBarMenuItem.OnTextChanged)));

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(BindableApplicationBarMenuItem.CommandProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarMenuItem.CommandProperty, (object) value);
    }

    public object CommandParameter
    {
      get => ((DependencyObject) this).GetValue(BindableApplicationBarMenuItem.CommandParameterProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarMenuItem.CommandParameterProperty, value);
    }

    public object CommandParameterValue
    {
      get => ((DependencyObject) this).GetValue(BindableApplicationBarMenuItem.CommandParameterValueProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarMenuItem.CommandParameterValueProperty, value);
    }

    private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBarMenuItem) d).MenuItem.IsEnabled = (bool) e.NewValue;
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBarMenuItem) d).MenuItem.Text = e.NewValue.ToString();
    }

    public ApplicationBarMenuItem MenuItem { get; set; }

    public BindableApplicationBarMenuItem()
    {
      this.MenuItem = new ApplicationBarMenuItem();
      this.MenuItem.Text = nameof (Text);
      this.MenuItem.Click += new EventHandler(this.ApplicationBarMenuItemClick);
    }

    private void ApplicationBarMenuItemClick(object sender, EventArgs e)
    {
      if (this.Command != null && this.CommandParameter != null)
        this.Command.Execute(this.CommandParameter);
      else if (this.Command != null)
      {
        this.Command.Execute(this.CommandParameterValue);
      }
      else
      {
        if (this.Click == null)
          return;
        this.Click(sender, e);
      }
    }

    public bool IsEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(BindableApplicationBarMenuItem.IsEnabledProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarMenuItem.IsEnabledProperty, (object) value);
    }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(BindableApplicationBarMenuItem.TextProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarMenuItem.TextProperty, (object) value);
    }

    public event EventHandler Click;
  }
}
