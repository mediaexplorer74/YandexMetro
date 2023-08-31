// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.BindableApplicationBarIconButton
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Input;

namespace Y.UI.Common.Control
{
  public class BindableApplicationBarIconButton : 
    FrameworkElement,
    IApplicationBarIconButton,
    IApplicationBarMenuItem
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(nameof (Command), typeof (ICommand), typeof (BindableApplicationBarIconButton), (PropertyMetadata) null);
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(nameof (CommandParameter), typeof (object), typeof (BindableApplicationBarIconButton), (PropertyMetadata) null);
    public static readonly DependencyProperty CommandParameterValueProperty = DependencyProperty.RegisterAttached(nameof (CommandParameterValue), typeof (object), typeof (BindableApplicationBarMenuItem), (PropertyMetadata) null);
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(nameof (IsEnabled), typeof (bool), typeof (BindableApplicationBarIconButton), new PropertyMetadata((object) true, new PropertyChangedCallback(BindableApplicationBarIconButton.OnEnabledChanged)));
    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(nameof (Text), typeof (string), typeof (BindableApplicationBarIconButton), new PropertyMetadata(new PropertyChangedCallback(BindableApplicationBarIconButton.OnTextChanged)));
    public static readonly DependencyProperty IconUriProperty = DependencyProperty.RegisterAttached(nameof (IconUri), typeof (Uri), typeof (BindableApplicationBarIconButton), new PropertyMetadata(new PropertyChangedCallback(BindableApplicationBarIconButton.OnIconUriChanged)));

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.CommandProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.CommandProperty, (object) value);
    }

    public object CommandParameter
    {
      get => ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.CommandParameterProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.CommandParameterProperty, value);
    }

    public object CommandParameterValue
    {
      get => ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.CommandParameterValueProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.CommandParameterValueProperty, value);
    }

    private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBarIconButton) d).Button.IsEnabled = (bool) e.NewValue;
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBarIconButton) d).Button.Text = e.NewValue.ToString();
    }

    private static void OnIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBarIconButton) d).Button.IconUri = (Uri) e.NewValue;
    }

    public ApplicationBarIconButton Button { get; set; }

    public BindableApplicationBarIconButton()
    {
      this.Button = new ApplicationBarIconButton();
      this.Button.Text = nameof (Text);
      this.Button.IconUri = new Uri("/none.png", UriKind.Relative);
      this.Button.Click += new EventHandler(this.ApplicationBarIconButtonClick);
    }

    private void ApplicationBarIconButtonClick(object sender, EventArgs e)
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
        this.Click((object) this, EventArgs.Empty);
      }
    }

    public bool IsEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.IsEnabledProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.IsEnabledProperty, (object) value);
    }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.TextProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.TextProperty, (object) value);
    }

    public event EventHandler Click;

    public Uri IconUri
    {
      get => (Uri) ((DependencyObject) this).GetValue(BindableApplicationBarIconButton.IconUriProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBarIconButton.IconUriProperty, (object) value);
    }
  }
}
