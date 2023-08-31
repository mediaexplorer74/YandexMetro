// Decompiled with JetBrains decompiler
// Type: Y.Common.InternetConnectionException
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;

namespace Y.Common
{
  public class InternetConnectionException : InvalidOperationException
  {
    public InternetConnectionException()
    {
    }

    public InternetConnectionException(string message)
      : base(message)
    {
    }

    public InternetConnectionException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
