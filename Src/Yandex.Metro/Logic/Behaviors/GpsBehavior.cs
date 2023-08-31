// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Behaviors.GpsBehavior
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using Y.Metro.ServiceLayer.FastScheme;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Logic.Behaviors
{
  public class GpsBehavior : Behavior<Canvas>
  {
    public static string GpsEllipse = "gpsEllipse";
    public static string GpsEllipseSmall = "gpsEllipseSmall";
    public static double InitialSize = 600.0;
    public static readonly DependencyProperty StationProperty = DependencyProperty.Register(nameof (NearestStation), typeof (MetroStation), typeof (GpsBehavior), new PropertyMetadata(new PropertyChangedCallback(GpsBehavior.OnPropertyChanged)));

    public MetroStation NearestStation
    {
      get => (MetroStation) this.GetValue(GpsBehavior.StationProperty);
      set => this.SetValue(GpsBehavior.StationProperty, (object) value);
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      GpsBehavior gpsBehavior = (GpsBehavior) d;
      if (e.NewValue == null)
        GpsBehavior.ClearStation(gpsBehavior.AssociatedObject);
      if (e.NewValue == null || gpsBehavior == null || !e.NewValue.Equals((object) gpsBehavior.NearestStation))
        return;
      GpsBehavior.SetNearestStation(gpsBehavior.AssociatedObject, gpsBehavior.NearestStation);
    }

    private static void SetNearestStation(Canvas canvas, MetroStation station)
    {
      GpsBehavior.ClearStation(canvas);
      if (station == null)
        return;
      SolidColorBrush color1 = new SolidColorBrush("#1E0199bd".ToColor());
      SolidColorBrush strokeColor1 = new SolidColorBrush("#80026982".ToColor());
      SolidColorBrush strokeColor2 = new SolidColorBrush(Colors.Black);
      SolidColorBrush color2 = new SolidColorBrush(Colors.White);
      GpsBehavior.AddEllipse(GpsBehavior.GpsEllipse, station, canvas, color1, GpsBehavior.InitialSize, strokeColor1, 0.0);
      GpsBehavior.AddEllipse(GpsBehavior.GpsEllipseSmall, station, canvas, color2, 7.0, strokeColor2, 0.0);
      Locator.MainStatic.MetroMap.AnimateNearestStation();
    }

    private static void ClearStation(Canvas canvas)
    {
      UIElement name1 = (UIElement) ((FrameworkElement) canvas).FindName(GpsBehavior.GpsEllipse);
      UIElement name2 = (UIElement) ((FrameworkElement) canvas).FindName(GpsBehavior.GpsEllipseSmall);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) canvas).Children).Remove(name1);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) canvas).Children).Remove(name2);
    }

    private static void AddEllipse(
      string name,
      MetroStation station,
      Canvas originalCanvas,
      SolidColorBrush color,
      double size,
      SolidColorBrush strokeColor,
      double opacity = 1.0)
    {
      Ellipse ellipse1 = new Ellipse();
      ((FrameworkElement) ellipse1).Name = name;
      ((FrameworkElement) ellipse1).Height = size;
      ((FrameworkElement) ellipse1).Width = size;
      ((Shape) ellipse1).Fill = (Brush) color;
      ((FrameworkElement) ellipse1).DataContext = (object) station;
      ((Shape) ellipse1).Stroke = (Brush) strokeColor;
      ((Shape) ellipse1).StrokeThickness = 1.0;
      ((UIElement) ellipse1).IsHitTestVisible = false;
      ((UIElement) ellipse1).Opacity = opacity;
      Ellipse ellipse2 = ellipse1;
      Canvas.SetTop((UIElement) ellipse2, station.SchemePosition.Y - size / 2.0);
      Canvas.SetLeft((UIElement) ellipse2, station.SchemePosition.X - size / 2.0);
      Canvas.SetZIndex((UIElement) ellipse2, 9999);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) ellipse2);
    }

    public static void UpdatePosition(Ellipse ellipse, Ellipse smallEllipse, MetroStation station)
    {
      ((FrameworkElement) ellipse).Height = ((FrameworkElement) ellipse).Width = GpsBehavior.InitialSize;
      Canvas.SetTop((UIElement) ellipse, station.SchemePosition.Y - GpsBehavior.InitialSize / 2.0);
      Canvas.SetLeft((UIElement) ellipse, station.SchemePosition.X - GpsBehavior.InitialSize / 2.0);
      ((UIElement) smallEllipse).Opacity = ((UIElement) ellipse).Opacity = 0.0;
    }
  }
}
