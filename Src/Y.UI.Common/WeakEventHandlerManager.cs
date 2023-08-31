// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.WeakEventHandlerManager
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Y.UI.Common
{
  public static class WeakEventHandlerManager
  {
    public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
    {
      if (handlers == null)
        return;
      EventHandler[] callees = new EventHandler[handlers.Count];
      int count = 0;
      int num = WeakEventHandlerManager.CleanupOldHandlers(handlers, callees, count);
      for (int index = 0; index < num; ++index)
        WeakEventHandlerManager.CallHandler(sender, callees[index]);
    }

    private static void CallHandler(object sender, EventHandler eventHandler)
    {
      WeakEventHandlerManager.DispatcherProxy dispatcher = WeakEventHandlerManager.DispatcherProxy.CreateDispatcher();
      if (eventHandler == null)
        return;
      if (dispatcher != null && !dispatcher.CheckAccess())
        dispatcher.BeginInvoke((Delegate) new Action<object, EventHandler>(WeakEventHandlerManager.CallHandler), sender, (object) eventHandler);
      else
        eventHandler(sender, EventArgs.Empty);
    }

    private static int CleanupOldHandlers(
      List<WeakReference> handlers,
      EventHandler[] callees,
      int count)
    {
      for (int index = handlers.Count - 1; index >= 0; --index)
      {
        if (!(handlers[index].Target is EventHandler target))
        {
          handlers.RemoveAt(index);
        }
        else
        {
          callees[count] = target;
          ++count;
        }
      }
      return count;
    }

    public static void AddWeakReferenceHandler(
      ref List<WeakReference> handlers,
      EventHandler handler,
      int defaultListSize)
    {
      if (handlers == null)
        handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
      handlers.Add(new WeakReference((object) handler));
    }

    public static void RemoveWeakReferenceHandler(
      List<WeakReference> handlers,
      EventHandler handler)
    {
      if (handlers == null)
        return;
      for (int index = handlers.Count - 1; index >= 0; --index)
      {
        if (!(handlers[index].Target is EventHandler target) || target == handler)
          handlers.RemoveAt(index);
      }
    }

    private class DispatcherProxy
    {
      private Dispatcher innerDispatcher;

      private DispatcherProxy(Dispatcher dispatcher) => this.innerDispatcher = dispatcher;

      public static WeakEventHandlerManager.DispatcherProxy CreateDispatcher() => Deployment.Current == null ? (WeakEventHandlerManager.DispatcherProxy) null : new WeakEventHandlerManager.DispatcherProxy(((DependencyObject) Deployment.Current).Dispatcher);

      public bool CheckAccess() => this.innerDispatcher.CheckAccess();

      public DispatcherOperation BeginInvoke(Delegate method, params object[] args) => this.innerDispatcher.BeginInvoke(method, args);
    }
  }
}
