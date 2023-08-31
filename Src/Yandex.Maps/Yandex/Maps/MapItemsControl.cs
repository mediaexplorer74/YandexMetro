// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapItemsControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yandex.Maps.Controls;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class MapItemsControl : ItemsControl, IProjectable, IProjectablePanel
  {
    private readonly Size _infiniteSize = new Size((double) uint.MaxValue, (double) uint.MaxValue);
    private MapLayer _mapLayer;
    private ItemsPresenter _itemsPresenter;

    public MapItemsControl()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (MapItemsControl);
      this.PendingProjectionUpdateLevel = ProjectionUpdateLevel.Full;
    }

    public MapBase ParentMap { get; set; }

    public void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
      this.PendingProjectionUpdateLevel |= updateLevel;
      if (updateLevel == ProjectionUpdateLevel.None)
        return;
      switch (updateLevel)
      {
        case ProjectionUpdateLevel.Linear:
          ((UIElement) this).InvalidateArrange();
          break;
        case ProjectionUpdateLevel.Full:
          ((UIElement) this).InvalidateMeasure();
          break;
      }
    }

    protected ProjectionUpdateLevel PendingProjectionUpdateLevel { get; set; }

    public virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (VisualTreeHelper.GetChildrenCount((DependencyObject) this) <= 0)
        return;
      this._itemsPresenter = VisualTreeHelper.GetChild((DependencyObject) this, 0) as ItemsPresenter;
      if (this._itemsPresenter == null)
        return;
      ((FrameworkElement) this._itemsPresenter).LayoutUpdated += new EventHandler(this.ItemsPresenterLayoutUpdated);
    }

    private void ItemsPresenterLayoutUpdated(object sender, EventArgs e)
    {
      if (this._itemsPresenter == null)
        return;
      ((FrameworkElement) this._itemsPresenter).LayoutUpdated -= new EventHandler(this.ItemsPresenterLayoutUpdated);
      int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) this._itemsPresenter);
      for (int index = 0; index < childrenCount; ++index)
      {
        this._mapLayer = VisualTreeHelper.GetChild((DependencyObject) this._itemsPresenter, index) as MapLayer;
        if (this._mapLayer != null)
          break;
      }
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      if (this._mapLayer != null)
      {
        Size desiredSize = ((UIElement) this._mapLayer).DesiredSize;
        ((UIElement) this._mapLayer).Arrange(new Rect(0.0, 0.0, (double) uint.MaxValue, (double) uint.MaxValue));
      }
      return ((FrameworkElement) this).ArrangeOverride(finalSize);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(double.IsInfinity(availableSize.Width) ? (double) uint.MaxValue : availableSize.Width, double.IsInfinity(availableSize.Height) ? (double) uint.MaxValue : availableSize.Height);
      ProjectionUpdateLevel projectionUpdateLevel = this.PendingProjectionUpdateLevel;
      this.PendingProjectionUpdateLevel = ProjectionUpdateLevel.Linear;
      if (this._mapLayer != null)
      {
        this._mapLayer.ProjectionUpdated(projectionUpdateLevel);
        ((UIElement) this._mapLayer).Measure(this._infiniteSize);
      }
      return ((FrameworkElement) this).MeasureOverride(availableSize);
    }
  }
}
