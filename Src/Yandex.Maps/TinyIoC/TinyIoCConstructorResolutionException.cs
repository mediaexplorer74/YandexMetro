// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCConstructorResolutionException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace TinyIoC
{
  internal class TinyIoCConstructorResolutionException : Exception
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
