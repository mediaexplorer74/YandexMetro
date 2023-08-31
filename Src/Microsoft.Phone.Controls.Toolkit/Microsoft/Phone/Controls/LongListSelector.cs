﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.LongListSelector
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "TemplatedListBox", Type = typeof (TemplatedListBox))]
  public class LongListSelector : Control
  {
    private const string TemplatedListBoxName = "TemplatedListBox";
    private const double BufferSizeDefault = 1.0;
    private const string ScrollingState = "Scrolling";
    private const string NotScrollingState = "NotScrolling";
    private const string CompressionTop = "CompressionTop";
    private const string CompressionBottom = "CompressionBottom";
    private const string NoVerticalCompression = "NoVerticalCompression";
    private const string VerticalCompressionStateName = "VerticalCompression";
    private const string ScrollStatesGroupName = "ScrollStates";
    private PhoneApplicationPage _page;
    private bool _systemTrayVisible;
    private bool _applicationBarVisible;
    private Border _border;
    private LongListSelector.LongListSelectorItemsControl _itemsControl;
    private Popup _groupSelectorPopup;
    private static readonly double _screenWidth = Application.Current.Host.Content.ActualWidth;
    private static readonly double _screenHeight = Application.Current.Host.Content.ActualHeight;
    private TemplatedListBox _listBox;
    private VisualStateGroup _scrollGroup;
    private VisualStateGroup _verticalCompressionGroup;
    private INotifyCollectionChanged _rootCollection;
    private List<INotifyCollectionChanged> _groupCollections = new List<INotifyCollectionChanged>();
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnItemsSourceChanged)));
    public static readonly DependencyProperty ListHeaderProperty = DependencyProperty.Register(nameof (ListHeader), typeof (object), typeof (LongListSelector), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ListHeaderTemplateProperty = DependencyProperty.Register(nameof (ListHeaderTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnDataTemplateChanged)));
    public static readonly DependencyProperty ListFooterProperty = DependencyProperty.Register(nameof (ListFooter), typeof (object), typeof (LongListSelector), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ListFooterTemplateProperty = DependencyProperty.Register(nameof (ListFooterTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnDataTemplateChanged)));
    public static readonly DependencyProperty GroupHeaderProperty = DependencyProperty.Register(nameof (GroupHeaderTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnDataTemplateChanged)));
    public static readonly DependencyProperty GroupFooterProperty = DependencyProperty.Register(nameof (GroupFooterTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnDataTemplateChanged)));
    public static readonly DependencyProperty ItemsTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelector.OnDataTemplateChanged)));
    public static readonly DependencyProperty DisplayAllGroupsProperty = DependencyProperty.Register(nameof (DisplayAllGroups), typeof (bool), typeof (LongListSelector), new PropertyMetadata((object) false, new PropertyChangedCallback(LongListSelector.OnDisplayAllGroupsChanged)));
    public static readonly DependencyProperty GroupItemTemplateProperty = DependencyProperty.Register(nameof (GroupItemTemplate), typeof (DataTemplate), typeof (LongListSelector), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty GroupItemsPanelProperty = DependencyProperty.Register(nameof (GroupItemsPanel), typeof (ItemsPanelTemplate), typeof (LongListSelector), new PropertyMetadata((PropertyChangedCallback) null));
    [Obsolete("BufferSizeProperty no longer affect items virtualization")]
    public static readonly DependencyProperty BufferSizeProperty = DependencyProperty.Register(nameof (BufferSize), typeof (double), typeof (LongListSelector), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(LongListSelector.OnBufferSizeChanged)));
    [Obsolete("MaximumFlickVelocityProperty is not used anymore.")]
    public static readonly DependencyProperty MaximumFlickVelocityProperty = DependencyProperty.Register(nameof (MaximumFlickVelocity), typeof (double), typeof (LongListSelector), new PropertyMetadata((object) MotionParameters.MaximumSpeed));
    public static readonly DependencyProperty ShowListHeaderProperty = DependencyProperty.Register(nameof (ShowListHeader), typeof (bool), typeof (LongListSelector), new PropertyMetadata((object) true, new PropertyChangedCallback(LongListSelector.OnShowListHeaderChanged)));
    public static readonly DependencyProperty ShowListFooterProperty = DependencyProperty.Register(nameof (ShowListFooter), typeof (bool), typeof (LongListSelector), new PropertyMetadata((object) true, new PropertyChangedCallback(LongListSelector.OnShowListFooterChanged)));

    private void OpenPopup()
    {
      this.SaveSystemState(false, false);
      this.BuildPopup();
      this.AttachToPageEvents();
      this._groupSelectorPopup.IsOpen = true;
      ((UIElement) this).UpdateLayout();
    }

    private void popup_Opened(object sender, EventArgs e) => SafeRaise.Raise<GroupViewOpenedEventArgs>(this.GroupViewOpened, (object) this, (SafeRaise.GetEventArgs<GroupViewOpenedEventArgs>) (() => new GroupViewOpenedEventArgs((ItemsControl) this._itemsControl)));

    private bool ClosePopup(object selectedGroup, bool raiseEvent)
    {
      if (raiseEvent)
      {
        GroupViewClosingEventArgs args = (GroupViewClosingEventArgs) null;
        SafeRaise.Raise<GroupViewClosingEventArgs>(this.GroupViewClosing, (object) this, (SafeRaise.GetEventArgs<GroupViewClosingEventArgs>) (() => args = new GroupViewClosingEventArgs((ItemsControl) this._itemsControl, selectedGroup)));
        if (args != null && args.Cancel)
          return false;
      }
      if (this._groupSelectorPopup != null)
      {
        this.RestoreSystemState();
        this._groupSelectorPopup.IsOpen = false;
        this.DetachFromPageEvents();
        this._groupSelectorPopup.Child = (UIElement) null;
        this._border = (Border) null;
        this._itemsControl = (LongListSelector.LongListSelectorItemsControl) null;
        this._groupSelectorPopup = (Popup) null;
      }
      return true;
    }

    private void BuildPopup()
    {
      this._groupSelectorPopup = new Popup();
      this._groupSelectorPopup.Opened += new EventHandler(this.popup_Opened);
      SolidColorBrush background = this.Background as SolidColorBrush;
      Color color1 = (Color) ((FrameworkElement) this).Resources[(object) "PhoneBackgroundColor"];
      if (background != null)
      {
        Color color2 = background.Color;
        if (background.Color.A > (byte) 0)
          color1 = background.Color;
      }
      this._border = new Border()
      {
        Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 160, color1.R, color1.G, color1.B))
      };
      ((UIElement) this._border).ManipulationStarted += (EventHandler<ManipulationStartedEventArgs>) ((o, e) => e.Handled = true);
      ((UIElement) this._border).ManipulationCompleted += (EventHandler<ManipulationCompletedEventArgs>) ((o, e) => e.Handled = true);
      ((UIElement) this._border).ManipulationDelta += (EventHandler<ManipulationDeltaEventArgs>) ((o, e) => e.Handled = true);
      EventHandler<GestureEventArgs> eventHandler = (EventHandler<GestureEventArgs>) ((o, e) => e.Handled = true);
      ((UIElement) this._border).Tap += eventHandler;
      ((UIElement) this._border).DoubleTap += eventHandler;
      ((UIElement) this._border).Hold += eventHandler;
      this._itemsControl = new LongListSelector.LongListSelectorItemsControl();
      this._itemsControl.ItemTemplate = this.GroupItemTemplate;
      this._itemsControl.ItemsPanel = this.GroupItemsPanel;
      this._itemsControl.ItemsSource = this.ItemsSource;
      this._itemsControl.GroupSelected += new LongListSelector.GroupSelectedEventHandler(this.itemsControl_GroupSelected);
      this._groupSelectorPopup.Child = (UIElement) this._border;
      ScrollViewer scrollViewer = new ScrollViewer()
      {
        HorizontalScrollBarVisibility = (ScrollBarVisibility) 0
      };
      ((FrameworkElement) this._itemsControl).HorizontalAlignment = (HorizontalAlignment) 1;
      ((FrameworkElement) this._itemsControl).Margin = new Thickness(0.0, 12.0, 0.0, 0.0);
      this._border.Child = (UIElement) scrollViewer;
      ((ContentControl) scrollViewer).Content = (object) this._itemsControl;
      this.SetItemsControlSize();
    }

    private void SetItemsControlSize()
    {
      Rect transformedRect = LongListSelector.GetTransformedRect();
      if (this._border == null)
        return;
      ((UIElement) this._border).RenderTransform = LongListSelector.GetTransform();
      ((FrameworkElement) this._border).Width = transformedRect.Width;
      ((FrameworkElement) this._border).Height = transformedRect.Height;
    }

    private void itemsControl_GroupSelected(
      object sender,
      LongListSelector.GroupSelectedEventArgs e)
    {
      if (!this.ClosePopup(e.Group, true))
        return;
      this.ScrollToGroup(e.Group);
    }

    private void AttachToPageEvents()
    {
      if (!(Application.Current.RootVisual is PhoneApplicationFrame rootVisual))
        return;
      this._page = ((ContentControl) rootVisual).Content as PhoneApplicationPage;
      if (this._page == null)
        return;
      this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.page_BackKeyPress);
      this._page.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.page_OrientationChanged);
    }

    private void DetachFromPageEvents()
    {
      if (this._page == null)
        return;
      this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.page_BackKeyPress);
      this._page.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.page_OrientationChanged);
      this._page = (PhoneApplicationPage) null;
    }

    private void page_BackKeyPress(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      this.ClosePopup((object) null, true);
    }

    private void page_OrientationChanged(object sender, OrientationChangedEventArgs e) => this.SetItemsControlSize();

    private static Rect GetTransformedRect()
    {
      bool flag = LongListSelector.IsLandscape(LongListSelector.GetPageOrientation());
      return new Rect(0.0, 0.0, flag ? LongListSelector._screenHeight : LongListSelector._screenWidth, flag ? LongListSelector._screenWidth : LongListSelector._screenHeight);
    }

    private static Transform GetTransform()
    {
      PageOrientation pageOrientation = LongListSelector.GetPageOrientation();
      if (pageOrientation != 2 && pageOrientation != 18)
      {
        if (pageOrientation != 34)
          return (Transform) null;
        return (Transform) new CompositeTransform()
        {
          Rotation = -90.0,
          TranslateY = LongListSelector._screenHeight
        };
      }
      return (Transform) new CompositeTransform()
      {
        Rotation = 90.0,
        TranslateX = LongListSelector._screenWidth
      };
    }

    private static bool IsLandscape(PageOrientation orientation) => orientation == 2 || orientation == 18 || orientation == 34;

    private static PageOrientation GetPageOrientation() => Application.Current.RootVisual is PhoneApplicationFrame rootVisual ? (((ContentControl) rootVisual).Content as PhoneApplicationPage).Orientation : (PageOrientation) 0;

    private void SaveSystemState(bool newSystemTrayVisible, bool newApplicationBarVisible)
    {
      this._systemTrayVisible = SystemTray.IsVisible;
      SystemTray.IsVisible = newSystemTrayVisible;
      if (!(Application.Current.RootVisual is PhoneApplicationFrame rootVisual) || !(((ContentControl) rootVisual).Content is PhoneApplicationPage content) || content.ApplicationBar == null)
        return;
      this._applicationBarVisible = content.ApplicationBar.IsVisible;
      content.ApplicationBar.IsVisible = newApplicationBarVisible;
    }

    private void RestoreSystemState()
    {
      SystemTray.IsVisible = this._systemTrayVisible;
      if (!(Application.Current.RootVisual is PhoneApplicationFrame rootVisual) || !(((ContentControl) rootVisual).Content is PhoneApplicationPage content) || content.ApplicationBar == null)
        return;
      content.ApplicationBar.IsVisible = this._applicationBarVisible;
    }

    public bool IsFlatList { get; set; }

    public object SelectedItem
    {
      get
      {
        if (this._listBox != null && ((Selector) this._listBox).SelectedItem != null)
        {
          LongListSelectorItem selectedItem = (LongListSelectorItem) ((Selector) this._listBox).SelectedItem;
          if (selectedItem.ItemType == LongListSelectorItemType.Item)
            return selectedItem.Item;
        }
        return (object) null;
      }
      set
      {
        if (this._listBox == null)
          return;
        if (value == null)
        {
          ((Selector) this._listBox).SelectedItem = (object) null;
        }
        else
        {
          foreach (LongListSelectorItem listSelectorItem in ((ItemsControl) this._listBox).ItemsSource)
          {
            if (listSelectorItem.Item == value)
            {
              ((Selector) this._listBox).SelectedItem = (object) listSelectorItem;
              break;
            }
          }
        }
      }
    }

    [Obsolete("IsBouncy is always set to true.")]
    public bool IsBouncy
    {
      get => true;
      set
      {
      }
    }

    private bool HasListHeader => this.ListHeaderTemplate != null || this.ListHeader is UIElement;

    private bool HasListFooter => this.ListFooterTemplate != null || this.ListFooter is UIElement;

    public bool IsScrolling { get; private set; }

    public bool IsStretching { get; private set; }

    public IEnumerable ItemsSource
    {
      get => (IEnumerable) ((DependencyObject) this).GetValue(LongListSelector.ItemsSourceProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ItemsSourceProperty, (object) value);
    }

    private static void OnItemsSourceChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((LongListSelector) obj).OnItemsSourceChanged();
    }

    public object ListHeader
    {
      get => ((DependencyObject) this).GetValue(LongListSelector.ListHeaderProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ListHeaderProperty, value);
    }

    public DataTemplate ListHeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.ListHeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ListHeaderTemplateProperty, (object) value);
    }

    public object ListFooter
    {
      get => ((DependencyObject) this).GetValue(LongListSelector.ListFooterProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ListFooterProperty, value);
    }

    public DataTemplate ListFooterTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.ListFooterTemplateProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ListFooterTemplateProperty, (object) value);
    }

    public DataTemplate GroupHeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.GroupHeaderProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.GroupHeaderProperty, (object) value);
    }

    public DataTemplate GroupFooterTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.GroupFooterProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.GroupFooterProperty, (object) value);
    }

    public DataTemplate ItemTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.ItemsTemplateProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ItemsTemplateProperty, (object) value);
    }

    public bool DisplayAllGroups
    {
      get => (bool) ((DependencyObject) this).GetValue(LongListSelector.DisplayAllGroupsProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.DisplayAllGroupsProperty, (object) value);
    }

    public DataTemplate GroupItemTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(LongListSelector.GroupItemTemplateProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.GroupItemTemplateProperty, (object) value);
    }

    public ItemsPanelTemplate GroupItemsPanel
    {
      get => (ItemsPanelTemplate) ((DependencyObject) this).GetValue(LongListSelector.GroupItemsPanelProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.GroupItemsPanelProperty, (object) value);
    }

    [Obsolete("BufferSize no longer affect items virtualization")]
    public double BufferSize
    {
      get => (double) ((DependencyObject) this).GetValue(LongListSelector.BufferSizeProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.BufferSizeProperty, (object) value);
    }

    private static void OnBufferSizeChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      if ((double) e.NewValue < 0.0)
        throw new ArgumentOutOfRangeException("BufferSize");
    }

    [Obsolete("MaximumFlickVelocity is not used anymore.")]
    public double MaximumFlickVelocity
    {
      get => (double) ((DependencyObject) this).GetValue(LongListSelector.MaximumFlickVelocityProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.MaximumFlickVelocityProperty, (object) value);
    }

    public bool ShowListHeader
    {
      get => (bool) ((DependencyObject) this).GetValue(LongListSelector.ShowListHeaderProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ShowListHeaderProperty, (object) value);
    }

    private static void OnShowListHeaderChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      LongListSelector longListSelector = (LongListSelector) obj;
      if (longListSelector._listBox == null)
        return;
      Collection<LongListSelectorItem> itemsSource = (Collection<LongListSelectorItem>) ((ItemsControl) longListSelector._listBox).ItemsSource;
      if (longListSelector.ShowListHeader)
        longListSelector.AddListHeader((IList<LongListSelectorItem>) itemsSource);
      else
        LongListSelector.RemoveListHeader((IList<LongListSelectorItem>) itemsSource);
    }

    public bool ShowListFooter
    {
      get => (bool) ((DependencyObject) this).GetValue(LongListSelector.ShowListFooterProperty);
      set => ((DependencyObject) this).SetValue(LongListSelector.ShowListFooterProperty, (object) value);
    }

    private static void OnShowListFooterChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      LongListSelector longListSelector = (LongListSelector) obj;
      if (longListSelector._listBox == null)
        return;
      Collection<LongListSelectorItem> itemsSource = (Collection<LongListSelectorItem>) ((ItemsControl) longListSelector._listBox).ItemsSource;
      if (longListSelector.ShowListFooter)
        longListSelector.AddListFooter((IList<LongListSelectorItem>) itemsSource);
      else
        LongListSelector.RemoveListFooter((IList<LongListSelectorItem>) itemsSource);
    }

    public event SelectionChangedEventHandler SelectionChanged;

    public event EventHandler ScrollingStarted;

    public event EventHandler ScrollingCompleted;

    public event EventHandler<GroupViewOpenedEventArgs> GroupViewOpened;

    public event EventHandler<GroupViewClosingEventArgs> GroupViewClosing;

    public event EventHandler<LinkUnlinkEventArgs> Link;

    public event EventHandler<LinkUnlinkEventArgs> Unlink;

    public event EventHandler StretchingBottom;

    public event EventHandler StretchingCompleted;

    public event EventHandler StretchingTop;

    public LongListSelector() => this.DefaultStyleKey = (object) typeof (LongListSelector);

    public void ScrollTo(object item)
    {
      if (this._listBox == null || item == null)
        return;
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      this._listBox.ScrollIntoView((object) itemsSource[itemsSource.Count - 1]);
      ((UIElement) this).UpdateLayout();
      foreach (LongListSelectorItem listSelectorItem in ((ItemsControl) this._listBox).ItemsSource)
      {
        if (listSelectorItem.Item != null && listSelectorItem.Item.Equals(item))
        {
          this._listBox.ScrollIntoView((object) listSelectorItem);
          break;
        }
      }
    }

    public void ScrollToGroup(object group) => this.ScrollTo(group);

    public void DisplayGroupView()
    {
      if (this.GroupItemTemplate == null || this.IsFlatList)
        return;
      this.OpenPopup();
    }

    public void CloseGroupView() => this.ClosePopup((object) null, false);

    [Obsolete("AnimateTo(...) call ScrollTo(...) to jump without animation to the given item.")]
    public void AnimateTo(object item) => this.ScrollTo(item);

    [Obsolete("GetItemsWithContainers(...) always returns an empty collection of items. Rely on Link/Unlink to know an item get realized or unrealized.")]
    public ICollection<object> GetItemsWithContainers(bool onlyItemsInView, bool getContainers) => (ICollection<object>) new Collection<object>();

    [Obsolete("GetItemsInView() always returns an empty collection of items. Rely on Link/Unlink to know an item get realized or unrealized.")]
    public ICollection<object> GetItemsInView() => this.GetItemsWithContainers(true, false);

    private void OnItemsSourceChanged() => this.LoadDataIntoListBox();

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (this._listBox != null)
      {
        ((Selector) this._listBox).SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
        this._listBox.Link -= new EventHandler<LinkUnlinkEventArgs>(this.OnLink);
        this._listBox.Unlink -= new EventHandler<LinkUnlinkEventArgs>(this.OnUnlink);
      }
      if (this._scrollGroup != null)
        this._scrollGroup.CurrentStateChanging -= new EventHandler<VisualStateChangedEventArgs>(this.OnScrollStateChanging);
      if (!(this.GetTemplateChild("TemplatedListBox") is TemplatedListBox templatedListBox))
        templatedListBox = new TemplatedListBox();
      this._listBox = templatedListBox;
      this._listBox.ListHeaderTemplate = this.ListHeaderTemplate;
      this._listBox.ListFooterTemplate = this.ListFooterTemplate;
      this._listBox.GroupHeaderTemplate = this.GroupHeaderTemplate;
      this._listBox.GroupFooterTemplate = this.GroupFooterTemplate;
      ((ItemsControl) this._listBox).ItemTemplate = this.ItemTemplate;
      ((Selector) this._listBox).SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
      this._listBox.Link += new EventHandler<LinkUnlinkEventArgs>(this.OnLink);
      this._listBox.Unlink += new EventHandler<LinkUnlinkEventArgs>(this.OnUnlink);
      ScrollViewer logicalChildByType = ((FrameworkElement) this._listBox).GetFirstLogicalChildByType<ScrollViewer>(true);
      if (logicalChildByType != null && VisualTreeHelper.GetChild((DependencyObject) logicalChildByType, 0) is FrameworkElement)
      {
        this._scrollGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) logicalChildByType, "ScrollStates");
        if (this._scrollGroup != null)
          this._scrollGroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(this.OnScrollStateChanging);
        this._verticalCompressionGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) logicalChildByType, "VerticalCompression");
        if (this._verticalCompressionGroup != null)
          this._verticalCompressionGroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(this.OnStretchStateChanging);
      }
      this.LoadDataIntoListBox();
    }

    private void LoadDataIntoListBox()
    {
      if (this._listBox == null)
        return;
      ObservableCollection<LongListSelectorItem> tuples = new ObservableCollection<LongListSelectorItem>();
      this.AddListHeader((IList<LongListSelectorItem>) tuples);
      this.UnsubscribeFromAllCollections();
      if (this.IsFlatList)
      {
        if (this.ItemsSource != null)
        {
          foreach (object obj in this.ItemsSource)
            tuples.Add(new LongListSelectorItem()
            {
              Item = obj,
              ItemType = LongListSelectorItemType.Item
            });
        }
      }
      else
      {
        IEnumerable itemsSource = this.ItemsSource;
        if (itemsSource != null)
        {
          foreach (object groupToAdd in itemsSource)
            this.AddGroup(groupToAdd, (IList) tuples);
        }
      }
      this.AddListFooter((IList<LongListSelectorItem>) tuples);
      this._rootCollection = this.ItemsSource as INotifyCollectionChanged;
      if (this._rootCollection != null)
        this._rootCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      ((ItemsControl) this._listBox).ItemsSource = (IEnumerable) tuples;
    }

    private void AddListHeader(IList<LongListSelectorItem> tuples)
    {
      if (!this.HasListHeader || !this.ShowListHeader || tuples.Count != 0 && tuples[0].ItemType == LongListSelectorItemType.ListHeader)
        return;
      tuples.Insert(0, new LongListSelectorItem()
      {
        Item = this.ListHeader,
        ItemType = LongListSelectorItemType.ListHeader
      });
    }

    private void AddListHeader() => this.AddListHeader((IList<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource);

    private static void RemoveListHeader(IList<LongListSelectorItem> tuples)
    {
      if (tuples.Count <= 0 || tuples[0].ItemType != LongListSelectorItemType.ListHeader)
        return;
      tuples.RemoveAt(0);
    }

    private void RemoveListHeader() => LongListSelector.RemoveListHeader((IList<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource);

    private void AddListFooter(IList<LongListSelectorItem> tuples)
    {
      if (!this.HasListFooter || !this.ShowListFooter || tuples.Count != 0 && tuples[tuples.Count - 1].ItemType == LongListSelectorItemType.ListFooter)
        return;
      tuples.Add(new LongListSelectorItem()
      {
        Item = this.ListFooter,
        ItemType = LongListSelectorItemType.ListFooter
      });
    }

    private void AddListFooter() => this.AddListFooter((IList<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource);

    private static void RemoveListFooter(IList<LongListSelectorItem> tuples)
    {
      LongListSelectorItem tuple = tuples[tuples.Count - 1];
      if (tuple == null || tuple.ItemType != LongListSelectorItemType.ListFooter)
        return;
      tuples.RemoveAt(tuples.Count - 1);
    }

    private void RemoveListFooter() => LongListSelector.RemoveListFooter((IList<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource);

    private void AddGroup(object groupToAdd, IList tuples)
    {
      if (!(groupToAdd is IEnumerable enumerable))
        return;
      bool flag = false;
      if (this.GroupHeaderTemplate != null)
        tuples.Add((object) new LongListSelectorItem()
        {
          Item = (object) enumerable,
          ItemType = LongListSelectorItemType.GroupHeader
        });
      foreach (object obj in enumerable)
      {
        tuples.Add((object) new LongListSelectorItem()
        {
          Item = obj,
          ItemType = LongListSelectorItemType.Item,
          Group = (object) enumerable
        });
        flag = true;
      }
      if (flag || this.DisplayAllGroups)
      {
        if (this.GroupFooterTemplate != null)
          tuples.Add((object) new LongListSelectorItem()
          {
            Item = (object) enumerable,
            ItemType = LongListSelectorItemType.GroupFooter
          });
      }
      else if (this.GroupHeaderTemplate != null)
        tuples.RemoveAt(tuples.Count - 1);
      if (!(enumerable is INotifyCollectionChanged collectionChanged) || this._groupCollections.Contains(collectionChanged))
        return;
      collectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      this._groupCollections.Add(collectionChanged);
    }

    private void AddGroupHeadersAndFooters(bool addHeaders, bool addFooters)
    {
      int index = 0;
      if (this.HasListHeader && this.ShowListHeader)
        ++index;
      IEnumerable itemsSource1 = this.ItemsSource;
      ObservableCollection<LongListSelectorItem> itemsSource2 = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      if (itemsSource1 == null)
        return;
      foreach (object obj in itemsSource1)
      {
        if (obj is IEnumerable group)
        {
          int itemsCountFromGroup = this.GetHeadersAndItemsCountFromGroup(group);
          if (addHeaders && this.GroupHeaderTemplate != null && itemsCountFromGroup > 0)
            itemsSource2.Insert(index, new LongListSelectorItem()
            {
              Item = obj,
              ItemType = LongListSelectorItemType.GroupHeader
            });
          index += itemsCountFromGroup;
          if (addFooters && this.GroupFooterTemplate != null && itemsCountFromGroup > 0)
            itemsSource2.Insert(index - 1, new LongListSelectorItem()
            {
              Item = obj,
              ItemType = LongListSelectorItemType.GroupFooter
            });
        }
      }
    }

    private void AddGroupHeaders() => this.AddGroupHeadersAndFooters(true, false);

    private void AddGroupFooters() => this.AddGroupHeadersAndFooters(false, true);

    private void RemoveAllGroupHeadersAndFooters(bool removeHeaders, bool removeFooters)
    {
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      for (int index = 0; index < itemsSource.Count; ++index)
      {
        if (removeHeaders && itemsSource[index].ItemType == LongListSelectorItemType.GroupHeader || removeFooters && itemsSource[index].ItemType == LongListSelectorItemType.GroupFooter)
          itemsSource.RemoveAt(index--);
      }
    }

    private void RemoveAllGroupHeaders() => this.RemoveAllGroupHeadersAndFooters(true, false);

    private void RemoveAllGroupFooters() => this.RemoveAllGroupHeadersAndFooters(false, true);

    private void UnsubscribeFromAllCollections()
    {
      if (this._rootCollection != null)
        this._rootCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      foreach (INotifyCollectionChanged groupCollection in this._groupCollections)
        groupCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      this._rootCollection = (INotifyCollectionChanged) null;
      this._groupCollections.Clear();
    }

    private void InsertNewGroups(IList newGroups, int newGroupsIndex)
    {
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      List<LongListSelectorItem> tuples = new List<LongListSelectorItem>();
      foreach (object newGroup in (IEnumerable) newGroups)
        this.AddGroup(newGroup, (IList) tuples);
      if (tuples.Count <= 0)
        return;
      int groupIndexInListBox = this.GetGroupIndexInListBox(newGroupsIndex);
      foreach (LongListSelectorItem listSelectorItem in tuples)
        itemsSource.Insert(groupIndexInListBox++, listSelectorItem);
    }

    private void InsertNewItems(IList newItems, int newItemsIndex, IEnumerable group)
    {
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      List<LongListSelectorItem> listSelectorItemList = new List<LongListSelectorItem>();
      foreach (object newItem in (IEnumerable) newItems)
        listSelectorItemList.Add(new LongListSelectorItem()
        {
          Group = (object) group,
          Item = newItem,
          ItemType = LongListSelectorItemType.Item
        });
      if (group != null)
      {
        int indexInLLS = 0;
        bool flag = ((ICollection) group).Count == newItems.Count && !this.DisplayAllGroups;
        foreach (object obj in this.ItemsSource)
        {
          if (obj == group)
          {
            int groupIndexInListBox = this.GetGroupIndexInListBox(indexInLLS);
            if (this.GroupHeaderTemplate != null)
            {
              if (flag)
                itemsSource.Insert(groupIndexInListBox, new LongListSelectorItem()
                {
                  ItemType = LongListSelectorItemType.GroupHeader,
                  Item = (object) group
                });
              ++groupIndexInListBox;
            }
            int num1 = groupIndexInListBox + newItemsIndex;
            foreach (LongListSelectorItem listSelectorItem in listSelectorItemList)
              itemsSource.Insert(num1++, listSelectorItem);
            if (flag && this.GroupFooterTemplate != null)
            {
              ObservableCollection<LongListSelectorItem> observableCollection = itemsSource;
              int index = num1;
              int num2 = index + 1;
              LongListSelectorItem listSelectorItem = new LongListSelectorItem()
              {
                ItemType = LongListSelectorItemType.GroupFooter,
                Item = (object) group
              };
              observableCollection.Insert(index, listSelectorItem);
            }
          }
          ++indexInLLS;
        }
      }
      else
      {
        int num = newItemsIndex;
        if (this.HasListHeader && this.ShowListHeader)
          ++num;
        foreach (LongListSelectorItem listSelectorItem in listSelectorItemList)
          itemsSource.Insert(num++, listSelectorItem);
      }
    }

    private void RemoveOldGroups(IList oldGroups, int oldGroupsIndex)
    {
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      int index1 = 0;
      if (oldGroupsIndex > 0)
      {
        index1 = this.GetGroupIndexInListBox(oldGroupsIndex - 1);
        if (((IList) this.ItemsSource)[oldGroupsIndex - 1] is IEnumerable group)
          index1 += this.GetHeadersAndItemsCountFromGroup(group);
      }
      else if (this.HasListHeader && this.ShowListHeader)
        ++index1;
      int itemsCountFromGroups = this.GetItemsCountFromGroups((IEnumerable) oldGroups);
      for (int index2 = 0; index2 < itemsCountFromGroups; ++index2)
        itemsSource.RemoveAt(index1);
      foreach (INotifyCollectionChanged oldGroup in (IEnumerable) oldGroups)
        oldGroup.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
    }

    private void RemoveOldItems(IList oldItems, int oldItemsIndex, IEnumerable group)
    {
      ObservableCollection<LongListSelectorItem> itemsSource = (ObservableCollection<LongListSelectorItem>) ((ItemsControl) this._listBox).ItemsSource;
      if (group != null)
      {
        int indexInLLS = 0;
        foreach (object obj in this.ItemsSource)
        {
          if (obj == group)
          {
            int index1 = this.GetGroupIndexInListBox(indexInLLS) + oldItemsIndex;
            if (this.GroupHeaderTemplate != null)
              ++index1;
            for (int index2 = 0; index2 < oldItems.Count; ++index2)
              itemsSource.RemoveAt(index1);
            if (((ICollection) group).Count == 0 && !this.DisplayAllGroups)
            {
              if (this.GroupFooterTemplate != null)
                itemsSource.RemoveAt(index1);
              if (this.GroupHeaderTemplate != null)
                itemsSource.RemoveAt(index1 - 1);
            }
          }
          ++indexInLLS;
        }
      }
      else
      {
        int index3 = oldItemsIndex;
        if (this.HasListHeader && this.ShowListHeader)
          ++index3;
        for (int index4 = 0; index4 < oldItems.Count; ++index4)
          itemsSource.RemoveAt(index3);
      }
    }

    private int GetGroupIndexInListBox(int indexInLLS)
    {
      int groupIndexInListBox = 0;
      int num = 0;
      if (this.HasListHeader && this.ShowListHeader)
        ++groupIndexInListBox;
      IEnumerable itemsSource = this.ItemsSource;
      if (itemsSource != null)
      {
        foreach (object obj in itemsSource)
        {
          if (indexInLLS != num)
          {
            ++num;
            if (obj is IEnumerable group)
              groupIndexInListBox += this.GetHeadersAndItemsCountFromGroup(group);
          }
          else
            break;
        }
      }
      return groupIndexInListBox;
    }

    private int GetItemsCountFromGroups(IEnumerable groups)
    {
      int itemsCountFromGroups = 0;
      foreach (object group1 in groups)
      {
        if (group1 is IEnumerable group2)
          itemsCountFromGroups += this.GetHeadersAndItemsCountFromGroup(group2);
      }
      return itemsCountFromGroups;
    }

    private int GetHeadersAndItemsCountFromGroup(IEnumerable group)
    {
      int num = 0;
      int itemsCountFromGroup = !(group is IList list) ? num + group.Cast<object>().Count<object>() : num + list.Count;
      bool flag = itemsCountFromGroup > 0;
      if ((flag || this.DisplayAllGroups) && this.GroupHeaderTemplate != null)
        ++itemsCountFromGroup;
      if ((flag || this.DisplayAllGroups) && this.GroupFooterTemplate != null)
        ++itemsCountFromGroup;
      return itemsCountFromGroup;
    }

    private void UpdateItemsTemplate(LongListSelectorItemType itemType, DataTemplate newTemplate)
    {
      if (this._listBox == null)
        return;
      foreach (TemplatedListBoxItem templatedListBoxItem in ((FrameworkElement) this._listBox).GetLogicalChildrenByType<TemplatedListBoxItem>(false))
      {
        if (templatedListBoxItem.Tuple.ItemType == itemType)
          ((ContentControl) templatedListBoxItem).ContentTemplate = newTemplate;
      }
      switch (itemType)
      {
        case LongListSelectorItemType.Item:
          ((ItemsControl) this._listBox).ItemTemplate = newTemplate;
          break;
        case LongListSelectorItemType.GroupHeader:
          this._listBox.GroupHeaderTemplate = newTemplate;
          break;
        case LongListSelectorItemType.GroupFooter:
          this._listBox.GroupFooterTemplate = newTemplate;
          break;
        case LongListSelectorItemType.ListHeader:
          this._listBox.ListHeaderTemplate = newTemplate;
          break;
        case LongListSelectorItemType.ListFooter:
          this._listBox.ListFooterTemplate = newTemplate;
          break;
      }
    }

    private static void OnDataTemplateChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      LongListSelector longListSelector = (LongListSelector) o;
      if (longListSelector._listBox == null)
        return;
      DataTemplate newValue = (DataTemplate) e.NewValue;
      if (e.Property == LongListSelector.ListHeaderTemplateProperty)
      {
        longListSelector.UpdateItemsTemplate(LongListSelectorItemType.ListHeader, newValue);
        if (e.OldValue == null)
        {
          longListSelector.AddListHeader();
        }
        else
        {
          if (e.NewValue != null || longListSelector.HasListHeader)
            return;
          longListSelector.RemoveListHeader();
        }
      }
      else if (e.Property == LongListSelector.ListFooterTemplateProperty)
      {
        longListSelector.UpdateItemsTemplate(LongListSelectorItemType.ListFooter, newValue);
        if (e.OldValue == null)
        {
          longListSelector.AddListFooter();
        }
        else
        {
          if (e.NewValue != null || longListSelector.HasListHeader)
            return;
          longListSelector.RemoveListFooter();
        }
      }
      else if (e.Property == LongListSelector.GroupHeaderProperty)
      {
        longListSelector.UpdateItemsTemplate(LongListSelectorItemType.GroupHeader, newValue);
        if (e.OldValue == null)
        {
          longListSelector.AddGroupHeaders();
        }
        else
        {
          if (e.NewValue != null)
            return;
          longListSelector.RemoveAllGroupHeaders();
        }
      }
      else if (e.Property == LongListSelector.GroupFooterProperty)
      {
        longListSelector.UpdateItemsTemplate(LongListSelectorItemType.GroupFooter, newValue);
        if (e.OldValue == null)
        {
          longListSelector.AddGroupFooters();
        }
        else
        {
          if (e.NewValue != null)
            return;
          longListSelector.RemoveAllGroupFooters();
        }
      }
      else
      {
        if (e.Property != LongListSelector.ItemsTemplateProperty)
          return;
        longListSelector.UpdateItemsTemplate(LongListSelectorItemType.Item, newValue);
      }
    }

    private static void OnDisplayAllGroupsChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      ((LongListSelector) obj).LoadDataIntoListBox();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LongListSelectorItem listSelectorItem = (LongListSelectorItem) null;
      foreach (LongListSelectorItem addedItem in (IEnumerable) e.AddedItems)
      {
        if (addedItem.ItemType == LongListSelectorItemType.GroupHeader)
        {
          listSelectorItem = addedItem;
          break;
        }
      }
      if (listSelectorItem != null)
      {
        this.SelectedItem = (object) null;
        this.DisplayGroupView();
      }
      else
      {
        SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        List<LongListSelectorItem> listSelectorItemList1 = new List<LongListSelectorItem>();
        List<LongListSelectorItem> listSelectorItemList2 = new List<LongListSelectorItem>();
        foreach (LongListSelectorItem addedItem in (IEnumerable) e.AddedItems)
        {
          if (addedItem.ItemType == LongListSelectorItemType.Item)
            listSelectorItemList1.Add(addedItem);
        }
        foreach (LongListSelectorItem removedItem in (IEnumerable) e.RemovedItems)
        {
          if (removedItem.ItemType == LongListSelectorItemType.Item)
            listSelectorItemList2.Add(removedItem);
        }
        selectionChanged((object) this, new SelectionChangedEventArgs((IList) listSelectorItemList2, (IList) listSelectorItemList1));
      }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      IEnumerable group = sender as IEnumerable;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          if (sender == this._rootCollection)
          {
            if (this.IsFlatList)
            {
              this.InsertNewItems(e.NewItems, e.NewStartingIndex, (IEnumerable) null);
              break;
            }
            this.InsertNewGroups(e.NewItems, e.NewStartingIndex);
            break;
          }
          this.InsertNewItems(e.NewItems, e.NewStartingIndex, group);
          break;
        case NotifyCollectionChangedAction.Remove:
          if (sender == this._rootCollection)
          {
            if (this.IsFlatList)
            {
              this.RemoveOldItems(e.OldItems, e.OldStartingIndex, (IEnumerable) null);
              break;
            }
            this.RemoveOldGroups(e.OldItems, e.OldStartingIndex);
            break;
          }
          this.RemoveOldItems(e.OldItems, e.OldStartingIndex, group);
          break;
        case NotifyCollectionChangedAction.Replace:
        case NotifyCollectionChangedAction.Reset:
          this.LoadDataIntoListBox();
          break;
      }
    }

    private void OnScrollStateChanging(object sender, VisualStateChangedEventArgs e)
    {
      this.IsScrolling = e.NewState.Name == "Scrolling";
      if (e.NewState.Name == "Scrolling" && this.ScrollingStarted != null)
      {
        this.ScrollingStarted((object) this, (EventArgs) null);
      }
      else
      {
        if (!(e.NewState.Name == "NotScrolling") || this.ScrollingCompleted == null)
          return;
        this.ScrollingCompleted((object) this, (EventArgs) null);
      }
    }

    private void OnStretchStateChanging(object sender, VisualStateChangedEventArgs e)
    {
      this.IsStretching = e.NewState.Name == "CompressionBottom" || e.NewState.Name == "CompressionTop";
      if (e.NewState.Name == "CompressionTop" && this.StretchingTop != null)
        this.StretchingTop((object) this, (EventArgs) null);
      else if (e.NewState.Name == "CompressionBottom" && this.StretchingBottom != null)
      {
        this.StretchingBottom((object) this, (EventArgs) null);
      }
      else
      {
        if (!(e.NewState.Name == "NoVerticalCompression") || this.StretchingCompleted == null)
          return;
        this.StretchingCompleted((object) this, (EventArgs) null);
      }
    }

    private void OnLink(object sender, LinkUnlinkEventArgs e)
    {
      if (this.Link == null)
        return;
      this.Link((object) this, e);
    }

    private void OnUnlink(object sender, LinkUnlinkEventArgs e)
    {
      if (this.Unlink == null)
        return;
      this.Unlink((object) this, e);
    }

    private class GroupSelectedEventArgs : EventArgs
    {
      public GroupSelectedEventArgs(object group) => this.Group = group;

      public object Group { get; private set; }
    }

    private delegate void GroupSelectedEventHandler(
      object sender,
      LongListSelector.GroupSelectedEventArgs e);

    private class LongListSelectorItemsControl : ItemsControl
    {
      public event LongListSelector.GroupSelectedEventHandler GroupSelected;

      protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
      {
        base.PrepareContainerForItemOverride(element, item);
        ((UIElement) element).Tap += new EventHandler<GestureEventArgs>(this.LongListSelectorItemsControl_Tap);
      }

      protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
      {
        base.ClearContainerForItemOverride(element, item);
        ((UIElement) element).Tap -= new EventHandler<GestureEventArgs>(this.LongListSelectorItemsControl_Tap);
      }

      private void LongListSelectorItemsControl_Tap(object sender, GestureEventArgs e)
      {
        if (!(sender is ContentPresenter contentPresenter))
          return;
        LongListSelector.GroupSelectedEventHandler groupSelected = this.GroupSelected;
        if (groupSelected == null)
          return;
        LongListSelector.GroupSelectedEventArgs e1 = new LongListSelector.GroupSelectedEventArgs(contentPresenter.Content);
        groupSelected((object) this, e1);
      }
    }
  }
}
