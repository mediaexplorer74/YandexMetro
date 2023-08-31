// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ApplicationBar.ApplicationBarWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Yandex.Controls.ApplicationBar
{
  [ContentProperty("Buttons")]
  internal class ApplicationBarWrapper : Panel
  {
    private readonly ObservableCollection<ApplicationBarIconButtonWrapper> _buttons = new ObservableCollection<ApplicationBarIconButtonWrapper>();
    private IApplicationBar _applicationBar;
    private bool _isApplicationBarItemsInited;
    public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(nameof (IsVisible), typeof (bool), typeof (ApplicationBarWrapper), new PropertyMetadata((object) false, new PropertyChangedCallback(ApplicationBarWrapper.IsVisibleChanged)));
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (ApplicationBarMode), typeof (ApplicationBarWrapper), new PropertyMetadata((object) (ApplicationBarMode) 0, new PropertyChangedCallback(ApplicationBarWrapper.ModeChanged)));

    public ObservableCollection<ApplicationBarIconButtonWrapper> Buttons => this._buttons;

    public ObservableCollection<ApplicationBarMenuItemWrapper> MenuItems { get; private set; }

    public ApplicationBarWrapper()
    {
      this._buttons.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ButtonsCollectionChanged);
      this.MenuItems = new ObservableCollection<ApplicationBarMenuItemWrapper>();
      this.MenuItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.MenuItemsCollectionChanged);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.ApplicationBarWrapperLoaded);
    }

    private void ButtonsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (ApplicationBarIconButtonWrapper oldItem in (IEnumerable) e.OldItems)
        {
          oldItem.IsVisibleChanged -= new EventHandler(this.ItemIsVisibleChanged);
          if (this._applicationBar != null)
            this._applicationBar.Buttons.Remove((object) oldItem);
          ((PresentationFrameworkCollection<UIElement>) this.Children).Remove((UIElement) oldItem);
        }
      }
      if (e.NewItems == null)
        return;
      foreach (ApplicationBarIconButtonWrapper newItem in (IEnumerable) e.NewItems)
      {
        if (this._applicationBar != null && newItem.IsVisible && !this._applicationBar.Buttons.Contains((object) newItem.Original))
          this._applicationBar.Buttons.Add((object) newItem.Original);
        newItem.IsVisibleChanged += new EventHandler(this.ItemIsVisibleChanged);
        ((PresentationFrameworkCollection<UIElement>) this.Children).Add((UIElement) newItem);
      }
    }

    private void ItemIsVisibleChanged(object sender, EventArgs e)
    {
      ApplicationBarIconButtonWrapper iconButtonWrapper = (ApplicationBarIconButtonWrapper) sender;
      if (this._applicationBar == null || iconButtonWrapper.IsVisible && this._applicationBar.Buttons.Contains((object) iconButtonWrapper.Original))
        return;
      this._applicationBar.Buttons.Clear();
      foreach (ApplicationBarMenuItemGenericWrapper<ApplicationBarIconButton> itemGenericWrapper in ((IEnumerable<ApplicationBarIconButtonWrapper>) this._buttons).Where<ApplicationBarIconButtonWrapper>((Func<ApplicationBarIconButtonWrapper, bool>) (button => button.IsVisible)).Take<ApplicationBarIconButtonWrapper>(4))
        this._applicationBar.Buttons.Add((object) itemGenericWrapper.Original);
    }

    private void MenuItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (ApplicationBarMenuItemWrapper oldItem in (IEnumerable) e.OldItems)
        {
          oldItem.IsVisibleChanged -= new EventHandler(this.MenuItemIsVisibleChanged);
          if (this._applicationBar != null)
            this._applicationBar.MenuItems.Remove((object) oldItem);
          ((PresentationFrameworkCollection<UIElement>) this.Children).Remove((UIElement) oldItem);
        }
      }
      if (e.NewItems == null)
        return;
      foreach (ApplicationBarMenuItemWrapper newItem in (IEnumerable) e.NewItems)
      {
        if (this._applicationBar != null && newItem.IsVisible && !this._applicationBar.MenuItems.Contains((object) newItem.Original))
          this._applicationBar.MenuItems.Add((object) newItem.Original);
        newItem.IsVisibleChanged += new EventHandler(this.MenuItemIsVisibleChanged);
        ((PresentationFrameworkCollection<UIElement>) this.Children).Add((UIElement) newItem);
      }
    }

    private void MenuItemIsVisibleChanged(object sender, EventArgs e)
    {
      ApplicationBarMenuItemWrapper barMenuItemWrapper = (ApplicationBarMenuItemWrapper) sender;
      if (this._applicationBar == null)
        return;
      if (barMenuItemWrapper.IsVisible)
      {
        if (this._applicationBar.MenuItems.Contains((object) barMenuItemWrapper.Original))
          return;
        this._applicationBar.MenuItems.Clear();
        foreach (ApplicationBarMenuItemGenericWrapper<ApplicationBarMenuItem> itemGenericWrapper in ((IEnumerable<ApplicationBarMenuItemWrapper>) this.MenuItems).Where<ApplicationBarMenuItemWrapper>((Func<ApplicationBarMenuItemWrapper, bool>) (menuItem => menuItem.IsVisible)))
          this._applicationBar.MenuItems.Add((object) itemGenericWrapper.Original);
      }
      else
        this._applicationBar.MenuItems.Remove((object) barMenuItemWrapper.Original);
    }

    private void InitApplicationBarItems()
    {
      if (this._isApplicationBarItemsInited)
        return;
      if (this._applicationBar == null)
        throw new InvalidOperationException();
      this._isApplicationBarItemsInited = true;
      ApplicationBarIconButton[] array1 = ((IEnumerable<ApplicationBarIconButtonWrapper>) this._buttons).Where<ApplicationBarIconButtonWrapper>((Func<ApplicationBarIconButtonWrapper, bool>) (button => button.IsVisible)).Select<ApplicationBarIconButtonWrapper, ApplicationBarIconButton>((Func<ApplicationBarIconButtonWrapper, ApplicationBarIconButton>) (button => button.Original)).ToArray<ApplicationBarIconButton>();
      foreach (ApplicationBarIconButton button in (IEnumerable) this._applicationBar.Buttons)
        ((Collection<ApplicationBarIconButtonWrapper>) this._buttons).Add(new ApplicationBarIconButtonWrapper(button));
      foreach (object obj in array1)
        this._applicationBar.Buttons.Add(obj);
      ApplicationBarMenuItem[] array2 = ((IEnumerable<ApplicationBarMenuItemWrapper>) this.MenuItems).Where<ApplicationBarMenuItemWrapper>((Func<ApplicationBarMenuItemWrapper, bool>) (menuItem => menuItem.IsVisible)).Select<ApplicationBarMenuItemWrapper, ApplicationBarMenuItem>((Func<ApplicationBarMenuItemWrapper, ApplicationBarMenuItem>) (menuItem => menuItem.Original)).ToArray<ApplicationBarMenuItem>();
      foreach (ApplicationBarMenuItem menuItem in (IEnumerable) this._applicationBar.MenuItems)
        ((Collection<ApplicationBarMenuItemWrapper>) this.MenuItems).Add(new ApplicationBarMenuItemWrapper(menuItem));
      foreach (object obj in array2)
        this._applicationBar.MenuItems.Add(obj);
    }

    private void ApplicationBarWrapperLoaded(object sender, RoutedEventArgs e)
    {
      if (!(((DependencyObject) this).GetVisualAncestors().Where<DependencyObject>((Func<DependencyObject, bool>) (c => c is PhoneApplicationPage)).FirstOrDefault<DependencyObject>() is PhoneApplicationPage phoneApplicationPage))
        return;
      this._applicationBar = phoneApplicationPage.ApplicationBar ?? (phoneApplicationPage.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar());
      if (this._applicationBar == null)
        return;
      this.InitApplicationBarItems();
      this._applicationBar.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.ApplicationBarStateChanged);
      this._applicationBar.IsVisible = this.IsVisible;
      this._applicationBar.Mode = this.Mode;
    }

    public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;

    private void ApplicationBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e) => this.OnStateChanged(e);

    protected virtual void OnStateChanged(ApplicationBarStateChangedEventArgs e)
    {
      EventHandler<ApplicationBarStateChangedEventArgs> stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, e);
    }

    private static void IsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarWrapper applicationBarWrapper))
        return;
      applicationBarWrapper.IsVisibleChangedHandler(e);
    }

    private void IsVisibleChangedHandler(
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      if (this._applicationBar == null)
        return;
      this._applicationBar.IsVisible = (bool) dependencyPropertyChangedEventArgs.NewValue;
    }

    public bool IsVisible
    {
      get => (bool) ((DependencyObject) this).GetValue(ApplicationBarWrapper.IsVisibleProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarWrapper.IsVisibleProperty, (object) value);
    }

    private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarWrapper applicationBarWrapper))
        return;
      applicationBarWrapper.ModeChangedHandler(e);
    }

    private void ModeChangedHandler(DependencyPropertyChangedEventArgs e)
    {
      if (this._applicationBar == null)
        return;
      this._applicationBar.Mode = (ApplicationBarMode) e.NewValue;
    }

    public ApplicationBarMode Mode
    {
      get => (ApplicationBarMode) ((DependencyObject) this).GetValue(ApplicationBarWrapper.ModeProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarWrapper.ModeProperty, (object) value);
    }
  }
}
