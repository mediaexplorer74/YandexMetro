// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.VirtualizingMapItemsControl
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Yandex.Maps.Controls;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;
using Yandex.Media;

namespace Yandex.Maps
{
  [TemplatePart(Name = "MapLayer", Type = typeof (MapLayer))]
  internal abstract class VirtualizingMapItemsControl : Control, IProjectable
  {
    private const string MapLayerName = "MapLayer";
    private MapLayer _mapLayer;
    private MapBase _parentMap;
    private readonly Size _infiniteSize = new Size((double) uint.MaxValue, (double) uint.MaxValue);
    private ProjectionUpdateLevel _pendingProjectionUpdateLevel = ProjectionUpdateLevel.Full;
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (VirtualizingMapItemsControl), new PropertyMetadata(new PropertyChangedCallback(VirtualizingMapItemsControl.ItemsSourcePropertyChangedCallback)));
    private readonly Stack<UIElement> _containersCache = new Stack<UIElement>();
    private readonly IDictionary<object, UIElement> _children = (IDictionary<object, UIElement>) new Dictionary<object, UIElement>();

    protected VirtualizingMapItemsControl()
    {
      this.DefaultStyleKey = (object) typeof (VirtualizingMapItemsControl);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.OnLoaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => this.ParentMap.OperationStatusChanged += (EventHandler<OperationStatusChangedEventArgs>) ((param0, param1) => this.Invalidate());

    public virtual void OnApplyTemplate()
    {
      this._mapLayer = this.GetTemplateChild("MapLayer") as MapLayer;
      this.Invalidate();
    }

    public MapBase ParentMap
    {
      get => this._parentMap ?? (this._parentMap = this.InitializeParentMap());
      set => this._parentMap = value;
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

    public void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
      this.Invalidate();
      if ((this._pendingProjectionUpdateLevel = updateLevel) != ProjectionUpdateLevel.Full)
        return;
      ((UIElement) this).InvalidateMeasure();
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      if (this._mapLayer != null)
        ((UIElement) this._mapLayer).Arrange(new Rect(0.0, 0.0, (double) uint.MaxValue, (double) uint.MaxValue));
      return ((FrameworkElement) this).ArrangeOverride(finalSize);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      if (this._mapLayer != null)
      {
        this._mapLayer.ProjectionUpdated(this._pendingProjectionUpdateLevel);
        ((UIElement) this._mapLayer).Measure(this._infiniteSize);
      }
      return ((FrameworkElement) this).MeasureOverride(availableSize);
    }

    public IEnumerable ItemsSource
    {
      get => (IEnumerable) ((DependencyObject) this).GetValue(VirtualizingMapItemsControl.ItemsSourceProperty);
      set => ((DependencyObject) this).SetValue(VirtualizingMapItemsControl.ItemsSourceProperty, (object) value);
    }

    private static void ItemsSourcePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      VirtualizingMapItemsControl virtualizingMapItemsControl = (VirtualizingMapItemsControl) d;
      if (e.OldValue is INotifyCollectionChanged oldValue)
        oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(virtualizingMapItemsControl.CollectionChanged);
      if (e.NewValue is INotifyCollectionChanged newValue)
        newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(virtualizingMapItemsControl.CollectionChanged);
      virtualizingMapItemsControl.Invalidate(true);
    }

    private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => this.Invalidate(true);

    [NotNull]
    protected abstract UIElement CreateContainer();

    protected virtual void ResetContainer([NotNull] UIElement container)
    {
    }

    protected abstract void ReuseContainerForItem(UIElement container, object item, Rect bounds);

    protected abstract Rect GetItemBounds(object item, double zoomLevel);

    private void Invalidate(bool force = false)
    {
      if (!force && this.ParentMap.OperationStatus != OperationStatus.Idle)
        return;
      Rect viewport = this.ParentMap.Viewport;
      double instantZoomLevel = this.ParentMap.InstantZoomLevel;
      IEnumerable itemsSource = this.ItemsSource;
      if (this._mapLayer == null || itemsSource == null)
        return;
      Rect rect = new Rect(viewport.X - viewport.Width, viewport.Y - viewport.Height, viewport.Width * 3.0, viewport.Height * 3.0);
      foreach (object key in this._children.Keys.ToArray<object>())
      {
        if (force || !rect.Intersects(this.GetItemBounds(key, instantZoomLevel)))
        {
          UIElement child = this._children[key];
          this._children.Remove(key);
          ((PresentationFrameworkCollection<UIElement>) this._mapLayer.Children).Remove(child);
          this.ResetContainer(child);
          this._containersCache.Push(child);
        }
      }
      foreach (object key in itemsSource.Cast<object>().Except<object>((IEnumerable<object>) this._children.Keys))
      {
        Rect itemBounds = this.GetItemBounds(key, instantZoomLevel);
        if (rect.Intersects(itemBounds))
        {
          UIElement container = this._containersCache.Count > 0 ? this._containersCache.Pop() : this.CreateContainer();
          this.ReuseContainerForItem(container, key, itemBounds);
          this._children.Add(key, container);
          ((PresentationFrameworkCollection<UIElement>) this._mapLayer.Children).Add(container);
        }
      }
      while (this._containersCache.Count > this._children.Count)
      {
        if (this._containersCache.Pop() is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }
}
