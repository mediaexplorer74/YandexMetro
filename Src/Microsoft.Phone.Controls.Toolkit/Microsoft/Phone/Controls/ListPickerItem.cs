// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ListPickerItem
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  [TemplateVisualState(GroupName = "SelectionStates", Name = "Selected")]
  [TemplateVisualState(GroupName = "SelectionStates", Name = "Unselected")]
  public class ListPickerItem : ContentControl
  {
    private const string SelectionStatesGroupName = "SelectionStates";
    private const string SelectionStatesUnselectedStateName = "Unselected";
    private const string SelectionStatesSelectedStateName = "Selected";
    private bool _isSelected;

    public ListPickerItem() => ((Control) this).DefaultStyleKey = (object) typeof (ListPickerItem);

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      VisualStateManager.GoToState((Control) this, this.IsSelected ? "Selected" : "Unselected", false);
    }

    internal bool IsSelected
    {
      get => this._isSelected;
      set
      {
        this._isSelected = value;
        VisualStateManager.GoToState((Control) this, this._isSelected ? "Selected" : "Unselected", true);
      }
    }
  }
}
