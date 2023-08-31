// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PivotHeadersControl
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls.Primitives
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (PivotHeaderItem))]
  [TemplatePart(Name = "Canvas", Type = typeof (Canvas))]
  public class PivotHeadersControl : TemplatedItemsControl<PivotHeaderItem>
  {
    private const string CanvasName = "Canvas";
    private const double PivotSeconds = 0.5;
    private bool _isDesign;
    private Canvas _canvas;
    private Dictionary<Control, double> _sizes;
    private Dictionary<Control, TranslateTransform> _translations;
    private Dictionary<Control, OpacityAnimator> _opacities;
    private Image _leftMirror;
    private TranslateTransform _leftMirrorTranslation;
    private TransformAnimator _canvasAnimator;
    internal readonly IEasingFunction QuarticEase = (IEasingFunction) new System.Windows.Media.Animation.QuarticEase();
    internal bool _cancelClick;
    private bool _activeSelectionChange;
    private bool _isAnimating;
    private Queue<PivotHeadersControl.AnimationInstruction> _queuedAnimations;
    internal bool _wasClicked;
    private Pivot _pivot;
    private double _animatingWidth;
    private DateTime _currentItemAnimationStarted;
    internal static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (PivotHeadersControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(PivotHeadersControl.OnSelectedIndexPropertyChanged)));
    public static readonly DependencyProperty VisualFirstIndexProperty = DependencyProperty.Register(nameof (VisualFirstIndex), typeof (int), typeof (PivotHeadersControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(PivotHeadersControl.OnVisualFirstIndexPropertyChanged)));
    private bool _ignorePropertyChange;
    private int _previousVisualFirstIndex;

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotHeadersControl);
      Size size = ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_ARRANGE, PerfLog.PivotHeadersControl);
      return size;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotHeadersControl);
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_MEASURE, PerfLog.PivotHeadersControl);
      return size;
    }

    internal int SelectedIndex
    {
      get => (int) ((DependencyObject) this).GetValue(PivotHeadersControl.SelectedIndexProperty);
      set => ((DependencyObject) this).SetValue(PivotHeadersControl.SelectedIndexProperty, (object) value);
    }

    private static void OnSelectedIndexPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PivotHeadersControl pivotHeadersControl = d as PivotHeadersControl;
      int newValue = (int) e.NewValue;
      int oldValue = (int) e.OldValue;
      if (pivotHeadersControl._activeSelectionChange)
        return;
      pivotHeadersControl.SelectOne(oldValue, newValue);
    }

    public int VisualFirstIndex
    {
      get => (int) ((DependencyObject) this).GetValue(PivotHeadersControl.VisualFirstIndexProperty);
      set => ((DependencyObject) this).SetValue(PivotHeadersControl.VisualFirstIndexProperty, (object) value);
    }

    private static void OnVisualFirstIndexPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PivotHeadersControl pivotHeadersControl = d as PivotHeadersControl;
      if (pivotHeadersControl._ignorePropertyChange)
      {
        pivotHeadersControl._ignorePropertyChange = false;
      }
      else
      {
        int newValue = (int) e.NewValue;
        pivotHeadersControl._previousVisualFirstIndex = (int) e.OldValue;
        int count = ((PresentationFrameworkCollection<object>) pivotHeadersControl.Items).Count;
        if (count > 0 && newValue >= count)
        {
          pivotHeadersControl._ignorePropertyChange = true;
          d.SetValue(e.Property, (object) 0);
        }
        pivotHeadersControl.UpdateItemsLayout();
      }
    }

    internal event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

    public PivotHeadersControl()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PivotHeadersControl);
      ((Control) this).DefaultStyleKey = (object) typeof (PivotHeadersControl);
      this._leftMirror = new Image();
      ((UIElement) this._leftMirror).CacheMode = (CacheMode) new BitmapCache();
      this._sizes = new Dictionary<Control, double>();
      this._translations = new Dictionary<Control, TranslateTransform>();
      this._opacities = new Dictionary<Control, OpacityAnimator>();
      this._isDesign = DesignerProperties.IsInDesignTool;
      this._queuedAnimations = new Queue<PivotHeadersControl.AnimationInstruction>();
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.PivotHeadersControl_Loaded);
    }

    private void PivotHeadersControl_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.PivotHeadersControl_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_STARTUP, PerfLog.PanoramaItem);
    }

    public virtual void OnApplyTemplate()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotHeadersControl);
      this._pivot = (Pivot) null;
      if (this._canvas != null)
      {
        ((PresentationFrameworkCollection<UIElement>) ((Panel) this._canvas).Children).Remove((UIElement) this._leftMirror);
        this._leftMirror = (Image) null;
      }
      ((FrameworkElement) this).OnApplyTemplate();
      this._canvas = ((Control) this).GetTemplateChild("Canvas") as Canvas;
      if (this._canvas != null)
      {
        ((PresentationFrameworkCollection<UIElement>) ((Panel) this._canvas).Children).Add((UIElement) this._leftMirror);
        this._leftMirrorTranslation = TransformAnimator.GetTranslateTransform((UIElement) this._leftMirror);
        if (!double.IsNaN(((FrameworkElement) this._leftMirror).ActualWidth) && ((FrameworkElement) this._leftMirror).ActualWidth > 0.0)
          this._leftMirrorTranslation.X = -((FrameworkElement) this._leftMirror).ActualWidth;
      }
      if (((PresentationFrameworkCollection<object>) this.Items).Count > 0)
        this.VisualFirstIndex = this.SelectedIndex;
      DependencyObject dependencyObject = (DependencyObject) this;
      do
      {
        dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
        this._pivot = dependencyObject as Pivot;
      }
      while (this._pivot == null && dependencyObject != null);
      if (this._pivot != null)
        this._pivot._clickedHeadersControl = this;
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PIVOT_APPLYTEMPLATE, PerfLog.PivotHeadersControl);
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (((PresentationFrameworkCollection<object>) this.Items).Count > 0)
      {
        this.UpdateItemsLayout();
      }
      else
      {
        this.VisualFirstIndex = 0;
        this.SelectedIndex = 0;
      }
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      base.ClearContainerForItemOverride(element, item);
      PivotHeaderItem pivotHeaderItem = (PivotHeaderItem) element;
      pivotHeaderItem.ParentHeadersControl = (PivotHeadersControl) null;
      pivotHeaderItem.Item = (object) null;
      if (!object.ReferenceEquals((object) element, item))
        pivotHeaderItem.Item = item;
      if (!(item is Control key))
        return;
      ((FrameworkElement) key).SizeChanged -= new SizeChangedEventHandler(this.OnHeaderSizeChanged);
      this._sizes.Remove(key);
      this._translations.Remove(key);
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      PivotHeaderItem pivotHeaderItem = (PivotHeaderItem) element;
      pivotHeaderItem.ParentHeadersControl = this;
      int num = this.ItemContainerGenerator.IndexFromContainer(element);
      if (num != -1)
        pivotHeaderItem.IsSelected = this.SelectedIndex == num;
      if (!(item is Control control))
        return;
      ((FrameworkElement) control).SizeChanged += new SizeChangedEventHandler(this.OnHeaderSizeChanged);
    }

    private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e)
    {
      double width = e.NewSize.Width;
      double height = e.NewSize.Height;
      if (double.IsNaN(((FrameworkElement) this).Height) || height > ((FrameworkElement) this).Height)
        ((FrameworkElement) this).Height = height;
      this._sizes[(Control) sender] = width;
      this.UpdateItemsLayout();
      if (this._leftMirrorTranslation.X != 0.0)
        return;
      this.UpdateLeftMirrorImage(this.SelectedIndex);
    }

    internal void OnHeaderItemClicked(PivotHeaderItem item)
    {
      if (this._isAnimating)
        return;
      if (this._cancelClick)
      {
        this._cancelClick = false;
      }
      else
      {
        this._wasClicked = true;
        item.IsSelected = true;
      }
    }

    internal void NotifyHeaderItemSelected(PivotHeaderItem item, bool isSelected)
    {
      if (!isSelected)
        return;
      int index = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) item);
      this.SelectOne(this.SelectedIndex, index);
      this.SelectedIndex = index;
    }

    private void SelectOne(int previousIndex, int index)
    {
      if (this._activeSelectionChange)
        return;
      this.UpdateLeftMirrorImage(index);
      if (index < 0)
        return;
      if (index >= ((PresentationFrameworkCollection<object>) this.Items).Count)
        return;
      try
      {
        this._activeSelectionChange = true;
        for (int index1 = 0; index1 < ((PresentationFrameworkCollection<object>) this.Items).Count; ++index1)
        {
          PivotHeaderItem pivotHeaderItem = (PivotHeaderItem) this.ItemContainerGenerator.ContainerFromIndex(index1);
          if (pivotHeaderItem != null)
            pivotHeaderItem.IsSelected = index == index1;
        }
      }
      finally
      {
        SafeRaise.Raise<SelectedIndexChangedEventArgs>(this.SelectedIndexChanged, (object) this, new SelectedIndexChangedEventArgs(index));
        this._activeSelectionChange = false;
        this.BeginAnimate(previousIndex, index);
      }
    }

    internal AnimationDirection AnimationDirection { get; set; }

    internal void RestoreHeaderPosition(Duration duration)
    {
      if (this._canvas == null || this._isAnimating)
        return;
      TransformAnimator.EnsureAnimator((FrameworkElement) this._canvas, ref this._canvasAnimator);
      if (this._canvasAnimator == null)
        return;
      this._canvasAnimator.GoTo(0.0, duration);
    }

    internal void PanHeader(double cumulative, double contentWidth, Duration duration)
    {
      if (this._isAnimating || this._canvas == null)
        return;
      TransformAnimator.EnsureAnimator((FrameworkElement) this._canvas, ref this._canvasAnimator);
      if (this._canvasAnimator == null)
        return;
      double num = cumulative < 0.0 ? this.GetItemWidth(this.SelectedIndex) : this.GetLeftMirrorWidth(this.SelectedIndex);
      this._canvasAnimator.GoTo(cumulative / contentWidth * num, duration);
    }

    private void BeginAnimate(int previousIndex, int newIndex)
    {
      if (this._isDesign || this._canvas == null)
      {
        this.VisualFirstIndex = newIndex;
      }
      else
      {
        if (newIndex != this.RollingIncrement(previousIndex) && newIndex != this.RollingDecrement(previousIndex) || this._wasClicked)
        {
          this._wasClicked = false;
          int next;
          for (int index = previousIndex; index != newIndex; index = next)
          {
            next = this.RollingIncrement(index);
            PivotHeadersControl.AnimationInstruction animationInstruction = new PivotHeadersControl.AnimationInstruction(index, next);
            animationInstruction._width = this.GetItemWidth(index);
            if (animationInstruction._width > 0.0)
              this._queuedAnimations.Enqueue(animationInstruction);
          }
          this.UpdateActiveAndQueuedAnimations();
        }
        else
        {
          if (this._queuedAnimations.Count == 0 && !this._isAnimating)
          {
            this.BeginAnimateInternal(previousIndex, newIndex, this.QuarticEase, new Duration?());
            return;
          }
          this._queuedAnimations.Enqueue(new PivotHeadersControl.AnimationInstruction(previousIndex, newIndex)
          {
            _ease = this.QuarticEase,
            _width = this.GetItemWidth(previousIndex)
          });
          this.UpdateActiveAndQueuedAnimations();
        }
        if (this._isAnimating)
          return;
        this.AnimateComplete();
      }
    }

    private void UpdateActiveAndQueuedAnimations()
    {
      TransformAnimator.EnsureAnimator((FrameworkElement) this._canvas, ref this._canvasAnimator);
      if (this._canvasAnimator == null)
        return;
      double num1 = 0.0;
      foreach (PivotHeadersControl.AnimationInstruction queuedAnimation in this._queuedAnimations)
        num1 += queuedAnimation._width;
      if (this._isAnimating && this._animatingWidth > 0.0)
        num1 += this._animatingWidth;
      int num2 = 0;
      foreach (PivotHeadersControl.AnimationInstruction queuedAnimation in this._queuedAnimations)
      {
        ++num2;
        queuedAnimation._durationInSeconds = queuedAnimation._width / (num1 == 0.0 ? 1.0 : num1) * 0.5;
        queuedAnimation._ease = num2 == this._queuedAnimations.Count ? this.QuarticEase : (IEasingFunction) null;
        if (this._isAnimating)
          this._canvasAnimator.UpdateEasingFunction((IEasingFunction) null);
      }
      if (!this._isAnimating)
        return;
      double num3 = (DateTime.Now - this._currentItemAnimationStarted).TotalSeconds / 0.5;
      this._canvasAnimator.UpdateDuration(new Duration(TimeSpan.FromSeconds(this._animatingWidth * num3 / (num1 == 0.0 ? 1.0 : num1) * num3 * 0.5)));
    }

    private void BeginAnimateInternal(
      int previousIndex,
      int newIndex,
      IEasingFunction ease,
      Duration? optionalDuration)
    {
      if (previousIndex == newIndex || previousIndex < 0 || previousIndex >= ((PresentationFrameworkCollection<object>) this.Items).Count || this._isDesign || this._canvas == null)
      {
        if (this.VisualFirstIndex != newIndex)
          this.VisualFirstIndex = newIndex;
        this.AnimateComplete();
      }
      else
      {
        TransformAnimator.EnsureAnimator((FrameworkElement) this._canvas, ref this._canvasAnimator);
        this._isAnimating = true;
        bool flag = ((PresentationFrameworkCollection<object>) this.Items).Count != 2 ? newIndex == this.RollingIncrement(previousIndex) : this.AnimationDirection == AnimationDirection.Left;
        double itemWidth = this.GetItemWidth(flag ? previousIndex : newIndex);
        this._animatingWidth = itemWidth;
        this._currentItemAnimationStarted = DateTime.Now;
        double targetOffset = -itemWidth + (flag ? 0.0 : this._canvasAnimator.CurrentOffset);
        double num = itemWidth == 0.0 ? itemWidth : (itemWidth - Math.Abs(this._canvasAnimator.CurrentOffset)) / itemWidth;
        if (num == 0.0)
          num = 1.0;
        Duration currentSampleDuration = optionalDuration.HasValue ? optionalDuration.Value : new Duration(TimeSpan.FromSeconds(0.25 + Math.Abs(num * 0.25)));
        if (flag)
        {
          this._canvasAnimator.GoTo(targetOffset, currentSampleDuration, ease, (Action) (() =>
          {
            this.VisualFirstIndex = newIndex;
            this._canvasAnimator.GoTo(0.0, Pivot.ZeroDuration, new Action(this.AnimateComplete));
          }));
        }
        else
        {
          this.VisualFirstIndex = newIndex;
          this._canvasAnimator.GoTo(targetOffset, Pivot.ZeroDuration, (Action) (() => this._canvasAnimator.GoTo(0.0, currentSampleDuration, ease, new Action(this.AnimateComplete))));
        }
      }
    }

    private void AnimateComplete()
    {
      if (this._queuedAnimations.Count == 0)
      {
        this._isAnimating = false;
      }
      else
      {
        PivotHeadersControl.AnimationInstruction animationInstruction = this._queuedAnimations.Dequeue();
        Duration duration = new Duration(TimeSpan.FromSeconds(animationInstruction._durationInSeconds));
        this.BeginAnimateInternal(animationInstruction._previousIndex, animationInstruction._index, animationInstruction._ease, new Duration?(duration));
      }
    }

    private double GetLeftMirrorWidth(int index) => this.GetItemWidth(this.GetPreviousVisualIndex(index));

    private double GetNextHeaderWidth()
    {
      int index = this.VisualFirstIndex + 1;
      if (index >= ((PresentationFrameworkCollection<object>) this.Items).Count)
        index = 0;
      return this.GetItemWidth(index);
    }

    private double GetItemWidth(int index)
    {
      Control itemFromIndex = this.GetItemFromIndex(index) as Control;
      double d = 0.0;
      if (itemFromIndex != null && !this._sizes.TryGetValue(itemFromIndex, out d))
      {
        d = ((FrameworkElement) itemFromIndex).ActualWidth;
        if (!double.IsNaN(d))
          this._sizes[itemFromIndex] = d;
      }
      return d;
    }

    private int GetPreviousVisualIndex(int indexOfInterest)
    {
      int num = indexOfInterest - 1;
      return num >= 0 ? num : ((PresentationFrameworkCollection<object>) this.Items).Count - 1;
    }

    private void UpdateLeftMirrorImage(int visualRootIndex)
    {
      if (this._leftMirrorTranslation == null || this._sizes == null || this._leftMirror == null)
        return;
      if (((PresentationFrameworkCollection<object>) this.Items).Count <= 1)
      {
        this._leftMirror = (Image) null;
      }
      else
      {
        if (!(this.GetItemFromIndex(this.GetPreviousVisualIndex(visualRootIndex)) is PivotHeaderItem itemFromIndex) || !this._sizes.ContainsKey((Control) itemFromIndex))
          return;
        double siz = this._sizes[(Control) itemFromIndex];
        itemFromIndex.UpdateVisualStateToUnselected();
        try
        {
          this._leftMirror.Source = (ImageSource) new WriteableBitmap((UIElement) itemFromIndex, (Transform) new TranslateTransform());
        }
        catch (Exception ex)
        {
          this._leftMirror.Source = (ImageSource) null;
        }
        finally
        {
          itemFromIndex.RestoreVisualStates();
          this._leftMirrorTranslation.X = -siz;
        }
      }
    }

    private void UpdateItemsLayout()
    {
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      double offset = 0.0;
      int visualFirstIndex = this.VisualFirstIndex;
      for (int index = visualFirstIndex; index < ((PresentationFrameworkCollection<object>) this.Items).Count; ++index)
      {
        this.FadeInItemIfNeeded(index, visualFirstIndex, this._previousVisualFirstIndex, count);
        this.SetItemPosition(index, ref offset);
      }
      if (this.VisualFirstIndex <= 0)
        return;
      for (int index = 0; index < this.VisualFirstIndex; ++index)
      {
        this.FadeInItemIfNeeded(index, visualFirstIndex, this._previousVisualFirstIndex, count);
        this.SetItemPosition(index, ref offset);
      }
    }

    private void FadeInItemIfNeeded(
      int index,
      int visualFirstIndex,
      int previousVisualFirstIndex,
      int itemCount)
    {
      if (!this._isDesign && this.RollingIncrement(index) == visualFirstIndex && index == previousVisualFirstIndex)
      {
        if (itemCount <= 1 || itemCount == 2 && this.AnimationDirection == AnimationDirection.Right)
          return;
        double num = 0.0;
        for (int index1 = this.RollingIncrement(index); index1 != index; index1 = this.RollingIncrement(index1))
          num += this.GetItemWidth(index1);
        if (num >= ((FrameworkElement) this).ActualWidth)
          return;
        this.FadeIn(index);
      }
      else
      {
        if (!(this.GetItemFromIndex(index) is UIElement itemFromIndex))
          return;
        itemFromIndex.Opacity = 1.0;
      }
    }

    private object GetItemFromIndex(int index) => ((PresentationFrameworkCollection<object>) this.Items).Count > index ? ((PresentationFrameworkCollection<object>) this.Items)[index] : (object) null;

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

    private void FadeIn(int index)
    {
      Control control = (Control) ((PresentationFrameworkCollection<object>) this.Items)[index];
      OpacityAnimator oa;
      if (!this._opacities.TryGetValue(control, out oa))
      {
        OpacityAnimator.EnsureAnimator((UIElement) control, ref oa);
        this._opacities[control] = oa;
      }
      if (oa == null)
        return;
      oa.GoTo(0.0, Pivot.ZeroDuration, (Action) (() => oa.GoTo(1.0, new Duration(TimeSpan.FromSeconds(0.125)))));
    }

    private void SetItemPosition(int i, ref double offset)
    {
      if (!(this.GetItemFromIndex(i) is Control itemFromIndex))
        return;
      double num;
      if (!this._sizes.TryGetValue(itemFromIndex, out num))
        num = 0.0;
      TranslateTransform translateTransform;
      if (!this._translations.TryGetValue(itemFromIndex, out translateTransform))
      {
        translateTransform = TransformAnimator.GetTranslateTransform((UIElement) itemFromIndex);
        this._translations[itemFromIndex] = translateTransform;
      }
      translateTransform.X = offset;
      offset += num;
    }

    private class AnimationInstruction
    {
      public int _previousIndex;
      public int _index;
      public IEasingFunction _ease;
      public double _width;
      public double _durationInSeconds;

      public AnimationInstruction(int previous, int next)
      {
        this._previousIndex = previous;
        this._index = next;
      }
    }
  }
}
