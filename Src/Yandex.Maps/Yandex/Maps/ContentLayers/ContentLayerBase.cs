// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.ContentLayerBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Yandex.Collections.Interfaces;
using Yandex.Common;
using Yandex.DevUtils;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Controls;
using Yandex.Maps.Controls.ContentLayers;
using Yandex.Maps.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public abstract class ContentLayerBase : LayerContainer, IContentContainer
  {
    protected byte _activeZoom;
    private readonly IQueue<ILayerCommand> _commandQueue;
    private readonly object _commandsSync;
    private bool _exitWorker;
    private OperationMode _operationMode;
    protected readonly IViewportTileConverter _viewportTileConverter;

    protected ViewportRect ActiveViewportRect { get; private set; }

    protected ContentLayerBase(
      IUiDispatcher uiDispatcher,
      IViewportTileConverter viewportTileConverter,
      IQueue<ILayerCommand> commandQueue,
      IThread thread)
      : base(uiDispatcher)
    {
      ContentLayerBase contentLayerBase = this;
      if (viewportTileConverter == null)
        throw new ArgumentNullException(nameof (viewportTileConverter));
      if (commandQueue == null)
        throw new ArgumentNullException(nameof (commandQueue));
      if (thread == null)
        throw new ArgumentNullException(nameof (thread));
      this._viewportTileConverter = viewportTileConverter;
      this._commandQueue = commandQueue;
      this._commandsSync = new object();
      uiDispatcher.BeginInvokeExplicit((Action) (() => thread.Start(new Action(contentLayerBase.Worker))));
    }

    public OperationMode OperationMode
    {
      get => this._operationMode;
      set
      {
        this._operationMode = value;
        this.PulseMonitor();
      }
    }

    protected abstract void OnOperationModeChanged(OperationMode newValue);

    public virtual void Reload(ViewportRect viewportRect)
    {
      if (this.OperationMode == OperationMode.None)
        return;
      this.EnqueueCommand((ILayerCommand) new LayerCommand(LayerCommandTypes.Reload, viewportRect));
    }

    public virtual void DisposeContent() => this.EnqueueCommand((ILayerCommand) new LayerCommand(LayerCommandTypes.DisposeContent));

    public virtual void DisposeContentOutsideArea(ViewportRect viewportRect) => this.EnqueueCommand((ILayerCommand) new LayerCommand(LayerCommandTypes.DisposeContentOutsideArea, viewportRect));

    protected virtual void Reset()
    {
      this._commandQueue.Clear();
      this.EnqueueCommand((ILayerCommand) new LayerCommand(LayerCommandTypes.Reset));
    }

    protected void PulseMonitor()
    {
      if (!Monitor.TryEnter(this._commandsSync))
        return;
      Monitor.Pulse(this._commandsSync);
      Monitor.Exit(this._commandsSync);
    }

    private void Worker()
    {
      while (!this._exitWorker)
      {
        OperationMode operationMode = this.OperationMode;
        this.OnOperationModeChanged(operationMode);
        if (this._commandQueue.Count == 0 || operationMode == OperationMode.None)
        {
          lock (this._commandsSync)
            Monitor.Wait(this._commandsSync);
        }
        else if (operationMode != OperationMode.None && this._commandQueue.Count != 0)
        {
          ILayerCommand command = this._commandQueue.Dequeue();
          if (command != null)
          {
            try
            {
              this.OnCommand(command);
            }
            catch (Exception ex)
            {
              Logger.TrackException(ex);
            }
          }
        }
      }
    }

    protected virtual void OnCommand([NotNull] ILayerCommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof (command));
      switch (command.Type)
      {
        case LayerCommandTypes.Reset:
          this.OnReset();
          break;
        case LayerCommandTypes.Reload:
          if (this.OperationMode != OperationMode.Full)
            break;
          this.OnReload(command.ViewportRect);
          break;
        case LayerCommandTypes.DisposeContent:
          if (this.OperationMode == OperationMode.None)
            break;
          this.OnDisposeContent();
          break;
        case LayerCommandTypes.DisposeContentOutsideArea:
          if (this.OperationMode == OperationMode.None)
            break;
          this.OnDisposeContentOutsideArea(command.ViewportRect);
          break;
      }
    }

    protected abstract void OnDisposeContentOutsideArea(ViewportRect viewportRect);

    protected abstract void OnDisposeContent();

    protected virtual void OnReset()
    {
    }

    protected virtual void OnReload(ViewportRect viewportRect) => this.ActiveViewportRect = viewportRect;

    protected void EnqueueCommand(ILayerCommand command)
    {
      this._commandQueue.Enqueue(command);
      this.PulseMonitor();
    }

    protected void DoZoom(byte zoom) => this._activeZoom = zoom;

    public static byte ZoomLevelToZoom(double zoomLevel) => (byte) Math.Round(zoomLevel);

    protected override void OnDispose()
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      this._commandQueue.Clear();
      this._exitWorker = true;
      this.PulseMonitor();
      base.OnDispose();
    }
  }
}
