// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCConstructorResolutionException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace TinyIoC
{
  public class TinyIoCConstructorResolutionException : Exception
  {
    private const string ERROR_TEXT = "Unable to resolve constructor for {0} using provided Expression.";

    public TinyIoCConstructorResolutionException(Type type)
      : base(string.Format("Unable to resolve constructor for {0} using provided Expression.", (object) type.FullName))
    {
    }

    public TinyIoCConstructorResolutionException(Type type, Exception innerException)
      : base(string.Format("Unable to resolve constructor for {0} using provided Expression.", (object) type.FullName), innerException)
    {
    }

    public TinyIoCConstructorResolutionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public TinyIoCConstructorResolutionException(string message)
      : base(message)
    {
    }
  }
}
