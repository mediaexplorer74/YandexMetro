// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.MenuBase
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls.Primitives
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (MenuItem))]
  public abstract class MenuBase : ItemsControl
  {
    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof (ItemContainerStyle), typeof (Style), typeof (MenuBase), (PropertyMetadata) null);

    public Style ItemContainerStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(MenuBase.ItemContainerStyleProperty);
      set => ((DependencyObject) this).SetValue(MenuBase.ItemContainerStyleProperty, (object) value);
    }

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is MenuItem || item is Separator;

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new MenuItem();

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      if (!(element is MenuItem menuItem))
        return;
      menuItem.ParentMenuBase = this;
      if (menuItem == item)
        return;
      DataTemplate itemTemplate = this.ItemTemplate;
      Style itemContainerStyle = this.ItemContainerStyle;
      if (itemTemplate != null)
        ((DependencyObject) menuItem).SetValue(ItemsControl.ItemTemplateProperty, (object) itemTemplate);
      if (itemContainerStyle != null && MenuBase.HasDefaultValue((Control) menuItem, HeaderedItemsControl.ItemContainerStyleProperty))
        ((DependencyObject) menuItem).SetValue(HeaderedItemsControl.ItemContainerStyleProperty, (object) itemContainerStyle);
      if (MenuBase.HasDefaultValue((Control) menuItem, HeaderedItemsControl.HeaderProperty))
        menuItem.Header = item;
      if (itemTemplate != null)
        ((DependencyObject) menuItem).SetValue(HeaderedItemsControl.HeaderTemplateProperty, (object) itemTemplate);
      if (itemContainerStyle == null)
        return;
      ((DependencyObject) menuItem).SetValue(FrameworkElement.StyleProperty, (object) itemContainerStyle);
    }

    private static bool HasDefaultValue(Control control, DependencyProperty property) => ((DependencyObject) control).ReadLocalValue(property) == DependencyProperty.UnsetValue;
  }
}
