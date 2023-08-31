// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ExpanderView
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "Collapsed", GroupName = "ExpansionStates")]
  [TemplatePart(Name = "ExpanderPanel", Type = typeof (Grid))]
  [TemplatePart(Name = "Presenter", Type = typeof (ItemsPresenter))]
  [TemplateVisualState(Name = "Expanded", GroupName = "ExpansionStates")]
  [TemplatePart(Name = "ExpandedToCollapsedKeyFrame", Type = typeof (EasingDoubleKeyFrame))]
  [TemplateVisualState(Name = "NonExpandable", GroupName = "ExpandabilityStates")]
  [TemplateVisualState(Name = "Expandable", GroupName = "ExpandabilityStates")]
  [TemplatePart(Name = "ExpandedStateAnimation", Type = typeof (DoubleAnimation))]
  [TemplatePart(Name = "CollapsedToExpandedKeyFrame", Type = typeof (EasingDoubleKeyFrame))]
  public class ExpanderView : HeaderedItemsControl
  {
    public const string ExpansionStates = "ExpansionStates";
    public const string ExpandabilityStates = "ExpandabilityStates";
    public const string CollapsedState = "Collapsed";
    public const string ExpandedState = "Expanded";
    public const string ExpandableState = "Expandable";
    public const string NonExpandableState = "NonExpandable";
    private const string Presenter = "Presenter";
    private const string ExpanderPanel = "ExpanderPanel";
    private const string ExpandedStateAnimation = "ExpandedStateAnimation";
    private const string CollapsedToExpandedKeyFrame = "CollapsedToExpandedKeyFrame";
    private const string ExpandedToCollapsedKeyFrame = "ExpandedToCollapsedKeyFrame";
    private const int KeyTimeStep = 35;
    private const int InitialKeyTime = 225;
    private const int FinalKeyTime = 250;
    private ItemsPresenter _presenter;
    private Canvas _itemsCanvas;
    private Grid _expanderPanel;
    private DoubleAnimation _expandedStateAnimation;
    private EasingDoubleKeyFrame _collapsedToExpandedFrame;
    private EasingDoubleKeyFrame _expandedToCollapsedFrame;
    public static readonly DependencyProperty ExpanderProperty = DependencyProperty.Register(nameof (Expander), typeof (object), typeof (ExpanderView), new PropertyMetadata((object) null, new PropertyChangedCallback(ExpanderView.OnExpanderPropertyChanged)));
    public static readonly DependencyProperty ExpanderTemplateProperty = DependencyProperty.Register(nameof (ExpanderTemplate), typeof (DataTemplate), typeof (ExpanderView), new PropertyMetadata((object) null, new PropertyChangedCallback(ExpanderView.OnExpanderTemplatePropertyChanged)));
    public static readonly DependencyProperty NonExpandableHeaderProperty = DependencyProperty.Register(nameof (NonExpandableHeader), typeof (object), typeof (ExpanderView), new PropertyMetadata((object) null, new PropertyChangedCallback(ExpanderView.OnNonExpandableHeaderPropertyChanged)));
    public static readonly DependencyProperty NonExpandableHeaderTemplateProperty = DependencyProperty.Register(nameof (NonExpandableHeaderTemplate), typeof (DataTemplate), typeof (ExpanderView), new PropertyMetadata((object) null, new PropertyChangedCallback(ExpanderView.OnNonExpandableHeaderTemplatePropertyChanged)));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (ExpanderView), new PropertyMetadata((object) false, new PropertyChangedCallback(ExpanderView.OnIsExpandedPropertyChanged)));
    public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(nameof (HasItems), typeof (bool), typeof (ExpanderView), new PropertyMetadata((object) false, (PropertyChangedCallback) null));
    public static readonly DependencyProperty IsNonExpandableProperty = DependencyProperty.Register(nameof (IsNonExpandable), typeof (bool), typeof (ExpanderView), new PropertyMetadata((object) false, new PropertyChangedCallback(ExpanderView.OnIsNonExpandablePropertyChanged)));

    public event RoutedEventHandler Expanded;

    public event RoutedEventHandler Collapsed;

    public object Expander
    {
      get => ((DependencyObject) this).GetValue(ExpanderView.ExpanderProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.ExpanderProperty, value);
    }

    private static void OnExpanderPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderView) obj).OnExpanderChanged(e.OldValue, e.NewValue);
    }

    public DataTemplate ExpanderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ExpanderView.ExpanderTemplateProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.ExpanderTemplateProperty, (object) value);
    }

    private static void OnExpanderTemplatePropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderView) obj).OnExpanderTemplateChanged((DataTemplate) e.OldValue, (DataTemplate) e.NewValue);
    }

    public object NonExpandableHeader
    {
      get => ((DependencyObject) this).GetValue(ExpanderView.NonExpandableHeaderProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.NonExpandableHeaderProperty, value);
    }

    private static void OnNonExpandableHeaderPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderView) obj).OnNonExpandableHeaderChanged(e.OldValue, e.NewValue);
    }

    public DataTemplate NonExpandableHeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ExpanderView.NonExpandableHeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.NonExpandableHeaderTemplateProperty, (object) value);
    }

    private static void OnNonExpandableHeaderTemplatePropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderView) obj).OnNonExpandableHeaderTemplateChanged((DataTemplate) e.OldValue, (DataTemplate) e.NewValue);
    }

    public bool IsExpanded
    {
      get => (bool) ((DependencyObject) this).GetValue(ExpanderView.IsExpandedProperty);
      set
      {
        if (this.IsNonExpandable)
          throw new InvalidOperationException(Resources.InvalidExpanderViewOperation);
        ((DependencyObject) this).SetValue(ExpanderView.IsExpandedProperty, (object) value);
      }
    }

    private static void OnIsExpandedPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ExpanderView expanderView = (ExpanderView) obj;
      RoutedEventArgs e1 = new RoutedEventArgs();
      if ((bool) e.NewValue)
        expanderView.OnExpanded(e1);
      else
        expanderView.OnCollapsed(e1);
      expanderView.UpdateVisualState(true);
    }

    public bool HasItems
    {
      get => (bool) ((DependencyObject) this).GetValue(ExpanderView.HasItemsProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.HasItemsProperty, (object) value);
    }

    public bool IsNonExpandable
    {
      get => (bool) ((DependencyObject) this).GetValue(ExpanderView.IsNonExpandableProperty);
      set => ((DependencyObject) this).SetValue(ExpanderView.IsNonExpandableProperty, (object) value);
    }

    private static void OnIsNonExpandablePropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ExpanderView expanderView = (ExpanderView) obj;
      if ((bool) e.NewValue && expanderView.IsExpanded)
        expanderView.IsExpanded = false;
      expanderView.UpdateVisualState(true);
    }

    public override void OnApplyTemplate()
    {
      if (this._expanderPanel != null)
        ((UIElement) this._expanderPanel).Tap -= new EventHandler<GestureEventArgs>(this.OnExpanderPanelTap);
      base.OnApplyTemplate();
      this._expanderPanel = ((Control) this).GetTemplateChild("ExpanderPanel") as Grid;
      this._expandedStateAnimation = ((PresentationFrameworkCollection<Timeline>) (((Control) this).GetTemplateChild("Expanded") as VisualState).Storyboard.Children)[0] as DoubleAnimation;
      this._expandedToCollapsedFrame = ((Control) this).GetTemplateChild("ExpandedToCollapsedKeyFrame") as EasingDoubleKeyFrame;
      this._collapsedToExpandedFrame = ((Control) this).GetTemplateChild("CollapsedToExpandedKeyFrame") as EasingDoubleKeyFrame;
      this._itemsCanvas = ((Control) this).GetTemplateChild("ItemsCanvas") as Canvas;
      this._presenter = ((Control) this).GetTemplateChild("Presenter") as ItemsPresenter;
      ((FrameworkElement) this._presenter).SizeChanged += new SizeChangedEventHandler(this.OnPresenterSizeChanged);
      if (this._expanderPanel != null)
        ((UIElement) this._expanderPanel).Tap += new EventHandler<GestureEventArgs>(this.OnExpanderPanelTap);
      this.UpdateVisualState(false);
    }

    public ExpanderView()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (ExpanderView);
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this._presenter == null)
        return;
      UIElement parent = VisualTreeHelper.GetParent((DependencyObject) this._presenter) as UIElement;
      while (!(parent is ExpanderView))
        parent = VisualTreeHelper.GetParent((DependencyObject) parent) as UIElement;
      Point point = parent.TransformToVisual((UIElement) this._presenter).Transform(new Point(0.0, 0.0));
      ((FrameworkElement) this._presenter).Width = parent.RenderSize.Width + point.X;
    }

    private void OnPresenterSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this._itemsCanvas == null || this._presenter == null || !this.IsExpanded)
        return;
      ((FrameworkElement) this._itemsCanvas).Height = ((UIElement) this._presenter).DesiredSize.Height;
    }

    internal virtual void UpdateVisualState(bool useTransitions)
    {
      if (this._presenter != null)
      {
        if (this._expandedStateAnimation != null)
          this._expandedStateAnimation.To = new double?(((UIElement) this._presenter).DesiredSize.Height);
        if (this._collapsedToExpandedFrame != null)
          ((DoubleKeyFrame) this._collapsedToExpandedFrame).Value = ((UIElement) this._presenter).DesiredSize.Height;
        if (this._expandedToCollapsedFrame != null)
          ((DoubleKeyFrame) this._expandedToCollapsedFrame).Value = ((UIElement) this._presenter).DesiredSize.Height;
      }
      string str;
      if (this.IsExpanded)
      {
        str = "Expanded";
        if (useTransitions)
          this.AnimateContainerDropDown();
      }
      else
        str = "Collapsed";
      VisualStateManager.GoToState((Control) this, str, useTransitions);
      VisualStateManager.GoToState((Control) this, !this.IsNonExpandable ? "Expandable" : "NonExpandable", useTransitions);
    }

    private void RaiseEvent(RoutedEventHandler handler, RoutedEventArgs args)
    {
      if (handler == null)
        return;
      handler((object) this, args);
    }

    internal void AnimateContainerDropDown()
    {
      for (int index = 0; index < ((PresentationFrameworkCollection<object>) this.Items).Count && this.ItemContainerGenerator.ContainerFromIndex(index) is FrameworkElement frameworkElement; ++index)
      {
        Storyboard storyboard = new Storyboard();
        QuadraticEase quadraticEase = new QuadraticEase();
        ((EasingFunctionBase) quadraticEase).EasingMode = (EasingMode) 0;
        IEasingFunction ieasingFunction = (IEasingFunction) quadraticEase;
        int num1 = 225 + 35 * index;
        int num2 = 250 + 35 * index;
        TranslateTransform translateTransform = new TranslateTransform();
        ((UIElement) frameworkElement).RenderTransform = (Transform) translateTransform;
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
        EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame1.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame1).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds(0.0));
        ((DoubleKeyFrame) easingDoubleKeyFrame1).Value = -150.0;
        EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame2.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame2).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds((double) num1));
        ((DoubleKeyFrame) easingDoubleKeyFrame2).Value = 0.0;
        EasingDoubleKeyFrame easingDoubleKeyFrame3 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame3.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame3).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds((double) num2));
        ((DoubleKeyFrame) easingDoubleKeyFrame3).Value = 0.0;
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames1.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame1);
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames1.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame2);
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames1.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame3);
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames1, (DependencyObject) translateTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames1, new PropertyPath((object) TranslateTransform.YProperty));
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) animationUsingKeyFrames1);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
        EasingDoubleKeyFrame easingDoubleKeyFrame4 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame4.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame4).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds(0.0));
        ((DoubleKeyFrame) easingDoubleKeyFrame4).Value = 0.0;
        EasingDoubleKeyFrame easingDoubleKeyFrame5 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame5.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame5).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds((double) (num1 - 150)));
        ((DoubleKeyFrame) easingDoubleKeyFrame5).Value = 0.0;
        EasingDoubleKeyFrame easingDoubleKeyFrame6 = new EasingDoubleKeyFrame();
        easingDoubleKeyFrame6.EasingFunction = ieasingFunction;
        ((DoubleKeyFrame) easingDoubleKeyFrame6).KeyTime = KeyTime.op_Implicit(TimeSpan.FromMilliseconds((double) num2));
        ((DoubleKeyFrame) easingDoubleKeyFrame6).Value = 1.0;
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames2.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame4);
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames2.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame5);
        ((PresentationFrameworkCollection<DoubleKeyFrame>) animationUsingKeyFrames2.KeyFrames).Add((DoubleKeyFrame) easingDoubleKeyFrame6);
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames2, (DependencyObject) frameworkElement);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames2, new PropertyPath((object) UIElement.OpacityProperty));
        ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) animationUsingKeyFrames2);
        storyboard.Begin();
      }
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      this.HasItems = ((PresentationFrameworkCollection<object>) this.Items).Count > 0;
    }

    private void OnExpanderPanelTap(object sender, GestureEventArgs e)
    {
      if (this.IsNonExpandable)
        return;
      this.IsExpanded = !this.IsExpanded;
    }

    protected virtual void OnExpanderChanged(object oldExpander, object newExpander)
    {
    }

    protected virtual void OnExpanderTemplateChanged(
      DataTemplate oldTemplate,
      DataTemplate newTemplate)
    {
    }

    protected virtual void OnNonExpandableHeaderChanged(object oldHeader, object newHeader)
    {
    }

    protected virtual void OnNonExpandableHeaderTemplateChanged(
      DataTemplate oldTemplate,
      DataTemplate newTemplate)
    {
    }

    protected virtual void OnExpanded(RoutedEventArgs e) => this.RaiseEvent(this.Expanded, e);

    protected virtual void OnCollapsed(RoutedEventArgs e) => this.RaiseEvent(this.Collapsed, e);
  }
}
