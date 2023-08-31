// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCRegistrationTypeException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace TinyIoC
{
  public class TinyIoCRegistrationTypeException : Exception
  {
    private const string REGISTER_ERROR_TEXT = "Cannot register type {0} - abstract classes or interfaces are not valid implementation types for {1}.";

    public TinyIoCRegistrationTypeException(Type type, string factory)
      : base(string.Format("Cannot register type {0} - abstract classes or interfaces are not valid implementation types for {1}.", (object) type.FullName, (object) factory))
    {
    }

    public TinyIoCRegistrationTypeException(Type type, string factory, Exception innerException)
      : base(string.Format("Cannot register type {0} - abstract classes or interfaces are not valid implementation types for {1}.", (object) type.FullName, (object) factory), innerException)
    {
    }
  }
}
