// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.DataSource
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  internal abstract class DataSource : ILoopingSelectorDataSource
  {
    private DateTimeWrapper _selectedItem;

    public object GetNext(object relativeTo)
    {
      DateTime? relativeTo1 = this.GetRelativeTo(((DateTimeWrapper) relativeTo).DateTime, 1);
      return !relativeTo1.HasValue ? (object) null : (object) new DateTimeWrapper(relativeTo1.Value);
    }

    public object GetPrevious(object relativeTo)
    {
      DateTime? relativeTo1 = this.GetRelativeTo(((DateTimeWrapper) relativeTo).DateTime, -1);
      return !relativeTo1.HasValue ? (object) null : (object) new DateTimeWrapper(relativeTo1.Value);
    }

    protected abstract DateTime? GetRelativeTo(DateTime relativeDate, int delta);

    public object SelectedItem
    {
      get => (object) this._selectedItem;
      set
      {
        if (value == this._selectedItem)
          return;
        DateTimeWrapper dateTimeWrapper = (DateTimeWrapper) value;
        if (dateTimeWrapper != null && this._selectedItem != null && !(dateTimeWrapper.DateTime != this._selectedItem.DateTime))
          return;
        object selectedItem = (object) this._selectedItem;
        this._selectedItem = dateTimeWrapper;
        EventHandler<SelectionChangedEventArgs> selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        selectionChanged((object) this, new SelectionChangedEventArgs((IList) new object[1]
        {
          selectedItem
        }, (IList) new object[1]
        {
          (object) this._selectedItem
        }));
      }
    }

    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
  }
}
