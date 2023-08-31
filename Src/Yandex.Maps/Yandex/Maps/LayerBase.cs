// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.LayerBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yandex.Maps.Controls;
using Yandex.Maps.Interfaces;
using Yandex.Media;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class LayerBase : Panel, IProjectable, IBoundable
  {
    private MapBase _parentMap;
    protected readonly Size InfiniteSize = new Size(double.MaxValue, double.MaxValue);
    protected readonly Rect _maxSizeRect = new Rect(0.0, 0.0, (double) uint.MaxValue, (double) uint.MaxValue);

    protected LayerBase()
    {
      this.PendingProjectionUpdateLevel = ProjectionUpdateLevel.Full;
      ((FrameworkElement) this).HorizontalAlignment = (HorizontalAlignment) 3;
      ((FrameworkElement) this).VerticalAlignment = (VerticalAlignment) 3;
    }

    protected LayerBase(MapBase parentMap) => this._parentMap = parentMap != null ? parentMap : throw new ArgumentNullException(nameof (parentMap));

    public MapBase ParentMap
    {
      get => this._parentMap ?? this.InitializeParentMap();
      set => this._parentMap = value;
    }

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
      this._parentMap = mapBase;
      return this._parentMap;
    }

    public virtual void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
      switch (this.PendingProjectionUpdateLevel |= updateLevel)
      {
        case ProjectionUpdateLevel.Full:
          ((UIElement) this).InvalidateMeasure();
          break;
      }
    }

    public Rect BoundingRect => this._maxSizeRect;

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(double.IsInfinity(availableSize.Width) ? (double) uint.MaxValue : availableSize.Width, double.IsInfinity(availableSize.Height) ? (double) uint.MaxValue : availableSize.Height);
      ProjectionUpdateLevel projectionUpdateLevel = this.PendingProjectionUpdateLevel;
      this.PendingProjectionUpdateLevel = ProjectionUpdateLevel.Linear;
      foreach (UIElement uiElement in ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>())
      {
        switch (uiElement)
        {
          case ContentPresenter _:
          case ContentControl _:
            if (VisualTreeHelper.GetChildrenCount((DependencyObject) uiElement) > 0)
            {
              pattern_0 = VisualTreeHelper.GetChild((DependencyObject) uiElement, 0) as IProjectable;
              break;
            }
            break;
        }
        pattern_0?.ProjectionUpdated(projectionUpdateLevel);
        uiElement.Measure(this.InfiniteSize);
      }
      return size;
    }

    protected ProjectionUpdateLevel PendingProjectionUpdateLevel { get; set; }
  }
}
