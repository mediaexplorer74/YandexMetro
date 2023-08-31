﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PerformanceProgressBar
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Hidden")]
  [TemplatePart(Name = "EmbeddedProgressBar", Type = typeof (ProgressBar))]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Normal")]
  public class PerformanceProgressBar : Control
  {
    private const string EmbeddedProgressBarName = "EmbeddedProgressBar";
    private const string VisualStateGroupName = "VisibilityStates";
    private const string NormalState = "Normal";
    private const string HiddenState = "Hidden";
    private ProgressBar _progressBar;
    private static readonly PropertyPath ActualIsIndeterminatePath = new PropertyPath(nameof (ActualIsIndeterminate), new object[0]);
    private VisualStateGroup _visualStateGroup;
    private bool _cachedIsIndeterminate;
    private BindingExpression _cachedIsIndeterminateBindingExpression;
    public static readonly DependencyProperty ActualIsIndeterminateProperty = DependencyProperty.Register(nameof (ActualIsIndeterminate), typeof (bool), typeof (PerformanceProgressBar), new PropertyMetadata((object) false));
    public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (PerformanceProgressBar), new PropertyMetadata((object) false, new PropertyChangedCallback(PerformanceProgressBar.OnIsIndeterminatePropertyChanged)));

    public bool ActualIsIndeterminate
    {
      get => (bool) ((DependencyObject) this).GetValue(PerformanceProgressBar.ActualIsIndeterminateProperty);
      set => ((DependencyObject) this).SetValue(PerformanceProgressBar.ActualIsIndeterminateProperty, (object) value);
    }

    public bool IsIndeterminate
    {
      get => (bool) ((DependencyObject) this).GetValue(PerformanceProgressBar.IsIndeterminateProperty);
      set => ((DependencyObject) this).SetValue(PerformanceProgressBar.IsIndeterminateProperty, (object) value);
    }

    private static void OnIsIndeterminatePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is PerformanceProgressBar performanceProgressBar))
        return;
      performanceProgressBar.OnIsIndeterminateChanged((bool) e.NewValue);
    }

    public PerformanceProgressBar()
    {
      this.DefaultStyleKey = (object) typeof (PerformanceProgressBar);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
    }

    public virtual void OnApplyTemplate()
    {
      if (this._visualStateGroup != null)
        this._visualStateGroup.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
      ((FrameworkElement) this).OnApplyTemplate();
      this._visualStateGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) this, "VisibilityStates");
      if (this._visualStateGroup != null)
        this._visualStateGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnVisualStateChanged);
      this._progressBar = this.GetTemplateChild("EmbeddedProgressBar") as ProgressBar;
      this._cachedIsIndeterminateBindingExpression = ((FrameworkElement) this).GetBindingExpression(PerformanceProgressBar.IsIndeterminateProperty);
      this.UpdateVisualStates(false);
    }

    private void OnVisualStateChanged(object sender, VisualStateChangedEventArgs e)
    {
      if (!this.ActualIsIndeterminate || e == null || e.NewState == null || !(e.NewState.Name == "Hidden") || this.IsIndeterminate)
        return;
      this.ActualIsIndeterminate = false;
    }

    private void OnIsIndeterminateChanged(bool newValue)
    {
      if (newValue)
      {
        this.ActualIsIndeterminate = true;
        this._cachedIsIndeterminate = true;
      }
      else if (this.ActualIsIndeterminate && this._visualStateGroup == null)
      {
        this.ActualIsIndeterminate = false;
        this._cachedIsIndeterminate = false;
      }
      this.UpdateVisualStates(true);
    }

    private void UpdateVisualStates(bool useTransitions) => VisualStateManager.GoToState((Control) this, this.IsIndeterminate ? "Normal" : "Hidden", useTransitions);

    private void OnUnloaded(object sender, RoutedEventArgs ea)
    {
      if (this._progressBar == null)
        return;
      this._cachedIsIndeterminate = this.IsIndeterminate;
      this._cachedIsIndeterminateBindingExpression = ((FrameworkElement) this).GetBindingExpression(PerformanceProgressBar.IsIndeterminateProperty);
      this._progressBar.IsIndeterminate = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs ea)
    {
      if (this._progressBar == null)
        return;
      if (this._cachedIsIndeterminateBindingExpression != null)
        ((FrameworkElement) this).SetBinding(PerformanceProgressBar.IsIndeterminateProperty, this._cachedIsIndeterminateBindingExpression.ParentBinding);
      else
        this.IsIndeterminate = this._cachedIsIndeterminate;
      ((FrameworkElement) this._progressBar).SetBinding(ProgressBar.IsIndeterminateProperty, new Binding()
      {
        Source = (object) this,
        Path = PerformanceProgressBar.ActualIsIndeterminatePath
      });
    }

    private static T FindFirstChildOfType<T>(DependencyObject root) where T : class
    {
      Queue<DependencyObject> dependencyObjectQueue = new Queue<DependencyObject>();
      dependencyObjectQueue.Enqueue(root);
      while (0 < dependencyObjectQueue.Count)
      {
        DependencyObject dependencyObject = dependencyObjectQueue.Dequeue();
        for (int index = VisualTreeHelper.GetChildrenCount(dependencyObject) - 1; 0 <= index; --index)
        {
          DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
          if (child is T firstChildOfType)
            return firstChildOfType;
          dependencyObjectQueue.Enqueue(child);
        }
      }
      return default (T);
    }
  }
}
