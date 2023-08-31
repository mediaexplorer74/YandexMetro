// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestFailedEventArgs`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.WebUtils.Events
{
  [ComVisible(true)]
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
