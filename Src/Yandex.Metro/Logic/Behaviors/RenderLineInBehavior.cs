// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Behaviors.RenderLineInBehavior
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using Y.Metro.ServiceLayer.FastScheme;
using Yandex.Metro.Views;

namespace Yandex.Metro.Logic.Behaviors
{
  public class RenderLineInBehavior : Behavior<Canvas>
  {
    public static readonly DependencyProperty RouteProperty = DependencyProperty.Register(nameof (Route), typeof (Route), typeof (RenderLineInBehavior), new PropertyMetadata((PropertyChangedCallback) null));

    public Route Route
    {
      get => (Route) this.GetValue(RenderLineInBehavior.RouteProperty);
      set => this.SetValue(RenderLineInBehavior.RouteProperty, (object) value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      ((FrameworkElement) this.AssociatedObject).Loaded += new RoutedEventHandler(this.AssociatedObjectLoaded);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      ((FrameworkElement) this.AssociatedObject).Loaded -= new RoutedEventHandler(this.AssociatedObjectLoaded);
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      Canvas associatedObject = this.AssociatedObject;
      double actualWidth = ((FrameworkElement) associatedObject).ActualWidth;
      int num1 = ((IEnumerable<int[]>) this.Route.RouteScheme.Transfers).Count<int[]>((Func<int[], bool>) (r => r.Length >= 2));
      int count = this.Route.SortStations.Count;
      int num2 = this.Route.SortStations.Count - 1;
      double num3 = (actualWidth - 6.0) / (double) (num2 - num1);
      double num4 = 0.0;
      int num5 = 0;
      for (int index = 0; index < count; ++index)
      {
        MetroStation sortStation = this.Route.SortStations[index];
        SolidColorBrush brush = MapGenerator.GetBrush(sortStation.LineReference.Color);
        bool flag = index == count - 1;
        MetroStation metroStation = (MetroStation) null;
        if (!flag)
          metroStation = this.Route.SortStations[index + 1];
        if (index == 0 || num5 != sortStation.LineId || flag || metroStation != null && sortStation.LineId != metroStation.LineId)
        {
          Ellipse ellipse1 = new Ellipse();
          ((FrameworkElement) ellipse1).Height = 6.0;
          ((FrameworkElement) ellipse1).Width = 6.0;
          ((Shape) ellipse1).Fill = (Brush) brush;
          Ellipse ellipse2 = ellipse1;
          Canvas.SetTop((UIElement) ellipse2, 2.0);
          Canvas.SetLeft((UIElement) ellipse2, num4);
          Canvas.SetZIndex((UIElement) ellipse2, 100);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) associatedObject).Children).Add((UIElement) ellipse2);
        }
        num5 = sortStation.LineId;
        if (index < num2)
        {
          if (metroStation != null && sortStation.LineId != metroStation.LineId)
          {
            Border border1 = new Border();
            border1.CornerRadius = new CornerRadius(4.0);
            ((FrameworkElement) border1).Height = 10.0;
            ((FrameworkElement) border1).Width = 21.0;
            border1.Background = (Brush) new SolidColorBrush(Colors.White);
            Border border2 = border1;
            Canvas.SetLeft((UIElement) border2, num4 - 3.0);
            Canvas.SetZIndex((UIElement) border2, 50);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) associatedObject).Children).Add((UIElement) border2);
            num4 += 9.0;
          }
          else
          {
            Line line1 = new Line();
            line1.X1 = 3.0;
            line1.X2 = num3 + 6.0;
            line1.Y1 = 5.0;
            line1.Y2 = 5.0;
            ((Shape) line1).Stroke = (Brush) brush;
            ((Shape) line1).StrokeThickness = 4.0;
            Line line2 = line1;
            Canvas.SetLeft((UIElement) line2, num4);
            Canvas.SetZIndex((UIElement) line2, 100);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) associatedObject).Children).Add((UIElement) line2);
            num4 += num3;
          }
        }
      }
    }
  }
}
