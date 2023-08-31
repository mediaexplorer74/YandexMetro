// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCRegistrationTypeException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace TinyIoC
{
  internal class TinyIoCRegistrationTypeException : Exception
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
