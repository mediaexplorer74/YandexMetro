// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ListPicker
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(GroupName = "PickerStates", Name = "Normal")]
  [TemplatePart(Name = "ItemsPresenterHost", Type = typeof (Canvas))]
  [TemplatePart(Name = "ItemsPresenter", Type = typeof (ItemsPresenter))]
  [TemplatePart(Name = "ItemsPresenterTranslateTransform", Type = typeof (TranslateTransform))]
  [TemplateVisualState(GroupName = "PickerStates", Name = "Highlighted")]
  [TemplateVisualState(GroupName = "PickerStates", Name = "Disabled")]
  [TemplatePart(Name = "MultipleSelectionModeSummary", Type = typeof (TextBlock))]
  public class ListPicker : ItemsControl
  {
    private const string ItemsPresenterPartName = "ItemsPresenter";
    private const string ItemsPresenterTranslateTransformPartName = "ItemsPresenterTranslateTransform";
    private const string ItemsPresenterHostPartName = "ItemsPresenterHost";
    private const string MultipleSelectionModeSummaryPartName = "MultipleSelectionModeSummary";
    private const string BorderPartName = "Border";
    private const string PickerStatesGroupName = "PickerStates";
    private const string PickerStatesNormalStateName = "Normal";
    private const string PickerStatesHighlightedStateName = "Highlighted";
    private const string PickerStatesDisabledStateName = "Disabled";
    private const double NormalModeOffset = 4.0;
    private readonly DoubleAnimation _heightAnimation = new DoubleAnimation();
    private readonly DoubleAnimation _translateAnimation = new DoubleAnimation();
    private readonly Storyboard _storyboard = new Storyboard();
    private PhoneApplicationFrame _frame;
    private PhoneApplicationPage _page;
    private FrameworkElement _itemsPresenterHostParent;
    private Canvas _itemsPresenterHostPart;
    private ItemsPresenter _itemsPresenterPart;
    private TranslateTransform _itemsPresenterTranslateTransformPart;
    private bool _updatingSelection;
    private int _deferredSelectedIndex = -1;
    private object _deferredSelectedItem;
    private object _frameContentWhenOpened;
    private NavigationInTransition _savedNavigationInTransition;
    private NavigationOutTransition _savedNavigationOutTransition;
    private ListPickerPage _listPickerPage;
    private TextBlock _multipleSelectionModeSummary;
    private Border _border;
    private bool _hasPickerPageOpen;
    public static readonly DependencyProperty SummaryForSelectedItemsDelegateProperty = DependencyProperty.Register(nameof (SummaryForSelectedItemsDelegate), typeof (Func<IList, string>), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty ListPickerModeProperty = DependencyProperty.Register(nameof (ListPickerMode), typeof (ListPickerMode), typeof (ListPicker), new PropertyMetadata((object) ListPickerMode.Normal, new PropertyChangedCallback(ListPicker.OnListPickerModeChanged)));
    private static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(nameof (IsHighlighted), typeof (bool), typeof (ListPicker), new PropertyMetadata((object) false, new PropertyChangedCallback(ListPicker.OnIsHighlightedChanged)));
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (ListPicker), new PropertyMetadata((object) -1, new PropertyChangedCallback(ListPicker.OnSelectedIndexChanged)));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (ListPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(ListPicker.OnSelectedItemChanged)));
    private static readonly DependencyProperty ShadowItemTemplateProperty = DependencyProperty.Register("ShadowItemTemplate", typeof (DataTemplate), typeof (ListPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(ListPicker.OnShadowOrFullModeItemTemplateChanged)));
    public static readonly DependencyProperty FullModeItemTemplateProperty = DependencyProperty.Register(nameof (FullModeItemTemplate), typeof (DataTemplate), typeof (ListPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(ListPicker.OnShadowOrFullModeItemTemplateChanged)));
    private static readonly DependencyProperty ActualFullModeItemTemplateProperty = DependencyProperty.Register("ActualFullModeItemTemplate", typeof (DataTemplate), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty FullModeHeaderProperty = DependencyProperty.Register(nameof (FullModeHeader), typeof (object), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty ItemCountThresholdProperty = DependencyProperty.Register(nameof (ItemCountThreshold), typeof (int), typeof (ListPicker), new PropertyMetadata((object) 5, new PropertyChangedCallback(ListPicker.OnItemCountThresholdChanged)));
    public static readonly DependencyProperty PickerPageUriProperty = DependencyProperty.Register(nameof (PickerPageUri), typeof (Uri), typeof (ListPicker), (PropertyMetadata) null);
    public static readonly DependencyProperty ExpansionModeProperty = DependencyProperty.Register(nameof (ExpansionMode), typeof (ExpansionMode), typeof (ListPicker), new PropertyMetadata((object) ExpansionMode.ExpansionAllowed, (PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof (SelectionMode), typeof (SelectionMode), typeof (ListPicker), new PropertyMetadata((object) (SelectionMode) 0, new PropertyChangedCallback(ListPicker.OnSelectionModeChanged)));
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof (SelectedItems), typeof (IList), typeof (ListPicker), new PropertyMetadata(new PropertyChangedCallback(ListPicker.OnSelectedItemsChanged)));

    public event SelectionChangedEventHandler SelectionChanged;

    public Func<IList, string> SummaryForSelectedItemsDelegate
    {
      get => (Func<IList, string>) ((DependencyObject) this).GetValue(ListPicker.SummaryForSelectedItemsDelegateProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.SummaryForSelectedItemsDelegateProperty, (object) value);
    }

    public ListPickerMode ListPickerMode
    {
      get => (ListPickerMode) ((DependencyObject) this).GetValue(ListPicker.ListPickerModeProperty);
      private set => ((DependencyObject) this).SetValue(ListPicker.ListPickerModeProperty, (object) value);
    }

    private static void OnListPickerModeChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnListPickerModeChanged((ListPickerMode) e.OldValue, (ListPickerMode) e.NewValue);
    }

    private void OnListPickerModeChanged(ListPickerMode oldValue, ListPickerMode newValue)
    {
      if (ListPickerMode.Expanded == oldValue)
      {
        if (this._page != null)
        {
          this._page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.OnPageBackKeyPress);
          this._page = (PhoneApplicationPage) null;
        }
        if (this._frame != null)
        {
          ((UIElement) this._frame).ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(this.OnFrameManipulationStarted);
          this._frame = (PhoneApplicationFrame) null;
        }
      }
      if (ListPickerMode.Expanded == newValue)
      {
        if (this._frame == null)
        {
          this._frame = Application.Current.RootVisual as PhoneApplicationFrame;
          if (this._frame != null)
            ((UIElement) this._frame).AddHandler(UIElement.ManipulationStartedEvent, (Delegate) new EventHandler<ManipulationStartedEventArgs>(this.OnFrameManipulationStarted), true);
        }
        if (this._frame != null)
        {
          this._page = ((ContentControl) this._frame).Content as PhoneApplicationPage;
          if (this._page != null)
            this._page.BackKeyPress += new EventHandler<CancelEventArgs>(this.OnPageBackKeyPress);
        }
      }
      if (ListPickerMode.Full == oldValue)
        this.ClosePickerPage();
      if (ListPickerMode.Full == newValue)
        this.OpenPickerPage();
      this.SizeForAppropriateView(ListPickerMode.Full != oldValue);
      this.IsHighlighted = ListPickerMode.Expanded == newValue;
    }

    private bool IsHighlighted
    {
      get => (bool) ((DependencyObject) this).GetValue(ListPicker.IsHighlightedProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.IsHighlightedProperty, (object) value);
    }

    private static void OnIsHighlightedChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      (o as ListPicker).OnIsHighlightedChanged();
    }

    private void OnIsHighlightedChanged() => this.UpdateVisualStates(true);

    private static void OnIsEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => (o as ListPicker).OnIsEnabledChanged();

    private void OnIsEnabledChanged() => this.UpdateVisualStates(true);

    public int SelectedIndex
    {
      get => (int) ((DependencyObject) this).GetValue(ListPicker.SelectedIndexProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.SelectedIndexProperty, (object) value);
    }

    private static void OnSelectedIndexChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnSelectedIndexChanged((int) e.OldValue, (int) e.NewValue);
    }

    private void OnSelectedIndexChanged(int oldValue, int newValue)
    {
      if (((PresentationFrameworkCollection<object>) this.Items).Count <= newValue || 0 < ((PresentationFrameworkCollection<object>) this.Items).Count && newValue < 0 || ((PresentationFrameworkCollection<object>) this.Items).Count == 0 && newValue != -1)
      {
        this._deferredSelectedIndex = ((Control) this).Template == null && 0 <= newValue ? newValue : throw new InvalidOperationException(Resources.InvalidSelectedIndex);
      }
      else
      {
        if (!this._updatingSelection)
        {
          this._updatingSelection = true;
          this.SelectedItem = -1 != newValue ? ((PresentationFrameworkCollection<object>) this.Items)[newValue] : (object) null;
          this._updatingSelection = false;
        }
        if (-1 == oldValue)
          return;
        ListPickerItem listPickerItem = (ListPickerItem) this.ItemContainerGenerator.ContainerFromIndex(oldValue);
        if (listPickerItem == null)
          return;
        listPickerItem.IsSelected = false;
      }
    }

    public object SelectedItem
    {
      get => ((DependencyObject) this).GetValue(ListPicker.SelectedItemProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.SelectedItemProperty, value);
    }

    private static void OnSelectedItemChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnSelectedItemChanged(e.OldValue, e.NewValue);
    }

    private void OnSelectedItemChanged(object oldValue, object newValue)
    {
      if (this.Items == null || ((PresentationFrameworkCollection<object>) this.Items).Count == 0)
      {
        if (((Control) this).Template != null)
          throw new InvalidOperationException(Resources.InvalidSelectedItem);
        this._deferredSelectedItem = newValue;
      }
      else
      {
        int num = newValue != null ? ((PresentationFrameworkCollection<object>) this.Items).IndexOf(newValue) : -1;
        if (-1 == num && 0 < ((PresentationFrameworkCollection<object>) this.Items).Count)
          throw new InvalidOperationException(Resources.InvalidSelectedItem);
        if (!this._updatingSelection)
        {
          this._updatingSelection = true;
          this.SelectedIndex = num;
          this._updatingSelection = false;
        }
        if (this.ListPickerMode != ListPickerMode.Normal)
          this.ListPickerMode = ListPickerMode.Normal;
        else
          this.SizeForAppropriateView(false);
        SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        object[] objArray1;
        if (oldValue != null)
          objArray1 = new object[1]{ oldValue };
        else
          objArray1 = new object[0];
        IList list1 = (IList) objArray1;
        object[] objArray2;
        if (newValue != null)
          objArray2 = new object[1]{ newValue };
        else
          objArray2 = new object[0];
        IList list2 = (IList) objArray2;
        selectionChanged((object) this, new SelectionChangedEventArgs(list1, list2));
      }
    }

    public DataTemplate FullModeItemTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ListPicker.FullModeItemTemplateProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.FullModeItemTemplateProperty, (object) value);
    }

    private static void OnShadowOrFullModeItemTemplateChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnShadowOrFullModeItemTemplateChanged();
    }

    private void OnShadowOrFullModeItemTemplateChanged() => ((DependencyObject) this).SetValue(ListPicker.ActualFullModeItemTemplateProperty, (object) (this.FullModeItemTemplate ?? this.ItemTemplate));

    public object Header
    {
      get => ((DependencyObject) this).GetValue(ListPicker.HeaderProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ListPicker.HeaderTemplateProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.HeaderTemplateProperty, (object) value);
    }

    public object FullModeHeader
    {
      get => ((DependencyObject) this).GetValue(ListPicker.FullModeHeaderProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.FullModeHeaderProperty, value);
    }

    public int ItemCountThreshold
    {
      get => (int) ((DependencyObject) this).GetValue(ListPicker.ItemCountThresholdProperty);
      private set => ((DependencyObject) this).SetValue(ListPicker.ItemCountThresholdProperty, (object) value);
    }

    private static void OnItemCountThresholdChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnItemCountThresholdChanged((int) e.NewValue);
    }

    private void OnItemCountThresholdChanged(int newValue)
    {
      if (newValue < 0)
        throw new ArgumentOutOfRangeException("ItemCountThreshold");
    }

    public Uri PickerPageUri
    {
      get => (Uri) ((DependencyObject) this).GetValue(ListPicker.PickerPageUriProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.PickerPageUriProperty, (object) value);
    }

    public ExpansionMode ExpansionMode
    {
      get => (ExpansionMode) ((DependencyObject) this).GetValue(ListPicker.ExpansionModeProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.ExpansionModeProperty, (object) value);
    }

    public SelectionMode SelectionMode
    {
      get => (SelectionMode) ((DependencyObject) this).GetValue(ListPicker.SelectionModeProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.SelectionModeProperty, (object) value);
    }

    private static void OnSelectionModeChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnSelectionModeChanged((SelectionMode) e.NewValue);
    }

    private void OnSelectionModeChanged(SelectionMode newValue)
    {
      if (newValue == 1 || newValue == 2)
      {
        if (this._multipleSelectionModeSummary == null || this._itemsPresenterHostPart == null)
          return;
        ((UIElement) this._multipleSelectionModeSummary).Visibility = (Visibility) 0;
        ((UIElement) this._itemsPresenterHostPart).Visibility = (Visibility) 1;
      }
      else
      {
        if (this._multipleSelectionModeSummary == null || this._itemsPresenterHostPart == null)
          return;
        ((UIElement) this._multipleSelectionModeSummary).Visibility = (Visibility) 1;
        ((UIElement) this._itemsPresenterHostPart).Visibility = (Visibility) 0;
      }
    }

    public IList SelectedItems
    {
      get => (IList) ((DependencyObject) this).GetValue(ListPicker.SelectedItemsProperty);
      set => ((DependencyObject) this).SetValue(ListPicker.SelectedItemsProperty, (object) value);
    }

    private static void OnSelectedItemsChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ((ListPicker) o).OnSelectedItemsChanged((IList) e.OldValue, (IList) e.NewValue);
    }

    private void OnSelectedItemsChanged(IList oldValue, IList newValue)
    {
      this.UpdateSummary(newValue);
      SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      IList list1 = (IList) new List<object>();
      if (oldValue != null)
      {
        foreach (object obj in (IEnumerable) oldValue)
        {
          if (newValue == null || !newValue.Contains(obj))
            list1.Add(obj);
        }
      }
      IList list2 = (IList) new List<object>();
      if (newValue != null)
      {
        foreach (object obj in (IEnumerable) newValue)
        {
          if (oldValue == null || !oldValue.Contains(obj))
            list2.Add(obj);
        }
      }
      selectionChanged((object) this, new SelectionChangedEventArgs(list1, list2));
    }

    public ListPicker()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (ListPicker);
      Storyboard.SetTargetProperty((Timeline) this._heightAnimation, new PropertyPath((object) FrameworkElement.HeightProperty));
      Storyboard.SetTargetProperty((Timeline) this._translateAnimation, new PropertyPath((object) TranslateTransform.YProperty));
      Duration duration = Duration.op_Implicit(TimeSpan.FromSeconds(0.2));
      ((Timeline) this._heightAnimation).Duration = duration;
      ((Timeline) this._translateAnimation).Duration = duration;
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).EasingMode = (EasingMode) 2;
      exponentialEase.Exponent = 4.0;
      IEasingFunction ieasingFunction = (IEasingFunction) exponentialEase;
      this._heightAnimation.EasingFunction = ieasingFunction;
      this._translateAnimation.EasingFunction = ieasingFunction;
      ((FrameworkElement) this).RegisterNotification("IsEnabled", new PropertyChangedCallback(ListPicker.OnIsEnabledChanged));
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => this.UpdateVisualStates(true);

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._frame == null)
        return;
      ((UIElement) this._frame).ManipulationStarted -= new EventHandler<ManipulationStartedEventArgs>(this.OnFrameManipulationStarted);
      this._frame = (PhoneApplicationFrame) null;
    }

    public virtual void OnApplyTemplate()
    {
      if (this._itemsPresenterHostParent != null)
        this._itemsPresenterHostParent.SizeChanged -= new SizeChangedEventHandler(this.OnItemsPresenterHostParentSizeChanged);
      this._storyboard.Stop();
      ((FrameworkElement) this).OnApplyTemplate();
      this._itemsPresenterPart = ((Control) this).GetTemplateChild("ItemsPresenter") as ItemsPresenter;
      this._itemsPresenterTranslateTransformPart = ((Control) this).GetTemplateChild("ItemsPresenterTranslateTransform") as TranslateTransform;
      this._itemsPresenterHostPart = ((Control) this).GetTemplateChild("ItemsPresenterHost") as Canvas;
      this._itemsPresenterHostParent = this._itemsPresenterHostPart != null ? ((FrameworkElement) this._itemsPresenterHostPart).Parent as FrameworkElement : (FrameworkElement) null;
      this._multipleSelectionModeSummary = ((Control) this).GetTemplateChild("MultipleSelectionModeSummary") as TextBlock;
      this._border = ((Control) this).GetTemplateChild("Border") as Border;
      if (this._itemsPresenterHostParent != null)
        this._itemsPresenterHostParent.SizeChanged += new SizeChangedEventHandler(this.OnItemsPresenterHostParentSizeChanged);
      if (this._itemsPresenterHostPart != null)
      {
        Storyboard.SetTarget((Timeline) this._heightAnimation, (DependencyObject) this._itemsPresenterHostPart);
        if (!((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Contains((Timeline) this._heightAnimation))
          ((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Add((Timeline) this._heightAnimation);
      }
      else if (((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Contains((Timeline) this._heightAnimation))
        ((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Remove((Timeline) this._heightAnimation);
      if (this._itemsPresenterTranslateTransformPart != null)
      {
        Storyboard.SetTarget((Timeline) this._translateAnimation, (DependencyObject) this._itemsPresenterTranslateTransformPart);
        if (!((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Contains((Timeline) this._translateAnimation))
          ((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Add((Timeline) this._translateAnimation);
      }
      else if (((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Contains((Timeline) this._translateAnimation))
        ((PresentationFrameworkCollection<Timeline>) this._storyboard.Children).Remove((Timeline) this._translateAnimation);
      ((FrameworkElement) this).SetBinding(ListPicker.ShadowItemTemplateProperty, new Binding("ItemTemplate")
      {
        Source = (object) this
      });
      if (-1 != this._deferredSelectedIndex)
      {
        this.SelectedIndex = this._deferredSelectedIndex;
        this._deferredSelectedIndex = -1;
      }
      if (this._deferredSelectedItem != null)
      {
        this.SelectedItem = this._deferredSelectedItem;
        this._deferredSelectedItem = (object) null;
      }
      this.OnSelectionModeChanged(this.SelectionMode);
      this.OnSelectedItemsChanged(this.SelectedItems, this.SelectedItems);
    }

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is ListPickerItem;

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new ListPickerItem();

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      ContentControl contentControl = (ContentControl) element;
      ((UIElement) contentControl).Tap += new EventHandler<GestureEventArgs>(this.OnContainerTap);
      ((FrameworkElement) contentControl).SizeChanged += new SizeChangedEventHandler(this.OnListPickerItemSizeChanged);
      if (!object.Equals(item, this.SelectedItem))
        return;
      this.SizeForAppropriateView(false);
    }

    protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      base.ClearContainerForItemOverride(element, item);
      ContentControl contentControl = (ContentControl) element;
      ((UIElement) contentControl).Tap -= new EventHandler<GestureEventArgs>(this.OnContainerTap);
      ((FrameworkElement) contentControl).SizeChanged -= new SizeChangedEventHandler(this.OnListPickerItemSizeChanged);
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (0 < ((PresentationFrameworkCollection<object>) this.Items).Count && this.SelectedItem == null)
      {
        if (((FrameworkElement) this).GetBindingExpression(ListPicker.SelectedIndexProperty) == null && ((FrameworkElement) this).GetBindingExpression(ListPicker.SelectedItemProperty) == null)
          this.SelectedIndex = 0;
      }
      else if (((PresentationFrameworkCollection<object>) this.Items).Count == 0)
      {
        this.SelectedIndex = -1;
        this.ListPickerMode = ListPickerMode.Normal;
      }
      else if (((PresentationFrameworkCollection<object>) this.Items).Count <= this.SelectedIndex)
        this.SelectedIndex = ((PresentationFrameworkCollection<object>) this.Items).Count - 1;
      else if (!object.Equals(((PresentationFrameworkCollection<object>) this.Items)[this.SelectedIndex], this.SelectedItem))
      {
        int num = ((PresentationFrameworkCollection<object>) this.Items).IndexOf(this.SelectedItem);
        if (-1 == num)
          this.SelectedItem = ((PresentationFrameworkCollection<object>) this.Items)[0];
        else
          this.SelectedIndex = num;
      }
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() => this.SizeForAppropriateView(false)));
    }

    private bool IsValidManipulation(object OriginalSource, Point p)
    {
      for (DependencyObject dependencyObject = OriginalSource as DependencyObject; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
      {
        if (this._itemsPresenterHostPart == dependencyObject || this._multipleSelectionModeSummary == dependencyObject || this._border == dependencyObject)
        {
          double num = 11.0;
          return p.X > 0.0 && p.Y > 0.0 - num && p.X < ((UIElement) this._border).RenderSize.Width && p.Y < ((UIElement) this._border).RenderSize.Height + num;
        }
      }
      return false;
    }

    protected virtual void OnTap(GestureEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      if (this.ListPickerMode != ListPickerMode.Normal)
        return;
      if (!((Control) this).IsEnabled)
      {
        e.Handled = true;
      }
      else
      {
        Point position = e.GetPosition((UIElement) ((RoutedEventArgs) e).OriginalSource);
        if (!this.IsValidManipulation(((RoutedEventArgs) e).OriginalSource, position) || !this.Open())
          return;
        e.Handled = true;
      }
    }

    protected virtual void OnManipulationStarted(ManipulationStartedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Control) this).OnManipulationStarted(e);
      if (this.ListPickerMode != ListPickerMode.Normal)
        return;
      if (!((Control) this).IsEnabled)
      {
        e.Complete();
      }
      else
      {
        Point p = e.ManipulationOrigin;
        if (((RoutedEventArgs) e).OriginalSource != e.ManipulationContainer)
          p = e.ManipulationContainer.TransformToVisual((UIElement) ((RoutedEventArgs) e).OriginalSource).Transform(p);
        if (!this.IsValidManipulation(((RoutedEventArgs) e).OriginalSource, p))
          return;
        this.IsHighlighted = true;
      }
    }

    protected virtual void OnManipulationDelta(ManipulationDeltaEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Control) this).OnManipulationDelta(e);
      if (this.ListPickerMode != ListPickerMode.Normal)
        return;
      if (!((Control) this).IsEnabled)
      {
        e.Complete();
      }
      else
      {
        Point p = e.ManipulationOrigin;
        if (((RoutedEventArgs) e).OriginalSource != e.ManipulationContainer)
          p = e.ManipulationContainer.TransformToVisual((UIElement) ((RoutedEventArgs) e).OriginalSource).Transform(p);
        if (this.IsValidManipulation(((RoutedEventArgs) e).OriginalSource, p))
          return;
        this.IsHighlighted = false;
        e.Complete();
      }
    }

    protected virtual void OnManipulationCompleted(ManipulationCompletedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Control) this).OnManipulationCompleted(e);
      if (!((Control) this).IsEnabled || this.ListPickerMode != ListPickerMode.Normal)
        return;
      this.IsHighlighted = false;
    }

    public bool Open()
    {
      if (this.SelectionMode == null)
      {
        if (this.ListPickerMode != ListPickerMode.Normal)
          return false;
        this.ListPickerMode = this.ExpansionMode != ExpansionMode.ExpansionAllowed || ((PresentationFrameworkCollection<object>) this.Items).Count > this.ItemCountThreshold ? ListPickerMode.Full : ListPickerMode.Expanded;
        return true;
      }
      this.ListPickerMode = ListPickerMode.Full;
      return true;
    }

    private void OnItemsPresenterHostParentSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this._itemsPresenterPart != null)
        ((FrameworkElement) this._itemsPresenterPart).Width = e.NewSize.Width;
      ((UIElement) this._itemsPresenterHostParent).Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(new Point(), e.NewSize)
      };
    }

    private void OnListPickerItemSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!object.Equals(this.ItemContainerGenerator.ItemFromContainer((DependencyObject) sender), this.SelectedItem))
        return;
      this.SizeForAppropriateView(false);
    }

    private void OnPageBackKeyPress(object sender, CancelEventArgs e)
    {
      this.ListPickerMode = ListPickerMode.Normal;
      e.Cancel = true;
    }

    private void SizeForAppropriateView(bool animate)
    {
      switch (this.ListPickerMode)
      {
        case ListPickerMode.Normal:
          this.SizeForNormalMode(animate);
          break;
        case ListPickerMode.Expanded:
          this.SizeForExpandedMode();
          break;
      }
      this._storyboard.Begin();
      if (animate)
        return;
      this._storyboard.SkipToFill();
    }

    private void SizeForNormalMode(bool animate)
    {
      ContentControl contentControl = (ContentControl) this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem);
      if (contentControl != null)
      {
        if (0.0 < ((FrameworkElement) contentControl).ActualHeight)
          this.SetContentHeight(((FrameworkElement) contentControl).ActualHeight + ((FrameworkElement) contentControl).Margin.Top + ((FrameworkElement) contentControl).Margin.Bottom - 8.0);
        if (this._itemsPresenterTranslateTransformPart != null)
        {
          if (!animate)
            this._itemsPresenterTranslateTransformPart.Y = -4.0;
          this._translateAnimation.To = new double?(((FrameworkElement) contentControl).Margin.Top - LayoutInformation.GetLayoutSlot((FrameworkElement) contentControl).Top - 4.0);
          this._translateAnimation.From = animate ? new double?() : this._translateAnimation.To;
        }
      }
      else
        this.SetContentHeight(0.0);
      ListPickerItem listPickerItem = (ListPickerItem) this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex);
      if (listPickerItem == null)
        return;
      listPickerItem.IsSelected = false;
    }

    private void SizeForExpandedMode()
    {
      if (this._itemsPresenterPart != null)
        this.SetContentHeight(((FrameworkElement) this._itemsPresenterPart).ActualHeight);
      if (this._itemsPresenterTranslateTransformPart != null)
        this._translateAnimation.To = new double?(0.0);
      ListPickerItem listPickerItem = (ListPickerItem) this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex);
      if (listPickerItem == null)
        return;
      listPickerItem.IsSelected = true;
    }

    private void SetContentHeight(double height)
    {
      if (this._itemsPresenterHostPart == null || double.IsNaN(height))
        return;
      double height1 = ((FrameworkElement) this._itemsPresenterHostPart).Height;
      this._heightAnimation.From = new double?(double.IsNaN(height1) ? height : height1);
      this._heightAnimation.To = new double?(height);
    }

    private void OnFrameManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      if (ListPickerMode.Expanded != this.ListPickerMode)
        return;
      DependencyObject dependencyObject1 = ((RoutedEventArgs) e).OriginalSource as DependencyObject;
      DependencyObject dependencyObject2 = (DependencyObject) (this._itemsPresenterHostPart ?? (Canvas) this);
      for (; dependencyObject1 != null; dependencyObject1 = VisualTreeHelper.GetParent(dependencyObject1))
      {
        if (dependencyObject2 == dependencyObject1)
          return;
      }
      this.ListPickerMode = ListPickerMode.Normal;
    }

    private void OnContainerTap(object sender, GestureEventArgs e)
    {
      if (ListPickerMode.Expanded != this.ListPickerMode)
        return;
      this.SelectedItem = this.ItemContainerGenerator.ItemFromContainer((DependencyObject) sender);
      this.ListPickerMode = ListPickerMode.Normal;
      e.Handled = true;
    }

    private void UpdateVisualStates(bool useTransitions)
    {
      if (!((Control) this).IsEnabled)
        VisualStateManager.GoToState((Control) this, "Disabled", useTransitions);
      else if (this.IsHighlighted)
        VisualStateManager.GoToState((Control) this, "Highlighted", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "Normal", useTransitions);
    }

    private void UpdateSummary(IList newValue)
    {
      string str = (string) null;
      if (this.SummaryForSelectedItemsDelegate != null)
        str = this.SummaryForSelectedItemsDelegate(newValue);
      if (str == null)
        str = newValue == null || newValue.Count == 0 ? " " : newValue[0].ToString();
      if (string.IsNullOrEmpty(str))
        str = " ";
      if (this._multipleSelectionModeSummary == null)
        return;
      this._multipleSelectionModeSummary.Text = str;
    }

    private void OpenPickerPage()
    {
      if ((Uri) null == this.PickerPageUri)
        throw new ArgumentException("PickerPageUri");
      if (this._frame != null)
        return;
      this._frame = Application.Current.RootVisual as PhoneApplicationFrame;
      if (this._frame == null)
        return;
      this._frameContentWhenOpened = ((ContentControl) this._frame).Content;
      if (this._frameContentWhenOpened is UIElement contentWhenOpened)
      {
        this._savedNavigationInTransition = TransitionService.GetNavigationInTransition(contentWhenOpened);
        TransitionService.SetNavigationInTransition(contentWhenOpened, (NavigationInTransition) null);
        this._savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(contentWhenOpened);
        TransitionService.SetNavigationOutTransition(contentWhenOpened, (NavigationOutTransition) null);
      }
      ((Frame) this._frame).Navigated += new NavigatedEventHandler(this.OnFrameNavigated);
      ((Frame) this._frame).NavigationStopped += new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      ((Frame) this._frame).NavigationFailed += new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
      this._hasPickerPageOpen = true;
      ((Frame) this._frame).Navigate(this.PickerPageUri);
    }

    private void ClosePickerPage()
    {
      if (this._frame != null)
      {
        ((Frame) this._frame).Navigated -= new NavigatedEventHandler(this.OnFrameNavigated);
        ((Frame) this._frame).NavigationStopped -= new NavigationStoppedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        ((Frame) this._frame).NavigationFailed -= new NavigationFailedEventHandler(this.OnFrameNavigationStoppedOrFailed);
        if (this._frameContentWhenOpened is UIElement contentWhenOpened)
        {
          TransitionService.SetNavigationInTransition(contentWhenOpened, this._savedNavigationInTransition);
          this._savedNavigationInTransition = (NavigationInTransition) null;
          TransitionService.SetNavigationOutTransition(contentWhenOpened, this._savedNavigationOutTransition);
          this._savedNavigationOutTransition = (NavigationOutTransition) null;
        }
        this._frame = (PhoneApplicationFrame) null;
        this._frameContentWhenOpened = (object) null;
      }
      if (this._listPickerPage == null)
        return;
      if (this.SelectionMode == null && this._listPickerPage.SelectedItem != null)
        this.SelectedItem = this._listPickerPage.SelectedItem;
      else if ((this.SelectionMode == 1 || this.SelectionMode == 2) && this._listPickerPage.SelectedItems != null)
        this.SelectedItems = this._listPickerPage.SelectedItems;
      this._listPickerPage = (ListPickerPage) null;
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
      if (e.Content == this._frameContentWhenOpened)
      {
        this.ListPickerMode = ListPickerMode.Normal;
      }
      else
      {
        if (this._listPickerPage != null || !this._hasPickerPageOpen)
          return;
        this._hasPickerPageOpen = false;
        this._listPickerPage = e.Content as ListPickerPage;
        if (this._listPickerPage == null)
          return;
        this._listPickerPage.HeaderText = this.FullModeHeader == null ? (string) this.Header : (string) this.FullModeHeader;
        this._listPickerPage.FullModeItemTemplate = this.FullModeItemTemplate;
        this._listPickerPage.Items.Clear();
        if (this.Items != null)
        {
          foreach (object obj in (PresentationFrameworkCollection<object>) this.Items)
            this._listPickerPage.Items.Add(obj);
        }
        this._listPickerPage.SelectionMode = this.SelectionMode;
        if (this.SelectionMode == null)
        {
          this._listPickerPage.SelectedItem = this.SelectedItem;
        }
        else
        {
          this._listPickerPage.SelectedItems.Clear();
          if (this.SelectedItems == null)
            return;
          foreach (object selectedItem in (IEnumerable) this.SelectedItems)
            this._listPickerPage.SelectedItems.Add(selectedItem);
        }
      }
    }

    private void OnFrameNavigationStoppedOrFailed(object sender, EventArgs e) => this.ListPickerMode = ListPickerMode.Normal;
  }
}
