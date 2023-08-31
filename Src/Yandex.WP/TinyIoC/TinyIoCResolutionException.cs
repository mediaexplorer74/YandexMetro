// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCResolutionException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace TinyIoC
{
  public class TinyIoCResolutionException : Exception
  {
    private const string ERROR_TEXT = "Unable to resolve type: {0}";

    public TinyIoCResolutionException(Type type)
      : base(string.Format("Unable to resolve type: {0}", (object) type.FullName))
    {
    }

    public TinyIoCResolutionException(Type type, Exception innerException)
      : base(string.Format("Unable to resolve type: {0}", (object) type.FullName), innerException)
    {
    }
  }
}
