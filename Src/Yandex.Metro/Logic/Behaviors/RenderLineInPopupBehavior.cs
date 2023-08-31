// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Behaviors.RenderLineInPopupBehavior
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
using Yandex.Metro.Logic.FastScheme;
using Yandex.Metro.Views;

namespace Yandex.Metro.Logic.Behaviors
{
  public class RenderLineInPopupBehavior : Behavior<Canvas>
  {
    public static readonly DependencyProperty StationProperty = DependencyProperty.Register(nameof (Station), typeof (MetroStation), typeof (RenderLineInPopupBehavior), new PropertyMetadata(new PropertyChangedCallback(RenderLineInPopupBehavior.OnPropertyChanged)));

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      RenderLineInPopupBehavior lineInPopupBehavior = (RenderLineInPopupBehavior) d;
      if (lineInPopupBehavior == null || !e.NewValue.Equals((object) lineInPopupBehavior.Station))
        return;
      RenderLineInPopupBehavior.SetLines(lineInPopupBehavior.AssociatedObject, lineInPopupBehavior.Station);
    }

    public MetroStation Station
    {
      get => (MetroStation) this.GetValue(RenderLineInPopupBehavior.StationProperty);
      set => this.SetValue(RenderLineInPopupBehavior.StationProperty, (object) value);
    }

    private static void SetLines(Canvas associatedObject, MetroStation station)
    {
      Canvas canvas = associatedObject;
      if (FastKeeper.Scheme == null)
        return;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) canvas).Children).Clear();
      List<MetroStation> list = FastKeeper.Scheme.Stations.Values.Where<MetroStation>((Func<MetroStation, bool>) (r => r.LineId == station.LineId)).ToList<MetroStation>();
      int count = list.Count;
      int num1 = list.Count - 1;
      int num2 = 344 / num1;
      SolidColorBrush brush = MapGenerator.GetBrush(station.LineReference.Color);
      double num3 = 0.0;
      for (int index = 0; index < count; ++index)
      {
        bool flag = station.Id == list[index].Id;
        int num4 = flag ? 10 : 6;
        int num5 = flag ? 0 : 2;
        Ellipse ellipse1 = new Ellipse();
        ((FrameworkElement) ellipse1).Height = (double) num4;
        ((FrameworkElement) ellipse1).Width = (double) num4;
        ((Shape) ellipse1).Fill = (Brush) brush;
        Ellipse ellipse2 = ellipse1;
        Canvas.SetTop((UIElement) ellipse2, (double) num5);
        Canvas.SetLeft((UIElement) ellipse2, num3);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) canvas).Children).Add((UIElement) ellipse2);
        if (index < num1)
        {
          Line line1 = new Line();
          line1.X1 = 3.0;
          line1.X2 = (double) (num2 + 6);
          line1.Y1 = 5.0;
          line1.Y2 = 5.0;
          ((Shape) line1).Stroke = (Brush) brush;
          ((Shape) line1).StrokeThickness = 4.0;
          Line line2 = line1;
          Canvas.SetLeft((UIElement) line2, num3);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) canvas).Children).Add((UIElement) line2);
        }
        num3 += (double) num2;
      }
    }
  }
}
