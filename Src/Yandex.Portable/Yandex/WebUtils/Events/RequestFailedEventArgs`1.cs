// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestFailedEventArgs`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.WebUtils.Events
{
  public class RequestFailedEventArgs<TParameters> : EventArgs
  {
    public RequestFailedEventArgs(Exception exception) => this.Exception = exception != null ? exception : throw new ArgumentNullException(nameof (exception));

    public RequestFailedEventArgs(Exception exception, TParameters parameters)
    {
      this.Exception = exception != null ? exception : throw new ArgumentNullException(nameof (exception));
      this.Parameters = parameters;
    }

    public Exception Exception { get; private set; }

    public TParameters Parameters { get; private set; }
  }
}
