// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Panorama
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Gestures;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.Phone.Controls
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (PanoramaItem))]
  [TemplatePart(Name = "TitleLayer", Type = typeof (PanningLayer))]
  [TemplatePart(Name = "BackgroundLayer", Type = typeof (PanningLayer))]
  [TemplatePart(Name = "ItemsLayer", Type = typeof (PanningLayer))]
  public class Panorama : TemplatedItemsControl<PanoramaItem>
  {
    internal const int Spacing = 48;
    internal const double PanningOpacity = 0.7;
    private const string BackgroundLayerElement = "BackgroundLayer";
    private const string TitleLayerElement = "TitleLayer";
    private const string ItemsLayerElement = "ItemsLayer";
    internal static readonly Duration Immediately = new Duration(TimeSpan.Zero);
    private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(800.0));
    private static readonly Duration EntranceDuration = Panorama.DefaultDuration;
    private static readonly Duration FlickDuration = Panorama.DefaultDuration;
    private static readonly Duration SnapDuration = Panorama.DefaultDuration;
    private static readonly Duration PanDuration = new Duration(TimeSpan.FromMilliseconds(150.0));
    private int _cumulativeDragDelta;
    private int _flickDirection;
    private int _targetOffset;
    private bool _dragged;
    private int _frameCount;
    private bool _updateBackgroundPending = true;
    private bool _entranceAnimationPlayed;
    private PanningLayer _panningBackground;
    private PanningLayer _panningTitle;
    private PanningLayer _panningItems;
    private bool _adjustSelectedRequested;
    private bool _suppressSelectionChangedEvent;
    private bool _loaded;
    private float previousBackgroundWidth;
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (object), typeof (Panorama), (PropertyMetadata) null);
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof (TitleTemplate), typeof (DataTemplate), typeof (Panorama), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (Panorama), (PropertyMetadata) null);
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (Panorama), new PropertyMetadata((object) null, new PropertyChangedCallback(Panorama.OnSelectionChanged)));
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (Panorama), new PropertyMetadata((object) -1));
    public static readonly DependencyProperty DefaultItemProperty = DependencyProperty.Register(nameof (DefaultItem), typeof (object), typeof (Panorama), new PropertyMetadata((object) null, new PropertyChangedCallback(Panorama.OnDefaultItemChanged)));
    private static readonly DependencyProperty BackgroundShadowProperty = DependencyProperty.Register("BackgroundShadow", typeof (Brush), typeof (Panorama), new PropertyMetadata((object) null, new PropertyChangedCallback(Panorama.OnBackgroundShadowChanged)));

    internal PanoramaPanel Panel { get; set; }

    internal int ItemsWidth { get; set; }

    internal int ViewportWidth { get; private set; }

    internal int ViewportHeight { get; private set; }

    internal int AdjustedViewportWidth => Math.Max(0, this.ViewportWidth - 48);

    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

    public object Title
    {
      get => ((DependencyObject) this).GetValue(Panorama.TitleProperty);
      set => ((DependencyObject) this).SetValue(Panorama.TitleProperty, value);
    }

    public DataTemplate TitleTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(Panorama.TitleTemplateProperty);
      set => ((DependencyObject) this).SetValue(Panorama.TitleTemplateProperty, (object) value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(Panorama.HeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(Panorama.HeaderTemplateProperty, (object) value);
    }

    public object SelectedItem
    {
      get => ((DependencyObject) this).GetValue(Panorama.SelectedItemProperty);
      private set => ((DependencyObject) this).SetValue(Panorama.SelectedItemProperty, value);
    }

    public int SelectedIndex
    {
      get => (int) ((DependencyObject) this).GetValue(Panorama.SelectedIndexProperty);
      private set => ((DependencyObject) this).SetValue(Panorama.SelectedIndexProperty, (object) value);
    }

    public object DefaultItem
    {
      get => ((DependencyObject) this).GetValue(Panorama.DefaultItemProperty);
      set
      {
        ((DependencyObject) this).SetValue(Panorama.DefaultItemProperty, value);
        this.OnDefaultItemSet();
      }
    }

    internal PanoramaItem GetDefaultItemContainer() => this.GetContainer(this.DefaultItem);

    public Panorama()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.Panorama);
      ((Control) this).DefaultStyleKey = (object) typeof (Panorama);
      GestureHelper gestureHelper = GestureHelper.Create((UIElement) this, true);
      gestureHelper.GestureStart += (EventHandler<GestureEventArgs>) ((sender, args) => this.GestureStart(args));
      gestureHelper.HorizontalDrag += (EventHandler<DragEventArgs>) ((sender, args) => this.HorizontalDrag(args));
      gestureHelper.Flick += (EventHandler<FlickEventArgs>) ((sender, args) => this.Flick(args));
      gestureHelper.GestureEnd += (EventHandler<EventArgs>) ((sender, args) => this.GestureEnd());
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      if (DesignerProperties.IsInDesignTool)
      {
        ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
        ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
      }
      else
        CompositionTarget.Rendering += new EventHandler(this.EntranceAnimationCallback);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.Panorama_Loaded);
    }

    private void Panorama_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.Panorama_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.Panorama);
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => this._loaded = true;

    private void OnUnloaded(object sender, RoutedEventArgs e) => this._loaded = false;

    public virtual void OnApplyTemplate()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.Panorama);
      ((FrameworkElement) this).OnApplyTemplate();
      this._panningBackground = ((Control) this).GetTemplateChild("BackgroundLayer") as PanningLayer;
      this._panningTitle = ((Control) this).GetTemplateChild("TitleLayer") as PanningLayer;
      this._panningItems = ((Control) this).GetTemplateChild("ItemsLayer") as PanningLayer;
      if (this._panningBackground != null)
        this._panningBackground.Owner = this;
      if (this._panningTitle != null)
        this._panningTitle.Owner = this;
      if (this._panningItems != null)
        this._panningItems.Owner = this;
      ((FrameworkElement) this).SetBinding(Panorama.BackgroundShadowProperty, new Binding("Background")
      {
        RelativeSource = new RelativeSource((RelativeSourceMode) 2)
      });
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_APPLYTEMPLATE, PerfLog.Panorama);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.Panorama);
      if (Application.Current.Host.Content.ActualWidth > 0.0)
      {
        this.ViewportWidth = !double.IsInfinity(availableSize.Width) ? (int) availableSize.Width : (int) Application.Current.Host.Content.ActualWidth;
        this.ViewportHeight = !double.IsInfinity(availableSize.Height) ? (int) availableSize.Height : (int) Application.Current.Host.Content.ActualHeight;
      }
      else
      {
        this.ViewportWidth = (int) Math.Min(availableSize.Width, 480.0);
        this.ViewportHeight = (int) Math.Min(availableSize.Height, 800.0);
      }
      ((FrameworkElement) this).MeasureOverride(new Size(double.PositiveInfinity, (double) this.ViewportHeight));
      if (double.IsInfinity(availableSize.Width))
        availableSize.Width = (double) this.ViewportWidth;
      if (double.IsInfinity(availableSize.Height))
        availableSize.Height = (double) this.ViewportHeight;
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.Panorama);
      return availableSize;
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (this.Panel != null)
        this.Panel.ResetItemPositions();
      this.RequestAdjustSelection();
    }

    internal void RequestAdjustSelection()
    {
      if (this._adjustSelectedRequested)
        return;
      ((FrameworkElement) this).LayoutUpdated += new EventHandler(this.LayoutUpdatedAdjustSelection);
      this._adjustSelectedRequested = true;
    }

    private void LayoutUpdatedAdjustSelection(object sender, EventArgs e)
    {
      this._adjustSelectedRequested = false;
      ((FrameworkElement) this).LayoutUpdated -= new EventHandler(this.LayoutUpdatedAdjustSelection);
      this.AdjustSelection();
    }

    private void AdjustSelection()
    {
      if (DesignerProperties.IsInDesignTool)
      {
        if (!this._loaded)
          return;
        this._targetOffset = 0;
        this.GoTo(this._targetOffset, Panorama.Immediately);
      }
      else
      {
        object selectedItem1 = this.SelectedItem;
        object selectedItem2 = (object) null;
        bool flag1 = false;
        bool flag2 = false;
        if (this.Panel != null && this.Panel.VisibleChildren.Count > 0)
        {
          if (selectedItem1 == null)
          {
            selectedItem2 = this.GetItem(this.Panel.VisibleChildren[0]);
          }
          else
          {
            PanoramaItem container = this.GetContainer(selectedItem1);
            flag2 = this._entranceAnimationPlayed;
            selectedItem2 = container == null || !this.Panel.VisibleChildren.Contains(container) ? this.GetItem(this.Panel.VisibleChildren[0]) : selectedItem1;
          }
        }
        else
        {
          this._targetOffset = 0;
          this.GoTo(this._targetOffset, Panorama.Immediately);
        }
        if (flag1)
          this.SelectedItem = selectedItem2;
        else
          this.SetSelectionInternal(selectedItem2);
        this.UpdateItemPositions();
        if (!flag2)
          return;
        PanoramaItem container1 = this.GetContainer(selectedItem2);
        if (container1 == null)
          return;
        this._targetOffset = -container1.StartPosition;
        this.GoTo(this._targetOffset, Panorama.Immediately);
      }
    }

    private void UpdateItemPositions()
    {
      bool flag = true;
      if (this.Panel == null)
        return;
      if (this.Panel.VisibleChildren.Count > 2 && this.SelectedItem != null)
      {
        PanoramaItem container = this.GetContainer(this.SelectedItem);
        if (container != null)
        {
          int num = this.Panel.VisibleChildren.IndexOf(container);
          if (num == this.Panel.VisibleChildren.Count - 1)
          {
            this.Panel.ShowFirstItemOnRight();
            flag = false;
          }
          else if (num == 0)
          {
            this.Panel.ShowLastItemOnLeft();
            flag = false;
          }
        }
      }
      if (!flag)
        return;
      this.Panel.ResetItemPositions();
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.Panorama);
      finalSize.Width = ((UIElement) this).DesiredSize.Width;
      ((FrameworkElement) this).ArrangeOverride(finalSize);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.Panorama);
      return finalSize;
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      if (!(element is PanoramaItem panoramaItem))
        return;
      if (panoramaItem.Content == null && panoramaItem != item)
        panoramaItem.Content = item;
      if (panoramaItem.HeaderTemplate == null && element.ReadLocalValue(PanoramaItem.HeaderTemplateProperty) == DependencyProperty.UnsetValue)
        panoramaItem.HeaderTemplate = this.HeaderTemplate;
      if (panoramaItem.Header != null || item is UIElement || ((DependencyObject) panoramaItem).ReadLocalValue(PanoramaItem.HeaderProperty) != DependencyProperty.UnsetValue)
        return;
      panoramaItem.Header = item;
    }

    private void GestureStart(GestureEventArgs args)
    {
      this._targetOffset = (int) this._panningItems.ActualOffset;
      this._flickDirection = 0;
      this._cumulativeDragDelta = 0;
      this._dragged = false;
    }

    private void HorizontalDrag(DragEventArgs args)
    {
      if (this._flickDirection != 0)
        return;
      this._cumulativeDragDelta = (int) args.CumulativeDistance.X;
      this._targetOffset += (int) args.DeltaDistance.X;
      if (Math.Abs(this._cumulativeDragDelta) > this.ViewportWidth)
        return;
      this._dragged = true;
      this.GoTo(this._targetOffset, Panorama.PanDuration);
    }

    private void Flick(FlickEventArgs e)
    {
      if (e.Angle == 180.0)
      {
        this._flickDirection = -1;
      }
      else
      {
        if (e.Angle != 0.0)
          return;
        this._flickDirection = 1;
      }
    }

    private void GestureEnd()
    {
      if (this._flickDirection == 0)
      {
        if (!this._dragged)
          return;
        int snapTo;
        PanoramaItem newSelection;
        bool wraparound;
        this.Panel.GetSnapOffset(this._targetOffset, this.ViewportWidth, Math.Sign(this._cumulativeDragDelta), out snapTo, out int _, out newSelection, out wraparound);
        if (wraparound)
          this.WrapAround(Math.Sign(this._cumulativeDragDelta));
        object obj = this.GetItem(newSelection);
        if (obj != null)
          this.SelectedItem = obj;
        this.UpdateItemPositions();
        this.GoTo(snapTo, Panorama.SnapDuration, (Action) (() => this._panningItems.Refresh()));
      }
      else
        this.ProcessFlick();
    }

    private void ProcessFlick()
    {
      if (this._flickDirection == 0)
        return;
      PanoramaPanel.ItemStop previous;
      PanoramaPanel.ItemStop current;
      PanoramaPanel.ItemStop next;
      this.Panel.GetStops((int) this._panningItems.ActualOffset, this.ItemsWidth, out previous, out current, out next);
      if (previous == current && current == next && next == null)
        return;
      this._targetOffset = this._flickDirection < 0 ? -next.Position : -previous.Position;
      if (Math.Sign((double) this._targetOffset - this._panningItems.ActualOffset) != Math.Sign(this._flickDirection))
        this.WrapAround(Math.Sign(this._flickDirection));
      this.SelectedItem = this.GetItem(this._flickDirection < 0 ? next.Item : previous.Item);
      this.UpdateItemPositions();
      this.GoTo(this._targetOffset, Panorama.FlickDuration, (Action) (() => this._panningItems.Refresh()));
    }

    private void GoTo(int offset, Duration duration, Action completionAction)
    {
      if (this._panningBackground != null)
        this._panningBackground.GoTo(offset, duration, (Action) null);
      if (this._panningTitle != null)
        this._panningTitle.GoTo(offset, duration, (Action) null);
      if (this._panningItems == null)
        return;
      this._panningItems.GoTo(offset, duration, completionAction);
    }

    private void GoTo(int offset) => this.GoTo(offset, (Action) null);

    private void GoTo(int offset, Action completionAction)
    {
      int num = Math.Abs((int) this._panningItems.ActualOffset - offset);
      this.GoTo(offset, Duration.op_Implicit(TimeSpan.FromMilliseconds((double) (num * 2))), completionAction);
    }

    private void GoTo(int offset, Duration duration) => this.GoTo(offset, duration, (Action) null);

    private void WrapAround(int direction)
    {
      this._panningBackground.Wraparound(direction);
      this._panningTitle.Wraparound(direction);
      this._panningItems.Wraparound(direction);
    }

    private void SetSelectionInternal(object selectedItem)
    {
      this._suppressSelectionChangedEvent = true;
      this.SelectedItem = selectedItem;
      this._suppressSelectionChangedEvent = false;
    }

    private static void OnSelectionChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      if (!(obj is Panorama sender))
        return;
      sender.SelectedIndex = ((PresentationFrameworkCollection<object>) sender.Items).IndexOf(args.NewValue);
      if (sender._suppressSelectionChangedEvent || !((PresentationFrameworkCollection<object>) sender.Items).Contains(args.NewValue))
        return;
      SafeRaise.Raise<SelectionChangedEventArgs>(sender.SelectionChanged, (object) sender, (SafeRaise.GetEventArgs<SelectionChangedEventArgs>) (() =>
      {
        object[] objArray1;
        if (args.OldValue != null)
          objArray1 = new object[1]{ args.OldValue };
        else
          objArray1 = new object[0];
        object[] objArray2;
        if (args.NewValue != null)
          objArray2 = new object[1]{ args.NewValue };
        else
          objArray2 = new object[0];
        return new SelectionChangedEventArgs((IList) objArray1, (IList) objArray2);
      }));
    }

    private static void OnDefaultItemChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      ((Panorama) obj).OnDefaultItemSet();
    }

    private void OnDefaultItemSet()
    {
      if (this.Panel == null)
        return;
      this.Panel.NotifyDefaultItemChanged();
      if (this.Panel.VisibleChildren.Count > 0)
        this.SelectedItem = this.DefaultItem;
      if (this.Panel != null)
        this.Panel.ResetItemPositions();
      this._panningItems.Refresh();
      this.UpdateItemPositions();
      this.GoTo(0, Panorama.Immediately);
    }

    private static void OnBackgroundShadowChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      Panorama panorama = (Panorama) obj;
      if (panorama._updateBackgroundPending)
        return;
      panorama.UpdateBackground();
    }

    private void UpdateBackground()
    {
      this._updateBackgroundPending = false;
      ((FrameworkElement) this._panningBackground.ContentPresenter).Height = (double) this.ViewportHeight;
      if (((Control) this).Background is SolidColorBrush)
      {
        ((FrameworkElement) this._panningBackground.ContentPresenter).Width = (double) this.ViewportWidth;
        this._panningBackground.IsStatic = true;
      }
      else if (((Control) this).Background is GradientBrush)
      {
        ((FrameworkElement) this._panningBackground.ContentPresenter).Width = (double) Math.Max(this.ItemsWidth, this.ViewportWidth);
        this._panningBackground.IsStatic = ((FrameworkElement) this._panningBackground.ContentPresenter).Width == (double) this.ViewportWidth;
      }
      else if (((Control) this).Background is ImageBrush)
      {
        BitmapImage bmp = ((ImageBrush) ((Control) this).Background).ImageSource as BitmapImage;
        if (this._panningBackground.ContentPresenter != null && bmp != null)
        {
          if (((BitmapSource) bmp).PixelWidth == 0)
          {
            bmp.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
            bmp.ImageOpened += new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
            ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() => this.AsyncUpdateBackground(bmp)));
          }
          ((FrameworkElement) this._panningBackground.ContentPresenter).Width = (double) ((BitmapSource) bmp).PixelWidth;
          this.previousBackgroundWidth = (float) ((BitmapSource) bmp).PixelWidth;
        }
        this._panningBackground.IsStatic = false;
      }
      this._panningBackground.Refresh();
    }

    private void OnBackgroundImageOpened(object sender, RoutedEventArgs e) => this.AsyncUpdateBackground((BitmapImage) sender);

    private void AsyncUpdateBackground(BitmapImage img)
    {
      img.ImageOpened -= new EventHandler<RoutedEventArgs>(this.OnBackgroundImageOpened);
      this.UpdateBackground();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.ViewportWidth = (int) e.NewSize.Width;
      this.ViewportHeight = (int) e.NewSize.Height;
      this.ItemsWidth = (int) ((FrameworkElement) this.Panel).ActualWidth;
      this.UpdateBackground();
      ((UIElement) this).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(0.0, 0.0, e.NewSize.Width, e.NewSize.Height)
      };
    }

    private void EntranceAnimationCallback(object sender, EventArgs e)
    {
      switch (this._frameCount++)
      {
        case 0:
          this.GoTo(this.ViewportWidth, Panorama.Immediately);
          break;
        case 1:
          this.GoTo(0, Panorama.EntranceDuration);
          this._entranceAnimationPlayed = true;
          CompositionTarget.Rendering -= new EventHandler(this.EntranceAnimationCallback);
          break;
      }
    }
  }
}
