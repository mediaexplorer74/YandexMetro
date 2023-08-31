// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCResolutionException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace TinyIoC
{
  internal class TinyIoCResolutionException : Exception
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
