// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ItemsControlExtensions
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  internal static class ItemsControlExtensions
  {
    public static T GetParentItemsControl<T>(DependencyObject element) where T : ItemsControl
    {
      DependencyObject parent = VisualTreeHelper.GetParent(element);
      while (true)
      {
        switch (parent)
        {
          case T _:
          case null:
            goto label_3;
          default:
            parent = VisualTreeHelper.GetParent(parent);
            continue;
        }
      }
label_3:
      return (T) parent;
    }

    public static IList<WeakReference> GetItemsInViewPort(ItemsControl list)
    {
      IList<WeakReference> itemsInViewPort = (IList<WeakReference>) new List<WeakReference>();
      ScrollViewer child = VisualTreeHelper.GetChild((DependencyObject) list, 0) as ScrollViewer;
      ((UIElement) list).UpdateLayout();
      if (child == null)
        return itemsInViewPort;
      int num;
      GeneralTransform generalTransform;
      Rect rect;
      for (num = 0; num < ((PresentationFrameworkCollection<object>) list.Items).Count; ++num)
      {
        FrameworkElement target = (FrameworkElement) list.ItemContainerGenerator.ContainerFromIndex(num);
        if (target != null)
        {
          generalTransform = (GeneralTransform) null;
          GeneralTransform visual;
          try
          {
            visual = ((UIElement) target).TransformToVisual((UIElement) child);
          }
          catch (ArgumentException ex)
          {
            return itemsInViewPort;
          }
          rect = new Rect(visual.Transform(new Point()), visual.Transform(new Point(target.ActualWidth, target.ActualHeight)));
          if (rect.Bottom > 0.0)
          {
            itemsInViewPort.Add(new WeakReference((object) target));
            ++num;
            break;
          }
        }
      }
      for (; num < ((PresentationFrameworkCollection<object>) list.Items).Count; ++num)
      {
        FrameworkElement target = (FrameworkElement) list.ItemContainerGenerator.ContainerFromIndex(num);
        generalTransform = (GeneralTransform) null;
        GeneralTransform visual;
        try
        {
          visual = ((UIElement) target).TransformToVisual((UIElement) child);
        }
        catch (ArgumentException ex)
        {
          return itemsInViewPort;
        }
        rect = new Rect(visual.Transform(new Point()), visual.Transform(new Point(target.ActualWidth, target.ActualHeight)));
        if (rect.Top < ((FrameworkElement) child).ActualHeight)
          itemsInViewPort.Add(new WeakReference((object) target));
        else
          break;
      }
      return itemsInViewPort;
    }
  }
}
