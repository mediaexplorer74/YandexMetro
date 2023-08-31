// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IDiversityCommunicator`3
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.WebUtils.Events;

namespace Yandex.WebUtils.Interfaces
{
  internal interface IDiversityCommunicator<TParameters, TResult, TErrorResult> : 
    ICommunicator<TParameters, TResult>
    where TResult : class
    where TErrorResult : class
  {
    event RequestCompletedEventHandler<TParameters, TErrorResult> RequestCompletedWithErrorResult;
  }
}
