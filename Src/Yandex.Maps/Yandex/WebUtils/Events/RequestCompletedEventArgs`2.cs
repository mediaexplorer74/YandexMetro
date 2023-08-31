// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestCompletedEventArgs`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.WebUtils.Events
{
  [ComVisible(true)]
  public class RequestCompletedEventArgs<TParameters, TResults> : EventArgs where TResults : class
  {
    public RequestCompletedEventArgs(TParameters parameters, TResults requestResults)
      : this(parameters, requestResults, (CacheValidators) null)
    {
      this.Parameters = parameters;
      this.RequestResults = requestResults;
    }

    public RequestCompletedEventArgs(
      TParameters parameters,
      TResults requestResults,
      CacheValidators cacheValidators)
    {
      this.Parameters = parameters;
      this.RequestResults = requestResults;
      this.CacheValidators = cacheValidators;
    }

    public TParameters Parameters { get; private set; }

    public TResults RequestResults { get; private set; }

    public CacheValidators CacheValidators { get; private set; }
  }
}
