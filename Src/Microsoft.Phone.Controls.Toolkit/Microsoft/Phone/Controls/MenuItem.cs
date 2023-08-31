// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MenuItem
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (MenuItem))]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  public class MenuItem : HeaderedItemsControl
  {
    private bool _isFocused;
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (MenuItem), new PropertyMetadata((object) null, new PropertyChangedCallback(MenuItem.OnCommandChanged)));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (MenuItem), new PropertyMetadata((object) null, new PropertyChangedCallback(MenuItem.OnCommandParameterChanged)));

    public event RoutedEventHandler Click;

    internal MenuBase ParentMenuBase { get; set; }

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(MenuItem.CommandProperty);
      set => ((DependencyObject) this).SetValue(MenuItem.CommandProperty, (object) value);
    }

    private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((MenuItem) o).OnCommandChanged((ICommand) e.OldValue, (ICommand) e.NewValue);

    private void OnCommandChanged(ICommand oldValue, ICommand newValue)
    {
      if (oldValue != null)
        oldValue.CanExecuteChanged -= new EventHandler(this.OnCanExecuteChanged);
      if (newValue != null)
        newValue.CanExecuteChanged += new EventHandler(this.OnCanExecuteChanged);
      this.UpdateIsEnabled(true);
    }

    public object CommandParameter
    {
      get => ((DependencyObject) this).GetValue(MenuItem.CommandParameterProperty);
      set => ((DependencyObject) this).SetValue(MenuItem.CommandParameterProperty, value);
    }

    private static void OnCommandParameterChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((MenuItem) o).OnCommandParameterChanged();
    }

    private void OnCommandParameterChanged() => this.UpdateIsEnabled(true);

    public MenuItem()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (MenuItem);
      ((Control) this).IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
      ((DependencyObject) this).SetValue(TiltEffect.IsTiltEnabledProperty, (object) true);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
      this.UpdateIsEnabled(false);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.ChangeVisualState(false);
    }

    protected virtual void OnGotFocus(RoutedEventArgs e)
    {
      ((Control) this).OnGotFocus(e);
      this._isFocused = true;
      this.ChangeVisualState(true);
    }

    protected virtual void OnLostFocus(RoutedEventArgs e)
    {
      ((Control) this).OnLostFocus(e);
      this._isFocused = false;
      this.ChangeVisualState(true);
    }

    protected virtual void OnMouseEnter(MouseEventArgs e)
    {
      ((Control) this).OnMouseEnter(e);
      ((Control) this).Focus();
      this.ChangeVisualState(true);
    }

    protected virtual void OnMouseLeave(MouseEventArgs e)
    {
      ((Control) this).OnMouseLeave(e);
      if (this.ParentMenuBase != null)
        ((Control) this.ParentMenuBase).Focus();
      this.ChangeVisualState(true);
    }

    protected virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      if (!e.Handled)
      {
        this.OnClick();
        e.Handled = true;
      }
      ((Control) this).OnMouseLeftButtonUp(e);
    }

    protected virtual void OnKeyDown(KeyEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      if (!e.Handled && 3 == e.Key)
      {
        this.OnClick();
        e.Handled = true;
      }
      ((Control) this).OnKeyDown(e);
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e) => throw new NotImplementedException();

    protected virtual void OnClick()
    {
      if (this.ParentMenuBase is ContextMenu parentMenuBase)
        parentMenuBase.ChildMenuItemClicked();
      RoutedEventHandler click = this.Click;
      if (click != null)
        click((object) this, new RoutedEventArgs());
      if (this.Command == null || !this.Command.CanExecute(this.CommandParameter))
        return;
      this.Command.Execute(this.CommandParameter);
    }

    private void OnCanExecuteChanged(object sender, EventArgs e) => this.UpdateIsEnabled(true);

    private void UpdateIsEnabled(bool changeVisualState)
    {
      ((Control) this).IsEnabled = this.Command == null || this.Command.CanExecute(this.CommandParameter);
      if (!changeVisualState)
        return;
      this.ChangeVisualState(true);
    }

    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) => this.ChangeVisualState(true);

    private void OnLoaded(object sender, RoutedEventArgs e) => this.ChangeVisualState(false);

    protected virtual void ChangeVisualState(bool useTransitions)
    {
      if (!((Control) this).IsEnabled)
        VisualStateManager.GoToState((Control) this, "Disabled", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "Normal", useTransitions);
      if (this._isFocused && ((Control) this).IsEnabled)
        VisualStateManager.GoToState((Control) this, "Focused", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "Unfocused", useTransitions);
    }
  }
}
