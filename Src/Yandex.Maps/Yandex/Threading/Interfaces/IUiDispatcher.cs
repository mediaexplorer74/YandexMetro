// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Interfaces.IUiDispatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Threading;

namespace Yandex.Threading.Interfaces
{
  internal interface IUiDispatcher
  {
    object BeginInvoke(Action action);

    object BeginInvoke(Delegate d, params object[] args);

    object BeginInvokeExplicit(Action func);

    void Invoke(SendOrPostCallback d, object state);

    bool CheckAccess();
  }
}
