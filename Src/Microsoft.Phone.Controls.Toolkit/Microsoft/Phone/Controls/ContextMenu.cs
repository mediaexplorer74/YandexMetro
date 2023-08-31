// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ContextMenu
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "OpenReversed")]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Open")]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "Closed")]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "OpenLandscape")]
  [TemplateVisualState(GroupName = "VisibilityStates", Name = "OpenLandscapeReversed")]
  public class ContextMenu : MenuBase
  {
    private const double LandscapeWidth = 480.0;
    private const double SystemTrayLandscapeWidth = 72.0;
    private const double ApplicationBarLandscapeWidth = 72.0;
    private const double TotalBorderWidth = 8.0;
    private const string VisibilityGroupName = "VisibilityStates";
    private const string OpenVisibilityStateName = "Open";
    private const string OpenReversedVisibilityStateName = "OpenReversed";
    private const string ClosedVisibilityStateName = "Closed";
    private const string OpenLandscapeVisibilityStateName = "OpenLandscape";
    private const string OpenLandscapeReversedVisibilityStateName = "OpenLandscapeReversed";
    private StackPanel _outerPanel;
    private Grid _innerGrid;
    private PhoneApplicationPage _page;
    private readonly List<ApplicationBarIconButton> _applicationBarIconButtons = new List<ApplicationBarIconButton>();
    private Storyboard _backgroundResizeStoryboard;
    private List<Storyboard> _openingStoryboard;
    private bool _openingStoryboardPlaying;
    private DateTime _openingStoryboardReleaseThreshold;
    private PhoneApplicationFrame _rootVisual;
    private Point _mousePosition;
    private DependencyObject _owner;
    private Popup _popup;
    private Panel _overlay;
    private Point _popupAlignmentPoint;
    private bool _settingIsOpen;
    private bool _reversed;
    public static readonly DependencyProperty IsZoomEnabledProperty = DependencyProperty.Register(nameof (IsZoomEnabled), typeof (bool), typeof (ContextMenu), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsFadeEnabledProperty = DependencyProperty.Register(nameof (IsFadeEnabled), typeof (bool), typeof (ContextMenu), new PropertyMetadata((object) true));
    public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof (VerticalOffset), typeof (double), typeof (ContextMenu), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ContextMenu.OnVerticalOffsetChanged)));
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (ContextMenu), new PropertyMetadata((object) false, new PropertyChangedCallback(ContextMenu.OnIsOpenChanged)));
    public static readonly DependencyProperty RegionOfInterestProperty = DependencyProperty.Register(nameof (RegionOfInterest), typeof (Rect?), typeof (ContextMenu), (PropertyMetadata) null);
    private static readonly DependencyProperty ApplicationBarMirrorProperty = DependencyProperty.Register("ApplicationBarMirror", typeof (IApplicationBar), typeof (ContextMenu), new PropertyMetadata(new PropertyChangedCallback(ContextMenu.OnApplicationBarMirrorChanged)));

    public DependencyObject Owner
    {
      get => this._owner;
      internal set
      {
        if (this._owner != null && this._owner is FrameworkElement owner1)
        {
          ((UIElement) owner1).Hold -= new EventHandler<GestureEventArgs>(this.OnOwnerHold);
          owner1.Loaded -= new RoutedEventHandler(this.OnOwnerLoaded);
          owner1.Unloaded -= new RoutedEventHandler(this.OnOwnerUnloaded);
          this.OnOwnerUnloaded((object) null, (RoutedEventArgs) null);
        }
        this._owner = value;
        if (this._owner == null || !(this._owner is FrameworkElement owner2))
          return;
        ((UIElement) owner2).Hold += new EventHandler<GestureEventArgs>(this.OnOwnerHold);
        owner2.Loaded += new RoutedEventHandler(this.OnOwnerLoaded);
        owner2.Unloaded += new RoutedEventHandler(this.OnOwnerUnloaded);
        DependencyObject dependencyObject = (DependencyObject) owner2;
        while (dependencyObject != null)
        {
          dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
          if (dependencyObject != null && dependencyObject == this._rootVisual)
          {
            this.OnOwnerLoaded((object) null, (RoutedEventArgs) null);
            break;
          }
        }
      }
    }

    public bool IsZoomEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(ContextMenu.IsZoomEnabledProperty);
      set => ((DependencyObject) this).SetValue(ContextMenu.IsZoomEnabledProperty, (object) value);
    }

    public bool IsFadeEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(ContextMenu.IsFadeEnabledProperty);
      set => ((DependencyObject) this).SetValue(ContextMenu.IsFadeEnabledProperty, (object) value);
    }

    [TypeConverter(typeof (LengthConverter))]
    public double VerticalOffset
    {
      get => (double) ((DependencyObject) this).GetValue(ContextMenu.VerticalOffsetProperty);
      set => ((DependencyObject) this).SetValue(ContextMenu.VerticalOffsetProperty, (object) value);
    }

    private static void OnVerticalOffsetChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ContextMenu) o).UpdateContextMenuPlacement();
    }

    public bool IsOpen
    {
      get => (bool) ((DependencyObject) this).GetValue(ContextMenu.IsOpenProperty);
      set => ((DependencyObject) this).SetValue(ContextMenu.IsOpenProperty, (object) value);
    }

    private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((ContextMenu) o).OnIsOpenChanged((bool) e.NewValue);

    private void OnIsOpenChanged(bool newValue)
    {
      if (this._settingIsOpen)
        return;
      if (newValue)
        this.OpenPopup(this._mousePosition);
      else
        this.ClosePopup();
    }

    public Rect? RegionOfInterest
    {
      get => (Rect?) ((DependencyObject) this).GetValue(ContextMenu.RegionOfInterestProperty);
      set => ((DependencyObject) this).SetValue(ContextMenu.RegionOfInterestProperty, (object) value);
    }

    public event RoutedEventHandler Opened;

    protected virtual void OnOpened(RoutedEventArgs e)
    {
      this.UpdateContextMenuPlacement();
      this.SetRenderTransform();
      this.UpdateVisualStates(true);
      RoutedEventHandler opened = this.Opened;
      if (opened == null)
        return;
      opened((object) this, e);
    }

    private void SetRenderTransform()
    {
      if (DesignerProperties.IsInDesignTool || this._rootVisual.Orientation.IsPortrait())
      {
        double num = this._popupAlignmentPoint.X / ((FrameworkElement) this).Width;
        if (this._outerPanel != null)
          ((UIElement) this._outerPanel).RenderTransformOrigin = new Point(num, 0.0);
        if (this._innerGrid == null)
          return;
        ((UIElement) this._innerGrid).RenderTransformOrigin = new Point(0.0, this._reversed ? 1.0 : 0.0);
      }
      else
      {
        if (this._outerPanel != null)
          ((UIElement) this._outerPanel).RenderTransformOrigin = new Point(0.0, 0.5);
        if (this._innerGrid == null)
          return;
        ((UIElement) this._innerGrid).RenderTransformOrigin = new Point(this._reversed ? 1.0 : 0.0, 0.0);
      }
    }

    public event RoutedEventHandler Closed;

    protected virtual void OnClosed(RoutedEventArgs e)
    {
      this.UpdateVisualStates(true);
      RoutedEventHandler closed = this.Closed;
      if (closed == null)
        return;
      closed((object) this, e);
    }

    public ContextMenu()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (ContextMenu);
      this._openingStoryboard = new List<Storyboard>();
      ((FrameworkElement) this).LayoutUpdated += new EventHandler(this.OnLayoutUpdated);
    }

    public virtual void OnApplyTemplate()
    {
      if (this._openingStoryboard != null)
      {
        foreach (Timeline timeline in this._openingStoryboard)
          timeline.Completed -= new EventHandler(this.OnStoryboardCompleted);
        this._openingStoryboard.Clear();
      }
      this._openingStoryboardPlaying = false;
      ((FrameworkElement) this).OnApplyTemplate();
      this.SetDefaultStyle();
      if (VisualTreeHelper.GetChild((DependencyObject) this, 0) is FrameworkElement child)
      {
        foreach (VisualStateGroup visualStateGroup in (IEnumerable) VisualStateManager.GetVisualStateGroups(child))
        {
          if ("VisibilityStates" == visualStateGroup.Name)
          {
            foreach (VisualState state in (IEnumerable) visualStateGroup.States)
            {
              if (("Open" == state.Name || "OpenLandscape" == state.Name || "OpenReversed" == state.Name || "OpenLandscapeReversed" == state.Name) && state.Storyboard != null)
              {
                this._openingStoryboard.Add(state.Storyboard);
                ((Timeline) state.Storyboard).Completed += new EventHandler(this.OnStoryboardCompleted);
              }
            }
          }
        }
      }
      this._outerPanel = ((Control) this).GetTemplateChild("OuterPanel") as StackPanel;
      this._innerGrid = ((Control) this).GetTemplateChild("InnerGrid") as Grid;
      bool flag = DesignerProperties.IsInDesignTool || this._rootVisual.Orientation.IsPortrait();
      this.SetRenderTransform();
      if (!this.IsOpen)
        return;
      if (this._innerGrid != null)
        ((FrameworkElement) this._innerGrid).MinHeight = flag ? 0.0 : ((FrameworkElement) this._rootVisual).ActualWidth;
      this.UpdateVisualStates(true);
    }

    private void SetDefaultStyle()
    {
      SolidColorBrush solidColorBrush1;
      SolidColorBrush solidColorBrush2;
      if (DesignerProperties.IsInDesignTool || ((FrameworkElement) this).Resources.IsDarkThemeActive())
      {
        solidColorBrush1 = new SolidColorBrush(Colors.White);
        solidColorBrush2 = new SolidColorBrush(Colors.Black);
      }
      else
      {
        solidColorBrush1 = new SolidColorBrush(Colors.Black);
        solidColorBrush2 = new SolidColorBrush(Colors.White);
      }
      Style style = new Style(typeof (ContextMenu));
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) solidColorBrush1);
      Setter setter2 = new Setter(Control.BorderBrushProperty, (object) solidColorBrush2);
      if (((FrameworkElement) this).Style == null)
      {
        ((PresentationFrameworkCollection<SetterBase>) style.Setters).Add((SetterBase) setter1);
        ((PresentationFrameworkCollection<SetterBase>) style.Setters).Add((SetterBase) setter2);
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (Setter setter3 in (PresentationFrameworkCollection<SetterBase>) ((FrameworkElement) this).Style.Setters)
        {
          if (setter3.Property == Control.BackgroundProperty)
            flag1 = true;
          else if (setter3.Property == Control.BorderBrushProperty)
            flag2 = true;
          ((PresentationFrameworkCollection<SetterBase>) style.Setters).Add((SetterBase) new Setter(setter3.Property, setter3.Value));
        }
        if (!flag1)
          ((PresentationFrameworkCollection<SetterBase>) style.Setters).Add((SetterBase) setter1);
        if (!flag2)
          ((PresentationFrameworkCollection<SetterBase>) style.Setters).Add((SetterBase) setter2);
      }
      ((FrameworkElement) this).Style = style;
    }

    private void OnStoryboardCompleted(object sender, EventArgs e) => this._openingStoryboardPlaying = false;

    private void UpdateVisualStates(bool useTransitions)
    {
      string str;
      if (this.IsOpen)
      {
        if (this._openingStoryboard != null)
        {
          this._openingStoryboardPlaying = true;
          this._openingStoryboardReleaseThreshold = DateTime.UtcNow.AddSeconds(0.3);
        }
        if (this._rootVisual != null && this._rootVisual.Orientation.IsPortrait())
        {
          if (this._outerPanel != null)
            this._outerPanel.Orientation = (Orientation) 0;
          str = this._reversed ? "OpenReversed" : "Open";
        }
        else
        {
          if (this._outerPanel != null)
            this._outerPanel.Orientation = (Orientation) 1;
          str = this._reversed ? "OpenLandscapeReversed" : "OpenLandscape";
        }
        if (this._backgroundResizeStoryboard != null)
          this._backgroundResizeStoryboard.Begin();
      }
      else
        str = "Closed";
      VisualStateManager.GoToState((Control) this, str, useTransitions);
    }

    private bool PositionIsOnScreenRight(double position) => 18 != this._rootVisual.Orientation ? position < ((FrameworkElement) this._rootVisual).ActualHeight / 2.0 : position > ((FrameworkElement) this._rootVisual).ActualHeight / 2.0;

    protected virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      e.Handled = true;
      ((Control) this).OnMouseLeftButtonDown(e);
    }

    protected virtual void OnKeyDown(KeyEventArgs e)
    {
      Key key = e != null ? e.Key : throw new ArgumentNullException(nameof (e));
      if (key != 8)
      {
        switch (key - 15)
        {
          case 0:
            this.FocusNextItem(false);
            e.Handled = true;
            break;
          case 2:
            this.FocusNextItem(true);
            e.Handled = true;
            break;
        }
      }
      else
      {
        this.ClosePopup();
        e.Handled = true;
      }
      ((Control) this).OnKeyDown(e);
    }

    private void OnLayoutUpdated(object sender, EventArgs e)
    {
      if (Application.Current.RootVisual == null)
        return;
      this.InitializeRootVisual();
      ((FrameworkElement) this).LayoutUpdated -= new EventHandler(this.OnLayoutUpdated);
    }

    private void OnRootVisualMouseMove(object sender, MouseEventArgs e) => this._mousePosition = e.GetPosition((UIElement) null);

    private void OnRootVisualManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (!this._openingStoryboardPlaying || !(DateTime.UtcNow <= this._openingStoryboardReleaseThreshold))
        return;
      this.IsOpen = false;
    }

    private void OnOwnerHold(object sender, GestureEventArgs e)
    {
      if (this.IsOpen)
        return;
      this.OpenPopup(e.GetPosition((UIElement) null));
      e.Handled = true;
    }

    private static void OnApplicationBarMirrorChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ContextMenu) o).OnApplicationBarMirrorChanged((IApplicationBar) e.OldValue, (IApplicationBar) e.NewValue);
    }

    private void OnApplicationBarMirrorChanged(IApplicationBar oldValue, IApplicationBar newValue)
    {
      if (oldValue != null)
        oldValue.StateChanged -= new EventHandler<ApplicationBarStateChangedEventArgs>(this.OnEventThatClosesContextMenu);
      if (newValue == null)
        return;
      newValue.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.OnEventThatClosesContextMenu);
    }

    private void OnEventThatClosesContextMenu(object sender, EventArgs e) => this.IsOpen = false;

    private void OnOwnerLoaded(object sender, RoutedEventArgs e)
    {
      if (this._page != null)
        return;
      this.InitializeRootVisual();
      if (this._rootVisual == null)
        return;
      this._page = ((ContentControl) this._rootVisual).Content as PhoneApplicationPage;
      if (this._page == null)
        return;
      this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.OnPageBackKeyPress);
      ((FrameworkElement) this).SetBinding(ContextMenu.ApplicationBarMirrorProperty, new Binding()
      {
        Source = (object) this._page,
        Path = new PropertyPath("ApplicationBar", new object[0])
      });
    }

    private void OnOwnerUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._rootVisual != null)
      {
        ((UIElement) this._rootVisual).MouseMove -= new MouseEventHandler(this.OnRootVisualMouseMove);
        ((UIElement) this._rootVisual).ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.OnRootVisualManipulationCompleted);
        this._rootVisual.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.OnEventThatClosesContextMenu);
      }
      if (this._page == null)
        return;
      this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.OnPageBackKeyPress);
      ((DependencyObject) this).ClearValue(ContextMenu.ApplicationBarMirrorProperty);
      this._page = (PhoneApplicationPage) null;
    }

    private void OnPageBackKeyPress(object sender, CancelEventArgs e)
    {
      if (!this.IsOpen)
        return;
      this.IsOpen = false;
      e.Cancel = true;
    }

    private static GeneralTransform SafeTransformToVisual(UIElement element, UIElement visual)
    {
      try
      {
        return element.TransformToVisual(visual);
      }
      catch (ArgumentException ex)
      {
        return (GeneralTransform) new TranslateTransform();
      }
    }

    private void InitializeRootVisual()
    {
      if (this._rootVisual != null)
        return;
      this._rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
      if (this._rootVisual == null)
        return;
      ((UIElement) this._rootVisual).MouseMove -= new MouseEventHandler(this.OnRootVisualMouseMove);
      ((UIElement) this._rootVisual).MouseMove += new MouseEventHandler(this.OnRootVisualMouseMove);
      ((UIElement) this._rootVisual).ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(this.OnRootVisualManipulationCompleted);
      ((UIElement) this._rootVisual).ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.OnRootVisualManipulationCompleted);
      this._rootVisual.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.OnEventThatClosesContextMenu);
      this._rootVisual.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.OnEventThatClosesContextMenu);
    }

    private void FocusNextItem(bool down)
    {
      int count = ((PresentationFrameworkCollection<object>) this.Items).Count;
      int num1 = down ? -1 : count;
      if (FocusManager.GetFocusedElement() is MenuItem focusedElement && this == focusedElement.ParentMenuBase)
        num1 = this.ItemContainerGenerator.IndexFromContainer((DependencyObject) focusedElement);
      int num2 = num1;
      do
      {
        num2 = (num2 + count + (down ? 1 : -1)) % count;
      }
      while ((!(this.ItemContainerGenerator.ContainerFromIndex(num2) is MenuItem menuItem) || !((Control) menuItem).IsEnabled || !((Control) menuItem).Focus()) && num2 != num1);
    }

    internal void ChildMenuItemClicked() => this.ClosePopup();

    private void OnContextMenuOrRootVisualSizeChanged(object sender, SizeChangedEventArgs e) => this.UpdateContextMenuPlacement();

    private void OnOverlayMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (!(VisualTreeHelper.FindElementsInHostCoordinates(((MouseEventArgs) e).GetPosition((UIElement) null), (UIElement) this._rootVisual) as List<UIElement>).Contains((UIElement) this))
        this.ClosePopup();
      e.Handled = true;
    }

    private void UpdateContextMenuPlacement()
    {
      if (this._rootVisual == null || this._overlay == null)
        return;
      Point point = new Point(this._popupAlignmentPoint.X, this._popupAlignmentPoint.Y);
      bool flag = this._rootVisual.Orientation.IsPortrait();
      double num1 = flag ? ((FrameworkElement) this._rootVisual).ActualWidth : ((FrameworkElement) this._rootVisual).ActualHeight;
      double num2 = flag ? ((FrameworkElement) this._rootVisual).ActualHeight : ((FrameworkElement) this._rootVisual).ActualWidth;
      Rect rect = new Rect(0.0, 0.0, num1, num2);
      if (this._page != null)
        rect = ContextMenu.SafeTransformToVisual((UIElement) this._page, (UIElement) this._rootVisual).TransformBounds(new Rect(0.0, 0.0, ((FrameworkElement) this._page).ActualWidth, ((FrameworkElement) this._page).ActualHeight));
      if (flag && this._rootVisual != null)
      {
        double y;
        double num3;
        if (this.RegionOfInterest.HasValue)
        {
          y = this.RegionOfInterest.Value.Y;
          num3 = this.RegionOfInterest.Value.Height;
        }
        else if (this.Owner is FrameworkElement)
        {
          FrameworkElement owner = (FrameworkElement) this.Owner;
          y = ((UIElement) owner).TransformToVisual((UIElement) this._rootVisual).Transform(new Point(0.0, 0.0)).Y;
          num3 = owner.ActualHeight;
        }
        else
        {
          y = this._popupAlignmentPoint.Y;
          num3 = 0.0;
        }
        point.Y = y + num3;
        this._reversed = false;
        if (point.Y > rect.Bottom - ((FrameworkElement) this).ActualHeight)
        {
          point.Y = y - ((FrameworkElement) this).ActualHeight;
          this._reversed = true;
          if (point.Y < rect.Top)
          {
            point = this._popupAlignmentPoint;
            this._reversed = false;
            if (point.Y > rect.Bottom - ((FrameworkElement) this).ActualHeight)
            {
              this._reversed = true;
              if (point.Y < rect.Top)
              {
                point.Y = rect.Bottom - ((FrameworkElement) this).ActualHeight;
                this._reversed = true;
              }
            }
          }
        }
      }
      double x = point.X;
      double position = point.Y + this.VerticalOffset;
      double val1;
      if (flag)
      {
        val1 = rect.Left;
        ((FrameworkElement) this).Width = rect.Width;
        if (this._innerGrid != null)
          ((FrameworkElement) this._innerGrid).Width = ((FrameworkElement) this).Width;
      }
      else
      {
        if (this.PositionIsOnScreenRight(position))
        {
          ((FrameworkElement) this).Width = SystemTray.IsVisible ? 408.0 : 480.0;
          val1 = SystemTray.IsVisible ? 72.0 : 0.0;
          this._reversed = true;
        }
        else
        {
          ((FrameworkElement) this).Width = this._page.ApplicationBar == null || !this._page.ApplicationBar.IsVisible ? 480.0 : 408.0;
          val1 = rect.Width - ((FrameworkElement) this).Width + (SystemTray.IsVisible ? 72.0 : 0.0);
          this._reversed = false;
        }
        if (this._innerGrid != null)
          ((FrameworkElement) this._innerGrid).Width = ((FrameworkElement) this).Width - 8.0;
        position = 0.0;
      }
      Canvas.SetLeft((UIElement) this, Math.Max(val1, 0.0));
      Canvas.SetTop((UIElement) this, position);
      ((FrameworkElement) this._overlay).Width = num1;
      ((FrameworkElement) this._overlay).Height = num2;
    }

    private void OpenPopup(Point position)
    {
      this._popupAlignmentPoint = position;
      this.InitializeRootVisual();
      bool flag = this._rootVisual.Orientation.IsPortrait();
      if (flag)
      {
        if (this._innerGrid != null)
          ((FrameworkElement) this._innerGrid).MinHeight = 0.0;
      }
      else if (this._innerGrid != null)
        ((FrameworkElement) this._innerGrid).MinHeight = ((FrameworkElement) this._rootVisual).ActualWidth;
      Canvas canvas = new Canvas();
      ((Panel) canvas).Background = (Brush) new SolidColorBrush(Colors.Transparent);
      this._overlay = (Panel) canvas;
      ((UIElement) this._overlay).MouseLeftButtonUp += new MouseButtonEventHandler(this.OnOverlayMouseButtonUp);
      if (this.IsZoomEnabled && this._rootVisual != null)
      {
        double num1 = flag ? ((FrameworkElement) this._rootVisual).ActualWidth : ((FrameworkElement) this._rootVisual).ActualHeight;
        double num2 = flag ? ((FrameworkElement) this._rootVisual).ActualHeight : ((FrameworkElement) this._rootVisual).ActualWidth;
        Rectangle rectangle1 = new Rectangle();
        ((FrameworkElement) rectangle1).Width = num1;
        ((FrameworkElement) rectangle1).Height = num2;
        ((Shape) rectangle1).Fill = (Brush) Application.Current.Resources[(object) "PhoneBackgroundBrush"];
        ((UIElement) rectangle1).CacheMode = (CacheMode) new BitmapCache();
        ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Insert(0, (UIElement) rectangle1);
        if (this._owner is FrameworkElement owner)
          ((UIElement) owner).Opacity = 0.0;
        WriteableBitmap writeableBitmap = new WriteableBitmap((int) num1, (int) num2);
        writeableBitmap.Render((UIElement) this._rootVisual, (Transform) null);
        writeableBitmap.Invalidate();
        Transform transform = (Transform) new ScaleTransform()
        {
          CenterX = (num1 / 2.0),
          CenterY = (num2 / 2.0)
        };
        Image image = new Image();
        image.Source = (ImageSource) writeableBitmap;
        ((UIElement) image).RenderTransform = transform;
        ((UIElement) image).CacheMode = (CacheMode) new BitmapCache();
        ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Insert(1, (UIElement) image);
        Rectangle rectangle2 = new Rectangle();
        ((FrameworkElement) rectangle2).Width = num1;
        ((FrameworkElement) rectangle2).Height = num2;
        ((Shape) rectangle2).Fill = (Brush) Application.Current.Resources[(object) "PhoneBackgroundBrush"];
        ((UIElement) rectangle2).Opacity = 0.0;
        ((UIElement) rectangle2).CacheMode = (CacheMode) new BitmapCache();
        UIElement uiElement1 = (UIElement) rectangle2;
        ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Insert(2, uiElement1);
        if (owner != null)
        {
          ((UIElement) this.Owner).Opacity = 1.0;
          Point point = ContextMenu.SafeTransformToVisual((UIElement) owner, (UIElement) this._rootVisual).Transform(new Point());
          Rectangle rectangle3 = new Rectangle();
          ((FrameworkElement) rectangle3).Width = owner.ActualWidth;
          ((FrameworkElement) rectangle3).Height = owner.ActualHeight;
          ((Shape) rectangle3).Fill = (Brush) new SolidColorBrush(Colors.Transparent);
          ((UIElement) rectangle3).CacheMode = (CacheMode) new BitmapCache();
          UIElement uiElement2 = (UIElement) rectangle3;
          Canvas.SetLeft(uiElement2, point.X);
          Canvas.SetTop(uiElement2, point.Y);
          ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Insert(3, uiElement2);
          UIElement uiElement3 = (UIElement) new Image()
          {
            Source = (ImageSource) new WriteableBitmap((UIElement) owner, (Transform) null)
          };
          Canvas.SetLeft(uiElement3, point.X);
          Canvas.SetTop(uiElement3, point.Y);
          ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Insert(4, uiElement3);
        }
        double num3 = 1.0;
        double num4 = 0.94;
        TimeSpan timeSpan = TimeSpan.FromSeconds(0.42);
        ExponentialEase exponentialEase = new ExponentialEase();
        ((EasingFunctionBase) exponentialEase).EasingMode = (EasingMode) 2;
        IEasingFunction ieasingFunction = (IEasingFunction) exponentialEase;
        this._backgroundResizeStoryboard = new Storyboard();
        DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        doubleAnimation1.From = new double?(num3);
        doubleAnimation1.To = new double?(num4);
        ((Timeline) doubleAnimation1).Duration = Duration.op_Implicit(timeSpan);
        doubleAnimation1.EasingFunction = ieasingFunction;
        DoubleAnimation doubleAnimation2 = doubleAnimation1;
        Storyboard.SetTarget((Timeline) doubleAnimation2, (DependencyObject) transform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation2, new PropertyPath((object) ScaleTransform.ScaleXProperty));
        ((PresentationFrameworkCollection<Timeline>) this._backgroundResizeStoryboard.Children).Add((Timeline) doubleAnimation2);
        DoubleAnimation doubleAnimation3 = new DoubleAnimation();
        doubleAnimation3.From = new double?(num3);
        doubleAnimation3.To = new double?(num4);
        ((Timeline) doubleAnimation3).Duration = Duration.op_Implicit(timeSpan);
        doubleAnimation3.EasingFunction = ieasingFunction;
        DoubleAnimation doubleAnimation4 = doubleAnimation3;
        Storyboard.SetTarget((Timeline) doubleAnimation4, (DependencyObject) transform);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation4, new PropertyPath((object) ScaleTransform.ScaleYProperty));
        ((PresentationFrameworkCollection<Timeline>) this._backgroundResizeStoryboard.Children).Add((Timeline) doubleAnimation4);
        if (this.IsFadeEnabled)
        {
          DoubleAnimation doubleAnimation5 = new DoubleAnimation();
          doubleAnimation5.From = new double?(0.0);
          doubleAnimation5.To = new double?(0.3);
          ((Timeline) doubleAnimation5).Duration = Duration.op_Implicit(timeSpan);
          doubleAnimation5.EasingFunction = ieasingFunction;
          DoubleAnimation doubleAnimation6 = doubleAnimation5;
          Storyboard.SetTarget((Timeline) doubleAnimation6, (DependencyObject) uiElement1);
          Storyboard.SetTargetProperty((Timeline) doubleAnimation6, new PropertyPath((object) UIElement.OpacityProperty));
          ((PresentationFrameworkCollection<Timeline>) this._backgroundResizeStoryboard.Children).Add((Timeline) doubleAnimation6);
        }
      }
      TransformGroup transformGroup = new TransformGroup();
      if (this._rootVisual != null)
      {
        PageOrientation orientation = this._rootVisual.Orientation;
        if (orientation != 18)
        {
          if (orientation == 34)
          {
            ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add((Transform) new RotateTransform()
            {
              Angle = -90.0
            });
            ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add((Transform) new TranslateTransform()
            {
              Y = ((FrameworkElement) this._rootVisual).ActualHeight
            });
          }
        }
        else
        {
          ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add((Transform) new RotateTransform()
          {
            Angle = 90.0
          });
          ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add((Transform) new TranslateTransform()
          {
            X = ((FrameworkElement) this._rootVisual).ActualWidth
          });
        }
      }
      ((UIElement) this._overlay).RenderTransform = (Transform) transformGroup;
      if (this._page != null && this._page.ApplicationBar != null && this._page.ApplicationBar.Buttons != null)
      {
        foreach (object button in (IEnumerable) this._page.ApplicationBar.Buttons)
        {
          if (button is ApplicationBarIconButton applicationBarIconButton)
          {
            applicationBarIconButton.Click += new EventHandler(this.OnEventThatClosesContextMenu);
            this._applicationBarIconButtons.Add(applicationBarIconButton);
          }
        }
      }
      ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Add((UIElement) this);
      this._popup = new Popup()
      {
        Child = (UIElement) this._overlay
      };
      this._popup.Opened += (EventHandler) ((s, e) => this.OnOpened(new RoutedEventArgs()));
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.OnContextMenuOrRootVisualSizeChanged);
      if (this._rootVisual != null)
        ((FrameworkElement) this._rootVisual).SizeChanged += new SizeChangedEventHandler(this.OnContextMenuOrRootVisualSizeChanged);
      this.UpdateContextMenuPlacement();
      if (((DependencyObject) this).ReadLocalValue(FrameworkElement.DataContextProperty) == DependencyProperty.UnsetValue)
      {
        DependencyObject dependencyObject = (DependencyObject) ((object) this.Owner ?? (object) this._rootVisual);
        ((FrameworkElement) this).SetBinding(FrameworkElement.DataContextProperty, new Binding("DataContext")
        {
          Source = (object) dependencyObject
        });
      }
      this._popup.IsOpen = true;
      ((Control) this).Focus();
      this._settingIsOpen = true;
      this.IsOpen = true;
      this._settingIsOpen = false;
    }

    private void ClosePopup()
    {
      if (this._backgroundResizeStoryboard != null)
      {
        foreach (DoubleAnimation child in (PresentationFrameworkCollection<Timeline>) this._backgroundResizeStoryboard.Children)
        {
          double num = child.From.Value;
          child.From = child.To;
          child.To = new double?(num);
        }
        Popup popup = this._popup;
        Panel overlay = this._overlay;
        ((Timeline) this._backgroundResizeStoryboard).Completed += (EventHandler) delegate
        {
          if (popup != null)
          {
            popup.IsOpen = false;
            popup.Child = (UIElement) null;
          }
          ((PresentationFrameworkCollection<UIElement>) overlay?.Children).Clear();
        };
        this._backgroundResizeStoryboard.Begin();
        this._backgroundResizeStoryboard = (Storyboard) null;
        this._popup = (Popup) null;
        this._overlay = (Panel) null;
      }
      else
      {
        if (this._popup != null)
        {
          this._popup.IsOpen = false;
          this._popup.Child = (UIElement) null;
          this._popup = (Popup) null;
        }
        if (this._overlay != null)
        {
          ((PresentationFrameworkCollection<UIElement>) this._overlay.Children).Clear();
          this._overlay = (Panel) null;
        }
      }
      ((FrameworkElement) this).SizeChanged -= new SizeChangedEventHandler(this.OnContextMenuOrRootVisualSizeChanged);
      if (this._rootVisual != null)
        ((FrameworkElement) this._rootVisual).SizeChanged -= new SizeChangedEventHandler(this.OnContextMenuOrRootVisualSizeChanged);
      foreach (ApplicationBarIconButton applicationBarIconButton in this._applicationBarIconButtons)
        applicationBarIconButton.Click -= new EventHandler(this.OnEventThatClosesContextMenu);
      this._applicationBarIconButtons.Clear();
      this._settingIsOpen = true;
      this.IsOpen = false;
      this._settingIsOpen = false;
      this.OnClosed(new RoutedEventArgs());
    }
  }
}
