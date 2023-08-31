// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.TemplatedItemsControl`1
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls.Primitives
{
  public class TemplatedItemsControl<T> : ItemsControl where T : FrameworkElement, new()
  {
    private readonly Dictionary<object, T> _itemToContainer = new Dictionary<object, T>();
    private readonly Dictionary<T, object> _containerToItem = new Dictionary<T, object>();
    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof (ItemContainerStyle), typeof (Style), typeof (TemplatedItemsControl<T>), (PropertyMetadata) null);

    public Style ItemContainerStyle
    {
      get => ((DependencyObject) this).GetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty) as Style;
      set => ((DependencyObject) this).SetValue(TemplatedItemsControl<T>.ItemContainerStyleProperty, (object) value);
    }

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is T;

    protected virtual DependencyObject GetContainerForItemOverride()
    {
      T container = new T();
      this.ApplyItemContainerStyle((DependencyObject) (object) container);
      return (DependencyObject) (object) container;
    }

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      this.ApplyItemContainerStyle(element);
      base.PrepareContainerForItemOverride(element, item);
      this._itemToContainer[item] = (T) element;
      this._containerToItem[(T) element] = item;
    }

    protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      base.ClearContainerForItemOverride(element, item);
      this._itemToContainer.Remove(item);
      this._containerToItem.Remove((T) element);
    }

    protected virtual void ApplyItemContainerStyle(DependencyObject container)
    {
      if (!(container is T obj) || obj.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue)
        return;
      Style itemContainerStyle = this.ItemContainerStyle;
      if (itemContainerStyle != null)
        obj.Style = itemContainerStyle;
      else
        obj.ClearValue(FrameworkElement.StyleProperty);
    }

    protected object GetItem(T container)
    {
      object obj = (object) null;
      if ((object) container != null)
        this._containerToItem.TryGetValue(container, out obj);
      return obj;
    }

    protected T GetContainer(object item)
    {
      T container = default (T);
      if (item != null)
        this._itemToContainer.TryGetValue(item, out container);
      return container;
    }
  }
}
