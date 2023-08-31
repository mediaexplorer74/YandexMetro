// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.LoopingSelector
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls.Primitives
{
  [TemplatePart(Name = "PanningTransform", Type = typeof (TranslateTransform))]
  [TemplatePart(Name = "ItemsPanel", Type = typeof (Panel))]
  [TemplatePart(Name = "CenteringTransform", Type = typeof (TranslateTransform))]
  public class LoopingSelector : Control
  {
    private const string ItemsPanelName = "ItemsPanel";
    private const string CenteringTransformName = "CenteringTransform";
    private const string PanningTransformName = "PanningTransform";
    private const double DragSensitivity = 12.0;
    private static readonly Duration _selectDuration = new Duration(TimeSpan.FromMilliseconds(500.0));
    private readonly IEasingFunction _selectEase;
    private static readonly Duration _panDuration = new Duration(TimeSpan.FromMilliseconds(100.0));
    private readonly IEasingFunction _panEase;
    private DoubleAnimation _panelAnimation;
    private Storyboard _panelStoryboard;
    private Panel _itemsPanel;
    private TranslateTransform _panningTransform;
    private TranslateTransform _centeringTransform;
    private bool _isSelecting;
    private LoopingSelectorItem _selectedItem;
    private Queue<LoopingSelectorItem> _temporaryItemsPool;
    private double _minimumPanelScroll;
    private double _maximumPanelScroll;
    private int _additionalItemsCount;
    private bool _isAnimating;
    private double _dragTarget;
    private bool _isAllowedToDragVertically;
    private bool _isDragging;
    private LoopingSelector.State _state;
    public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof (DataSource), typeof (ILoopingSelectorDataSource), typeof (LoopingSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LoopingSelector.OnDataModelChanged)));
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (LoopingSelector), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (LoopingSelector), new PropertyMetadata((object) false, new PropertyChangedCallback(LoopingSelector.OnIsExpandedChanged)));

    public ILoopingSelectorDataSource DataSource
    {
      get => (ILoopingSelectorDataSource) ((DependencyObject) this).GetValue(LoopingSelector.DataSourceProperty);
      set
      {
        if (this.DataSource != null)
          this.DataSource.SelectionChanged -= new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
        ((DependencyObject) this).SetValue(LoopingSelector.DataSourceProperty, (object) value);
        if (value == null)
          return;
        value.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.value_SelectionChanged);
      }
    }

    private void value_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.IsReady || this._isSelecting || e.AddedItems.Count != 1)
        return;
      object addedItem = e.AddedItems[0];
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (((FrameworkElement) child).DataContext == addedItem)
        {
          this.SelectAndSnapTo(child);
          return;
        }
      }
      this.UpdateData();
    }

    private static void OnDataModelChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((LoopingSelector) obj).UpdateData();
    }

    private void DataModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.IsReady || this._isSelecting || e.AddedItems.Count != 1)
        return;
      object addedItem = e.AddedItems[0];
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (((FrameworkElement) child).DataContext == addedItem)
        {
          this.SelectAndSnapTo(child);
          break;
        }
      }
      this.UpdateData();
    }

    public DataTemplate ItemTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LoopingSelector.ItemTemplateProperty);
      set => ((DependencyObject) this).SetValue(LoopingSelector.ItemTemplateProperty, (object) value);
    }

    public Size ItemSize { get; set; }

    public Thickness ItemMargin { get; set; }

    public LoopingSelector()
    {
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).EasingMode = (EasingMode) 2;
      this._selectEase = (IEasingFunction) exponentialEase;
      this._panEase = (IEasingFunction) new ExponentialEase();
      this._minimumPanelScroll = -3.4028234663852886E+38;
      this._maximumPanelScroll = 3.4028234663852886E+38;
      this._isAllowedToDragVertically = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.DefaultStyleKey = (object) typeof (LoopingSelector);
      this.CreateEventHandlers();
    }

    public bool IsExpanded
    {
      get => (bool) ((DependencyObject) this).GetValue(LoopingSelector.IsExpandedProperty);
      set => ((DependencyObject) this).SetValue(LoopingSelector.IsExpandedProperty, (object) value);
    }

    public event DependencyPropertyChangedEventHandler IsExpandedChanged;

    private static void OnIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      LoopingSelector loopingSelector = (LoopingSelector) sender;
      loopingSelector.UpdateItemState();
      if (!loopingSelector.IsExpanded)
        loopingSelector.SelectAndSnapToClosest();
      if (loopingSelector._state == LoopingSelector.State.Normal || loopingSelector._state == LoopingSelector.State.Expanded)
        loopingSelector._state = loopingSelector.IsExpanded ? LoopingSelector.State.Expanded : LoopingSelector.State.Normal;
      DependencyPropertyChangedEventHandler isExpandedChanged = loopingSelector.IsExpandedChanged;
      if (isExpandedChanged == null)
        return;
      isExpandedChanged((object) loopingSelector, e);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._itemsPanel = (Panel) ((object) (this.GetTemplateChild("ItemsPanel") as Panel) ?? (object) new Canvas());
      if (!(this.GetTemplateChild("CenteringTransform") is TranslateTransform translateTransform1))
        translateTransform1 = new TranslateTransform();
      this._centeringTransform = translateTransform1;
      if (!(this.GetTemplateChild("PanningTransform") is TranslateTransform translateTransform2))
        translateTransform2 = new TranslateTransform();
      this._panningTransform = translateTransform2;
      this.CreateVisuals();
    }

    private void LoopingSelector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!this._isAnimating)
        return;
      double y = this._panningTransform.Y;
      this.StopAnimation();
      this._panningTransform.Y = y;
      this._isAnimating = false;
      this._state = LoopingSelector.State.Dragging;
    }

    private void LoopingSelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this._selectedItem == sender || this._state != LoopingSelector.State.Dragging || this._isAnimating)
        return;
      this.SelectAndSnapToClosest();
      this._state = LoopingSelector.State.Expanded;
    }

    private void OnTap(object sender, GestureEventArgs e)
    {
      if (this._panningTransform == null)
        return;
      this.SelectAndSnapToClosest();
      e.Handled = true;
    }

    private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      this._isAllowedToDragVertically = true;
      this._isDragging = false;
    }

    private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (this._isDragging)
      {
        Duration panDuration = LoopingSelector._panDuration;
        IEasingFunction panEase = this._panEase;
        LoopingSelector loopingSelector = this;
        double dragTarget = loopingSelector._dragTarget;
        double y = e.DeltaManipulation.Translation.Y;
        double num1;
        double num2 = num1 = dragTarget + y;
        loopingSelector._dragTarget = num1;
        double to = num2;
        this.AnimatePanel(panDuration, panEase, to);
        e.Handled = true;
      }
      else if (Math.Abs(e.CumulativeManipulation.Translation.X) > 12.0)
      {
        this._isAllowedToDragVertically = false;
      }
      else
      {
        if (!this._isAllowedToDragVertically || Math.Abs(e.CumulativeManipulation.Translation.Y) <= 12.0)
          return;
        this._isDragging = true;
        this._state = LoopingSelector.State.Dragging;
        e.Handled = true;
        this._selectedItem = (LoopingSelectorItem) null;
        if (!this.IsExpanded)
          this.IsExpanded = true;
        this._dragTarget = this._panningTransform.Y;
        this.UpdateItemState();
      }
    }

    private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (!this._isDragging)
        return;
      if (e.IsInertial)
      {
        this._state = LoopingSelector.State.Flicking;
        this._selectedItem = (LoopingSelectorItem) null;
        if (!this.IsExpanded)
          this.IsExpanded = true;
        Point initialVelocity = new Point(0.0, e.FinalVelocities.LinearVelocity.Y);
        double stopTime = PhysicsConstants.GetStopTime(initialVelocity);
        Point stopPoint = PhysicsConstants.GetStopPoint(initialVelocity);
        IEasingFunction easingFunction = PhysicsConstants.GetEasingFunction(stopTime);
        this.AnimatePanel(new Duration(TimeSpan.FromSeconds(stopTime)), easingFunction, this._panningTransform.Y + stopPoint.Y);
        e.Handled = true;
        this._selectedItem = (LoopingSelectorItem) null;
        this.UpdateItemState();
      }
      if (this._state == LoopingSelector.State.Dragging)
        this.SelectAndSnapToClosest();
      this._state = LoopingSelector.State.Expanded;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this._centeringTransform.Y = Math.Round(e.NewSize.Height / 2.0);
      ((UIElement) this).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height)
      };
      this.UpdateData();
    }

    private void OnWrapperClick(object sender, EventArgs e)
    {
      if (this._state == LoopingSelector.State.Normal)
      {
        this._state = LoopingSelector.State.Expanded;
        this.IsExpanded = true;
      }
      else
      {
        if (this._state != LoopingSelector.State.Expanded)
          return;
        if (!this._isAnimating && sender == this._selectedItem)
        {
          this._state = LoopingSelector.State.Normal;
          this.IsExpanded = false;
        }
        else
        {
          if (sender == this._selectedItem || this._isAnimating)
            return;
          this.SelectAndSnapTo((LoopingSelectorItem) sender);
        }
      }
    }

    private void SelectAndSnapTo(LoopingSelectorItem item)
    {
      if (item == null)
        return;
      if (this._selectedItem != null)
        this._selectedItem.SetState(this.IsExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
      if (this._selectedItem != item)
      {
        this._selectedItem = item;
        ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
        {
          this._isSelecting = true;
          this.DataSource.SelectedItem = ((FrameworkElement) item).DataContext;
          this._isSelecting = false;
        }));
      }
      this._selectedItem.SetState(LoopingSelectorItem.State.Selected, true);
      TranslateTransform transform = item.Transform;
      if (transform == null)
        return;
      double to = -transform.Y - Math.Round(((FrameworkElement) item).ActualHeight / 2.0);
      if (this._panningTransform.Y == to)
        return;
      this.AnimatePanel(LoopingSelector._selectDuration, this._selectEase, to);
    }

    private void UpdateData()
    {
      if (!this.IsReady)
        return;
      this._temporaryItemsPool = new Queue<LoopingSelectorItem>(((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children).Count);
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (child.GetState() == LoopingSelectorItem.State.Selected)
          child.SetState(LoopingSelectorItem.State.Normal, false);
        this._temporaryItemsPool.Enqueue(child);
        child.Remove();
      }
      ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children).Clear();
      this.StopAnimation();
      this._panningTransform.Y = 0.0;
      this._minimumPanelScroll = -3.4028234663852886E+38;
      this._maximumPanelScroll = 3.4028234663852886E+38;
      this.Balance();
    }

    private void AnimatePanel(Duration duration, IEasingFunction ease, double to)
    {
      double num1 = Math.Max(this._minimumPanelScroll, Math.Min(this._maximumPanelScroll, to));
      if (to != num1)
      {
        double num2 = Math.Abs(this._panningTransform.Y - to);
        double num3 = Math.Abs(this._panningTransform.Y - num1) / num2;
        duration = new Duration(TimeSpan.FromMilliseconds((double) duration.TimeSpan.Milliseconds * num3));
        to = num1;
      }
      double y = this._panningTransform.Y;
      this.StopAnimation();
      CompositionTarget.Rendering += new EventHandler(this.AnimationPerFrameCallback);
      ((Timeline) this._panelAnimation).Duration = duration;
      this._panelAnimation.EasingFunction = ease;
      this._panelAnimation.From = new double?(y);
      this._panelAnimation.To = new double?(to);
      this._panelStoryboard.Begin();
      this._panelStoryboard.SeekAlignedToLastTick(TimeSpan.Zero);
      this._isAnimating = true;
    }

    private void StopAnimation()
    {
      this._panelStoryboard.Stop();
      CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
    }

    private void Brake(double newStoppingPoint)
    {
      double num = this._panelAnimation.To.Value - this._panelAnimation.From.Value;
      this.AnimatePanel(new Duration(TimeSpan.FromMilliseconds((double) ((Timeline) this._panelAnimation).Duration.TimeSpan.Milliseconds * ((newStoppingPoint - this._panningTransform.Y) / num))), this._panelAnimation.EasingFunction, newStoppingPoint);
    }

    private bool IsReady => ((FrameworkElement) this).ActualHeight > 0.0 && this.DataSource != null && this._itemsPanel != null;

    private void Balance()
    {
      if (!this.IsReady)
        return;
      double actualItemWidth = this.ActualItemWidth;
      double actualItemHeight = this.ActualItemHeight;
      this._additionalItemsCount = (int) Math.Round(((FrameworkElement) this).ActualHeight * 1.5 / actualItemHeight);
      int num = -1;
      LoopingSelectorItem loopingSelectorItem1;
      if (((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children).Count == 0)
      {
        num = 0;
        this._selectedItem = loopingSelectorItem1 = this.CreateAndAddItem(this._itemsPanel, this.DataSource.SelectedItem);
        loopingSelectorItem1.Transform.Y = -actualItemHeight / 2.0;
        loopingSelectorItem1.Transform.X = (((FrameworkElement) this).ActualWidth - actualItemWidth) / 2.0;
        loopingSelectorItem1.SetState(LoopingSelectorItem.State.Selected, false);
      }
      else
        loopingSelectorItem1 = (LoopingSelectorItem) ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)[this.GetClosestItem()];
      int count1;
      LoopingSelectorItem before = LoopingSelector.GetFirstItem(loopingSelectorItem1, out count1);
      int count2;
      LoopingSelectorItem after = LoopingSelector.GetLastItem(loopingSelectorItem1, out count2);
      if (count1 < count2 || count1 < this._additionalItemsCount)
      {
        for (; count1 < this._additionalItemsCount; ++count1)
        {
          object previous = this.DataSource.GetPrevious(((FrameworkElement) before).DataContext);
          if (previous == null)
          {
            this._maximumPanelScroll = -before.Transform.Y - actualItemHeight / 2.0;
            if (this._isAnimating && this._panelAnimation.To.Value > this._maximumPanelScroll)
            {
              this.Brake(this._maximumPanelScroll);
              break;
            }
            break;
          }
          LoopingSelectorItem loopingSelectorItem2;
          if (count2 > this._additionalItemsCount)
          {
            loopingSelectorItem2 = after;
            after = after.Previous;
            loopingSelectorItem2.Remove();
            loopingSelectorItem2.Content = ((FrameworkElement) loopingSelectorItem2).DataContext = previous;
          }
          else
          {
            loopingSelectorItem2 = this.CreateAndAddItem(this._itemsPanel, previous);
            loopingSelectorItem2.Transform.X = (((FrameworkElement) this).ActualWidth - actualItemWidth) / 2.0;
          }
          loopingSelectorItem2.Transform.Y = before.Transform.Y - actualItemHeight;
          loopingSelectorItem2.InsertBefore(before);
          before = loopingSelectorItem2;
        }
      }
      if (count2 < count1 || count2 < this._additionalItemsCount)
      {
        for (; count2 < this._additionalItemsCount; ++count2)
        {
          object next = this.DataSource.GetNext(((FrameworkElement) after).DataContext);
          if (next == null)
          {
            this._minimumPanelScroll = -after.Transform.Y - actualItemHeight / 2.0;
            if (this._isAnimating && this._panelAnimation.To.Value < this._minimumPanelScroll)
            {
              this.Brake(this._minimumPanelScroll);
              break;
            }
            break;
          }
          LoopingSelectorItem loopingSelectorItem3;
          if (count1 > this._additionalItemsCount)
          {
            loopingSelectorItem3 = before;
            before = before.Next;
            loopingSelectorItem3.Remove();
            loopingSelectorItem3.Content = ((FrameworkElement) loopingSelectorItem3).DataContext = next;
          }
          else
          {
            loopingSelectorItem3 = this.CreateAndAddItem(this._itemsPanel, next);
            loopingSelectorItem3.Transform.X = (((FrameworkElement) this).ActualWidth - actualItemWidth) / 2.0;
          }
          loopingSelectorItem3.Transform.Y = after.Transform.Y + actualItemHeight;
          loopingSelectorItem3.InsertAfter(after);
          after = loopingSelectorItem3;
        }
      }
      this._temporaryItemsPool = (Queue<LoopingSelectorItem>) null;
    }

    private static LoopingSelectorItem GetFirstItem(LoopingSelectorItem item, out int count)
    {
      count = 0;
      for (; item.Previous != null; item = item.Previous)
        ++count;
      return item;
    }

    private static LoopingSelectorItem GetLastItem(LoopingSelectorItem item, out int count)
    {
      count = 0;
      for (; item.Next != null; item = item.Next)
        ++count;
      return item;
    }

    private void AnimationPerFrameCallback(object sender, EventArgs e) => this.Balance();

    private int GetClosestItem()
    {
      if (!this.IsReady)
        return -1;
      double actualItemHeight = this.ActualItemHeight;
      int count = ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children).Count;
      double y = this._panningTransform.Y;
      double num1 = actualItemHeight / 2.0;
      int closestItem = -1;
      double num2 = double.MaxValue;
      for (int index = 0; index < count; ++index)
      {
        double num3 = Math.Abs(((LoopingSelectorItem) ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)[index]).Transform.Y + num1 + y);
        if (num3 <= num1)
        {
          closestItem = index;
          break;
        }
        if (num2 > num3)
        {
          num2 = num3;
          closestItem = index;
        }
      }
      return closestItem;
    }

    private void PanelStoryboardCompleted(object sender, EventArgs e)
    {
      CompositionTarget.Rendering -= new EventHandler(this.AnimationPerFrameCallback);
      this._isAnimating = false;
      if (this._state == LoopingSelector.State.Dragging)
        return;
      this.SelectAndSnapToClosest();
    }

    private void SelectAndSnapToClosest()
    {
      if (!this.IsReady)
        return;
      int closestItem = this.GetClosestItem();
      if (closestItem == -1)
        return;
      this.SelectAndSnapTo((LoopingSelectorItem) ((PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)[closestItem]);
    }

    private void UpdateItemState()
    {
      if (!this.IsReady)
        return;
      bool isExpanded = this.IsExpanded;
      foreach (LoopingSelectorItem child in (PresentationFrameworkCollection<UIElement>) this._itemsPanel.Children)
      {
        if (child == this._selectedItem)
          child.SetState(LoopingSelectorItem.State.Selected, true);
        else
          child.SetState(isExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
      }
    }

    private double ActualItemWidth => this.Padding.Left + this.Padding.Right + this.ItemSize.Width;

    private double ActualItemHeight => this.Padding.Top + this.Padding.Bottom + this.ItemSize.Height;

    private void CreateVisuals()
    {
      this._panelAnimation = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) this._panelAnimation, (DependencyObject) this._panningTransform);
      Storyboard.SetTargetProperty((Timeline) this._panelAnimation, new PropertyPath("Y", new object[0]));
      this._panelStoryboard = new Storyboard();
      ((PresentationFrameworkCollection<Timeline>) this._panelStoryboard.Children).Add((Timeline) this._panelAnimation);
      ((Timeline) this._panelStoryboard).Completed += new EventHandler(this.PanelStoryboardCompleted);
    }

    private void CreateEventHandlers()
    {
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      ((UIElement) this).ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.OnManipulationStarted);
      ((UIElement) this).ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.OnManipulationCompleted);
      ((UIElement) this).ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.OnManipulationDelta);
      ((UIElement) this).Tap += new EventHandler<GestureEventArgs>(this.OnTap);
      ((UIElement) this).AddHandler(UIElement.MouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonDown), true);
      ((UIElement) this).AddHandler(UIElement.MouseLeftButtonUpEvent, (Delegate) new MouseButtonEventHandler(this.LoopingSelector_MouseLeftButtonUp), true);
    }

    private LoopingSelectorItem CreateAndAddItem(Panel parent, object content)
    {
      bool flag = this._temporaryItemsPool != null && this._temporaryItemsPool.Count > 0;
      LoopingSelectorItem andAddItem = flag ? this._temporaryItemsPool.Dequeue() : new LoopingSelectorItem();
      if (!flag)
      {
        andAddItem.ContentTemplate = this.ItemTemplate;
        ((FrameworkElement) andAddItem).Width = this.ItemSize.Width;
        ((FrameworkElement) andAddItem).Height = this.ItemSize.Height;
        ((Control) andAddItem).Padding = this.ItemMargin;
        andAddItem.Click += new EventHandler<EventArgs>(this.OnWrapperClick);
      }
      ((FrameworkElement) andAddItem).DataContext = andAddItem.Content = content;
      ((PresentationFrameworkCollection<UIElement>) parent.Children).Add((UIElement) andAddItem);
      if (!flag)
        ((Control) andAddItem).ApplyTemplate();
      return andAddItem;
    }

    private enum State
    {
      Normal,
      Expanded,
      Dragging,
      Snapping,
      Flicking,
    }
  }
}
