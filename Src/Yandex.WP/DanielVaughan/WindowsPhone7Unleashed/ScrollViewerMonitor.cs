// Decompiled with JetBrains decompiler
// Type: DanielVaughan.WindowsPhone7Unleashed.ScrollViewerMonitor
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DanielVaughan.WindowsPhone7Unleashed
{
  public class ScrollViewerMonitor
  {
    public static DependencyProperty AtEndCommandProperty = DependencyProperty.RegisterAttached("AtEndCommand", typeof (ICommand), typeof (ScrollViewerMonitor), new PropertyMetadata(new PropertyChangedCallback(ScrollViewerMonitor.OnAtEndCommandChanged)));

    public static ICommand GetAtEndCommand(DependencyObject obj) => (ICommand) obj.GetValue(ScrollViewerMonitor.AtEndCommandProperty);

    public static void SetAtEndCommand(DependencyObject obj, ICommand value) => obj.SetValue(ScrollViewerMonitor.AtEndCommandProperty, (object) value);

    public static void OnAtEndCommandChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FrameworkElement element = (FrameworkElement) d;
      if (element == null || ScrollViewerMonitor.TrySubscribeOffset(element))
        return;
      element.Loaded -= new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
      element.Loaded += new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
    }

    private static void ElementLoaded(object sender, RoutedEventArgs e)
    {
      FrameworkElement element = (FrameworkElement) sender;
      element.Loaded -= new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
      if (!ScrollViewerMonitor.TrySubscribeOffset(element))
        throw new InvalidOperationException("ScrollViewer not found.");
    }

    private static bool TrySubscribeOffset(FrameworkElement element)
    {
      ScrollViewer scrollViewer = ScrollViewerMonitor.FindChildOfType<ScrollViewer>((DependencyObject) element);
      if (scrollViewer == null)
        return false;
      DependencyPropertyListener propertyListener = new DependencyPropertyListener();
      propertyListener.Changed += (EventHandler<BindingChangedEventArgs>) delegate
      {
        if (scrollViewer.VerticalOffset <= 0.1 || scrollViewer.VerticalOffset < scrollViewer.ScrollableHeight - 1.0)
          return;
        ScrollViewerMonitor.GetAtEndCommand((DependencyObject) element)?.Execute((object) null);
      };
      Binding binding = new Binding("VerticalOffset")
      {
        Source = (object) scrollViewer
      };
      propertyListener.Attach((FrameworkElement) scrollViewer, binding);
      return true;
    }

    private static T FindChildOfType<T>(DependencyObject root) where T : class
    {
      Queue<DependencyObject> dependencyObjectQueue = new Queue<DependencyObject>();
      dependencyObjectQueue.Enqueue(root);
      while (dependencyObjectQueue.Count > 0)
      {
        DependencyObject dependencyObject = dependencyObjectQueue.Dequeue();
        for (int index = VisualTreeHelper.GetChildrenCount(dependencyObject) - 1; 0 <= index; --index)
        {
          DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
          if (child is T childOfType)
            return childOfType;
          dependencyObjectQueue.Enqueue(child);
        }
      }
      return default (T);
    }
  }
}
