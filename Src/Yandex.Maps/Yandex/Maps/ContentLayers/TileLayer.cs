// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Yandex.Ioc;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.IoC;

namespace Yandex.Maps.ContentLayers
{
  internal class TileLayer : LayerBase, ITileLayer
  {
    private const double AppendImageSize = 1.0;
    private readonly IConfigMediator _configMediator;
    public static readonly DependencyProperty TileInfoProperty = DependencyProperty.RegisterAttached("TileInfo", typeof (ITileInfo), typeof (TileLayer), new PropertyMetadata((object) null));
    private MatrixTransform _transform;
    private double _scale;
    private readonly uint _tileWidth;
    private readonly uint _tileHeight;
    private readonly double _tileWidthAppended;
    private readonly double _tileHeightAppended;

    public TileLayer()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IConfigMediator>())
    {
    }

    public TileLayer([NotNull] IConfigMediator configMediator)
    {
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
      IDisplayTileSize displayTileSize = this._configMediator.DisplayTileSize;
      this._tileWidth = displayTileSize != null ? displayTileSize.Width : throw new InvalidOperationException("DisplayTileSize is null");
      this._tileHeight = displayTileSize.Height;
      this._tileWidthAppended = (double) this._configMediator.DisplayTileSize.Width + 1.0;
      this._tileHeightAppended = (double) this._configMediator.DisplayTileSize.Height + 1.0;
      this.Initialize();
    }

    private void Initialize()
    {
      ((UIElement) this).RenderTransform = (Transform) (this._transform = new MatrixTransform());
      this.Scale = 1.0;
    }

    public TileLayer(Map parentMap)
      : base((MapBase) parentMap)
    {
    }

    public void AddChild(UIElement element, ITileInfo tileInfo)
    {
      ((DependencyObject) element).SetValue(TileLayer.TileInfoProperty, (object) tileInfo);
      ((PresentationFrameworkCollection<UIElement>) this.Children).Add(element);
    }

    public bool RemoveChild(UIElement element) => ((PresentationFrameworkCollection<UIElement>) this.Children).Remove(element);

    public double Scale
    {
      get => this._scale;
      set
      {
        this._scale = value;
        this._transform.Matrix = new Matrix(value, 0.0, 0.0, value, 0.0, 0.0);
      }
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement uiElement in ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>())
      {
        if (((DependencyObject) uiElement).GetValue(TileLayer.TileInfoProperty) is ITileInfo tileInfo)
        {
          double num1 = (double) ((long) tileInfo.X * (long) this._tileWidth);
          double num2 = (double) ((long) tileInfo.Y * (long) this._tileHeight);
          uiElement.Arrange(new Rect(num1, num2, this._tileWidthAppended, this._tileHeightAppended));
        }
      }
      return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(double.IsInfinity(availableSize.Width) ? (double) uint.MaxValue : availableSize.Width, double.IsInfinity(availableSize.Height) ? (double) uint.MaxValue : availableSize.Height);
      foreach (UIElement uiElement in ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>())
        uiElement.Measure(this.InfiniteSize);
      return size;
    }

    public override void ProjectionUpdated(ProjectionUpdateLevel updateLevel)
    {
    }
  }
}
