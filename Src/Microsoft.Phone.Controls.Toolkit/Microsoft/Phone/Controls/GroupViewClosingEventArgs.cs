// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.GroupViewClosingEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  public class GroupViewClosingEventArgs : EventArgs
  {
    internal GroupViewClosingEventArgs(ItemsControl itemsControl, object selectedGroup)
    {
      this.ItemsControl = itemsControl;
      this.SelectedGroup = selectedGroup;
    }

    public ItemsControl ItemsControl { get; private set; }

    public object SelectedGroup { get; private set; }

    public bool Cancel { get; set; }
  }
}
