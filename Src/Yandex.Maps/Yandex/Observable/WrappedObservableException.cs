// Decompiled with JetBrains decompiler
// Type: Yandex.Observable.WrappedObservableException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Observable
{
  internal class WrappedObservableException : Exception
  {
    public WrappedObservableException()
    {
    }

    public WrappedObservableException(string message)
      : base(message)
    {
    }

    public WrappedObservableException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
