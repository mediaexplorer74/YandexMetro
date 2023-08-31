// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.MultiselectList
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (MultiselectList))]
  public class MultiselectList : ItemsControl
  {
    public static readonly DependencyProperty IsInSelectionModeProperty = DependencyProperty.Register(nameof (IsSelectionEnabled), typeof (bool), typeof (MultiselectList), new PropertyMetadata((object) false, new PropertyChangedCallback(MultiselectList.OnIsSelectionEnabledPropertyChanged)));
    public static readonly DependencyProperty ItemInfoTemplateProperty = DependencyProperty.Register(nameof (ItemInfoTemplate), typeof (DataTemplate), typeof (MultiselectList), new PropertyMetadata((object) null, (PropertyChangedCallback) null));
    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof (ItemContainerStyle), typeof (Style), typeof (MultiselectList), new PropertyMetadata((object) null, (PropertyChangedCallback) null));

    public IList SelectedItems { get; private set; }

    public event SelectionChangedEventHandler SelectionChanged;

    public event DependencyPropertyChangedEventHandler IsSelectionEnabledChanged;

    public bool IsSelectionEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(MultiselectList.IsInSelectionModeProperty);
      set => ((DependencyObject) this).SetValue(MultiselectList.IsInSelectionModeProperty, (object) value);
    }

    private static void OnIsSelectionEnabledPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      MultiselectList multiselectList = (MultiselectList) obj;
      if ((bool) e.NewValue)
      {
        multiselectList.OpenSelection();
      }
      else
      {
        if (multiselectList.SelectedItems.Count > 0)
        {
          IList removedItems = (IList) new List<object>();
          foreach (object selectedItem in (IEnumerable) multiselectList.SelectedItems)
            removedItems.Add(selectedItem);
          for (int index = 0; index < ((PresentationFrameworkCollection<object>) multiselectList.Items).Count && multiselectList.SelectedItems.Count > 0; ++index)
          {
            MultiselectItem multiselectItem = (MultiselectItem) multiselectList.ItemContainerGenerator.ContainerFromIndex(index);
            if (multiselectItem != null && multiselectItem.IsSelected)
            {
              multiselectItem._canTriggerSelectionChanged = false;
              multiselectItem.IsSelected = false;
              multiselectItem._canTriggerSelectionChanged = true;
            }
          }
          multiselectList.SelectedItems.Clear();
          multiselectList.OnSelectionChanged(removedItems, (IList) new object[0]);
        }
        multiselectList.CloseSelection();
      }
      DependencyPropertyChangedEventHandler selectionEnabledChanged = multiselectList.IsSelectionEnabledChanged;
      if (selectionEnabledChanged == null)
        return;
      selectionEnabledChanged((object) obj, e);
    }

    public DataTemplate ItemInfoTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(MultiselectList.ItemInfoTemplateProperty);
      set => ((DependencyObject) this).SetValue(MultiselectList.ItemInfoTemplateProperty, (object) value);
    }

    public Style ItemContainerStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(MultiselectList.ItemContainerStyleProperty);
      set => ((DependencyObject) this).SetValue(MultiselectList.ItemContainerStyleProperty, (object) value);
    }

    public MultiselectList()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (MultiselectList);
      this.SelectedItems = (IList) new List<object>();
    }

    internal void OnSelectionChanged(IList removedItems, IList addedItems)
    {
      if (this.SelectedItems.Count <= 0)
        this.IsSelectionEnabled = false;
      else if (this.SelectedItems.Count == 1 && removedItems.Count == 0)
        this.IsSelectionEnabled = true;
      SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, new SelectionChangedEventArgs(removedItems, addedItems));
    }

    private void OpenSelection()
    {
      foreach (WeakReference weakReference in (IEnumerable<WeakReference>) ItemsControlExtensions.GetItemsInViewPort((ItemsControl) this))
      {
        MultiselectItem target = (MultiselectItem) weakReference.Target;
        target.State = SelectionEnabledState.Opened;
        target.UpdateVisualState(true);
      }
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        for (int index = 0; index < ((PresentationFrameworkCollection<object>) this.Items).Count; ++index)
        {
          MultiselectItem multiselectItem = (MultiselectItem) this.ItemContainerGenerator.ContainerFromIndex(index);
          if (multiselectItem != null)
          {
            multiselectItem.State = SelectionEnabledState.Opened;
            multiselectItem.UpdateVisualState(false);
          }
        }
      }));
    }

    private void CloseSelection()
    {
      foreach (WeakReference weakReference in (IEnumerable<WeakReference>) ItemsControlExtensions.GetItemsInViewPort((ItemsControl) this))
      {
        MultiselectItem target = (MultiselectItem) weakReference.Target;
        target.State = SelectionEnabledState.Closed;
        target.UpdateVisualState(true);
      }
      ((DependencyObject) this).Dispatcher.BeginInvoke((Action) (() =>
      {
        for (int index = 0; index < ((PresentationFrameworkCollection<object>) this.Items).Count; ++index)
        {
          MultiselectItem multiselectItem = (MultiselectItem) this.ItemContainerGenerator.ContainerFromIndex(index);
          if (multiselectItem != null)
          {
            multiselectItem.State = SelectionEnabledState.Closed;
            multiselectItem.UpdateVisualState(false);
          }
        }
      }));
    }

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new MultiselectItem();

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is MultiselectItem;

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      MultiselectItem multiselectItem = (MultiselectItem) element;
      ((FrameworkElement) multiselectItem).Style = this.ItemContainerStyle;
      multiselectItem._isBeingVirtualized = true;
      multiselectItem.IsSelected = this.SelectedItems.Contains(item);
      multiselectItem.State = this.IsSelectionEnabled ? SelectionEnabledState.Opened : SelectionEnabledState.Closed;
      multiselectItem.UpdateVisualState(false);
      multiselectItem._isBeingVirtualized = false;
    }

    protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (this.SelectedItems.Count <= 0)
        return;
      IList removedItems = (IList) new List<object>();
      for (int index = 0; index < this.SelectedItems.Count; ++index)
      {
        object selectedItem = this.SelectedItems[index];
        if (!((PresentationFrameworkCollection<object>) this.Items).Contains(selectedItem))
        {
          this.SelectedItems.Remove(selectedItem);
          removedItems.Add(selectedItem);
          --index;
        }
      }
      this.OnSelectionChanged(removedItems, (IList) new object[0]);
    }
  }
}
