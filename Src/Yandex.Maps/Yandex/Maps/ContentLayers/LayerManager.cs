// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.LayerManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Maps.API;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Maps.Controls.ContentLayers;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public class LayerManager : LayerContainer
  {
    private OperationMode _operationMode;
    private bool _childrenLoaded;
    private readonly object _contentLayersLock = new object();
    private ViewportRect _viewportRect;

    public LayerManager()
    {
      this.ContentLayers = (IList<IContentContainer>) new List<IContentContainer>();
      ((FrameworkElement) this).LayoutUpdated += new EventHandler(this.LayerManagerLayoutUpdated);
    }

    private void LayerManagerLayoutUpdated(object sender, EventArgs e)
    {
      if (this._childrenLoaded)
        return;
      this._childrenLoaded = true;
      lock (this._contentLayersLock)
      {
        foreach (IContentLayer contentLayer in ((IEnumerable) this.Children).OfType<IContentLayer>().ToArray<IContentLayer>())
          this.AddContentLayer(contentLayer);
      }
      this.Reload(this._viewportRect);
    }

    private IList<IContentContainer> ContentLayers { get; set; }

    public OperationMode OperationMode
    {
      get => this._operationMode;
      set
      {
        this._operationMode = value;
        lock (this._contentLayersLock)
        {
          foreach (IContentContainer contentLayer in (IEnumerable<IContentContainer>) this.ContentLayers)
            contentLayer.OperationMode = value;
        }
      }
    }

    public void AddContentLayer(IContentLayer contentLayer)
    {
      lock (this._contentLayersLock)
      {
        contentLayer.OperationMode = this.OperationMode;
        this.ContentLayers.Add((IContentContainer) contentLayer);
      }
    }

    public void RemoveContentLayer(IContentLayer contentLayer)
    {
      lock (this._contentLayersLock)
        this.ContentLayers.Remove((IContentContainer) contentLayer);
    }

    public void AddTileContentLayer(ITileContentLayer contentLayer)
    {
      lock (this._contentLayersLock)
      {
        ((PresentationFrameworkCollection<UIElement>) this.Children).Add((UIElement) contentLayer.Control);
        this.AddContentLayer((IContentLayer) contentLayer);
      }
    }

    public void RemoveTileContentLayer(ITileContentLayer contentLayer)
    {
      lock (this._contentLayersLock)
      {
        ((PresentationFrameworkCollection<UIElement>) this.Children).Remove((UIElement) contentLayer.Control);
        this.ContentLayers.Remove((IContentContainer) contentLayer);
      }
    }

    public void Reload(ViewportRect viewportRect)
    {
      this._viewportRect = viewportRect;
      lock (this._contentLayersLock)
      {
        foreach (IContentContainer contentLayer in (IEnumerable<IContentContainer>) this.ContentLayers)
          contentLayer.Reload(viewportRect);
      }
    }

    public void DisposeContent()
    {
      lock (this._contentLayersLock)
      {
        foreach (IContentContainer contentLayer in (IEnumerable<IContentContainer>) this.ContentLayers)
          contentLayer.DisposeContent();
      }
    }

    public void DisposeContentOutsideArea(ViewportRect viewportRect)
    {
      lock (this._contentLayersLock)
      {
        foreach (IContentContainer contentLayer in (IEnumerable<IContentContainer>) this.ContentLayers)
          contentLayer.DisposeContentOutsideArea(viewportRect);
      }
    }
  }
}
