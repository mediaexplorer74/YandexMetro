// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestCompletedEventArgs`2
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.WebUtils.Events
{
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
