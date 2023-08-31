// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCRegistrationException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;

namespace TinyIoC
{
  public class TinyIoCRegistrationException : Exception
  {
    private const string CONVERT_ERROR_TEXT = "Cannot convert current registration of {0} to {1}";
    private const string GENERIC_CONSTRAINT_ERROR_TEXT = "Type {1} is not valid for a registration of type {0}";

    public TinyIoCRegistrationException(Type type, string method)
      : base(string.Format("Cannot convert current registration of {0} to {1}", (object) type.FullName, (object) method))
    {
    }

    public TinyIoCRegistrationException(Type type, string method, Exception innerException)
      : base(string.Format("Cannot convert current registration of {0} to {1}", (object) type.FullName, (object) method), innerException)
    {
    }

    public TinyIoCRegistrationException(Type registerType, Type implementationType)
      : base(string.Format("Type {1} is not valid for a registration of type {0}", (object) registerType.FullName, (object) implementationType.FullName))
    {
    }

    public TinyIoCRegistrationException(
      Type registerType,
      Type implementationType,
      Exception innerException)
      : base(string.Format("Type {1} is not valid for a registration of type {0}", (object) registerType.FullName, (object) implementationType.FullName), innerException)
    {
    }
  }
}
