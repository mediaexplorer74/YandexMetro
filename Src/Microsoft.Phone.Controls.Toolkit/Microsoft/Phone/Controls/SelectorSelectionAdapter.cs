// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.SelectorSelectionAdapter
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
  public class SelectorSelectionAdapter : ISelectionAdapter
  {
    private Selector _selector;

    private bool IgnoringSelectionChanged { get; set; }

    public Selector SelectorControl
    {
      get => this._selector;
      set
      {
        if (this._selector != null)
          this._selector.SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
        this._selector = value;
        if (this._selector == null)
          return;
        this._selector.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
      }
    }

    public event SelectionChangedEventHandler SelectionChanged;

    public event RoutedEventHandler Commit;

    public event RoutedEventHandler Cancel;

    public SelectorSelectionAdapter()
    {
    }

    public SelectorSelectionAdapter(Selector selector) => this.SelectorControl = selector;

    public object SelectedItem
    {
      get => this.SelectorControl != null ? this.SelectorControl.SelectedItem : (object) null;
      set
      {
        this.IgnoringSelectionChanged = true;
        if (this.SelectorControl != null)
          this.SelectorControl.SelectedItem = value;
        if (value == null)
          this.ResetScrollViewer();
        this.IgnoringSelectionChanged = false;
      }
    }

    public IEnumerable ItemsSource
    {
      get => this.SelectorControl != null ? ((ItemsControl) this.SelectorControl).ItemsSource : (IEnumerable) null;
      set
      {
        if (this.SelectorControl == null)
          return;
        ((ItemsControl) this.SelectorControl).ItemsSource = value;
      }
    }

    private void ResetScrollViewer()
    {
      if (this.SelectorControl == null)
        return;
      ((FrameworkElement) this.SelectorControl).GetLogicalChildrenBreadthFirst().OfType<ScrollViewer>().FirstOrDefault<ScrollViewer>()?.ScrollToVerticalOffset(0.0);
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.IgnoringSelectionChanged)
        return;
      SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged != null)
        selectionChanged(sender, e);
      this.OnCommit();
    }

    protected void SelectedIndexIncrement()
    {
      if (this.SelectorControl == null)
        return;
      this.SelectorControl.SelectedIndex = this.SelectorControl.SelectedIndex + 1 >= ((PresentationFrameworkCollection<object>) ((ItemsControl) this.SelectorControl).Items).Count ? -1 : this.SelectorControl.SelectedIndex + 1;
    }

    protected void SelectedIndexDecrement()
    {
      if (this.SelectorControl == null)
        return;
      int selectedIndex = this.SelectorControl.SelectedIndex;
      if (selectedIndex >= 0)
      {
        --this.SelectorControl.SelectedIndex;
      }
      else
      {
        if (selectedIndex != -1)
          return;
        this.SelectorControl.SelectedIndex = ((PresentationFrameworkCollection<object>) ((ItemsControl) this.SelectorControl).Items).Count - 1;
      }
    }

    public void HandleKeyDown(KeyEventArgs e)
    {
      Key key = e != null ? e.Key : throw new ArgumentNullException(nameof (e));
      if (key != 3)
      {
        if (key != 8)
        {
          switch (key - 15)
          {
            case 0:
              this.SelectedIndexDecrement();
              e.Handled = true;
              break;
            case 2:
              if ((1 & Keyboard.Modifiers) != null)
                break;
              this.SelectedIndexIncrement();
              e.Handled = true;
              break;
          }
        }
        else
        {
          this.OnCancel();
          e.Handled = true;
        }
      }
      else
      {
        this.OnCommit();
        e.Handled = true;
      }
    }

    protected virtual void OnCommit() => this.OnCommit((object) this, new RoutedEventArgs());

    private void OnCommit(object sender, RoutedEventArgs e)
    {
      RoutedEventHandler commit = this.Commit;
      if (commit != null)
        commit(sender, e);
      this.AfterAdapterAction();
    }

    protected virtual void OnCancel() => this.OnCancel((object) this, new RoutedEventArgs());

    private void OnCancel(object sender, RoutedEventArgs e)
    {
      RoutedEventHandler cancel = this.Cancel;
      if (cancel != null)
        cancel(sender, e);
      this.AfterAdapterAction();
    }

    private void AfterAdapterAction()
    {
      this.IgnoringSelectionChanged = true;
      if (this.SelectorControl != null)
      {
        this.SelectorControl.SelectedItem = (object) null;
        this.SelectorControl.SelectedIndex = -1;
      }
      this.IgnoringSelectionChanged = false;
    }

    public AutomationPeer CreateAutomationPeer() => this._selector == null ? (AutomationPeer) null : FrameworkElementAutomationPeer.CreatePeerForElement((UIElement) this._selector);
  }
}
