// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Pivot
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Gestures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "PivotItemPresenter", Type = typeof (ItemsPresenter))]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (PivotItem))]
  [TemplatePart(Name = "HeadersListElement", Type = typeof (PivotHeadersControl))]
  public class Pivot : TemplatedItemsControl<PivotItem>
  {
    private const string ElementHeadersRowDefinitionName = "HeadersRowDefinition";
    private const string HeadersListElement = "HeadersListElement";
    private const string PivotItemPresenterElement = "PivotItemPresenter";
    internal const string ItemContainerStyleName = "ItemContainerStyle";
    private const double pixelsPerSecondTemporary = 600.0;
    internal const double PivotAnimationSeconds = 0.25;
    private static readonly TimeSpan PivotAnimationTimeSpan = TimeSpan.FromSeconds(0.25);
    internal static readonly Duration PivotAnimationDuration = new Duration(Pivot.PivotAnimationTimeSpan);
    internal static readonly Duration ZeroDuration = new Duration(TimeSpan.Zero);
    internal readonly IEasingFunction QuarticEase = (IEasingFunction) new System.Windows.Media.Animation.QuarticEase();
    private PivotHeadersControl _headers;
    private ItemsPresenter _itemsPresenter;
    private Panel _itemsPanel;
    private AnimationDirection? _animationHint = new AnimationDirection?();
    private bool _updatingHeaderItems;
    private bool _ignorePropertyChange;
    internal PivotHeadersControl _clickedHeadersControl;
    private bool _animating;
    private bool _isHorizontalDragging;
    private double _actualWidth;
    private bool _isDesignTime;
    private bool _skippedLoadingPivotItem;
    private bool _skippedSwapVisibleContent;
    private bool _templateApplied;
    private Queue<int> _queuedIndexChanges;
    private TransformAnimator _panAnimator;
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (Pivot), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (Pivot), new PropertyMetadata(new PropertyChangedCallback(Pivot.OnSelectedIndexPropertyChanged)));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (Pivot), new PropertyMetadata((object) null, new PropertyChangedCallback(Pivot.OnSelectedItemPropertyChanged)));
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (object), typeof (Pivot), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof (TitleTemplate), typeof (DataTemplate), typeof (Pivot), new PropertyMetadata((PropertyChangedCallback) null));

    public event EventHandler<PivotItemEventArgs> LoadingPivotItem;

    public event EventHandler<PivotItemEventArgs> LoadedPivotItem;

    public event EventHandler<PivotItemEventArgs> UnloadingPivotItem;

    public event EventHandler<PivotItemEventArgs> UnloadedPivotItem;

    public event SelectionChangedEventHandler SelectionChanged;

    public DataTemplate HeaderTemplate
    {
      get => ((DependencyObject) this).GetValue(Pivot.HeaderTemplateProperty) as DataTemplate;
      set => ((DependencyObject) this).SetValue(Pivot.HeaderTemplateProperty, (object) value);
    }

    public int SelectedIndex
    {
      get => (int) ((DependencyObject) this).GetValue(Pivot.SelectedIndexProperty);
      set => ((DependencyObject) this).SetValue(Pivot.SelectedIndexProperty, (object) value);
    }

    private static void OnSelectedIndexPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Pivot pivot = d as Pivot;
      if (pivot._ignorePropertyChange)
        pivot._ignorePropertyChange = false;
      else
        pivot.UpdateSelectedIndex((int) e.OldValue, (int) e.NewValue);
    }

    public object SelectedItem
    {
      get => ((DependencyObject) this).GetValue(Pivot.SelectedItemProperty);
      set => ((DependencyObject) this).SetValue(Pivot.SelectedItemProperty, value);
    }

    private static void OnSelectedItemPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Pivot pivot = d as Pivot;
      if (pivot._ignorePropertyChange)
        pivot._ignorePropertyChange = false;
      else
        pivot.UpdateSelectedItem(e.OldValue, e.NewValue);
    }

    public object Title
    {
      get => ((DependencyObject) this).GetValue(Pivot.TitleProperty);
      set => ((DependencyObject) this).SetValue(Pivot.TitleProperty, value);
    }

    public DataTemplate TitleTemplate
    {
      get => ((DependencyObject) this).GetValue(Pivot.TitleTemplateProperty) as DataTemplate;
      set => ((DependencyObject) this).SetValue(Pivot.TitleTemplateProperty, (object) value);
    }

    public Pivot()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.Pivot);
      ((Control) this).DefaultStyleKey = (object) typeof (Pivot);
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      GestureHelper gestureHelper = GestureHelper.Create((UIElement) this);
      gestureHelper.GestureStart += new EventHandler<GestureEventArgs>(this.OnGestureStart);
      gestureHelper.HorizontalDrag += new EventHandler<DragEventArgs>(this.OnHorizontalDrag);
      gestureHelper.Flick += new EventHandler<FlickEventArgs>(this.OnFlick);
      gestureHelper.GestureEnd += new EventHandler<EventArgs>(this.OnGesturesComplete);
      this._isDesignTime = DesignerProperties.IsInDesignTool;
      this._queuedIndexChanges = new Queue<int>(5);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.Pivot_Loaded);
    }

    private void Pivot_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.Pivot_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.Pivot);
    }

    public virtual void OnApplyTemplate()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.Pivot);
      if (this._headers != null)
        this._headers.SelectedIndexChanged -= new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
      ((FrameworkElement) this).OnApplyTemplate();
      this._itemsPresenter = ((Control) this).GetTemplateChild("PivotItemPresenter") as ItemsPresenter;
      this._headers = ((Control) this).GetTemplateChild("HeadersListElement") as PivotHeadersControl;
      if (this._headers != null)
      {
        this._headers.SelectedIndexChanged += new EventHandler<SelectedIndexChangedEventArgs>(this.OnHeaderSelectionChanged);
        this.UpdateHeaders();
      }
      if (((PresentationFrameworkCollection<object>) this.Items).Count > 0)
      {
        if (this.SelectedIndex < 0)
          this.SelectedIndex = 0;
        else
          this.UpdateSelectedIndex(-1, this.SelectedIndex);
      }
      this.UpdateVisibleContent(this.SelectedIndex);
      this.SetSelectedHeaderIndex(this.SelectedIndex);
      this._templateApplied = true;
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.Pivot);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.Pivot);
      Size size = ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.Pivot);
      return size;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.Pivot);
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.Pivot);
      return size;
    }

    protected virtual void OnLoadingPivotItem(PivotItem item)
    {
      if (item != null && ((UIElement) item).Visibility == 1)
        ((UIElement) item).Visibility = (Visibility) 0;
      SafeRaise.Raise<PivotItemEventArgs>(this.LoadingPivotItem, (object) this, (SafeRaise.GetEventArgs<PivotItemEventArgs>) (() => new PivotItemEventArgs(item)));
    }

    protected virtual void OnLoadedPivotItem(PivotItem item)
    {
      SafeRaise.Raise<PivotItemEventArgs>(this.LoadedPivotItem, (object) this, (SafeRaise.GetEventArgs<PivotItemEventArgs>) (() => new PivotItemEventArgs(item)));
      this.OptimizeVisuals();
    }

    private void OptimizeVisuals()
    {
      int selectedIndex = this.SelectedIndex;
      if (selectedIndex < 0 || ((PresentationFrameworkCollection<object>) this.Items).Count <= 1)
        return;
      PivotItem next = this.GetContainer(((PresentationFrameworkCollection<object>) this.Items)[this.RollingIncrement(selectedIndex)]);
      PivotItem previous = this.GetContainer(((PresentationFrameworkCollection<object>) this.Items)[this.RollingDecrement(selectedIndex)]);
      bool flag = true;
      if (next != null && previous != null && ((UIElement) next).Visibility == ((UIElement) previous).Visibility && ((UIElement) previous).Visibility == null)
        flag = false;
      if (!flag)
        return;
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        if (next != null && ((UIElement) next).Visibility == 1)
          ((UIElement) next).Visibility = (Visibility) 0;
        if (previous == next)
          return;
        ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
        {
          if (previous == null || ((UIElement) previous).Visibility != 1)
            return;
          ((UIElement) previous).Visibility = (Visibility) 0;
        }));
      }));
    }

    protected virtual void OnUnloadingPivotItem(PivotItemEventArgs e)
    {
      EventHandler<PivotItemEventArgs> unloadingPivotItem = this.UnloadingPivotItem;
      if (unloadingPivotItem == null)
        return;
      unloadingPivotItem((object) this, e);
    }

    protected virtual void OnUnloadedPivotItem(PivotItemEventArgs e)
    {
      EventHandler<PivotItemEventArgs> unloadedPivotItem = this.UnloadedPivotItem;
      if (unloadedPivotItem == null)
        return;
      unloadedPivotItem((object) this, e);
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      int selectedIndex = this.SelectedIndex;
      int? nullable1 = new int?();
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      if (e != null)
      {
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
            if (this._templateApplied && e.NewStartingIndex == selectedIndex)
            {
              nullable1 = new int?(selectedIndex);
              break;
            }
            break;
          case NotifyCollectionChangedAction.Remove:
            nullable1 = new int?(selectedIndex);
            if (selectedIndex == e.OldStartingIndex || selectedIndex >= count)
            {
              nullable1 = new int?(0);
              break;
            }
            break;
        }
      }
      if (nullable1.HasValue)
      {
        int? nullable2 = nullable1;
        int num = selectedIndex;
        this._animationHint = new AnimationDirection?((nullable2.GetValueOrDefault() >= num ? 0 : (nullable2.HasValue ? 1 : 0)) != 0 ? AnimationDirection.Right : AnimationDirection.Left);
        this.SetSelectedIndexInternal(nullable1.Value);
      }
      this.UpdateHeaders();
      this.OptimizeVisuals();
      base.OnItemsChanged(e);
      ((UIElement) this).UpdateLayout();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      PivotItem element1 = element as PivotItem;
      int selectedIndex = this.SelectedIndex;
      if (selectedIndex >= 0 && ((PresentationFrameworkCollection<object>) this.Items).Count > selectedIndex)
      {
        object obj = ((PresentationFrameworkCollection<object>) this.Items)[selectedIndex];
        if (item == obj)
        {
          if (element1 == null || !this._skippedLoadingPivotItem)
            return;
          this.OnLoadingPivotItem(element1);
          if (!this._skippedSwapVisibleContent)
            return;
          this.OnLoadedPivotItem(element1);
          return;
        }
      }
      if (element1 == null)
        return;
      this.UpdateItemVisibility((UIElement) element1, false);
      if (((UIElement) element1).Visibility != null)
        return;
      ((UIElement) element1).Visibility = (Visibility) 1;
    }

    private void UpdateSelectedIndex(int oldIndex, int newIndex)
    {
      object obj = (object) null;
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      if (newIndex >= 0 && newIndex < count)
        obj = ((PresentationFrameworkCollection<object>) this.Items)[newIndex];
      else if (count > 0 && !this._isDesignTime)
      {
        this._ignorePropertyChange = true;
        this.SelectedIndex = oldIndex;
        throw new ArgumentException("SelectedIndex");
      }
      if (newIndex < 0 && ((PresentationFrameworkCollection<object>) this.Items).Count > 0 && !this._isDesignTime)
      {
        this._ignorePropertyChange = true;
        this.SelectedIndex = 0;
        throw new ArgumentException("SelectedIndex");
      }
      this.SelectedItem = obj;
    }

    private void SetSelectedIndexInternal(int newIndex)
    {
      this._ignorePropertyChange = true;
      this.SelectedIndex = newIndex - 1;
      this.SelectedIndex = newIndex;
    }

    private void UpdateSelectedItem(object oldValue, object newValue)
    {
      if (newValue == null && ((PresentationFrameworkCollection<object>) this.Items).Count > 0 && !this._isDesignTime)
      {
        this._ignorePropertyChange = true;
        this.SelectedItem = oldValue;
        throw new ArgumentException("SelectedItem");
      }
      int num1 = ((PresentationFrameworkCollection<object>) this.Items).IndexOf(oldValue);
      int num2 = ((PresentationFrameworkCollection<object>) this.Items).IndexOf(newValue);
      if (!this._animationHint.HasValue && num1 != -1 && num2 != -1)
        this._animationHint = new AnimationDirection?(this.RollingIncrement(num2) == num1 ? AnimationDirection.Right : AnimationDirection.Left);
      PivotItem container1 = this.GetContainer(newValue);
      PivotItem container2 = this.GetContainer(oldValue);
      this.BeginAnimateContent(num2, container2, this._animationHint.GetValueOrDefault());
      this.SetSelectedHeaderIndex(num2);
      if (this.SelectedIndex != num2)
        this.SetSelectedIndexInternal(num2);
      if (oldValue != null)
        this.OnUnloadingPivotItem(new PivotItemEventArgs(container2));
      if (container1 != null)
        this.OnLoadingPivotItem(container1);
      else
        this._skippedLoadingPivotItem = true;
      this.OnSelectionChanged(new SelectionChangedEventArgs((IList) new List<object>()
      {
        oldValue
      }, (IList) new List<object>() { newValue }));
    }

    private void SetSelectedHeaderIndex(int selectedIndex)
    {
      try
      {
        this._updatingHeaderItems = true;
        if (this._headers == null || ((PresentationFrameworkCollection<object>) this.Items).Count <= 0)
          return;
        this._headers.SelectedIndex = selectedIndex;
      }
      finally
      {
        this._updatingHeaderItems = false;
      }
    }

    private int RollingIncrement(int index)
    {
      ++index;
      return index >= ((PresentationFrameworkCollection<object>) this.Items).Count ? 0 : index;
    }

    private int RollingDecrement(int index)
    {
      --index;
      return index >= 0 ? index : ((PresentationFrameworkCollection<object>) this.Items).Count - 1;
    }

    protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, e);
    }

    private bool EnoughItemsForManipulation => ((PresentationFrameworkCollection<object>) this.Items).Count > 1;

    private void OnGestureStart(object sender, GestureEventArgs e)
    {
      this._isHorizontalDragging = false;
      if (this._clickedHeadersControl == null)
        return;
      this._clickedHeadersControl._wasClicked = false;
      this._clickedHeadersControl._cancelClick = false;
    }

    private void OnGesturesComplete(object sender, EventArgs e)
    {
      if (this.EnoughItemsForManipulation)
      {
        if (e is DragEventArgs dragEventArgs && this._isHorizontalDragging)
        {
          double x = dragEventArgs.CumulativeDistance.X;
          double num = Math.Abs(x);
          if (x != 0.0 && num >= this._actualWidth / 3.0)
            this.NavigateByIndexChange(x <= 0.0 ? 1 : -1);
        }
        if (!this._animating && this._headers != null && this._itemsPresenter != null)
        {
          TransformAnimator.EnsureAnimator((FrameworkElement) this._itemsPresenter, ref this._panAnimator);
          this._panAnimator.GoTo(this.CalculateContentDestination(AnimationDirection.Center), Pivot.PivotAnimationDuration, this.QuarticEase);
          this._headers.RestoreHeaderPosition(Pivot.PivotAnimationDuration);
        }
      }
      this._isHorizontalDragging = false;
    }

    private void OnFlick(object sender, FlickEventArgs e)
    {
      if (this._clickedHeadersControl != null)
      {
        this._clickedHeadersControl._wasClicked = false;
        this._clickedHeadersControl._cancelClick = true;
      }
      if (!this.EnoughItemsForManipulation)
        return;
      int angle = (int) e.Angle;
      switch (angle)
      {
        case 0:
        case 180:
          this.NavigateByIndexChange(angle == 180 ? 1 : -1);
          break;
      }
    }

    private void OnHorizontalDrag(object sender, DragEventArgs e)
    {
      this._isHorizontalDragging = true;
      if (this._clickedHeadersControl != null)
        this._clickedHeadersControl._cancelClick = true;
      if (this._animating || !this.EnoughItemsForManipulation || this._itemsPresenter == null)
        return;
      TransformAnimator.EnsureAnimator((FrameworkElement) this._itemsPresenter, ref this._panAnimator);
      double num = Math.Abs(e.DeltaDistance.X);
      if (e.IsTouchComplete || this._animating || this._panAnimator == null || this._headers == null)
        return;
      TimeSpan timeSpan = TimeSpan.FromSeconds(num / 600.0);
      this._panAnimator.GoTo(e.CumulativeDistance.X, new Duration(timeSpan));
      this._headers.PanHeader(e.CumulativeDistance.X, this._actualWidth, new Duration(timeSpan));
    }

    private PivotHeaderItem CreateHeaderBindingControl(object item)
    {
      PivotHeaderItem pivotHeaderItem = new PivotHeaderItem();
      pivotHeaderItem.ContentTemplate = this.HeaderTemplate;
      PivotHeaderItem headerBindingControl = pivotHeaderItem;
      Binding binding = new Binding() { Source = item };
      if (item is PivotItem)
        binding.Path = new PropertyPath("Header", new object[0]);
      try
      {
        binding.Mode = (BindingMode) 1;
        ((FrameworkElement) headerBindingControl).SetBinding(ContentControl.ContentProperty, binding);
        return headerBindingControl;
      }
      catch
      {
        if (this._isDesignTime)
          return (PivotHeaderItem) null;
        throw;
      }
    }

    private void UpdateHeaders()
    {
      if (this._headers == null)
        return;
      List<PivotHeaderItem> pivotHeaderItemList = new List<PivotHeaderItem>();
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      for (int index = 0; index < count; ++index)
      {
        object obj = ((PresentationFrameworkCollection<object>) this.Items)[index];
        pivotHeaderItemList.Add(this.CreateHeaderBindingControl(obj));
      }
      try
      {
        this._updatingHeaderItems = true;
        this._headers.ItemsSource = count == 0 ? (IEnumerable) null : (IEnumerable) pivotHeaderItemList;
      }
      finally
      {
        this._updatingHeaderItems = false;
      }
    }

    private void OnHeaderSelectionChanged(object s, SelectedIndexChangedEventArgs e)
    {
      if (this._updatingHeaderItems)
        return;
      this._animationHint = new AnimationDirection?(AnimationDirection.Left);
      this.SelectedIndex = e.SelectedIndex;
    }

    private void NavigateByIndexChange(int indexDelta)
    {
      if (this._animating && this._queuedIndexChanges != null)
      {
        this._queuedIndexChanges.Enqueue(indexDelta);
      }
      else
      {
        int selectedIndex = this.SelectedIndex;
        if (selectedIndex == -1)
          return;
        this._animationHint = new AnimationDirection?(indexDelta > 0 ? AnimationDirection.Left : AnimationDirection.Right);
        int num = selectedIndex + indexDelta;
        if (num >= ((PresentationFrameworkCollection<object>) this.Items).Count)
          num = 0;
        else if (num < 0)
          num = ((PresentationFrameworkCollection<object>) this.Items).Count - 1;
        if (this._clickedHeadersControl != null)
        {
          this._clickedHeadersControl._wasClicked = false;
          this._clickedHeadersControl._cancelClick = true;
        }
        this.SelectedIndex = num;
      }
    }

    private int GetPreviousIndex()
    {
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      if (count <= 0)
        return 0;
      int previousIndex = this.SelectedIndex - 1;
      if (previousIndex < 0)
        previousIndex = count - 1;
      return previousIndex;
    }

    private int GetNextIndex()
    {
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      if (count <= 0)
        return 0;
      int nextIndex = this.SelectedIndex + 1;
      if (nextIndex > count)
        nextIndex = 0;
      return nextIndex;
    }

    private void UpdateVisibleContent(int index)
    {
      if (!this.TryHasItemsHost())
        return;
      for (int index1 = 0; index1 < ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children).Count; ++index1)
        this.UpdateItemVisibility(((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)[index1], index1 == index);
    }

    private bool TryHasItemsHost()
    {
      if (this._itemsPanel != null)
        return true;
      if (this.ItemContainerGenerator != null)
      {
        DependencyObject dependencyObject = this.ItemContainerGenerator.ContainerFromIndex(0);
        if (dependencyObject != null)
        {
          this._itemsPanel = VisualTreeHelper.GetParent(dependencyObject) as Panel;
          return this._itemsPanel != null;
        }
      }
      return false;
    }

    protected virtual void UpdateItemVisibility(UIElement element, bool toVisible)
    {
      if (element == null)
        return;
      element.Opacity = toVisible ? 1.0 : 0.0;
      element.IsHitTestVisible = toVisible;
      if (toVisible && element.Visibility == 1)
        element.Visibility = (Visibility) 0;
      if (!this._isDesignTime)
        return;
      TranslateTransform translateTransform = TransformAnimator.GetTranslateTransform(element);
      if (translateTransform == null)
        return;
      translateTransform.X = toVisible ? 0.0 : -((FrameworkElement) this).ActualWidth;
    }

    private double CalculateContentDestination(AnimationDirection direction)
    {
      double contentDestination = 0.0;
      double actualWidth = ((FrameworkElement) this).ActualWidth;
      switch (direction)
      {
        case AnimationDirection.Left:
          contentDestination = -actualWidth;
          break;
        case AnimationDirection.Right:
          contentDestination = actualWidth;
          break;
      }
      return contentDestination;
    }

    private static AnimationDirection InvertAnimationDirection(AnimationDirection direction)
    {
      switch (direction)
      {
        case AnimationDirection.Left:
          return AnimationDirection.Right;
        case AnimationDirection.Right:
          return AnimationDirection.Left;
        default:
          return direction;
      }
    }

    private void BeginAnimateContent(
      int newIndex,
      PivotItem oldItem,
      AnimationDirection animationDirection)
    {
      if (this._isDesignTime)
        this.SwapVisibleContent(oldItem, newIndex);
      else if (this._itemsPresenter != null)
      {
        this._animating = true;
        TransformAnimator.EnsureAnimator((FrameworkElement) this._itemsPresenter, ref this._panAnimator);
        this.GetContainer(this.SelectedItem)?.MoveTo(AnimationDirection.Center);
        if (this._headers != null && animationDirection != AnimationDirection.Center)
          this._headers.AnimationDirection = animationDirection;
        this._panAnimator.GoTo(this.CalculateContentDestination(animationDirection), Pivot.PivotAnimationDuration, (Action) (() =>
        {
          this._panAnimator.GoTo(this.CalculateContentDestination(Pivot.InvertAnimationDirection(animationDirection)), Pivot.ZeroDuration);
          this.SwapVisibleContent(oldItem, newIndex);
          this.GetContainer(this.SelectedItem)?.MoveTo(animationDirection);
          this._panAnimator.GoTo(0.0, Pivot.PivotAnimationDuration, this.QuarticEase, (Action) (() =>
          {
            this._animationHint = new AnimationDirection?();
            this._animating = false;
            this.ProcessQueuedChanges();
          }));
        }));
      }
      else
        this._skippedSwapVisibleContent = true;
    }

    private void SwapVisibleContent(PivotItem oldItem, int newIndex)
    {
      if (oldItem != null)
        this.OnUnloadedPivotItem(new PivotItemEventArgs(oldItem));
      this.UpdateVisibleContent(newIndex);
      this.OnLoadedPivotItem(this.GetContainer(this.SelectedItem));
    }

    private void ProcessQueuedChanges()
    {
      if (this._queuedIndexChanges == null || this._queuedIndexChanges.Count <= 0 || this._animating)
        return;
      this.NavigateByIndexChange(this._queuedIndexChanges.Dequeue());
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this._actualWidth = ((FrameworkElement) this).ActualWidth;
      ((UIElement) this).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, this._actualWidth, ((FrameworkElement) this).ActualHeight)
      };
      if (!this._isDesignTime)
        return;
      this.UpdateVisibleContent(this.SelectedIndex);
    }
  }
}
