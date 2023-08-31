// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PivotHeaderItem
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls.Primitives
{
  [TemplateVisualState(Name = "Selected", GroupName = "SelectionStates")]
  [TemplateVisualState(Name = "Unselected", GroupName = "SelectionStates")]
  public class PivotHeaderItem : ContentControl
  {
    private const string SelectedState = "Selected";
    private const string UnselectedState = "Unselected";
    private const string SelectionStatesGroup = "SelectionStates";
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (PivotHeaderItem), new PropertyMetadata((object) false, new PropertyChangedCallback(PivotHeaderItem.OnIsSelectedPropertyChanged)));

    public bool IsSelected
    {
      get => (bool) ((DependencyObject) this).GetValue(PivotHeaderItem.IsSelectedProperty);
      set => ((DependencyObject) this).SetValue(PivotHeaderItem.IsSelectedProperty, (object) value);
    }

    private static void OnIsSelectedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PivotHeaderItem pivotHeaderItem = d as PivotHeaderItem;
      if (pivotHeaderItem.ParentHeadersControl == null)
        return;
      pivotHeaderItem.ParentHeadersControl.NotifyHeaderItemSelected(pivotHeaderItem, (bool) e.NewValue);
      pivotHeaderItem.UpdateVisualStates(true);
    }

    internal PivotHeadersControl ParentHeadersControl { get; set; }

    internal object Item { get; set; }

    public PivotHeaderItem() => ((Control) this).DefaultStyleKey = (object) typeof (PivotHeaderItem);

    protected virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      ((Control) this).OnMouseLeftButtonUp(e);
      if (e == null || e.Handled)
        return;
      e.Handled = true;
      if (this.ParentHeadersControl == null)
        return;
      this.ParentHeadersControl.OnHeaderItemClicked(this);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.UpdateVisualStates(false);
    }

    internal void UpdateVisualStateToUnselected() => VisualStateManager.GoToState((Control) this, "Unselected", false);

    internal void RestoreVisualStates() => this.UpdateVisualStates(false);

    private void UpdateVisualStates(bool useTransitions) => VisualStateManager.GoToState((Control) this, this.IsSelected ? "Selected" : "Unselected", useTransitions);
  }
}
