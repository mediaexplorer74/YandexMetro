// Decompiled with JetBrains decompiler
// Type: Yandex.Extensions.GridHelper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace Yandex.Extensions
{
  public static class GridHelper
  {
    public static readonly DependencyProperty ItemsPerColumnProperty = DependencyProperty.RegisterAttached("ItemsPerColumn", typeof (int), typeof (GridHelper), new PropertyMetadata((object) 0, new PropertyChangedCallback(GridHelper.ItemsPerColumnChangedCallback)));
    public static readonly DependencyProperty ItemsPerRowProperty = DependencyProperty.RegisterAttached("ItemsPerRow", typeof (int), typeof (GridHelper), new PropertyMetadata((object) 0, new PropertyChangedCallback(GridHelper.ItemsPerRowChangedCallback)));
    public static readonly DependencyProperty OnePerColumnItemsPerRowProperty = DependencyProperty.RegisterAttached("OnePerColumnItemsPerRow", typeof (int), typeof (GridHelper), new PropertyMetadata((object) 0, new PropertyChangedCallback(GridHelper.OnePerColumnItemsPerRowChangedCallback)));

    private static void ItemsPerColumnChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      Grid grid = (Grid) dependencyObject;
      int itemsPerColumn = (int) e.NewValue;
      ((FrameworkElement) grid).LayoutUpdated += (EventHandler) ((s, e2) =>
      {
        int count = ((PresentationFrameworkCollection<UIElement>) ((Panel) grid).Children).Count;
        int num = (int) Math.Ceiling((double) count / (double) itemsPerColumn) - ((PresentationFrameworkCollection<ColumnDefinition>) grid.ColumnDefinitions).Count;
        for (int index = 0; index < num; ++index)
          ((PresentationFrameworkCollection<ColumnDefinition>) grid.ColumnDefinitions).Add(new ColumnDefinition());
        for (int index = 0; index < count; ++index)
          Grid.SetColumn((FrameworkElement) ((PresentationFrameworkCollection<UIElement>) ((Panel) grid).Children)[index], index / itemsPerColumn);
      });
    }

    public static void SetItemsPerColumn(Grid element, int value) => ((DependencyObject) element).SetValue(GridHelper.ItemsPerColumnProperty, (object) value);

    public static int GetItemsPerColumn(Grid element) => (int) ((DependencyObject) element).GetValue(GridHelper.ItemsPerColumnProperty);

    private static void ItemsPerRowChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      Grid grid = (Grid) dependencyObject;
      int itemsPerRow = (int) e.NewValue;
      ((FrameworkElement) grid).LayoutUpdated += (EventHandler) ((s, e2) => GridHelper.UpdateGridRows(grid, itemsPerRow, false));
    }

    private static void UpdateGridRows(Grid grid, int itemsPerRow, bool setColumns)
    {
      int count = ((PresentationFrameworkCollection<UIElement>) ((Panel) grid).Children).Count;
      int num1 = (int) Math.Ceiling((double) count / (double) itemsPerRow) - ((PresentationFrameworkCollection<RowDefinition>) grid.RowDefinitions).Count;
      for (int index = 0; index < num1; ++index)
        ((PresentationFrameworkCollection<RowDefinition>) grid.RowDefinitions).Add(new RowDefinition());
      int num2 = 0;
      int num3 = 0;
      for (int index = 0; index < count; ++index)
      {
        FrameworkElement child = (FrameworkElement) ((PresentationFrameworkCollection<UIElement>) ((Panel) grid).Children)[index];
        int num4 = index / itemsPerRow;
        if (num4 != num3)
          num2 = 0;
        num3 = num4;
        Grid.SetRow(child, num3);
        if (setColumns)
          Grid.SetColumn(child, num2++);
      }
    }

    public static void SetItemsPerRow(Grid element, int value) => ((DependencyObject) element).SetValue(GridHelper.ItemsPerRowProperty, (object) value);

    public static int GetItemsPerRow(Grid element) => (int) ((DependencyObject) element).GetValue(GridHelper.ItemsPerRowProperty);

    private static void OnePerColumnItemsPerRowChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      Grid grid = (Grid) dependencyObject;
      int itemsPerRow = (int) e.NewValue;
      ((FrameworkElement) grid).LayoutUpdated += (EventHandler) ((s, e2) => GridHelper.UpdateGridRows(grid, itemsPerRow, true));
    }

    public static void SetOnePerColumnItemsPerRow(Grid element, int value) => ((DependencyObject) element).SetValue(GridHelper.OnePerColumnItemsPerRowProperty, (object) value);

    public static int GetOnePerColumnItemsPerRow(Grid element) => (int) ((DependencyObject) element).GetValue(GridHelper.OnePerColumnItemsPerRowProperty);
  }
}
