// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.Partial.AutoComplete
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Y.UI.Common.Control;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views.Partial
{
  public class AutoComplete : UserControl
  {
    private readonly DispatcherTimer _delayTimer = new DispatcherTimer()
    {
      Interval = TimeSpan.FromMilliseconds(300.0)
    };
    private string _searchText;
    public static readonly DependencyProperty AutoCompleteStateProperty = DependencyProperty.Register(nameof (AutoCompleteState), typeof (AutoCompleteState), typeof (AutoComplete), new PropertyMetadata(new PropertyChangedCallback(AutoComplete.AutoCompleteStatePropertyChanged)));
    private bool _isLostFocus = true;
    private bool _isSkipAnimation;
    internal Grid LayoutRoot;
    internal VisualStateGroup VisualStateGroup;
    internal VisualState ShownEmpty;
    internal VisualState Shown;
    internal VisualState ShownFast;
    internal VisualState Hidden;
    internal VisualState Search;
    internal VisualState SearchEmpty;
    internal TextBox txtSearch;
    internal RoundButton btnOpen;
    internal RoundButton btnCancel;
    internal ScrollViewer Suggest;
    internal Border History;
    internal Border SuggestEmpty;
    private bool _contentLoaded;

    public AutoComplete()
    {
      this.InitializeComponent();
      this._delayTimer.Tick += new EventHandler(this.DelayTimerTick);
      Binding binding = new Binding(nameof (AutoCompleteState))
      {
        Source = ((FrameworkElement) this).DataContext
      };
      ((FrameworkElement) this).SetBinding(AutoComplete.AutoCompleteStateProperty, binding);
    }

    public AutoCompleteState AutoCompleteState
    {
      get => (AutoCompleteState) ((DependencyObject) this).GetValue(AutoComplete.AutoCompleteStateProperty);
      set => ((DependencyObject) this).SetValue(AutoComplete.AutoCompleteStateProperty, (object) value);
    }

    private static void AutoCompleteStatePropertyChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      AutoComplete autoComplete = (AutoComplete) sender;
      if (e.NewValue == null || autoComplete == null)
        return;
      VisualStateManager.GoToState((System.Windows.Controls.Control) autoComplete, e.NewValue.ToString(), true);
    }

    private void DelayTimerTick(object sender, EventArgs e)
    {
      this._delayTimer.Stop();
      this.Suggest.ScrollToVerticalOffset(0.0);
      Locator.MainStatic.StartAutocompleteCommand.Execute(this._searchText);
    }

    private void TxtSearchTextChanged(object sender, TextChangedEventArgs e)
    {
      if (this._searchText == this.txtSearch.Text)
        return;
      MetroService.Instance.UniqObject = new object();
      this._searchText = this.txtSearch.Text;
      ((UIElement) this.btnCancel).Visibility = string.IsNullOrEmpty(this._searchText) ? (Visibility) 1 : (Visibility) 0;
      ((UIElement) this.btnOpen).Visibility = string.IsNullOrEmpty(this._searchText) ? (Visibility) 0 : (Visibility) 1;
      this._delayTimer.Start();
    }

    private void TxtSearchLostFocus(object sender, RoutedEventArgs e)
    {
      if (!this._isLostFocus)
        return;
      this._isLostFocus = true;
      ThreadPool.QueueUserWorkItem((WaitCallback) (s =>
      {
        Thread.Sleep(100);
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          if (!this._isLostFocus)
            return;
          this.txtSearch.Text = string.Empty;
          bool isSkipAnimation = this._isSkipAnimation;
          this._isSkipAnimation = false;
          MainViewModel mainStatic = Locator.MainStatic;
          mainStatic.MetroMap.SetSearch(false, isSkipAnimation);
          if (!isSkipAnimation)
            return;
          mainStatic.OpenStationView.Execute(mainStatic.IsFromDirection);
        }));
      }));
    }

    private void OpenStationClick(object sender, RoutedEventArgs e) => this._isSkipAnimation = true;

    private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
    {
      this._isLostFocus = false;
      this.txtSearch.Text = string.Empty;
      ((System.Windows.Controls.Control) this.txtSearch).Focus();
    }

    private void ShownStoryboardCompleted(object sender, EventArgs e) => ((System.Windows.Controls.Control) this.txtSearch).Focus();

    private void TxtSearch_OnKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != 3 && e.PlatformKeyCode != 10)
        return;
      ((System.Windows.Controls.Control) (((FrameworkElement) this.txtSearch).Parent as FrameworkElement).FindPageBaseParent())?.Focus();
      Locator.MainStatic.SetFirstResultCommand.Execute((object) null);
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Yandex.Metro;component/Views/Partial/AutoComplete.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.VisualStateGroup = (VisualStateGroup) ((FrameworkElement) this).FindName("VisualStateGroup");
      this.ShownEmpty = (VisualState) ((FrameworkElement) this).FindName("ShownEmpty");
      this.Shown = (VisualState) ((FrameworkElement) this).FindName("Shown");
      this.ShownFast = (VisualState) ((FrameworkElement) this).FindName("ShownFast");
      this.Hidden = (VisualState) ((FrameworkElement) this).FindName("Hidden");
      this.Search = (VisualState) ((FrameworkElement) this).FindName("Search");
      this.SearchEmpty = (VisualState) ((FrameworkElement) this).FindName("SearchEmpty");
      this.txtSearch = (TextBox) ((FrameworkElement) this).FindName("txtSearch");
      this.btnOpen = (RoundButton) ((FrameworkElement) this).FindName("btnOpen");
      this.btnCancel = (RoundButton) ((FrameworkElement) this).FindName("btnCancel");
      this.Suggest = (ScrollViewer) ((FrameworkElement) this).FindName("Suggest");
      this.History = (Border) ((FrameworkElement) this).FindName("History");
      this.SuggestEmpty = (Border) ((FrameworkElement) this).FindName("SuggestEmpty");
    }
  }
}
