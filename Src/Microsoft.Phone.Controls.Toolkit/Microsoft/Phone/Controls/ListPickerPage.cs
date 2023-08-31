// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ListPickerPage
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.LocalizedResources;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
  public class ListPickerPage : PhoneApplicationPage
  {
    private const string StateKey_Value = "ListPickerPage_State_Value";
    private PageOrientation lastOrientation;
    private static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("isOIsOpenpen", typeof (bool), typeof (ListPickerPage), new PropertyMetadata((object) false, new PropertyChangedCallback(ListPickerPage.OnIsOpenChanged)));
    internal Grid MainGrid;
    internal TextBlock HeaderTitle;
    internal ListBox Picker;
    private bool _contentLoaded;

    public string HeaderText { get; set; }

    public IList Items { get; private set; }

    public SelectionMode SelectionMode { get; set; }

    public object SelectedItem { get; set; }

    public IList SelectedItems { get; private set; }

    public DataTemplate FullModeItemTemplate { get; set; }

    private bool IsOpen
    {
      get => (bool) ((DependencyObject) this).GetValue(ListPickerPage.IsOpenProperty);
      set => ((DependencyObject) this).SetValue(ListPickerPage.IsOpenProperty, (object) value);
    }

    private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => (o as ListPickerPage).OnIsOpenChanged();

    private void OnIsOpenChanged() => this.UpdateVisualState(true);

    public ListPickerPage()
    {
      this.InitializeComponent();
      this.Items = (IList) new List<object>();
      this.SelectedItems = (IList) new List<object>();
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.OnUnloaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.OnOrientationChanged);
      this.lastOrientation = this.Orientation;
      if (this.ApplicationBar != null)
      {
        foreach (object button in (IEnumerable) this.ApplicationBar.Buttons)
        {
          if (button is IApplicationBarIconButton iapplicationBarIconButton)
          {
            if ("DONE" == ((IApplicationBarMenuItem) iapplicationBarIconButton).Text)
            {
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Text = ControlResources.DateTimePickerDoneText;
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Click += new EventHandler(this.OnDoneButtonClick);
            }
            else if ("CANCEL" == ((IApplicationBarMenuItem) iapplicationBarIconButton).Text)
            {
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Text = ControlResources.DateTimePickerCancelText;
              ((IApplicationBarMenuItem) iapplicationBarIconButton).Click += new EventHandler(this.OnCancelButtonClick);
            }
          }
        }
      }
      this.SetupListItems(-90.0);
      PlaneProjection planeProjection = (PlaneProjection) ((UIElement) this.HeaderTitle).Projection;
      if (planeProjection == null)
      {
        planeProjection = new PlaneProjection();
        ((UIElement) this.HeaderTitle).Projection = (Projection) planeProjection;
      }
      planeProjection.RotationX = -90.0;
      ((UIElement) this.Picker).Opacity = 1.0;
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() => this.IsOpen = true));
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) => this.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.OnOrientationChanged);

    private void SetupListItems(double degree)
    {
      for (int index = 0; index < ((PresentationFrameworkCollection<object>) ((ItemsControl) this.Picker).Items).Count; ++index)
      {
        FrameworkElement frameworkElement = (FrameworkElement) ((ItemsControl) this.Picker).ItemContainerGenerator.ContainerFromIndex(index);
        if (frameworkElement != null)
        {
          PlaneProjection planeProjection = (PlaneProjection) ((UIElement) frameworkElement).Projection;
          if (planeProjection == null)
          {
            planeProjection = new PlaneProjection();
            ((UIElement) frameworkElement).Projection = (Projection) planeProjection;
          }
          planeProjection.RotationX = degree;
        }
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Page) this).OnNavigatedTo(e);
      if (this.State.ContainsKey("ListPickerPage_State_Value"))
      {
        this.State.Remove("ListPickerPage_State_Value");
        if (((Page) this).NavigationService.CanGoBack)
        {
          ((Page) this).NavigationService.GoBack();
          return;
        }
      }
      if (this.HeaderText != null)
        this.HeaderTitle.Text = this.HeaderText.ToUpper(CultureInfo.CurrentCulture);
      ((FrameworkElement) this.Picker).DataContext = (object) this.Items;
      this.Picker.SelectionMode = this.SelectionMode;
      if (this.FullModeItemTemplate != null)
        ((ItemsControl) this.Picker).ItemTemplate = this.FullModeItemTemplate;
      if (this.SelectionMode == null)
      {
        this.ApplicationBar.IsVisible = false;
        ((Selector) this.Picker).SelectedItem = this.SelectedItem;
      }
      else
      {
        this.ApplicationBar.IsVisible = true;
        this.Picker.ItemContainerStyle = (Style) ((FrameworkElement) this).Resources[(object) "ListBoxItemCheckedStyle"];
        foreach (object obj in (IEnumerable) this.Items)
        {
          if (this.SelectedItems != null && this.SelectedItems.Contains(obj))
            this.Picker.SelectedItems.Add(obj);
        }
      }
    }

    private void OnDoneButtonClick(object sender, EventArgs e)
    {
      this.SelectedItem = ((Selector) this.Picker).SelectedItem;
      this.SelectedItems = this.Picker.SelectedItems;
      this.ClosePickerPage();
    }

    private void OnCancelButtonClick(object sender, EventArgs e)
    {
      this.SelectedItem = (object) null;
      this.SelectedItems = (IList) null;
      this.ClosePickerPage();
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      e.Cancel = true;
      this.SelectedItem = (object) null;
      this.SelectedItems = (IList) null;
      this.ClosePickerPage();
    }

    private void ClosePickerPage() => this.IsOpen = false;

    private void OnClosedStoryboardCompleted(object sender, EventArgs e) => ((Page) this).NavigationService.GoBack();

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Page) this).OnNavigatedFrom(e);
      if (!e.Uri.IsExternalNavigation())
        return;
      this.State["ListPickerPage_State_Value"] = (object) "ListPickerPage_State_Value";
    }

    private void OnOrientationChanged(object sender, OrientationChangedEventArgs e)
    {
      PageOrientation orientation = e.Orientation;
      RotateTransition rotateTransition = new RotateTransition();
      if (this.MainGrid != null)
      {
        PageOrientation pageOrientation = orientation;
        switch (pageOrientation - 1)
        {
          case 0:
          case 4:
            ((FrameworkElement) this.HeaderTitle).Margin = new Thickness(20.0, 12.0, 12.0, 12.0);
            ((FrameworkElement) this.Picker).Margin = new Thickness(8.0, 12.0, 0.0, 0.0);
            rotateTransition.Mode = this.lastOrientation == 18 ? RotateTransitionMode.In90Counterclockwise : RotateTransitionMode.In90Clockwise;
            break;
          case 1:
            ((FrameworkElement) this.HeaderTitle).Margin = new Thickness(72.0, 0.0, 0.0, 0.0);
            ((FrameworkElement) this.Picker).Margin = new Thickness(60.0, 0.0, 0.0, 0.0);
            rotateTransition.Mode = this.lastOrientation == 34 ? RotateTransitionMode.In180Counterclockwise : RotateTransitionMode.In90Clockwise;
            break;
          case 2:
          case 3:
            break;
          default:
            if (pageOrientation != 18)
            {
              if (pageOrientation == 34)
              {
                ((FrameworkElement) this.HeaderTitle).Margin = new Thickness(20.0, 0.0, 0.0, 0.0);
                ((FrameworkElement) this.Picker).Margin = new Thickness(8.0, 0.0, 0.0, 0.0);
                rotateTransition.Mode = this.lastOrientation == 5 ? RotateTransitionMode.In90Counterclockwise : RotateTransitionMode.In180Clockwise;
                break;
              }
              break;
            }
            goto case 1;
        }
      }
      PhoneApplicationPage content = (PhoneApplicationPage) ((ContentControl) Application.Current.RootVisual).Content;
      ITransition transition = rotateTransition.GetTransition((UIElement) content);
      transition.Completed += (EventHandler) delegate
      {
        transition.Stop();
      };
      transition.Begin();
      this.lastOrientation = orientation;
    }

    private void UpdateVisualState(bool useTransitions)
    {
      if (useTransitions)
      {
        if (!this.IsOpen)
          this.SetupListItems(0.0);
        Storyboard storyboard1 = new Storyboard();
        Storyboard storyboard2 = this.AnimationForElement((FrameworkElement) this.HeaderTitle, 0);
        ((PresentationFrameworkCollection<Timeline>) storyboard1.Children).Add((Timeline) storyboard2);
        IList<WeakReference> itemsInViewPort = ItemsControlExtensions.GetItemsInViewPort((ItemsControl) this.Picker);
        for (int index = 0; index < itemsInViewPort.Count; ++index)
        {
          Storyboard storyboard3 = this.AnimationForElement((FrameworkElement) itemsInViewPort[index].Target, index + 1);
          ((PresentationFrameworkCollection<Timeline>) storyboard1.Children).Add((Timeline) storyboard3);
        }
        ((DependencyObject) this).Dispatcher.BeginInvoke(new Action(this.UpdateOutOfViewItems));
        if (!this.IsOpen)
          ((Timeline) storyboard1).Completed += new EventHandler(this.OnClosedStoryboardCompleted);
        storyboard1.Begin();
      }
      else
      {
        if (this.IsOpen)
          return;
        this.OnClosedStoryboardCompleted((object) null, (EventArgs) null);
      }
    }

    private Storyboard AnimationForElement(FrameworkElement element, int index)
    {
      double num1 = 30.0;
      double num2 = this.IsOpen ? 350.0 : 250.0;
      double num3 = this.IsOpen ? -45.0 : 0.0;
      double num4 = this.IsOpen ? 0.0 : 90.0;
      ExponentialEase exponentialEase1 = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase1).EasingMode = this.IsOpen ? (EasingMode) 0 : (EasingMode) 1;
      exponentialEase1.Exponent = 5.0;
      ExponentialEase exponentialEase2 = exponentialEase1;
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      ((Timeline) doubleAnimation1).Duration = new Duration(TimeSpan.FromMilliseconds(num2));
      doubleAnimation1.From = new double?(num3);
      doubleAnimation1.To = new double?(num4);
      doubleAnimation1.EasingFunction = (IEasingFunction) exponentialEase2;
      DoubleAnimation doubleAnimation2 = doubleAnimation1;
      Storyboard.SetTarget((Timeline) doubleAnimation2, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) doubleAnimation2, new PropertyPath("(UIElement.Projection).(PlaneProjection.RotationX)", new object[0]));
      Storyboard storyboard = new Storyboard();
      ((Timeline) storyboard).BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(num1 * (double) index));
      ((PresentationFrameworkCollection<Timeline>) storyboard.Children).Add((Timeline) doubleAnimation2);
      return storyboard;
    }

    private void UpdateOutOfViewItems()
    {
      IList<WeakReference> itemsInViewPort = ItemsControlExtensions.GetItemsInViewPort((ItemsControl) this.Picker);
      for (int index = 0; index < ((PresentationFrameworkCollection<object>) ((ItemsControl) this.Picker).Items).Count; ++index)
      {
        FrameworkElement frameworkElement = (FrameworkElement) ((ItemsControl) this.Picker).ItemContainerGenerator.ContainerFromIndex(index);
        if (frameworkElement != null)
        {
          bool flag = false;
          foreach (WeakReference weakReference in (IEnumerable<WeakReference>) itemsInViewPort)
          {
            if (weakReference.Target == frameworkElement)
              flag = true;
          }
          if (!flag)
          {
            ((UIElement) frameworkElement).Opacity = this.IsOpen ? 1.0 : 0.0;
            if (((UIElement) frameworkElement).Projection is PlaneProjection projection)
              projection.RotationX = 0.0;
          }
        }
      }
    }

    private void OnPickerTapped(object sender, GestureEventArgs e)
    {
      if (this.SelectionMode != null)
        return;
      this.SelectedItem = ((Selector) this.Picker).SelectedItem;
      this.ClosePickerPage();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft.Phone.Controls.Toolkit;component/ListPicker/ListPickerPage.xaml", UriKind.Relative));
      this.MainGrid = (Grid) ((FrameworkElement) this).FindName("MainGrid");
      this.HeaderTitle = (TextBlock) ((FrameworkElement) this).FindName("HeaderTitle");
      this.Picker = (ListBox) ((FrameworkElement) this).FindName("Picker");
    }
  }
}
