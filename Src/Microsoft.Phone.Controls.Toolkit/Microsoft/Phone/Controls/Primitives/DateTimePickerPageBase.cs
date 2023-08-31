// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.DateTimePickerPageBase
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.LocalizedResources;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls.Primitives
{
  public abstract class DateTimePickerPageBase : PhoneApplicationPage, IDateTimePickerPage
  {
    private const string VisibilityGroupName = "VisibilityStates";
    private const string OpenVisibilityStateName = "Open";
    private const string ClosedVisibilityStateName = "Closed";
    private const string StateKey_Value = "DateTimePickerPageBase_State_Value";
    private LoopingSelector _primarySelectorPart;
    private LoopingSelector _secondarySelectorPart;
    private LoopingSelector _tertiarySelectorPart;
    private Storyboard _closedStoryboard;
    private DateTime? _value;

    protected void InitializeDateTimePickerPage(
      LoopingSelector primarySelector,
      LoopingSelector secondarySelector,
      LoopingSelector tertiarySelector)
    {
      if (primarySelector == null)
        throw new ArgumentNullException(nameof (primarySelector));
      if (secondarySelector == null)
        throw new ArgumentNullException(nameof (secondarySelector));
      if (tertiarySelector == null)
        throw new ArgumentNullException(nameof (tertiarySelector));
      this._primarySelectorPart = primarySelector;
      this._secondarySelectorPart = secondarySelector;
      this._tertiarySelectorPart = tertiarySelector;
      this._primarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._secondarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._tertiarySelectorPart.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(this.OnDataSourceSelectionChanged);
      this._primarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      this._secondarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      this._tertiarySelectorPart.IsExpandedChanged += new DependencyPropertyChangedEventHandler(this.OnSelectorIsExpandedChanged);
      ((UIElement) this._primarySelectorPart).Visibility = (Visibility) 1;
      ((UIElement) this._secondarySelectorPart).Visibility = (Visibility) 1;
      ((UIElement) this._tertiarySelectorPart).Visibility = (Visibility) 1;
      int num = 0;
      foreach (LoopingSelector loopingSelector in this.GetSelectorsOrderedByCulturePattern())
      {
        Grid.SetColumn((FrameworkElement) loopingSelector, num);
        ((UIElement) loopingSelector).Visibility = (Visibility) 0;
        ++num;
      }
      if (VisualTreeHelper.GetChild((DependencyObject) this, 0) is FrameworkElement child)
      {
        foreach (VisualStateGroup visualStateGroup in (IEnumerable) VisualStateManager.GetVisualStateGroups(child))
        {
          if ("VisibilityStates" == visualStateGroup.Name)
          {
            foreach (VisualState state in (IEnumerable) visualStateGroup.States)
            {
              if ("Closed" == state.Name && state.Storyboard != null)
              {
                this._closedStoryboard = state.Storyboard;
                ((Timeline) this._closedStoryboard).Completed += new EventHandler(this.OnClosedStoryboardCompleted);
              }
            }
          }
        }
      }
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
      VisualStateManager.GoToState((Control) this, "Open", true);
    }

    private void OnDataSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DataSource dataSource = (DataSource) sender;
      this._primarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._secondarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
      this._tertiarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
    }

    private void OnSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
        return;
      this._primarySelectorPart.IsExpanded = sender == this._primarySelectorPart;
      this._secondarySelectorPart.IsExpanded = sender == this._secondarySelectorPart;
      this._tertiarySelectorPart.IsExpanded = sender == this._tertiarySelectorPart;
    }

    private void OnDoneButtonClick(object sender, EventArgs e)
    {
      this._value = new DateTime?(((DateTimeWrapper) this._primarySelectorPart.DataSource.SelectedItem).DateTime);
      this.ClosePickerPage();
    }

    private void OnCancelButtonClick(object sender, EventArgs e)
    {
      this._value = new DateTime?();
      this.ClosePickerPage();
    }

    protected virtual void OnBackKeyPress(CancelEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      e.Cancel = true;
      this.ClosePickerPage();
    }

    private void ClosePickerPage()
    {
      if (this._closedStoryboard != null)
        VisualStateManager.GoToState((Control) this, "Closed", true);
      else
        this.OnClosedStoryboardCompleted((object) null, (EventArgs) null);
    }

    private void OnClosedStoryboardCompleted(object sender, EventArgs e) => ((Page) this).NavigationService.GoBack();

    protected abstract IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern();

    protected static IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern(
      string pattern,
      char[] patternCharacters,
      LoopingSelector[] selectors)
    {
      if (pattern == null)
        throw new ArgumentNullException(nameof (pattern));
      if (patternCharacters == null)
        throw new ArgumentNullException(nameof (patternCharacters));
      if (selectors == null)
        throw new ArgumentNullException(nameof (selectors));
      if (patternCharacters.Length != selectors.Length)
        throw new ArgumentException("Arrays must contain the same number of elements.");
      List<Tuple<int, LoopingSelector>> source = new List<Tuple<int, LoopingSelector>>(patternCharacters.Length);
      for (int index = 0; index < patternCharacters.Length; ++index)
        source.Add(new Tuple<int, LoopingSelector>(pattern.IndexOf(patternCharacters[index]), selectors[index]));
      return source.Where<Tuple<int, LoopingSelector>>((Func<Tuple<int, LoopingSelector>, bool>) (p => -1 != p.Item1)).OrderBy<Tuple<int, LoopingSelector>, int>((Func<Tuple<int, LoopingSelector>, int>) (p => p.Item1)).Select<Tuple<int, LoopingSelector>, LoopingSelector>((Func<Tuple<int, LoopingSelector>, LoopingSelector>) (p => p.Item2)).Where<LoopingSelector>((Func<LoopingSelector, bool>) (s => null != s));
    }

    public DateTime? Value
    {
      get => this._value;
      set
      {
        this._value = value;
        DateTimeWrapper dateTimeWrapper = new DateTimeWrapper(this._value.GetValueOrDefault(DateTime.Now));
        this._primarySelectorPart.DataSource.SelectedItem = (object) dateTimeWrapper;
        this._secondarySelectorPart.DataSource.SelectedItem = (object) dateTimeWrapper;
        this._tertiarySelectorPart.DataSource.SelectedItem = (object) dateTimeWrapper;
      }
    }

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Page) this).OnNavigatedFrom(e);
      if (!("app://external/" == e.Uri.ToString()))
        return;
      this.State["DateTimePickerPageBase_State_Value"] = (object) this.Value;
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      ((Page) this).OnNavigatedTo(e);
      if (!this.State.ContainsKey("DateTimePickerPageBase_State_Value"))
        return;
      this.Value = this.State["DateTimePickerPageBase_State_Value"] as DateTime?;
      if (!((Page) this).NavigationService.CanGoBack)
        return;
      ((Page) this).NavigationService.GoBack();
    }
  }
}
