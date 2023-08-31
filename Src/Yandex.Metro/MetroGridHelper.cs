// Decompiled with JetBrains decompiler
// Type: System.Windows.MetroGridHelper
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows
{
  public static class MetroGridHelper
  {
    private static bool _visible;
    private static double _opacity = 0.15;
    private static Color _color = Colors.Red;
    private static List<Rectangle> _squares;
    private static Grid _grid;

    public static bool IsVisible
    {
      get => MetroGridHelper._visible;
      set
      {
        MetroGridHelper._visible = value;
        MetroGridHelper.UpdateGrid();
      }
    }

    public static Color Color
    {
      get => MetroGridHelper._color;
      set
      {
        MetroGridHelper._color = value;
        MetroGridHelper.UpdateGrid();
      }
    }

    public static double Opacity
    {
      get => MetroGridHelper._opacity;
      set
      {
        MetroGridHelper._opacity = value;
        MetroGridHelper.UpdateGrid();
      }
    }

    private static void UpdateGrid()
    {
      if (MetroGridHelper._squares != null)
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(MetroGridHelper._color);
        foreach (Shape square in MetroGridHelper._squares)
          square.Fill = (Brush) solidColorBrush;
        if (MetroGridHelper._grid == null)
          return;
        ((UIElement) MetroGridHelper._grid).Visibility = MetroGridHelper._visible ? (Visibility) 0 : (Visibility) 1;
        ((UIElement) MetroGridHelper._grid).Opacity = MetroGridHelper._opacity;
      }
      else
        MetroGridHelper.BuildGrid();
    }

    private static void BuildGrid()
    {
      MetroGridHelper._squares = new List<Rectangle>();
      Frame frame = Application.Current.RootVisual as Frame;
      if (frame == null || VisualTreeHelper.GetChildrenCount((DependencyObject) frame) == 0)
      {
        ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke(new Action(MetroGridHelper.BuildGrid));
      }
      else
      {
        DependencyObject child = VisualTreeHelper.GetChild((DependencyObject) frame, 0);
        Border childAsBorder = child as Border;
        Grid parent1 = child as Grid;
        if (childAsBorder != null)
        {
          UIElement content = childAsBorder.Child;
          if (content == null)
          {
            ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke(new Action(MetroGridHelper.BuildGrid));
          }
          else
          {
            childAsBorder.Child = (UIElement) null;
            ((DependencyObject) Deployment.Current).Dispatcher.BeginInvoke((Action) (() =>
            {
              Grid parent2 = new Grid();
              childAsBorder.Child = (UIElement) parent2;
              ((PresentationFrameworkCollection<UIElement>) ((Panel) parent2).Children).Add(content);
              MetroGridHelper.PrepareGrid(frame, parent2);
            }));
          }
        }
        else
        {
          if (parent1 == null)
            return;
          MetroGridHelper.PrepareGrid(frame, parent1);
        }
      }
    }

    private static void PrepareGrid(Frame frame, Grid parent)
    {
      SolidColorBrush solidColorBrush = new SolidColorBrush(MetroGridHelper._color);
      MetroGridHelper._grid = new Grid();
      ((UIElement) MetroGridHelper._grid).IsHitTestVisible = false;
      double num = Math.Max(((FrameworkElement) frame).ActualWidth, ((FrameworkElement) frame).ActualHeight);
      for (int index1 = 24; (double) index1 < num; index1 += 37)
      {
        for (int index2 = 24; (double) index2 < num; index2 += 37)
        {
          Rectangle rectangle1 = new Rectangle();
          ((FrameworkElement) rectangle1).Width = 25.0;
          ((FrameworkElement) rectangle1).Height = 25.0;
          ((FrameworkElement) rectangle1).VerticalAlignment = (VerticalAlignment) 0;
          ((FrameworkElement) rectangle1).HorizontalAlignment = (HorizontalAlignment) 0;
          ((FrameworkElement) rectangle1).Margin = new Thickness((double) index1, (double) index2, 0.0, 0.0);
          ((UIElement) rectangle1).IsHitTestVisible = false;
          ((Shape) rectangle1).Fill = (Brush) solidColorBrush;
          Rectangle rectangle2 = rectangle1;
          ((PresentationFrameworkCollection<UIElement>) ((Panel) MetroGridHelper._grid).Children).Add((UIElement) rectangle2);
          MetroGridHelper._squares.Add(rectangle2);
        }
      }
      ((UIElement) MetroGridHelper._grid).Visibility = MetroGridHelper._visible ? (Visibility) 0 : (Visibility) 1;
      ((UIElement) MetroGridHelper._grid).Opacity = MetroGridHelper._opacity;
      ((UIElement) MetroGridHelper._grid).CacheMode = (CacheMode) new BitmapCache();
      ((PresentationFrameworkCollection<UIElement>) ((Panel) parent).Children).Add((UIElement) MetroGridHelper._grid);
    }
  }
}
