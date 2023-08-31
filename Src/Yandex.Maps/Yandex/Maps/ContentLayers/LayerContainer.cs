// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.LayerContainer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Yandex.Ioc;
using Yandex.Maps.IoC;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  [ComVisible(false)]
  public class LayerContainer : LayerBase, IDisposable
  {
    protected readonly IUiDispatcher _uiDispatcher;
    private bool _disposed;

    public LayerContainer(IUiDispatcher uiDispatcher) => this._uiDispatcher = uiDispatcher != null ? uiDispatcher : throw new ArgumentNullException(nameof (uiDispatcher));

    public LayerContainer()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>())
    {
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      if (this.ParentMap == null)
        return new Size(0.0, 0.0);
      foreach (UIElement uiElement in ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>())
      {
        if (uiElement.Visibility != 1)
        {
          Size desiredSize = uiElement.DesiredSize;
          uiElement.Arrange(new Rect(0.0, 0.0, desiredSize.Width, desiredSize.Height));
        }
      }
      return finalSize;
    }

    private IEnumerable<UIElement> GetChildrenSync()
    {
      IEnumerable<UIElement> children = (IEnumerable<UIElement>) null;
      this._uiDispatcher.Invoke((SendOrPostCallback) (nothing => children = (IEnumerable<UIElement>) ((IEnumerable<UIElement>) this.Children).ToArray<UIElement>()), (object) null);
      return children;
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this.OnDispose();
      this._disposed = true;
    }

    protected virtual void OnDispose()
    {
      foreach (UIElement uiElement in this.GetChildrenSync())
      {
        if (uiElement is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }
}
