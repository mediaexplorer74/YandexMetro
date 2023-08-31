// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.Behaviors.PlainModeBehavior
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Y.Common.Extensions;
using Y.Metro.ServiceLayer.Enums;
using Y.Metro.ServiceLayer.FastScheme;
using Y.UI.Common.Behaviors;
using Y.UI.Common.Converters;
using Y.UI.Common.Utility;
using Yandex.Metro.ViewModel;
using Yandex.Metro.Views;

namespace Yandex.Metro.Logic.Behaviors
{
  public class PlainModeBehavior : Behavior<Canvas>
  {
    public static string GpsEllipse = "gpsPlainEllipse";
    public static string GpsEllipseSmall = "gpsPlainEllipseSmall";
    public static int InitialSize = 300;
    public static double NerestLeftPosition = -104.0;
    public static readonly DependencyProperty RouteProperty = DependencyProperty.Register(nameof (Route), typeof (Route), typeof (PlainModeBehavior), new PropertyMetadata(new PropertyChangedCallback(PlainModeBehavior.OnPropertyChanged)));

    public static double NerestTopPositionSmall { get; set; }

    public static double NerestTopPositionDrag { get; set; }

    public static double NerestTopPositionLarge { get; set; }

    public Route Route
    {
      get => (Route) this.GetValue(PlainModeBehavior.RouteProperty);
      set => this.SetValue(PlainModeBehavior.RouteProperty, (object) value);
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      PlainModeBehavior plainModeBehavior = (PlainModeBehavior) d;
      if (e.NewValue == null || plainModeBehavior == null || plainModeBehavior.Route == null || !e.NewValue.Equals((object) plainModeBehavior.Route))
        return;
      PlainModeBehavior.SetLines(plainModeBehavior.AssociatedObject, plainModeBehavior.Route);
    }

    private static void SetLines(Canvas associatedObject, Route route)
    {
      Canvas originalCanvas = associatedObject;
      FrameworkElement name1 = (FrameworkElement) ((FrameworkElement) originalCanvas).FindName("revertButton");
      FrameworkElement name2 = (FrameworkElement) ((FrameworkElement) originalCanvas).FindName(PlainModeBehavior.GpsEllipseSmall);
      FrameworkElement name3 = (FrameworkElement) ((FrameworkElement) originalCanvas).FindName(PlainModeBehavior.GpsEllipse);
      bool isDragAnimation = name2 != null;
      bool flag1 = false;
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Clear();
      if (isDragAnimation)
      {
        ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) name2);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) name3);
      }
      double top = 50.0;
      int count1 = route.SortStations.Count;
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      List<MetroStation> metroStationList1 = new List<MetroStation>();
      List<MetroStation> metroStationList2 = new List<MetroStation>()
      {
        route.StartStation
      };
      List<int> intList4 = new List<int>();
      Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
      foreach (int[] transfer in route.RouteScheme.Transfers)
      {
        int[] transferMeta = transfer;
        if (transferMeta.Length >= 2)
        {
          bool flag2 = transferMeta.Length == 3;
          MetroStation metroStation1 = route.SortStations.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (r => r.Id == transferMeta[0]));
          int num1 = route.SortStations.IndexOf(metroStation1);
          MetroStation metroStation2 = route.SortStations.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (r => r.Id == transferMeta[1]));
          int num2 = route.SortStations.IndexOf(metroStation2);
          if (flag2)
          {
            MetroStation metroStation3 = route.SortStations.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (r => r.Id == transferMeta[2]));
            int num3 = route.SortStations.IndexOf(metroStation3);
            List<MetroStation> list = new Dictionary<int, MetroStation>()
            {
              {
                num1,
                metroStation1
              },
              {
                num2,
                metroStation2
              },
              {
                num3,
                metroStation3
              }
            }.OrderBy<KeyValuePair<int, MetroStation>, int>((Func<KeyValuePair<int, MetroStation>, int>) (r => r.Key)).Select<KeyValuePair<int, MetroStation>, MetroStation>((Func<KeyValuePair<int, MetroStation>, MetroStation>) (r => r.Value)).ToList<MetroStation>();
            metroStationList1.Add(list[0]);
            metroStationList2.Add(list[1]);
            intList3.Add(list[1].Id);
            ((IEnumerable<int>) transferMeta).Each<int>(new Action<int>(intList2.Add));
            dictionary1.Add(list[1].Id, list[2].Id);
          }
          else
          {
            bool flag3 = num2 > num1;
            metroStationList1.Add(flag3 ? metroStation1 : metroStation2);
            metroStationList2.Add(flag3 ? metroStation2 : metroStation1);
          }
          ((IEnumerable<int>) transferMeta).Each<int>(new Action<int>(intList1.Add));
        }
        else
          ((IEnumerable<int>) transferMeta).Each<int>(new Action<int>(intList4.Add));
      }
      metroStationList1.Add(route.EndStation);
      Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
      int count2 = metroStationList1.Count;
      for (int index = 0; index < count2; ++index)
      {
        MetroStation metroStation = metroStationList1[index];
        if (metroStation.BoardInfo != null)
        {
          int num = route.SortStations.IndexOf(metroStation);
          MetroStation prevStation = (MetroStation) null;
          if (num > 0)
            prevStation = route.SortStations[num - 1];
          bool flag4 = prevStation != null && prevStation.Id > metroStation.Id;
          bool flag5 = index == count2 - 1;
          List<int> position = new List<int>();
          List<int> target = (List<int>) null;
          if (flag5)
          {
            target = metroStation.BoardInfo.Exit;
          }
          else
          {
            MetroStation toStation = metroStationList2[index + 1];
            MetroStation nextStation = (MetroStation) null;
            if (num + 2 < count1)
              nextStation = route.SortStations[num + 2];
            TransferBoardPositions transferBoardPositions = ((IEnumerable<TransferBoardPositions>) metroStation.BoardInfo.Transfer).FirstOrDefault<TransferBoardPositions>((Func<TransferBoardPositions, bool>) (r =>
            {
              int? toStation1 = r.ToStation;
              int id1 = toStation.Id;
              if ((toStation1.GetValueOrDefault() != id1 ? 0 : (toStation1.HasValue ? 1 : 0)) != 0)
              {
                if (r.NextStation.HasValue && nextStation != null)
                {
                  int? nextStation1 = r.NextStation;
                  int id2 = nextStation.Id;
                  if ((nextStation1.GetValueOrDefault() != id2 ? 0 : (nextStation1.HasValue ? 1 : 0)) == 0)
                    goto label_8;
                }
                if (!r.PrevStation.HasValue || prevStation == null)
                  return true;
                int? prevStation1 = r.PrevStation;
                int id3 = prevStation.Id;
                return prevStation1.GetValueOrDefault() == id3 && prevStation1.HasValue;
              }
label_8:
              return false;
            }));
            if (transferBoardPositions != null)
              target = transferBoardPositions.Positions;
          }
          if (target != null)
            target.Each<int>((Action<int>) (r => position.Add(r)));
          if (flag4)
            position = PlainModeBehavior.ReverseValue(position);
          int id = metroStationList2[index].Id;
          if (dictionary1.ContainsKey(id))
            id = dictionary1[id];
          dictionary2.Add(id, position);
        }
      }
      Style style1 = ResourcesHelper.Get<Style>("TextBlock23Black");
      Style style2 = ResourcesHelper.Get<Style>("TextBlock42LightBlack");
      Style style3 = ResourcesHelper.Get<Style>("TextBlock23LightGray");
      SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.White);
      for (int index = 0; index < count1; ++index)
      {
        MetroStation sortStation = route.SortStations[index];
        bool flag6 = index == 0;
        bool flag7 = index == count1 - 2;
        bool flag8 = index == count1 - 1;
        bool flag9 = intList1.Contains(sortStation.Id);
        bool flag10 = intList4.Contains(sortStation.Id);
        Style style4 = flag9 || flag6 || flag8 ? style2 : style1;
        SolidColorBrush brush = MapGenerator.GetBrush(sortStation.LineReference.Color);
        MetroStation metroStation4 = (MetroStation) null;
        if (!flag8)
          metroStation4 = route.SortStations[index + 1];
        bool flag11 = metroStation4 != null && intList1.Contains(metroStation4.Id);
        bool flag12 = metroStation4 != null && metroStation4.LineId != sortStation.LineId;
        bool flag13 = flag6 && flag9 && flag12;
        bool flag14 = dictionary2.ContainsKey(sortStation.Id) && dictionary2[sortStation.Id].Count > 0;
        int num4 = flag14 ? 140 : 85;
        int num5 = 40;
        if (flag7)
          num5 = 65;
        if (flag9)
          num5 = flag12 ? 50 : num4;
        else if (flag11)
          num5 = 65;
        if (flag8)
          num5 = 0;
        if (flag6)
        {
          num5 = num4;
          if (flag13)
            num5 = 50;
        }
        if (flag8 || flag6)
          PlainModeBehavior.AddEllipse(sortStation, originalCanvas, brush, top, 21.0, 50);
        if (flag10)
          PlainModeBehavior.AddEllipse(sortStation, originalCanvas, brush, top - 1.0, 29.0, 34, 15);
        if (flag9 || flag8 || flag6)
          PlainModeBehavior.AddEllipse(sortStation, originalCanvas, brush, top + 4.0, 25.0, 42, 30, solidColorBrush, 3.0);
        else
          PlainModeBehavior.AddEllipse(sortStation, originalCanvas, brush, top, 30.0, 32, 20, solidColorBrush, 3.0);
        MetroStation nearestStation = Locator.MainStatic.NearestStation;
        if (nearestStation != null && sortStation.Id == nearestStation.Id)
        {
          flag1 = true;
          SolidColorBrush color = new SolidColorBrush("#1E0199bd".ToColor());
          SolidColorBrush strokeColor1 = new SolidColorBrush("#80026982".ToColor());
          SolidColorBrush strokeColor2 = new SolidColorBrush(Colors.Black);
          int num6 = flag9 || flag8 || flag6 ? 15 : 24;
          int num7 = flag9 || flag8 || flag6 ? 125 : 134;
          int num8 = flag9 || flag8 || flag6 ? 15 : 6;
          PlainModeBehavior.NerestTopPositionSmall = top - (double) num6;
          PlainModeBehavior.NerestTopPositionLarge = top - (double) num7;
          PlainModeBehavior.NerestTopPositionDrag = top + (double) num8;
          if (!isDragAnimation)
          {
            PlainModeBehavior.AddNearestStationEllipse(PlainModeBehavior.GpsEllipse, sortStation, originalCanvas, color, PlainModeBehavior.NerestTopPositionLarge, PlainModeBehavior.NerestLeftPosition, PlainModeBehavior.InitialSize, 60, strokeColor1, 0.0);
            PlainModeBehavior.AddNearestStationEllipse(PlainModeBehavior.GpsEllipseSmall, sortStation, originalCanvas, solidColorBrush, PlainModeBehavior.NerestTopPositionDrag, 36.0, 20, 60, strokeColor2);
          }
          Locator.MainStatic.MetroMap.AnimateNearestStation(isDragAnimation);
        }
        if (!flag8 && (!flag9 || !flag12))
        {
          Line line1 = new Line();
          line1.X1 = 0.0;
          line1.X2 = 0.0;
          line1.Y1 = 0.0;
          line1.Y2 = (double) num5;
          ((Shape) line1).Stroke = (Brush) brush;
          ((Shape) line1).StrokeThickness = 10.0;
          Line line2 = line1;
          Canvas.SetTop((UIElement) line2, top + 15.0);
          Canvas.SetLeft((UIElement) line2, 46.0);
          Canvas.SetZIndex((UIElement) line2, 5);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) line2);
        }
        bool flag15 = intList3.Contains(sortStation.Id);
        if (flag9 && flag12 && !flag15)
        {
          bool flag16 = intList2.Contains(sortStation.Id);
          LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
          {
            StartPoint = new Point(0.0, 0.0),
            EndPoint = new Point(0.0, 1.0)
          };
          GradientStop gradientStop1 = new GradientStop()
          {
            Color = brush.Color,
            Offset = 0.3
          };
          ((PresentationFrameworkCollection<GradientStop>) ((GradientBrush) linearGradientBrush).GradientStops).Add(gradientStop1);
          GradientStop gradientStop2 = new GradientStop()
          {
            Color = metroStation4.LineReference.Color.ToColor(),
            Offset = flag16 ? 0.5 : 0.7
          };
          ((PresentationFrameworkCollection<GradientStop>) ((GradientBrush) linearGradientBrush).GradientStops).Add(gradientStop2);
          if (flag16)
          {
            MetroStation metroStation5 = (MetroStation) null;
            if (!flag7)
              metroStation5 = route.SortStations[index + 2];
            GradientStop gradientStop3 = new GradientStop()
            {
              Color = metroStation5.LineReference.Color.ToColor(),
              Offset = 0.7
            };
            ((PresentationFrameworkCollection<GradientStop>) ((GradientBrush) linearGradientBrush).GradientStops).Add(gradientStop3);
          }
          Border border1 = new Border();
          border1.CornerRadius = new CornerRadius(25.0);
          ((FrameworkElement) border1).Height = flag16 ? 154.0 : 104.0;
          ((FrameworkElement) border1).Width = 50.0;
          border1.Background = (Brush) linearGradientBrush;
          Border border2 = border1;
          Canvas.SetTop((UIElement) border2, top - 2.0);
          Canvas.SetLeft((UIElement) border2, 21.0);
          Canvas.SetZIndex((UIElement) border2, 12);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) border2);
          Border border3 = new Border();
          border3.CornerRadius = new CornerRadius(21.0);
          ((FrameworkElement) border3).Height = flag16 ? 146.0 : 96.0;
          ((FrameworkElement) border3).Width = 42.0;
          border3.Background = (Brush) solidColorBrush;
          Border border4 = border3;
          Canvas.SetTop((UIElement) border4, top + 2.0);
          Canvas.SetLeft((UIElement) border4, 25.0);
          Canvas.SetZIndex((UIElement) border4, 12);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) border4);
        }
        if (!flag13 && !flag8 && flag14)
        {
          List<int> intList5 = dictionary2[sortStation.Id];
          Image image1 = new Image();
          image1.Source = (ImageSource) new BitmapImage(new Uri("/Images/train.png", UriKind.Relative));
          ((FrameworkElement) image1).DataContext = (object) sortStation;
          Image image2 = image1;
          Canvas.SetTop((UIElement) image2, top + 65.0);
          Canvas.SetLeft((UIElement) image2, 85.0);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) image2);
          foreach (int num9 in intList5)
          {
            int num10 = num9 - 1;
            Image image3 = new Image()
            {
              Source = (ImageSource) new BitmapImage(new Uri("/Images/train_arrow.png", UriKind.Relative))
            };
            Canvas.SetTop((UIElement) image3, top + 65.0);
            Canvas.SetLeft((UIElement) image3, (double) (114 + num10 * 70));
            ((UIElement) image2).Tap += new EventHandler<GestureEventArgs>(PlainModeBehavior.ChooseStationExecuted);
            ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) image3);
          }
        }
        bool flag17 = flag6 || flag8;
        Border border5 = new Border();
        ((FrameworkElement) border5).Width = flag17 ? 370.0 : 320.0;
        ((FrameworkElement) border5).DataContext = (object) sortStation;
        Border border6 = border5;
        TextBlock textBlock1 = new TextBlock();
        textBlock1.Text = sortStation.Name.Text;
        ((FrameworkElement) textBlock1).Style = style4;
        ((FrameworkElement) textBlock1).DataContext = (object) sortStation;
        TextBlock textBlock2 = textBlock1;
        SmartTrimmingTextBlockBehavior textBlockBehavior = new SmartTrimmingTextBlockBehavior()
        {
          TextTrimmingMargin = new double?(0.0),
          EllipcesSize = new double?(10.0)
        };
        Interaction.GetBehaviors((DependencyObject) textBlock2).Add((Behavior) textBlockBehavior);
        border6.Child = (UIElement) textBlock2;
        ((UIElement) border6).Tap += new EventHandler<GestureEventArgs>(PlainModeBehavior.ChooseStationExecuted);
        Canvas.SetTop((UIElement) border6, flag9 || flag8 || flag6 ? top - 5.0 : top);
        Canvas.SetLeft((UIElement) border6, 85.0);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) border6);
        if (!flag8 && !flag6)
        {
          TimeType timeType = MetroService.Instance.AppSettings.TimeType;
          Border border7 = new Border();
          ((FrameworkElement) border7).Width = 54.0;
          border7.Background = (Brush) new SolidColorBrush(Colors.Transparent);
          Border border8 = border7;
          TextBlock textBlock3 = new TextBlock();
          ((FrameworkElement) textBlock3).Name = string.Format("timeTextBlock{0}", (object) sortStation.Id);
          textBlock3.Text = timeType == TimeType.ArrivalTime ? sortStation.ArrivalTime : sortStation.IntervalTime;
          ((FrameworkElement) textBlock3).Style = style3;
          ((FrameworkElement) textBlock3).HorizontalAlignment = (HorizontalAlignment) 2;
          TextBlock textBlock4 = textBlock3;
          border8.Child = (UIElement) textBlock4;
          ((UIElement) border8).Tap += new EventHandler<GestureEventArgs>(PlainModeBehavior.timeTextBlock_Tap);
          Canvas.SetTop((UIElement) border8, flag9 || flag8 || flag6 ? top + 15.0 : top);
          Canvas.SetLeft((UIElement) border8, 403.0);
          ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) border8);
        }
        top += (double) num5;
      }
      if (!flag1)
      {
        ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Remove((UIElement) name2);
        ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Remove((UIElement) name3);
      }
      double num11 = top + 80.0;
      Canvas.SetTop((UIElement) name1, num11);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) name1);
      double num12 = num11 + 180.0;
      Border border9 = new Border();
      ((FrameworkElement) border9).Height = num12;
      ((FrameworkElement) border9).Width = 480.0;
      border9.Background = (Brush) new SolidColorBrush(Colors.Transparent);
      Border border10 = border9;
      ((UIElement) border10).MouseLeftButtonDown += new MouseButtonEventHandler(PlainModeBehavior.OnHideSelectStationControl);
      Binding binding = new Binding("IsStationPopupVisible")
      {
        Source = (object) Locator.MainStatic,
        Converter = (IValueConverter) new BooleanToVisibilityConverter()
      };
      BindingOperations.SetBinding((DependencyObject) border10, UIElement.VisibilityProperty, (BindingBase) binding);
      Canvas.SetLeft((UIElement) border10, 0.0);
      Canvas.SetTop((UIElement) border10, 0.0);
      Canvas.SetZIndex((UIElement) border10, 400);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) border10);
      ((FrameworkElement) originalCanvas).Height = num12;
    }

    public static void UpdatePosition(Ellipse ellipse)
    {
      ((FrameworkElement) ellipse).Height = ((FrameworkElement) ellipse).Width = (double) PlainModeBehavior.InitialSize;
      Canvas.SetTop((UIElement) ellipse, PlainModeBehavior.NerestTopPositionLarge);
      Canvas.SetLeft((UIElement) ellipse, PlainModeBehavior.NerestLeftPosition);
      ((UIElement) ellipse).Opacity = 0.0;
    }

    private static void timeTextBlock_Tap(object sender, GestureEventArgs e)
    {
      MetroService.Instance.AppSettings.TimeType = MetroService.Instance.AppSettings.TimeType == TimeType.ArrivalTime ? TimeType.IntervalTime : TimeType.ArrivalTime;
      Route selectRoute = Locator.MainStatic.SelectRoute;
      RouteTimeHelper.UpdateRouteTime(selectRoute, MetroService.Instance.AppSettings.TimeType);
      if (!(((FrameworkElement) sender).Parent is Canvas parent))
        return;
      foreach (MetroStation sortStation in selectRoute.SortStations)
      {
        string str = string.Format("timeTextBlock{0}", (object) sortStation.Id);
        TextBlock name = (TextBlock) ((FrameworkElement) parent).FindName(str);
        if (name != null)
          name.Text = MetroService.Instance.AppSettings.TimeType == TimeType.ArrivalTime ? sortStation.ArrivalTime : sortStation.IntervalTime;
      }
    }

    private static List<int> ReverseValue(List<int> position)
    {
      List<int> intList = new List<int>();
      foreach (int num1 in position)
      {
        int num2 = 0;
        switch (num1)
        {
          case 1:
            num2 = 5;
            break;
          case 2:
            num2 = 4;
            break;
          case 3:
            num2 = 3;
            break;
          case 4:
            num2 = 2;
            break;
          case 5:
            num2 = 1;
            break;
        }
        intList.Add(num2);
      }
      return intList;
    }

    private static void AddNearestStationEllipse(
      string name,
      MetroStation station,
      Canvas originalCanvas,
      SolidColorBrush color,
      double top,
      double left,
      int size,
      int zIndex,
      SolidColorBrush strokeColor,
      double opacity = 1.0)
    {
      Ellipse ellipse1 = new Ellipse();
      ((FrameworkElement) ellipse1).Name = name;
      ((FrameworkElement) ellipse1).Height = (double) size;
      ((FrameworkElement) ellipse1).Width = (double) size;
      ((Shape) ellipse1).Fill = (Brush) color;
      ((FrameworkElement) ellipse1).DataContext = (object) station;
      ((Shape) ellipse1).StrokeThickness = 3.0;
      ((Shape) ellipse1).Stroke = (Brush) strokeColor;
      ((UIElement) ellipse1).Opacity = opacity;
      Ellipse ellipse2 = ellipse1;
      ((UIElement) ellipse2).Tap += new EventHandler<GestureEventArgs>(PlainModeBehavior.ChooseStationExecuted);
      Canvas.SetTop((UIElement) ellipse2, top);
      Canvas.SetLeft((UIElement) ellipse2, left);
      Canvas.SetZIndex((UIElement) ellipse2, zIndex);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) ellipse2);
    }

    private static void AddEllipse(
      MetroStation station,
      Canvas originalCanvas,
      SolidColorBrush color,
      double top,
      double left,
      int size,
      int zIndex = 10,
      SolidColorBrush strokeColor = null,
      double strokeT = 0.0)
    {
      Ellipse ellipse1 = new Ellipse();
      ((FrameworkElement) ellipse1).Height = (double) size;
      ((FrameworkElement) ellipse1).Width = (double) size;
      ((Shape) ellipse1).Fill = (Brush) color;
      ((FrameworkElement) ellipse1).DataContext = (object) station;
      ((Shape) ellipse1).StrokeThickness = strokeT;
      ((Shape) ellipse1).Stroke = (Brush) strokeColor;
      Ellipse ellipse2 = ellipse1;
      ((UIElement) ellipse2).Tap += new EventHandler<GestureEventArgs>(PlainModeBehavior.ChooseStationExecuted);
      Canvas.SetTop((UIElement) ellipse2, top);
      Canvas.SetLeft((UIElement) ellipse2, left);
      Canvas.SetZIndex((UIElement) ellipse2, zIndex);
      ((PresentationFrameworkCollection<UIElement>) ((Panel) originalCanvas).Children).Add((UIElement) ellipse2);
    }

    private static void ChooseStationExecuted(object sender, EventArgs e) => Locator.MainStatic.ShowStationAtCityMap.Execute(((FrameworkElement) sender).DataContext as MetroStation);

    private static void OnHideSelectStationControl(object sender, MouseButtonEventArgs e)
    {
      Locator.MainStatic.IsStationPopupVisible = false;
      e.Handled = true;
    }
  }
}
