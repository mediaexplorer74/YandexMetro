// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yandex.Maps.Interfaces;
using Yandex.Maps.TypeConverters;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class MapLayer : LayerBase, IMapLayer
  {
    public static readonly DependencyProperty AlignmentProperty = DependencyProperty.RegisterAttached("Alignment", typeof (Alignment?), typeof (MapLayer), new PropertyMetadata((object) new Alignment?(), new PropertyChangedCallback(MapLayer.AlignmentChangedCallback)));
    public static readonly DependencyProperty LocationProperty = DependencyProperty.RegisterAttached("Location", typeof (GeoCoordinate), typeof (MapLayer), new PropertyMetadata((object) null, new PropertyChangedCallback(MapLayer.LocationChangedCallback)));
    public static readonly DependencyProperty PositionOffsetProperty = DependencyProperty.RegisterAttached("PositionOffset", typeof (Point), typeof (MapLayer), new PropertyMetadata((object) new Point(), new PropertyChangedCallback(MapLayer.PositionOffsetChangedCallback)));
    public static readonly DependencyProperty LocationRectangleProperty = DependencyProperty.RegisterAttached("LocationRectangle", typeof (GeoCoordinatesRect), typeof (MapLayer), new PropertyMetadata((object) null));
    public static readonly DependencyProperty RelativeRectangleProperty = DependencyProperty.RegisterAttached("RelativeRectangle", typeof (RelativeRect), typeof (MapLayer), new PropertyMetadata((object) null));

    private static void AlignmentChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      MapLayer.InvalidateParentArrange(d);
    }

    [UsedImplicitly]
    public static Alignment GetAlignment(DependencyObject child) => child != null ? (Alignment) child.GetValue(MapLayer.AlignmentProperty) : throw new ArgumentNullException(nameof (child));

    public static void SetAlignment(DependencyObject child, Alignment value)
    {
      if (child == null)
        throw new ArgumentNullException(nameof (child));
      child.SetValue(MapLayer.AlignmentProperty, (object) value);
    }

    private static void LocationChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      MapLayer.InvalidateParentArrange(d);
    }

    [TypeConverter(typeof (GeoCoordinatesConverter))]
    [UsedImplicitly]
    public static GeoCoordinate GetLocation(DependencyObject child) => child != null ? child.GetValue(MapLayer.LocationProperty) as GeoCoordinate : throw new ArgumentNullException(nameof (child));

    public static void SetLocation(DependencyObject child, GeoCoordinate value)
    {
      if (child == null)
        throw new ArgumentNullException(nameof (child));
      child.SetValue(MapLayer.LocationProperty, (object) value);
    }

    public static void SetPositionOffset(DependencyObject element, Point value) => element.SetValue(MapLayer.PositionOffsetProperty, (object) value);

    public static Point GetPositionOffset(DependencyObject element) => (Point) element.GetValue(MapLayer.PositionOffsetProperty);

    private static void PositionOffsetChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      MapLayer.InvalidateParentArrange(d);
    }

    [UsedImplicitly]
    public static GeoCoordinatesRect GetLocationRectangle(DependencyObject child) => child != null ? (GeoCoordinatesRect) child.GetValue(MapLayer.LocationRectangleProperty) : throw new ArgumentNullException(nameof (child));

    public static void SetLocationRectangle(DependencyObject child, GeoCoordinatesRect value)
    {
      if (child == null)
        throw new ArgumentNullException(nameof (child));
      child.SetValue(MapLayer.LocationRectangleProperty, (object) value);
    }

    public static void SetRelativeRectangle(DependencyObject child, RelativeRect value)
    {
      if (child == null)
        throw new ArgumentNullException(nameof (child));
      child.SetValue(MapLayer.RelativeRectangleProperty, (object) value);
    }

    [UsedImplicitly]
    public static RelativeRect GetRelativeRectangle(DependencyObject child) => child != null ? child.GetValue(MapLayer.RelativeRectangleProperty) as RelativeRect : throw new ArgumentNullException(nameof (child));

    private static void InvalidateParentArrange(DependencyObject d)
    {
      if (!(d is FrameworkElement frameworkElement) || !(frameworkElement.Parent is UIElement parent1))
        return;
      if (parent1 is ContentPresenter contentPresenter && ((FrameworkElement) contentPresenter).Parent is UIElement parent2)
        parent2.InvalidateArrange();
      else
        parent1.InvalidateArrange();
    }

    public void AddChild(UIElement element)
    {
      if (element is IProjectable projectable)
        projectable.ParentMap = this.ParentMap;
      ((PresentationFrameworkCollection<UIElement>) this.Children).Add(element);
    }

    public void AddChild(UIElement element, GeoCoordinate location)
    {
      ((DependencyObject) element).SetValue(MapLayer.LocationProperty, (object) location);
      ((PresentationFrameworkCollection<UIElement>) this.Children).Add(element);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement child in ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>())
      {
        if (!(child is IBoundable boundable) && child is ContentPresenter contentPresenter && VisualTreeHelper.GetChildrenCount((DependencyObject) child) > 0)
          boundable = VisualTreeHelper.GetChild((DependencyObject) contentPresenter, 0) as IBoundable;
        Rect rect;
        if (boundable == null)
        {
          DependencyObject childObject = (DependencyObject) null;
          GeoCoordinate dependencyPropertyValue1 = MapLayer.GetDependencyPropertyValue<GeoCoordinate>((DependencyObject) child, MapLayer.LocationProperty, ref childObject);
          Alignment? dependencyPropertyValue2 = MapLayer.GetDependencyPropertyValue<Alignment?>((DependencyObject) child, MapLayer.AlignmentProperty, ref childObject);
          Point dependencyPropertyValue3 = MapLayer.GetDependencyPropertyValue<Point>((DependencyObject) child, MapLayer.PositionOffsetProperty, ref childObject);
          rect = MapLayer.GetViewportChildRect(this.ParentMap, child, dependencyPropertyValue1, dependencyPropertyValue2, dependencyPropertyValue3);
        }
        else
        {
          Rect boundingRect = boundable.BoundingRect;
          rect = new Rect(boundingRect.X, boundingRect.Y, boundingRect.Width, boundingRect.Height);
        }
        child.Arrange(rect);
      }
      return ((FrameworkElement) this).ArrangeOverride(finalSize);
    }

    public static Rect GetViewportChildRect(
      MapBase map,
      UIElement child,
      GeoCoordinate position,
      Alignment? alignment,
      Point positionOffset)
    {
      double x = 0.0;
      double y = 0.0;
      if (position != null)
      {
        Point viewportPoint = map.CoordinatesToViewportPoint(position);
        if (double.IsNaN(viewportPoint.X) || double.IsNaN(viewportPoint.Y))
          return new Rect(0.0, 0.0, 0.0, 0.0);
        x = Math.Round(viewportPoint.X);
        y = Math.Round(viewportPoint.Y);
      }
      return MapLayer.GetAlignedChildRect(child, x, y, alignment, positionOffset);
    }

    private static Rect GetAlignedChildRect(
      UIElement child,
      double x,
      double y,
      Alignment? alignment,
      Point positionOffset)
    {
      Size desiredSize = child.DesiredSize;
      double width = desiredSize.Width;
      if (width < double.MaxValue)
      {
        ref Alignment? local = ref alignment;
        Alignment valueOrDefault = local.GetValueOrDefault();
        if (local.HasValue)
        {
          switch (valueOrDefault)
          {
            case Alignment.TopCenter:
              x -= width * 0.5;
              break;
            case Alignment.Center:
              x -= width * 0.5;
              y -= desiredSize.Height * 0.5;
              break;
            case Alignment.BottomCenter:
              x -= width * 0.5;
              y -= desiredSize.Height;
              break;
          }
        }
      }
      x += positionOffset.X;
      y += positionOffset.Y;
      return new Rect(x, y, width, desiredSize.Height);
    }

    [CanBeNull]
    public static T GetDependencyPropertyValue<T>(
      DependencyObject dependencyObject,
      DependencyProperty dependencyProperty,
      ref DependencyObject childObject)
    {
      if (dependencyObject.GetValue(dependencyProperty) is T dependencyPropertyValue1)
        return dependencyPropertyValue1;
      if (childObject == null)
      {
        switch (dependencyObject)
        {
          case ContentPresenter _:
          case ContentControl _:
            if (VisualTreeHelper.GetChildrenCount(dependencyObject) <= 0)
              goto label_8;
            else
              break;
          default:
            goto label_8;
        }
      }
      DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, 0);
      if (child != null)
      {
        childObject = child;
        if (child.GetValue(dependencyProperty) is T dependencyPropertyValue2)
          return dependencyPropertyValue2;
      }
label_8:
      return default (T);
    }

    [SpecialName]
    UIElementCollection IMapLayer.get_Children() => this.Children;
  }
}
