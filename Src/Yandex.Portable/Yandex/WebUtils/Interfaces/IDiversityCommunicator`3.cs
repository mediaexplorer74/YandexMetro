﻿// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IDiversityCommunicator`3
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using Yandex.WebUtils.Events;

namespace Yandex.WebUtils.Interfaces
{
  public interface IDiversityCommunicator<TParameters, TResult, TErrorResult> : 
    ICommunicator<TParameters, TResult>
    where TResult : class
    where TErrorResult : class
  {
    event RequestCompletedEventHandler<TParameters, TErrorResult> RequestCompletedWithErrorResult;
  }
}
