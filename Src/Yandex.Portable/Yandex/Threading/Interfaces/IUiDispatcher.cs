// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Interfaces.IUiDispatcher
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Threading;

namespace Yandex.Threading.Interfaces
{
  public interface IUiDispatcher
  {
    object BeginInvoke(Action action);

    object BeginInvoke(Delegate d, params object[] args);

    object BeginInvokeExplicit(Action func);

    void Invoke(SendOrPostCallback d, object state);

    bool CheckAccess();
  }
}
