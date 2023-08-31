// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapShape
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Yandex.Ioc;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Maps.IoC;
using Yandex.Media;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public abstract class MapShape : Control, IProjectable, IBoundable, IDisposable
  {
    protected const string ShapeControlName = "Shape";
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (MapShape), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(MapShape.StrokeThicknessChanged)));
    public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(nameof (StrokeLineJoin), typeof (PenLineJoin), typeof (MapShape), new PropertyMetadata((object) (PenLineJoin) 0, new PropertyChangedCallback(MapShape.StrokeLineJoinChanged)));
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (MapShape), new PropertyMetadata((object) null, new PropertyChangedCallback(MapShape.StrokeChanged)));
    public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register(nameof (StrokeEndLineCap), typeof (PenLineCap), typeof (MapShape), new PropertyMetadata((object) (PenLineCap) 0, new PropertyChangedCallback(MapShape.StrokeEndLineCapChanged)));
    public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register(nameof (StrokeStartLineCap), typeof (PenLineCap), typeof (MapShape), new PropertyMetadata((object) (PenLineCap) 0, new PropertyChangedCallback(MapShape.StrokeStartLineCapChanged)));
    public static readonly DependencyProperty RelativePointsListProperty = DependencyProperty.Register(nameof (RelativePointsList), typeof (IList<Point>), typeof (MapShape), new PropertyMetadata((object) null, new PropertyChangedCallback(MapShape.RelativePointsListChanged)));
    protected Shape Shape;
    private MapBase _parentMap;
    private bool _isInvalidatePending;
    private readonly IUiDispatcher _uiDispatcher;
    private double _strokeThicknessSquared;
    protected readonly IViewportPointConveter ViewportPointConveter;
    private double _maxX;
    private double _maxY;
    private double _minX;
    private double _minY;

    private static void StrokeThicknessChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnStrokeThicknessChanged((double) e.NewValue);
    }

    private void OnStrokeThicknessChanged(double value)
    {
      if (this.Shape != null)
        this.Shape.StrokeThickness = value;
      this._strokeThicknessSquared = value * value;
    }

    public double StrokeThickness
    {
      get => (double) ((DependencyObject) this).GetValue(MapShape.StrokeThicknessProperty);
      set => ((DependencyObject) this).SetValue(MapShape.StrokeThicknessProperty, (object) value);
    }

    private static void StrokeLineJoinChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnStrokeLineChanged((PenLineJoin) e.NewValue);
    }

    private void OnStrokeLineChanged(PenLineJoin value)
    {
      if (this.Shape == null)
        return;
      this.Shape.StrokeLineJoin = value;
    }

    public PenLineJoin StrokeLineJoin
    {
      get => (PenLineJoin) ((DependencyObject) this).GetValue(MapShape.StrokeLineJoinProperty);
      set => ((DependencyObject) this).SetValue(MapShape.StrokeLineJoinProperty, (object) value);
    }

    private static void StrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnStrokeChanged(e.NewValue as Brush);
    }

    private void OnStrokeChanged(Brush value)
    {
      if (this.Shape == null)
        return;
      this.Shape.Stroke = value;
    }

    public Brush Stroke
    {
      get => (Brush) ((DependencyObject) this).GetValue(MapShape.StrokeProperty);
      set => ((DependencyObject) this).SetValue(MapShape.StrokeProperty, (object) value);
    }

    private static void StrokeEndLineCapChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnStrokeEndLineCapChanged((PenLineCap) e.NewValue);
    }

    private void OnStrokeEndLineCapChanged(PenLineCap newValue)
    {
      if (this.Shape == null)
        return;
      this.Shape.StrokeEndLineCap = newValue;
    }

    public PenLineCap StrokeEndLineCap
    {
      get => (PenLineCap) ((DependencyObject) this).GetValue(MapShape.StrokeEndLineCapProperty);
      set => ((DependencyObject) this).SetValue(MapShape.StrokeEndLineCapProperty, (object) value);
    }

    private static void StrokeStartLineCapChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnStrokeStartLineCapChanged((PenLineCap) e.NewValue);
    }

    private void OnStrokeStartLineCapChanged(PenLineCap newValue)
    {
      if (this.Shape == null)
        return;
      this.Shape.StrokeStartLineCap = newValue;
    }

    public PenLineCap StrokeStartLineCap
    {
      get => (PenLineCap) ((DependencyObject) this).GetValue(MapShape.StrokeStartLineCapProperty);
      set => ((DependencyObject) this).SetValue(MapShape.StrokeStartLineCapProperty, (object) value);
    }

    public IList<Point> RelativePointsList
    {
      get => (IList<Point>) ((DependencyObject) this).GetValue(MapShape.RelativePointsListProperty);
      set => ((DependencyObject) this).SetValue(MapShape.RelativePointsListProperty, (object) value);
    }

    private static void RelativePointsListChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is MapShape mapShape))
        return;
      mapShape.OnRelativePointsListChanged((IList<Point>) e.NewValue);
    }

    private void OnRelativePointsListChanged(IList<Point> newValue) => this.RelativePointsList = newValue;

    protected MapShape()
    {
      ((FrameworkElement) this).Loaded += (RoutedEventHandler) ((sender, args) =>
      {
        if (this.ParentMap == null)
          this.ParentMap = this.InitializeParentMap();
        this.Invalidate();
      });
      ((FrameworkElement) this).LayoutUpdated += (EventHandler) ((sender, args) => this.OnLayoutUpdated());
      this._uiDispatcher = IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>();
      this.ViewportPointConveter = IocSingleton<ControlsIocInitializer>.Resolve<IViewportPointConveter>();
    }

    private void OnLayoutUpdated()
    {
      if (this.ParentMap == null)
        return;
      this.InvalidateShape(this.ParentMap.OperationMode);
    }

    private void InvalidateShape(OperationMode operationMode)
    {
      if (!this._isInvalidatePending || operationMode != OperationMode.Full || this.Shape == null)
        return;
      this._isInvalidatePending = false;
      this._uiDispatcher.BeginInvoke(new Action(this.UpdatePoints));
    }

    protected void Invalidate()
    {
      this._isInvalidatePending = true;
      if (this.ParentMap == null)
        return;
      this.InvalidateShape(this.ParentMap.OperationMode);
    }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.Shape = (Shape) this.GetTemplateChild("Shape");
      this.Shape.StrokeEndLineCap = this.StrokeEndLineCap;
      this.Shape.StrokeStartLineCap = this.StrokeStartLineCap;
      this.Shape.Stroke = this.Stroke;
      this.Shape.StrokeLineJoin = this.StrokeLineJoin;
      this.Shape.StrokeThickness = this.StrokeThickness;
    }

    protected virtual Size MeasureOverride(Size availableSize) => new Size(Math.Min(availableSize.Width, this._maxX - this._minX), Math.Min(availableSize.Height, this._maxY - this._minY));

    private void UpdatePoints()
    {
      if (this.Shape == null || this.ParentMap == null)
        return;
      IList<Point> viewportPointsList = this.GetViewportPointsList(this.ParentMap.InstantZoomLevel);
      if (viewportPointsList != null && viewportPointsList.Any<Point>())
      {
        double num1 = this.ResetShape();
        bool flag1 = true;
        bool flag2 = false;
        foreach (Point point in (IEnumerable<Point>) viewportPointsList)
        {
          if (point.X < this._minX)
            this._minX = point.X;
          if (point.X > this._maxX)
            this._maxX = point.X;
          if (point.Y < this._minY)
            this._minY = point.Y;
          if (point.Y > this._maxY)
            this._maxY = point.Y;
        }
        double strokeThickness = this.StrokeThickness;
        this._minX -= strokeThickness;
        this._minY -= strokeThickness;
        this._maxX += strokeThickness;
        this._maxY += strokeThickness;
        Point point1 = new Point();
        foreach (Point point2 in (IEnumerable<Point>) viewportPointsList)
        {
          if (flag1)
          {
            flag1 = false;
          }
          else
          {
            double num2 = point2.X - point1.X;
            double num3 = point2.Y - point1.Y;
            if (num2 * num2 + num3 * num3 < num1)
            {
              flag2 = true;
              continue;
            }
          }
          this.AddPoint(new Point(point2.X - this._minX, point2.Y - this._minY));
          point1 = point2;
          flag2 = false;
        }
        if (!flag1 && flag2)
        {
          Point point3 = viewportPointsList.Last<Point>();
          this.AddPoint(new Point(point3.X - this._minX, point3.Y - this._minY));
        }
      }
      ((UIElement) this.Shape).UpdateLayout();
      ((UIElement) this).InvalidateMeasure();
    }

    [CanBeNull]
    protected virtual IList<Point> GetViewportPointsList(double zoomLevel) => this.RelativePointsList == null ? (IList<Point>) null : (IList<Point>) this.RelativePointsList.Select<Point, Point>((Func<Point, Point>) (item => this.ViewportPointConveter.RelativePointToViewportPoint(item, zoomLevel))).ToArray<Point>();

    protected abstract void AddPoint(Point point);

    protected abstract void ClearShape();

    private double ResetShape()
    {
      this._maxX = 0.0;
      this._maxY = 0.0;
      this._minX = (double) uint.MaxValue;
      this._minY = (double) uint.MaxValue;
      this.ClearShape();
      this.Shape.StrokeThickness = this.StrokeThickness;
      return this._strokeThicknessSquared;
    }

    public MapBase ParentMap
    {
      get => this._parentMap;
      set
      {
        if (this._parentMap != null)
          this._parentMap.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.ParentMapOperationStatusChanged);
        this._parentMap = value;
        if (this._parentMap == null)
          return;
        this._parentMap.OperationStatusChanged += new EventHandler<OperationStatusChangedEventArgs>(this.ParentMapOperationStatusChanged);
      }
    }

    [CanBeNull]
    private MapBase InitializeParentMap()
    {
      mapBase = (MapBase) null;
      DependencyObject dependencyObject = (DependencyObject) this;
      while (true)
      {
        switch (dependencyObject)
        {
          case null:
          case MapBase mapBase:
            goto label_3;
          default:
            dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            continue;
        }
      }
label_3:
      return mapBase;
    }

    private void ParentMapOperationStatusChanged(object sender, OperationStatusChangedEventArgs e)
    {
      if (!this._isInvalidatePending)
        return;
      this.InvalidateShape(e.OperationStatus != OperationStatus.Idle ? OperationMode.None : OperationMode.Full);
    }

    public void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
      if (updateLevel == ProjectionUpdateLevel.None || updateLevel == ProjectionUpdateLevel.Linear)
        return;
      this.Invalidate();
    }

    public Rect BoundingRect
    {
      get
      {
        double width = this._maxX - this._minX;
        double height = this._maxY - this._minY;
        if (width < 0.0)
          width = 0.0;
        if (height < 0.0)
          height = 0.0;
        return new Rect(this._minX, this._minY, width, height);
      }
    }

    public void Dispose()
    {
      if (this._parentMap == null)
        return;
      this._parentMap.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.ParentMapOperationStatusChanged);
    }
  }
}
