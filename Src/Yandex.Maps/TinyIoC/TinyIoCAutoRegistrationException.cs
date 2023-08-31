// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCAutoRegistrationException
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyIoC
{
  internal class TinyIoCAutoRegistrationException : Exception
  {
    private const string ERROR_TEXT = "Duplicate implementation of type {0} found ({1}).";

    public TinyIoCAutoRegistrationException(Type registerType, IEnumerable<Type> types)
      : base(string.Format("Duplicate implementation of type {0} found ({1}).", (object) registerType, (object) TinyIoCAutoRegistrationException.GetTypesString(types)))
    {
    }

    public TinyIoCAutoRegistrationException(
      Type registerType,
      IEnumerable<Type> types,
      Exception innerException)
      : base(string.Format("Duplicate implementation of type {0} found ({1}).", (object) registerType, (object) TinyIoCAutoRegistrationException.GetTypesString(types)), innerException)
    {
    }

    private static string GetTypesString(IEnumerable<Type> types) => string.Join(",", types.Select<Type, string>((Func<Type, string>) (type => type.FullName)).ToArray<string>());
  }
}
