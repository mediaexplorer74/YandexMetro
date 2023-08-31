// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.BindableApplicationBar
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Y.UI.Common.Control
{
  [ContentProperty("Buttons")]
  public class BindableApplicationBar : ItemsControl, IApplicationBar
  {
    private readonly ApplicationBar _applicationBar;
    public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(nameof (IsVisible), typeof (bool), typeof (BindableApplicationBar), new PropertyMetadata((object) true, new PropertyChangedCallback(BindableApplicationBar.OnVisibleChanged)));
    public static readonly DependencyProperty IsMenuEnabledProperty = DependencyProperty.RegisterAttached(nameof (IsMenuEnabled), typeof (bool), typeof (BindableApplicationBar), new PropertyMetadata((object) true, new PropertyChangedCallback(BindableApplicationBar.OnEnabledChanged)));

    public BindableApplicationBar()
    {
      this._applicationBar = new ApplicationBar();
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.BindableApplicationBarLoaded);
      this._applicationBar.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.ApplicationBarStateChanged);
    }

    private void ApplicationBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
    {
      if (this.StateChanged == null)
        return;
      this.StateChanged((object) this, e);
    }

    private void BindableApplicationBarLoaded(object sender, RoutedEventArgs e)
    {
      if (!(Enumerable.FirstOrDefault<FrameworkElement>(Enumerable.Where<FrameworkElement>(VisualTreeHelperExtensions.GetVisualAncestors((FrameworkElement) this), (Func<FrameworkElement, bool>) (c => c is PhoneApplicationPage))) is PhoneApplicationPage phoneApplicationPage))
        return;
      phoneApplicationPage.ApplicationBar = (IApplicationBar) this._applicationBar;
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      this._applicationBar.Buttons.Clear();
      this._applicationBar.MenuItems.Clear();
      foreach (BindableApplicationBarIconButton applicationBarIconButton in Enumerable.Where<object>((IEnumerable<object>) this.Items, (Func<object, bool>) (c => c is BindableApplicationBarIconButton)))
        this._applicationBar.Buttons.Add((object) applicationBarIconButton.Button);
      foreach (BindableApplicationBarMenuItem applicationBarMenuItem in Enumerable.Where<object>((IEnumerable<object>) this.Items, (Func<object, bool>) (c => c is BindableApplicationBarMenuItem)))
        this._applicationBar.MenuItems.Add((object) applicationBarMenuItem.MenuItem);
    }

    private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBar) d)._applicationBar.IsVisible = (bool) e.NewValue;
    }

    private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((BindableApplicationBar) d)._applicationBar.IsMenuEnabled = (bool) e.NewValue;
    }

    public bool IsVisible
    {
      get => (bool) ((DependencyObject) this).GetValue(BindableApplicationBar.IsVisibleProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBar.IsVisibleProperty, (object) value);
    }

    public double BarOpacity
    {
      get => this._applicationBar.Opacity;
      set => this._applicationBar.Opacity = value;
    }

    public bool IsMenuEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(BindableApplicationBar.IsMenuEnabledProperty);
      set => ((DependencyObject) this).SetValue(BindableApplicationBar.IsMenuEnabledProperty, (object) value);
    }

    public Color BackgroundColor
    {
      get => this._applicationBar.BackgroundColor;
      set => this._applicationBar.BackgroundColor = value;
    }

    public Color ForegroundColor
    {
      get => this._applicationBar.ForegroundColor;
      set => this._applicationBar.ForegroundColor = value;
    }

    public ApplicationBarMode Mode
    {
      get => this._applicationBar.Mode;
      set => this._applicationBar.Mode = value;
    }

    public IList Buttons => (IList) this.Items;

    public IList MenuItems => (IList) this.Items;

    public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;

    public double DefaultSize { get; set; }

    public double MiniSize { get; set; }

    [SpecialName]
    double IApplicationBar.get_Opacity() => ((UIElement) this).Opacity;

    [SpecialName]
    void IApplicationBar.set_Opacity(double value) => ((UIElement) this).Opacity = value;
  }
}
