// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.ICommunicator`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.WebUtils.Events;

namespace Yandex.WebUtils.Interfaces
{
  internal interface ICommunicator<TParameters, TResult> where TResult : class
  {
    event RequestCompletedEventHandler<TParameters, TResult> RequestCompleted;

    event EventHandler<RequestFailedEventArgs<TParameters>> RequestFailed;

    void Request(TParameters parameters);
  }
}
