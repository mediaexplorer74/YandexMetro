// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IDataManager`2
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using Yandex.WebUtils.Events;

namespace Yandex.WebUtils.Interfaces
{
  public interface IDataManager<TParameters, TResult> where TResult : class
  {
    event RequestCompletedEventHandler<TParameters, TResult> RequestCompleted;

    event EventHandler<RequestFailedEventArgs<TParameters>> RequestFailed;

    void Query(TParameters parameters);
  }
}
