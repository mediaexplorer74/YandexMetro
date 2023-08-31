// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.UnloadingPivot
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Yandex.Controls
{
  internal class UnloadingPivot : Pivot
  {
    private DataTemplate _emptyItemTemplate;
    public static readonly DependencyProperty EmptyItemTemplateProperty = DependencyProperty.Register(nameof (EmptyItemTemplate), typeof (DataTemplate), typeof (UnloadingPivot), new PropertyMetadata((object) null));

    public DataTemplate EmptyItemTemplate
    {
      get => ((DependencyObject) this).GetValue(UnloadingPivot.EmptyItemTemplateProperty) is DataTemplate dataTemplate ? dataTemplate : this.DeafultEmptyItemTemplate;
      set => ((DependencyObject) this).SetValue(UnloadingPivot.EmptyItemTemplateProperty, (object) value);
    }

    private DataTemplate DeafultEmptyItemTemplate => this._emptyItemTemplate ?? (this._emptyItemTemplate = new DataTemplate());

    protected override void OnLoadedPivotItem(PivotItem item)
    {
      base.OnLoadedPivotItem(item);
      ContentControl contentControl = (ContentControl) item;
      if (contentControl == null)
        return;
      contentControl.ContentTemplate = this.ItemTemplate;
    }

    protected override void OnUnloadedPivotItem(PivotItemEventArgs e)
    {
      base.OnUnloadedPivotItem(e);
      ContentControl contentControl = (ContentControl) e.Item;
      if (contentControl == null)
        return;
      contentControl.ContentTemplate = this.EmptyItemTemplate;
    }
  }
}
