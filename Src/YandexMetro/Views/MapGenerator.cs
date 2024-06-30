// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Views.MapGenerator
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Y.Common.Extensions;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common;
using Y.UI.Common.Converters;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;
using Yandex.Metro.Logic.FastScheme;
using Yandex.Metro.ViewModel;

namespace Yandex.Metro.Views
{
  public static class MapGenerator
  {
    private static Dictionary<Color, SolidColorBrush> _brushesCache = new Dictionary<Color, SolidColorBrush>();
    private static Dictionary<int, Ellipse> _stationsCache = new Dictionary<int, Ellipse>();

    internal static SolidColorBrush GetBrush(Color color) => !MapGenerator._brushesCache.ContainsKey(color) ? (MapGenerator._brushesCache[color] = new SolidColorBrush(color)) : MapGenerator._brushesCache[color];

    internal static SolidColorBrush GetBrush(MetroColor color) => MapGenerator.GetBrush(Color.FromArgb(byte.MaxValue, color.R, color.G, color.B));

    internal static Ellipse GetStation(int id) => MapGenerator._stationsCache.ContainsKey(id) ? MapGenerator._stationsCache[id] : throw new Exception(string.Format("station with id {0} not found", (object) id));

    internal static void GenerateMapFromScheme(
      Canvas MapRoot,
      MetroScheme metroScheme,
      EventHandler<GestureEventArgs> OnTapExecuted,
      bool isRoute = false,
      List<UIElement> _routeObjects = null)
    {
      if (!isRoute)
        MapGenerator._stationsCache.Clear();
      Dictionary<int, MetroLine> dictionary = ((IEnumerable<MetroLine>) metroScheme.Lines).ToDictionary<MetroLine, int>((Func<MetroLine, int>) (line => line.Id));
      List<MetroStation> list = metroScheme.Stations.Values.ToList<MetroStation>();
      foreach (MetroStation metroStation in list)
      {
        metroStation.LineReference = dictionary[metroStation.LineId];
        metroStation.LineForGroup = new LineForGroup()
        {
          Color = metroStation.LineReference.Color,
          Name = metroStation.LineReference.Title
        };
      }
      foreach (MetroLine line in metroScheme.Lines)
      {
        MetroLine lineMeta = line;
        PathGeometry pathGeometry = new PathGeometry();
        Path path1 = new Path();
        path1.Data = (Geometry) pathGeometry;
        ((Shape) path1).StrokeThickness = 11.0;
        ((Shape) path1).Stroke = (Brush) MapGenerator.GetBrush(lineMeta.Color);
        ((FrameworkElement) path1).DataContext = (object) lineMeta;
        ((UIElement) path1).IsHitTestVisible = false;
        Path path2 = path1;
        MetroStation[] array1 = list.Where<MetroStation>((Func<MetroStation, bool>) (s => s.LineId == lineMeta.Id)).ToArray<MetroStation>();
        if (((IEnumerable<MetroStation>) array1).Any<MetroStation>())
        {
          List<int> lineStationsIds = ((IEnumerable<MetroStation>) array1).Select<MetroStation, int>((Func<MetroStation, int>) (s => s.Id)).ToList<int>();
          PathFigure pathFigure = (PathFigure) null;
          foreach (MetroStation metroStation1 in array1)
          {
            MetroStation fromStation = metroStation1;
            if (pathFigure == null)
              pathFigure = new PathFigure()
              {
                StartPoint = new Point(fromStation.SchemePosition.X, fromStation.SchemePosition.Y),
                IsClosed = false,
                IsFilled = true
              };
            MetroLink[] array2 = ((IEnumerable<MetroLink>) metroScheme.Links).Where<MetroLink>((Func<MetroLink, bool>) (m => lineStationsIds.Contains(m.To) && m.From == fromStation.Id)).ToArray<MetroLink>();
            if (array2.Length > 0)
            {
              foreach (MetroLink metroLink in array2)
              {
                MetroLink link = metroLink;
                if (pathFigure != null && pathFigure.Segments != null && ((PresentationFrameworkCollection<PathSegment>) pathFigure.Segments).Count > 0)
                {
                  ((PresentationFrameworkCollection<PathFigure>) pathGeometry.Figures).Add(pathFigure);
                  pathFigure = new PathFigure()
                  {
                    StartPoint = new Point(fromStation.SchemePosition.X, fromStation.SchemePosition.Y),
                    IsClosed = false,
                    IsFilled = true
                  };
                }
                MetroStation metroStation2 = ((IEnumerable<MetroStation>) array1).Single<MetroStation>((Func<MetroStation, bool>) (ls => ls.Id == link.To));
                if (link.CustomDraws == null)
                {
                  LineSegment lineSegment = new LineSegment()
                  {
                    Point = new Point(metroStation2.SchemePosition.X, metroStation2.SchemePosition.Y)
                  };
                  ((PresentationFrameworkCollection<PathSegment>) pathFigure.Segments).Add((PathSegment) lineSegment);
                }
                else
                {
                  foreach (CustomLinkDraw customDraw in link.CustomDraws)
                  {
                    PathSegment pathSegment = (PathSegment) null;
                    switch (customDraw.Type)
                    {
                      case "line":
                        pathSegment = (PathSegment) new LineSegment()
                        {
                          Point = new Point(customDraw.X, customDraw.Y)
                        };
                        break;
                      case "arc":
                        pathSegment = (PathSegment) new ArcSegment()
                        {
                          Point = new Point(metroStation2.SchemePosition.X, metroStation2.SchemePosition.Y),
                          Size = new Size(customDraw.Radius, customDraw.Radius)
                        };
                        break;
                    }
                    if (pathSegment != null)
                      ((PresentationFrameworkCollection<PathSegment>) pathFigure.Segments).Add(pathSegment);
                  }
                }
              }
            }
            else
            {
              ((PresentationFrameworkCollection<PathFigure>) pathGeometry.Figures).Add(pathFigure);
              pathFigure = (PathFigure) null;
            }
          }
          if (pathFigure != null)
            ((PresentationFrameworkCollection<PathFigure>) pathGeometry.Figures).Add(pathFigure);
          int num = isRoute ? 1100 : 100;
          Canvas.SetZIndex((UIElement) path2, num);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) MapRoot).Children).Add((UIElement) path2);
          if (isRoute)
            _routeObjects.Add((UIElement) path2);
        }
      }
      foreach (MetroStation metroStation in list)
      {
        Ellipse ellipse1 = new Ellipse();
        ((FrameworkElement) ellipse1).Height = 13.0;
        ((FrameworkElement) ellipse1).Width = 13.0;
        ((Shape) ellipse1).Fill = (Brush) MapGenerator.GetBrush(metroStation.LineReference.Color);
        ((Shape) ellipse1).StrokeThickness = 1.0;
        ((Shape) ellipse1).Stroke = (Brush) MapGenerator.GetBrush(metroStation.IsTransfer ? Colors.White : Colors.Black);
        ((FrameworkElement) ellipse1).DataContext = (object) metroStation;
        Ellipse ellipse2 = ellipse1;
        if (isRoute)
        {
          ((UIElement) ellipse2).IsHitTestVisible = false;
        }
        else
        {
          ((UIElement) ellipse2).Tap += OnTapExecuted;
          MapGenerator._stationsCache.Add(metroStation.Id, ellipse2);
        }
        Canvas.SetLeft((UIElement) ellipse2, metroStation.SchemePosition.X - 6.5);
        Canvas.SetTop((UIElement) ellipse2, metroStation.SchemePosition.Y - 6.5);
        Canvas.SetZIndex((UIElement) ellipse2, isRoute ? 1300 : 300);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) MapRoot).Children).Add((UIElement) ellipse2);
        if (isRoute)
          _routeObjects.Add((UIElement) ellipse2);
        bool flag1 = isRoute && metroStation.SameAsId != 0;
        MetroStation station = flag1 ? FastKeeper.Scheme.Stations[metroStation.SameAsId] : metroStation;
        if (station.SameAsId == 0 || flag1)
        {
          Border border1 = new Border();
          ((FrameworkElement) border1).Height = 19.5;
          ((FrameworkElement) border1).DataContext = (object) station;
          border1.Background = (Brush) new SolidColorBrush(Colors.Transparent);
          Border border2 = border1;
          Ellipse ellipse3 = new Ellipse();
          ((FrameworkElement) ellipse3).Width = 6.0;
          ((FrameworkElement) ellipse3).Height = 6.0;
          ((FrameworkElement) ellipse3).Margin = new Thickness(1.0, 0.0, 5.0, 0.0);
          ((FrameworkElement) ellipse3).DataContext = (object) station;
          Ellipse ellipse4 = ellipse3;
          StackPanel stackPanel1 = new StackPanel();
          stackPanel1.Orientation = (Orientation) 1;
          ((FrameworkElement) stackPanel1).Height = 16.0;
          ((FrameworkElement) stackPanel1).Margin = new Thickness(5.0, 1.0, 5.0, 1.0);
          ((FrameworkElement) stackPanel1).DataContext = (object) station;
          StackPanel stackPanel2 = stackPanel1;
          Binding binding1 = new Binding("Background")
          {
            Source = (object) station
          };
          BindingOperations.SetBinding((DependencyObject) stackPanel2, Panel.BackgroundProperty, (BindingBase) binding1);
          Binding binding2 = new Binding("EllipseBrush")
          {
            Source = (object) station
          };
          BindingOperations.SetBinding((DependencyObject) ellipse4, Shape.FillProperty, (BindingBase) binding2);
          Binding binding3 = new Binding("EllipseVisibility")
          {
            Source = (object) station
          };
          BindingOperations.SetBinding((DependencyObject) ellipse4, UIElement.VisibilityProperty, (BindingBase) binding3);
          if (isRoute)
            ((UIElement) border2).IsHitTestVisible = false;
          else
            ((UIElement) border2).Tap += OnTapExecuted;
          double x = 0.0;
          double y = 0.0;
          if (station.Name.CustomPosition.HasValue)
          {
            ((FrameworkElement) stackPanel2).Margin = new Thickness(0.0);
            Point point = station.Name.CustomPosition.Value;
            y = point.Y - 2.0;
            point = station.Name.CustomPosition.Value;
            x = point.X;
          }
          else
          {
            y = station.SchemePosition.Y - 6.5 - 2.0 - 2.0;
            x = station.SchemePosition.X + 6.5 + 3.0 - 5.0;
          }
          UIElement uiElement;
          if (station.Name.TextLines != null)
          {
            StackPanel stackPanel3 = new StackPanel();
            bool flag2 = true;
            foreach (string textLine in station.Name.TextLines)
            {
              TextBlock stationLabel = MapGenerator.CreateStationLabel(station);
              ((FrameworkElement) stationLabel).Margin = new Thickness(0.0, flag2 ? -2.0 : -8.0, 0.0, 0.0);
              stationLabel.Text = textLine;
              ((PresentationFrameworkCollection<UIElement>) ((Panel) stackPanel3).Children).Add((UIElement) stationLabel);
              flag2 = false;
            }
            ((FrameworkElement) border2).Height = 28.0;
            ((FrameworkElement) stackPanel2).Height = 26.0;
            uiElement = (UIElement) stackPanel3;
            if (!station.Name.CustomPosition.HasValue)
              y -= 6.0;
          }
          else
          {
            TextBlock stationLabel = MapGenerator.CreateStationLabel(station);
            stationLabel.Text = station.Name.Text;
            uiElement = (UIElement) stationLabel;
          }
          MetroStation metaClosure = station;
          MetroStation stationTextPosition = metroStation;
          NameAlignment alignment = station.Name.Alignment;
          if (alignment == NameAlignment.Right)
            ((FrameworkElement) border2).SizeChanged += (SizeChangedEventHandler) ((s, e) =>
            {
              FrameworkElement frameworkElement = (FrameworkElement) s;
              double num = metaClosure.Name.CustomPosition.HasValue ? metaClosure.Name.CustomPosition.Value.X - frameworkElement.ActualWidth : metaClosure.SchemePosition.X - frameworkElement.ActualWidth - 6.5 - 3.0 + 5.0;
              Canvas.SetLeft((UIElement) frameworkElement, num);
              stationTextPosition.TextPosition = new Point(num, y + frameworkElement.ActualHeight);
            });
          if (alignment == NameAlignment.Left)
            ((FrameworkElement) border2).SizeChanged += (SizeChangedEventHandler) ((s, e) =>
            {
              FrameworkElement frameworkElement = (FrameworkElement) s;
              stationTextPosition.TextPosition = new Point(x + frameworkElement.ActualWidth, y + frameworkElement.ActualHeight);
            });
          if (alignment == NameAlignment.Left)
          {
            ((PresentationFrameworkCollection<UIElement>) ((Panel) stackPanel2).Children).Add(uiElement);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) stackPanel2).Children).Add((UIElement) ellipse4);
          }
          else
          {
            ((FrameworkElement) ellipse4).Margin = new Thickness(5.0, 0.0, 1.0, 0.0);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) stackPanel2).Children).Add((UIElement) ellipse4);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) stackPanel2).Children).Add(uiElement);
          }
          border2.Child = (UIElement) stackPanel2;
          Canvas.SetLeft((UIElement) border2, x);
          Canvas.SetTop((UIElement) border2, y);
          Canvas.SetZIndex((UIElement) border2, isRoute ? 1300 : 300);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) MapRoot).Children).Add((UIElement) border2);
          if (isRoute)
            _routeObjects.Add((UIElement) border2);
        }
        else
          metroStation.TextPosition = new Point(metroStation.SchemePosition.X, metroStation.SchemePosition.Y);
      }
      foreach (int[] transfer in metroScheme.Transfers)
      {
        List<MetroStation> transferList = new List<MetroStation>(transfer.Length);
        ((IEnumerable<int>) transfer).Each<int>((Action<int>) (t => transferList.Add(metroScheme.Stations[t])));
        MetroStation metroStation3 = transferList.OrderBy<MetroStation, double>((Func<MetroStation, double>) (s => s.SchemePosition.Y)).First<MetroStation>();
        MetroStation metroStation4 = transferList.OrderByDescending<MetroStation, double>((Func<MetroStation, double>) (s => s.SchemePosition.Y)).First<MetroStation>();
        double num1 = metroStation4.SchemePosition.Y - metroStation3.SchemePosition.Y + 13.0 + 6.0;
        bool flag = metroStation3 == metroStation4;
        if (flag)
        {
          metroStation3 = transferList.OrderBy<MetroStation, double>((Func<MetroStation, double>) (s => s.SchemePosition.X)).First<MetroStation>();
          num1 = transferList.OrderByDescending<MetroStation, double>((Func<MetroStation, double>) (s => s.SchemePosition.X)).First<MetroStation>().SchemePosition.X - metroStation3.SchemePosition.X + 13.0 + 6.0;
        }
        Border border3 = new Border();
        border3.BorderBrush = (Brush) new SolidColorBrush("#A0000000".ToColor());
        border3.BorderThickness = new Thickness(3.0);
        border3.CornerRadius = new CornerRadius(8.0);
        ((FrameworkElement) border3).Height = flag ? 19.0 : num1;
        ((FrameworkElement) border3).Width = flag ? num1 : 19.0;
        ((FrameworkElement) border3).DataContext = (object) transfer;
        border3.Background = (Brush) new SolidColorBrush("#80ffffff".ToColor());
        ((UIElement) border3).IsHitTestVisible = false;
        Border border4 = border3;
        Canvas.SetLeft((UIElement) border4, metroStation3.SchemePosition.X - 6.5 - 3.0);
        Canvas.SetTop((UIElement) border4, metroStation3.SchemePosition.Y - 6.5 - 3.0);
        int num2 = isRoute ? 1200 : 200;
        Canvas.SetZIndex((UIElement) border4, num2);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) MapRoot).Children).Add((UIElement) border4);
        if (isRoute)
          _routeObjects.Add((UIElement) border4);
      }
    }

    private static TextBlock CreateStationLabel(MetroStation station)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.FontSize = 14.0;
      ((FrameworkElement) textBlock).Margin = new Thickness(0.0, -2.0, 0.0, 0.0);
      textBlock.Padding = new Thickness(3.0, 0.0, 3.0, 0.0);
      TextBlock stationLabel = textBlock;
      station.Foreground = MapGenerator.GetBrush(Colors.Black);
      Binding binding = new Binding("Foreground")
      {
        Source = (object) station
      };
      BindingOperations.SetBinding((DependencyObject) stationLabel, TextBlock.ForegroundProperty, (BindingBase) binding);
      return stationLabel;
    }

    internal static void GenerateSelectUserControl(
      Canvas MapRoot,
      MetroScheme metroScheme,
      MouseButtonEventHandler OnHideSelectStationControl)
    {
      Border border1 = new Border();
      ((FrameworkElement) border1).Height = ((FrameworkElement) MapRoot).Height + 50.0;
      ((FrameworkElement) border1).Width = ((FrameworkElement) MapRoot).Width + 50.0;
      ((FrameworkElement) border1).Margin = new Thickness(-25.0, -25.0, 0.0, 0.0);
      border1.Background = (Brush) new SolidColorBrush("#ccffffff".ToColor());
      Border border2 = border1;
      Binding binding = new Binding("IsRouteAvailable")
      {
        Source = (object) Locator.MainStatic,
        Converter = (IValueConverter) new BooleanToVisibilityConverter()
      };
      ((UIElement) border2).IsHitTestVisible = false;
      BindingOperations.SetBinding((DependencyObject) border2, UIElement.VisibilityProperty, (BindingBase) binding);
      Canvas.SetLeft((UIElement) border2, 0.0);
      Canvas.SetTop((UIElement) border2, 0.0);
      Canvas.SetZIndex((UIElement) border2, 400);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) MapRoot).Children).Add((UIElement) border2);
      ThreadPool.QueueUserWorkItem((WaitCallback) (unused =>
      {
        ApplicationSettings appSettings = MetroService.Instance.AppSettings;
        Dictionary<int, MetroStation>.ValueCollection values = metroScheme.Stations.Values;
        foreach (MetroStation metroStation in values)
          metroStation.Letter = char.ToUpper(metroStation.Name.Text[0]).ToString();
        List<MetroStation> favorites = appSettings.Favorites.Where<Y.Metro.ServiceLayer.Entities.Favorites>((Func<Y.Metro.ServiceLayer.Entities.Favorites, bool>) (r => r.SchemaId == metroScheme.CityId)).Select<Y.Metro.ServiceLayer.Entities.Favorites, MetroStation>((Func<Y.Metro.ServiceLayer.Entities.Favorites, MetroStation>) (item => metroScheme.Stations[item.StationId])).ToList<MetroStation>();
        List<GroupByTitle<MetroStation>> listByTitle = values.GroupBy<MetroStation, string>((Func<MetroStation, string>) (station => station.Letter)).OrderBy<IGrouping<string, MetroStation>, string>((Func<IGrouping<string, MetroStation>, string>) (c => c.Key)).Select<IGrouping<string, MetroStation>, GroupByTitle<MetroStation>>((Func<IGrouping<string, MetroStation>, GroupByTitle<MetroStation>>) (c => new GroupByTitle<MetroStation>(c.Key, (IEnumerable<MetroStation>) c.OrderBy<MetroStation, string>((Func<MetroStation, string>) (r => r.Name.Text))))).ToList<GroupByTitle<MetroStation>>();
        List<GroupByLine<MetroStation>> listByLine = values.GroupBy<MetroStation, LineForGroup>((Func<MetroStation, LineForGroup>) (station => station.LineForGroup)).OrderBy<IGrouping<LineForGroup, MetroStation>, LineForGroup>((Func<IGrouping<LineForGroup, MetroStation>, LineForGroup>) (c => c.Key)).Select<IGrouping<LineForGroup, MetroStation>, GroupByLine<MetroStation>>((Func<IGrouping<LineForGroup, MetroStation>, GroupByLine<MetroStation>>) (c => new GroupByLine<MetroStation>(c.Key, (IEnumerable<MetroStation>) c))).ToList<GroupByLine<MetroStation>>();
        Dictionary<StationName, string> sortStations = values.OrderBy<MetroStation, string>((Func<MetroStation, string>) (s => s.Name.Text)).Select(s => new
        {
          s = s,
          Text = s.Name.Text
        }).ToDictionary(k => new StationName()
        {
          Station = k.s
        }, v => ReplaceHelper.ReplaceChar(v.Text));
        List<MetroStation> list = values.Where<MetroStation>((Func<MetroStation, bool>) (r => r.OldNames != null)).ToList<MetroStation>();
        List<KeyValuePair<StationName, string>> oldNameStation = new List<KeyValuePair<StationName, string>>();
        foreach (MetroStation metroStation in list)
        {
          foreach (OldName oldName in metroStation.OldNames)
            oldNameStation.Add(new KeyValuePair<StationName, string>(new StationName()
            {
              Station = metroStation,
              OldName = string.Format("{0} (до {1}г.)", (object) oldName.Name, (object) oldName.Year.Substring(0, 4))
            }, ReplaceHelper.ReplaceChar(oldName.Name)));
        }
        oldNameStation = oldNameStation.OrderBy<KeyValuePair<StationName, string>, string>((Func<KeyValuePair<StationName, string>, string>) (r => r.Value)).ToList<KeyValuePair<StationName, string>>();
        DispatcherHelper.UI((Action) (() =>
        {
          Locator.MainStatic.SortNameStation = sortStations;
          Locator.MainStatic.OldNameStations = oldNameStation;
          Locator.MainStatic.Favorites = new ObservableCollection<MetroStation>(favorites);
          Locator.MainStatic.StationByName = new ObservableCollection<GroupByTitle<MetroStation>>(listByTitle);
          Locator.MainStatic.StationByLine = new ObservableCollection<GroupByLine<MetroStation>>(listByLine);
        }));
      }));
    }
  }
}
