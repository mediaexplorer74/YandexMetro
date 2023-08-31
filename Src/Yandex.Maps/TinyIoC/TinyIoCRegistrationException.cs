// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCRegistrationException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace TinyIoC
{
  internal class TinyIoCRegistrationException : Exception
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
