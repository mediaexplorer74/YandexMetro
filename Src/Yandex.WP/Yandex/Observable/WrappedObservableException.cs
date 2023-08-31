// Decompiled with JetBrains decompiler
// Type: Yandex.Observable.WrappedObservableException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace Yandex.Observable
{
  public class WrappedObservableException : Exception
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
