// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinStateBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Maps.Events;
using Yandex.Maps.Helpers;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.Behavior
{
  [ComVisible(false)]
  public class PushPinStateBehavior : PushPinBehaviorBase
  {
    private static readonly DependencyProperty _ExcludeFromHandlingGroupKeyProperty = DependencyProperty.Register(nameof (ExcludeFromHandlingGroupKey), typeof (string), typeof (PushPinStateBehavior), (PropertyMetadata) null);
    private double _lastZoomLevel;

    public string ExcludeFromHandlingGroupKey
    {
      get => (string) this.GetValue(PushPinStateBehavior._ExcludeFromHandlingGroupKeyProperty);
      set => this.SetValue(PushPinStateBehavior._ExcludeFromHandlingGroupKeyProperty, (object) value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this._lastZoomLevel = this.AssociatedObject.ZoomLevel;
      this.AssociatedObject.OperationStatusChanged += new EventHandler<OperationStatusChangedEventArgs>(this.MapOperationStatusChanged);
    }

    protected override void OnDetaching()
    {
      MapBase associatedObject = this.AssociatedObject;
      if (associatedObject != null)
        associatedObject.OperationStatusChanged -= new EventHandler<OperationStatusChangedEventArgs>(this.MapOperationStatusChanged);
      base.OnDetaching();
    }

    protected override void OnPushPinManagerPushPinAdded(PushPin pushpin) => this.HandlePushPinState(pushpin);

    private void MapOperationStatusChanged(object sender, OperationStatusChangedEventArgs e)
    {
      if (e.OperationStatus != OperationStatus.Idle || this.AssociatedObject.ZoomLevel == this._lastZoomLevel)
        return;
      this.HandleGroupPushPinStates();
      this._lastZoomLevel = this.AssociatedObject.ZoomLevel;
    }

    public void ForcePushPinStatesRecalculation() => this.HandleGroupPushPinStates();

    private static Point MediaPointToSystemPoint(Point point) => new Point(point.X, point.Y);

    private static Point AlignToGrid(Point p, double gridSize)
    {
      double num1 = p.X % gridSize;
      p.X -= num1;
      if (num1 > gridSize / 2.0)
        p.X += gridSize;
      double num2 = p.Y % gridSize;
      p.Y -= num2;
      if (num2 > gridSize / 2.0)
        p.Y += gridSize;
      return p;
    }

    private Point? GetPoint(FrameworkElement pushpin)
    {
      GeoCoordinate locationRecursively = MapLayerHelper.GetLocationRecursively(pushpin);
      return locationRecursively == null ? new Point?() : new Point?(PushPinStateBehavior.MediaPointToSystemPoint(this.AssociatedObject.CoordinatesToViewportPoint(locationRecursively)));
    }

    private static Rect CreateFlagField(Point point)
    {
      point = PushPinStateBehavior.AlignToGrid(point, 16.0);
      Rect flagField = new Rect()
      {
        Width = 96.0,
        Height = 96.0
      };
      flagField.X = point.X - flagField.Width / 2.0;
      flagField.Y = point.Y - flagField.Height / 2.0;
      return flagField;
    }

    private bool IsExcludedFromHandling(PushPin pushpin)
    {
      string handlingGroupKey = this.ExcludeFromHandlingGroupKey;
      return handlingGroupKey != null && PushPinManagerHelper.GetGroupKeys(pushpin).Contains(handlingGroupKey);
    }

    private void HandlePushPinState(PushPin pushpin)
    {
      if (this.IsExcludedFromHandling(pushpin))
        return;
      Point? point = this.GetPoint((FrameworkElement) pushpin);
      if (!point.HasValue)
        return;
      pushpin.State = this.Group.Where<PushPin>((Func<PushPin, bool>) (pp => !object.ReferenceEquals((object) pp, (object) pushpin) && pp.State == PushPinState.Expanded)).Select<PushPin, Point?>(new Func<PushPin, Point?>(this.GetPoint)).Where<Point?>((Func<Point?, bool>) (p => p.HasValue)).Select<Point?, Rect>((Func<Point?, Rect>) (p => PushPinStateBehavior.CreateFlagField(p.Value))).Any<Rect>((Func<Rect, bool>) (flagField => flagField.Contains(point.Value))) ? PushPinState.Collapsed : PushPinState.Expanded;
    }

    private void HandlePushPinStates(ICollection<Rect> flagFields, IEnumerable<PushPin> pushpins)
    {
      foreach (PushPin pushpin in pushpins)
      {
        Point? point = this.GetPoint((FrameworkElement) pushpin);
        if (point.HasValue)
        {
          pushpin.State = flagFields.Any<Rect>((Func<Rect, bool>) (flagField => flagField.Contains(point.Value))) ? PushPinState.Collapsed : PushPinState.Expanded;
          if (pushpin.State == PushPinState.Expanded)
            flagFields.Add(PushPinStateBehavior.CreateFlagField(point.Value));
        }
      }
    }

    private void HandleGroupPushPinStates()
    {
      List<Rect> list1 = this.Group.Where<PushPin>((Func<PushPin, bool>) (pushpin => pushpin.State == PushPinState.Expanded && this.IsExcludedFromHandling(pushpin))).Select<PushPin, Point?>(new Func<PushPin, Point?>(this.GetPoint)).Where<Point?>((Func<Point?, bool>) (point => point.HasValue)).Select<Point?, Rect>((Func<Point?, Rect>) (point => PushPinStateBehavior.CreateFlagField(point.Value))).ToList<Rect>();
      List<PushPin> list2 = this.Group.Where<PushPin>((Func<PushPin, bool>) (pushpin => pushpin.State == PushPinState.Expanded && !this.IsExcludedFromHandling(pushpin))).ToList<PushPin>();
      List<PushPin> list3 = this.Group.Where<PushPin>((Func<PushPin, bool>) (pushpin => pushpin.State == PushPinState.Collapsed && !this.IsExcludedFromHandling(pushpin))).ToList<PushPin>();
      this.HandlePushPinStates((ICollection<Rect>) list1, (IEnumerable<PushPin>) list2);
      this.HandlePushPinStates((ICollection<Rect>) list1, (IEnumerable<PushPin>) list3);
    }
  }
}
