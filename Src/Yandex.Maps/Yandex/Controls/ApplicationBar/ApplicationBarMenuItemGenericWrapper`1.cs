// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ApplicationBar.ApplicationBarMenuItemGenericWrapper`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Input;

namespace Yandex.Controls.ApplicationBar
{
  internal class ApplicationBarMenuItemGenericWrapper<TMenuItem> : FrameworkElement where TMenuItem : class, IApplicationBarMenuItem
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ApplicationBarMenuItemGenericWrapper<TMenuItem>), new PropertyMetadata(new PropertyChangedCallback(ApplicationBarMenuItemGenericWrapper<TMenuItem>.TextPropertyChangedCallback)));
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof (IsEnabled), typeof (bool), typeof (ApplicationBarMenuItemGenericWrapper<TMenuItem>), new PropertyMetadata((object) true, new PropertyChangedCallback(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsEnabledPropertyChangedCallback)));
    public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(nameof (IsVisible), typeof (bool), typeof (ApplicationBarMenuItemGenericWrapper<TMenuItem>), new PropertyMetadata((object) true, new PropertyChangedCallback(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsVisiblePropertyChangedCallback)));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (ApplicationBarMenuItemGenericWrapper<TMenuItem>), (PropertyMetadata) null);
    protected readonly TMenuItem ApplicationBarMenuItem;

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.TextProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.TextProperty, (object) value);
    }

    public bool IsEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsEnabledProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsEnabledProperty, (object) value);
    }

    public bool IsVisible
    {
      get => (bool) ((DependencyObject) this).GetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsVisibleProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.IsVisibleProperty, (object) value);
    }

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.CommandProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarMenuItemGenericWrapper<TMenuItem>.CommandProperty, (object) value);
    }

    public ApplicationBarMenuItemGenericWrapper([NotNull] TMenuItem applicationBarMenuItem)
    {
      this.ApplicationBarMenuItem = (object) applicationBarMenuItem != null ? applicationBarMenuItem : throw new ArgumentNullException(nameof (applicationBarMenuItem));
      this.ApplicationBarMenuItem.Click += new EventHandler(this.ApplicationBarMenuItemClick);
    }

    private void ApplicationBarMenuItemClick(object sender, EventArgs e)
    {
      ICommand command = this.Command;
      if (command == null || !command.CanExecute(sender))
        return;
      command.Execute(sender);
    }

    public TMenuItem Original => this.ApplicationBarMenuItem;

    public event EventHandler Click
    {
      add => this.ApplicationBarMenuItem.Click += value;
      remove => this.ApplicationBarMenuItem.Click -= value;
    }

    private static void TextPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarMenuItemGenericWrapper<TMenuItem> itemGenericWrapper))
        return;
      itemGenericWrapper.ApplicationBarMenuItem.Text = (string) e.NewValue;
    }

    private static void IsEnabledPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarMenuItemGenericWrapper<TMenuItem> itemGenericWrapper))
        return;
      itemGenericWrapper.ApplicationBarMenuItem.IsEnabled = (bool) e.NewValue;
    }

    public event EventHandler IsVisibleChanged;

    private static void IsVisiblePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarMenuItemGenericWrapper<TMenuItem> sender))
        return;
      EventHandler isVisibleChanged = sender.IsVisibleChanged;
      if (isVisibleChanged == null)
        return;
      isVisibleChanged((object) sender, EventArgs.Empty);
    }
  }
}
