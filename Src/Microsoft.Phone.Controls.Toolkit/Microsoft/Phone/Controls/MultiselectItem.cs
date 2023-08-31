// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MultiselectItem
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(Name = "Exposed", GroupName = "SelectionEnabledStates")]
  [TemplatePart(Name = "InnerHintPanel", Type = typeof (Rectangle))]
  [TemplatePart(Name = "OutterCover", Type = typeof (Grid))]
  [TemplatePart(Name = "InfoPresenter", Type = typeof (ContentControl))]
  [TemplateVisualState(Name = "Closed", GroupName = "SelectionEnabledStates")]
  [TemplateVisualState(Name = "Opened", GroupName = "SelectionEnabledStates")]
  [TemplatePart(Name = "OutterHintPanel", Type = typeof (Rectangle))]
  public class MultiselectItem : ContentControl
  {
    private const string SelectionEnabledStates = "SelectionEnabledStates";
    private const string Closed = "Closed";
    private const string Exposed = "Exposed";
    private const string Opened = "Opened";
    private const string SelectBox = "SelectBox";
    private const string OutterHintPanel = "OutterHintPanel";
    private const string InnerHintPanel = "InnerHintPanel";
    private const string OutterCover = "OutterCover";
    private const string InfoPresenter = "InfoPresenter";
    private const double _deltaLimitX = 0.0;
    private const double _deltaLimitY = 0.4;
    private Rectangle _outterHintPanel;
    private Rectangle _innerHintPanel;
    private Grid _outterCover;
    private ContentControl _infoPresenter;
    private MultiselectList _parent;
    private double _manipulationDeltaX;
    private double _manipulationDeltaY;
    internal bool _isBeingVirtualized;
    internal bool _canTriggerSelectionChanged = true;
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (MultiselectItem), new PropertyMetadata((object) false, new PropertyChangedCallback(MultiselectItem.OnIsSelectedPropertyChanged)));
    internal static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (SelectionEnabledState), typeof (MultiselectItem), new PropertyMetadata((object) SelectionEnabledState.Closed, (PropertyChangedCallback) null));
    public static readonly DependencyProperty HintPanelHeightProperty = DependencyProperty.Register(nameof (HintPanelHeight), typeof (double), typeof (MultiselectItem), new PropertyMetadata((object) double.NaN, (PropertyChangedCallback) null));
    public static readonly DependencyProperty ContentInfoProperty = DependencyProperty.Register(nameof (ContentInfo), typeof (object), typeof (MultiselectItem), new PropertyMetadata((object) null, new PropertyChangedCallback(MultiselectItem.OnContentInfoPropertyChanged)));
    public static readonly DependencyProperty ContentInfoTemplateProperty = DependencyProperty.Register(nameof (ContentInfoTemplate), typeof (DataTemplate), typeof (MultiselectItem), new PropertyMetadata((object) null, new PropertyChangedCallback(MultiselectItem.OnContentInfoTemplatePropertyChanged)));

    public event RoutedEventHandler Selected;

    public event RoutedEventHandler Unselected;

    public bool IsSelected
    {
      get => (bool) ((DependencyObject) this).GetValue(MultiselectItem.IsSelectedProperty);
      set => ((DependencyObject) this).SetValue(MultiselectItem.IsSelectedProperty, (object) value);
    }

    private static void OnIsSelectedPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      MultiselectItem multiselectItem = (MultiselectItem) obj;
      RoutedEventArgs e1 = new RoutedEventArgs();
      bool newValue = (bool) e.NewValue;
      if (newValue)
        multiselectItem.OnSelected(e1);
      else
        multiselectItem.OnUnselected(e1);
      if (multiselectItem._parent == null || multiselectItem._isBeingVirtualized)
        return;
      if (newValue)
      {
        multiselectItem._parent.SelectedItems.Add(multiselectItem.Content);
        if (!multiselectItem._canTriggerSelectionChanged)
          return;
        multiselectItem._parent.OnSelectionChanged((IList) new object[0], (IList) new object[1]
        {
          multiselectItem.Content
        });
      }
      else
      {
        multiselectItem._parent.SelectedItems.Remove(multiselectItem.Content);
        if (!multiselectItem._canTriggerSelectionChanged)
          return;
        multiselectItem._parent.OnSelectionChanged((IList) new object[1]
        {
          multiselectItem.Content
        }, (IList) new object[0]);
      }
    }

    internal SelectionEnabledState State
    {
      get => (SelectionEnabledState) ((DependencyObject) this).GetValue(MultiselectItem.StateProperty);
      set => ((DependencyObject) this).SetValue(MultiselectItem.StateProperty, (object) value);
    }

    public double HintPanelHeight
    {
      get => (double) ((DependencyObject) this).GetValue(MultiselectItem.HintPanelHeightProperty);
      set => ((DependencyObject) this).SetValue(MultiselectItem.HintPanelHeightProperty, (object) value);
    }

    private static void OnHintPanelHeightPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      MultiselectItem multiselectItem = (MultiselectItem) obj;
      if (multiselectItem._outterHintPanel != null)
      {
        if (double.IsNaN((double) e.NewValue))
          ((FrameworkElement) multiselectItem._outterHintPanel).VerticalAlignment = (VerticalAlignment) 3;
        else
          ((FrameworkElement) multiselectItem._outterHintPanel).VerticalAlignment = (VerticalAlignment) 0;
      }
      if (multiselectItem._innerHintPanel == null)
        return;
      if (double.IsNaN(multiselectItem.HintPanelHeight))
        ((FrameworkElement) multiselectItem._innerHintPanel).VerticalAlignment = (VerticalAlignment) 3;
      else
        ((FrameworkElement) multiselectItem._innerHintPanel).VerticalAlignment = (VerticalAlignment) 0;
    }

    public object ContentInfo
    {
      get => ((DependencyObject) this).GetValue(MultiselectItem.ContentInfoProperty);
      set => ((DependencyObject) this).SetValue(MultiselectItem.ContentInfoProperty, value);
    }

    private static void OnContentInfoPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((MultiselectItem) obj).OnContentInfoChanged(e.OldValue, e.NewValue);
    }

    public DataTemplate ContentInfoTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(MultiselectItem.ContentInfoTemplateProperty);
      set => ((DependencyObject) this).SetValue(MultiselectItem.ContentInfoTemplateProperty, (object) value);
    }

    private static void OnContentInfoTemplatePropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((MultiselectItem) obj).OnContentInfoTemplateChanged(e.OldValue as DataTemplate, e.NewValue as DataTemplate);
    }

    public virtual void OnApplyTemplate()
    {
      this._parent = ItemsControlExtensions.GetParentItemsControl<MultiselectList>((DependencyObject) this);
      if (this._innerHintPanel != null)
      {
        ((UIElement) this._innerHintPanel).ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(this.HintPanel_ManipulationStarted);
        ((UIElement) this._innerHintPanel).ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(this.HintPanel_ManipulationDelta);
        ((UIElement) this._innerHintPanel).ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.HintPanel_ManipulationCompleted);
      }
      if (this._outterHintPanel != null)
      {
        ((UIElement) this._outterHintPanel).ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(this.HintPanel_ManipulationStarted);
        ((UIElement) this._outterHintPanel).ManipulationDelta -= new EventHandler<ManipulationDeltaEventArgs>(this.HintPanel_ManipulationDelta);
        ((UIElement) this._outterHintPanel).ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.HintPanel_ManipulationCompleted);
      }
      if (this._outterCover != null)
        ((UIElement) this._outterCover).Tap -= new EventHandler<GestureEventArgs>(this.Cover_Tap);
      this._innerHintPanel = ((Control) this).GetTemplateChild("InnerHintPanel") as Rectangle;
      this._outterHintPanel = ((Control) this).GetTemplateChild("OutterHintPanel") as Rectangle;
      this._outterCover = ((Control) this).GetTemplateChild("OutterCover") as Grid;
      this._infoPresenter = ((Control) this).GetTemplateChild("InfoPresenter") as ContentControl;
      ((FrameworkElement) this).OnApplyTemplate();
      if (this._innerHintPanel != null)
      {
        ((UIElement) this._innerHintPanel).ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.HintPanel_ManipulationStarted);
        ((UIElement) this._innerHintPanel).ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.HintPanel_ManipulationDelta);
        ((UIElement) this._innerHintPanel).ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.HintPanel_ManipulationCompleted);
      }
      if (this._outterHintPanel != null)
      {
        ((UIElement) this._outterHintPanel).ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.HintPanel_ManipulationStarted);
        ((UIElement) this._outterHintPanel).ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.HintPanel_ManipulationDelta);
        ((UIElement) this._outterHintPanel).ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.HintPanel_ManipulationCompleted);
      }
      if (this._outterCover != null)
        ((UIElement) this._outterCover).Tap += new EventHandler<GestureEventArgs>(this.Cover_Tap);
      if (this.ContentInfo == null && this._parent != null && this._parent.ItemInfoTemplate != null)
      {
        this._infoPresenter.ContentTemplate = this._parent.ItemInfoTemplate;
        Binding binding = new Binding();
        ((FrameworkElement) this).SetBinding(MultiselectItem.ContentInfoProperty, binding);
      }
      if (this._outterHintPanel != null)
      {
        if (double.IsNaN(this.HintPanelHeight))
          ((FrameworkElement) this._outterHintPanel).VerticalAlignment = (VerticalAlignment) 3;
        else
          ((FrameworkElement) this._outterHintPanel).VerticalAlignment = (VerticalAlignment) 0;
      }
      if (this._innerHintPanel != null)
      {
        if (double.IsNaN(this.HintPanelHeight))
          ((FrameworkElement) this._innerHintPanel).VerticalAlignment = (VerticalAlignment) 3;
        else
          ((FrameworkElement) this._innerHintPanel).VerticalAlignment = (VerticalAlignment) 0;
      }
      this.UpdateVisualState(false);
    }

    public MultiselectItem() => ((Control) this).DefaultStyleKey = (object) typeof (MultiselectItem);

    internal void UpdateVisualState(bool useTransitions)
    {
      string str;
      switch (this.State)
      {
        case SelectionEnabledState.Closed:
          str = "Closed";
          break;
        case SelectionEnabledState.Exposed:
          str = "Exposed";
          break;
        case SelectionEnabledState.Opened:
          str = "Opened";
          break;
        default:
          str = "Closed";
          break;
      }
      VisualStateManager.GoToState((Control) this, str, useTransitions);
    }

    private void RaiseEvent(RoutedEventHandler handler, RoutedEventArgs args)
    {
      if (handler == null)
        return;
      handler((object) this, args);
    }

    protected virtual void OnSelected(RoutedEventArgs e)
    {
      if (this._parent == null)
      {
        this.State = SelectionEnabledState.Opened;
        this.UpdateVisualState(true);
      }
      this.RaiseEvent(this.Selected, e);
    }

    protected virtual void OnUnselected(RoutedEventArgs e)
    {
      if (this._parent == null)
      {
        this.State = SelectionEnabledState.Closed;
        this.UpdateVisualState(true);
      }
      this.RaiseEvent(this.Unselected, e);
    }

    protected virtual void OnContentInfoChanged(object oldContentInfo, object newContentInfo)
    {
    }

    protected virtual void OnContentInfoTemplateChanged(
      DataTemplate oldContentInfoTemplate,
      DataTemplate newContentInfoTemplate)
    {
    }

    private void HintPanel_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this.State = SelectionEnabledState.Exposed;
      this.UpdateVisualState(true);
    }

    private void HintPanel_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      this._manipulationDeltaX = e.DeltaManipulation.Translation.X;
      this._manipulationDeltaY = e.DeltaManipulation.Translation.Y;
      if (this._manipulationDeltaX < 0.0)
        this._manipulationDeltaX *= -1.0;
      if (this._manipulationDeltaY < 0.0)
        this._manipulationDeltaY *= -1.0;
      if (this._manipulationDeltaX <= 0.0 && this._manipulationDeltaY < 0.4)
        return;
      this.State = SelectionEnabledState.Closed;
      this.UpdateVisualState(true);
    }

    private void HintPanel_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (this._manipulationDeltaX == 0.0 && this._manipulationDeltaY < 0.4)
        this.IsSelected = true;
      this._manipulationDeltaX = 0.0;
      this._manipulationDeltaY = 0.0;
    }

    private void Cover_Tap(object sender, GestureEventArgs e) => this.IsSelected = !this.IsSelected;
  }
}
