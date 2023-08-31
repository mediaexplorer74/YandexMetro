// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.UiDispatcher
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  public class UiDispatcher : IUiDispatcher
  {
    private Dispatcher _dispatcher;
    private DispatcherSynchronizationContext _context;

    private Dispatcher Dispatcher
    {
      get
      {
        if (this._dispatcher == null)
        {
          this._dispatcher = ((DependencyObject) Deployment.Current).Dispatcher;
          this._context = new DispatcherSynchronizationContext(this._dispatcher);
        }
        return this._dispatcher;
      }
    }

    public object BeginInvoke(Action action)
    {
      if (!this.Dispatcher.CheckAccess())
        return (object) this.Dispatcher.BeginInvoke(action);
      action();
      return (object) null;
    }

    public object BeginInvoke(Delegate d, params object[] args)
    {
      if (!this.Dispatcher.CheckAccess())
        return (object) this.Dispatcher.BeginInvoke(d, args);
      d.DynamicInvoke(args);
      return (object) null;
    }

    public void Invoke(SendOrPostCallback d, object state)
    {
      if (this.Dispatcher.CheckAccess())
        d(state);
      else
        ((SynchronizationContext) this._context).Send(d, state);
    }

    public bool CheckAccess() => this.Dispatcher.CheckAccess();

    public object BeginInvokeExplicit(Action action) => (object) this.Dispatcher.BeginInvoke(action);
  }
}
