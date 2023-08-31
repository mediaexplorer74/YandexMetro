// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.TemplatedListBox
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Microsoft.Phone.Controls.Primitives
{
  public class TemplatedListBox : ListBox
  {
    public DataTemplate ListHeaderTemplate { get; set; }

    public DataTemplate ListFooterTemplate { get; set; }

    public DataTemplate GroupHeaderTemplate { get; set; }

    public DataTemplate GroupFooterTemplate { get; set; }

    public event EventHandler<LinkUnlinkEventArgs> Link;

    public event EventHandler<LinkUnlinkEventArgs> Unlink;

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new TemplatedListBoxItem();

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      ((Selector) this).PrepareContainerForItemOverride(element, item);
      DataTemplate dataTemplate = (DataTemplate) null;
      if (!(item is LongListSelectorItem listSelectorItem))
        return;
      switch (listSelectorItem.ItemType)
      {
        case LongListSelectorItemType.Item:
          dataTemplate = ((ItemsControl) this).ItemTemplate;
          break;
        case LongListSelectorItemType.GroupHeader:
          dataTemplate = this.GroupHeaderTemplate;
          break;
        case LongListSelectorItemType.GroupFooter:
          dataTemplate = this.GroupFooterTemplate;
          break;
        case LongListSelectorItemType.ListHeader:
          dataTemplate = this.ListHeaderTemplate;
          break;
        case LongListSelectorItemType.ListFooter:
          dataTemplate = this.ListFooterTemplate;
          break;
      }
      TemplatedListBoxItem parent = (TemplatedListBoxItem) element;
      ((ContentControl) parent).Content = listSelectorItem.Item;
      parent.Tuple = listSelectorItem;
      ((ContentControl) parent).ContentTemplate = dataTemplate;
      ContentPresenter logicalChildByType = ((FrameworkElement) parent).GetFirstLogicalChildByType<ContentPresenter>(true);
      EventHandler<LinkUnlinkEventArgs> link = this.Link;
      if (logicalChildByType == null || link == null)
        return;
      link((object) this, new LinkUnlinkEventArgs(logicalChildByType));
    }

    protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      if (item is LongListSelectorItem)
      {
        ContentPresenter logicalChildByType = ((FrameworkElement) element).GetFirstLogicalChildByType<ContentPresenter>(true);
        EventHandler<LinkUnlinkEventArgs> unlink = this.Unlink;
        if (logicalChildByType != null && unlink != null)
          unlink((object) this, new LinkUnlinkEventArgs(logicalChildByType));
      }
      ((Selector) this).ClearContainerForItemOverride(element, item);
    }
  }
}
