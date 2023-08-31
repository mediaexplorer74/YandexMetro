// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.InteractionHelper
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  internal sealed class InteractionHelper
  {
    private const double SequentialClickThresholdInMilliseconds = 500.0;
    private const double SequentialClickThresholdInPixelsSquared = 9.0;
    private IUpdateVisualState _updateVisualState;

    public Control Control { get; private set; }

    public bool IsFocused { get; private set; }

    public bool IsMouseOver { get; private set; }

    public bool IsReadOnly { get; private set; }

    public bool IsPressed { get; private set; }

    public InteractionHelper(Control control)
    {
      this.Control = control;
      this._updateVisualState = control as IUpdateVisualState;
      ((FrameworkElement) control).Loaded += new RoutedEventHandler(this.OnLoaded);
      control.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged);
    }

    private void UpdateVisualState(bool useTransitions)
    {
      if (this._updateVisualState == null)
        return;
      this._updateVisualState.UpdateVisualState(useTransitions);
    }

    public void UpdateVisualStateBase(bool useTransitions)
    {
      if (!this.Control.IsEnabled)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Disabled", "Normal");
      else if (this.IsReadOnly)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "ReadOnly", "Normal");
      else if (this.IsPressed)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Pressed", "MouseOver", "Normal");
      else if (this.IsMouseOver)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "MouseOver", "Normal");
      else
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Normal");
      if (this.IsFocused)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Focused", "Unfocused");
      else
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Unfocused");
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => this.UpdateVisualState(false);

    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
      {
        this.IsPressed = false;
        this.IsMouseOver = false;
        this.IsFocused = false;
      }
      this.UpdateVisualState(true);
    }

    public void OnIsReadOnlyChanged(bool value)
    {
      this.IsReadOnly = value;
      if (!value)
      {
        this.IsPressed = false;
        this.IsMouseOver = false;
        this.IsFocused = false;
      }
      this.UpdateVisualState(true);
    }

    public void OnApplyTemplateBase() => this.UpdateVisualState(false);
  }
}
