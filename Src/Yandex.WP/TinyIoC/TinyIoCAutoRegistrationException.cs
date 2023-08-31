// Decompiled with JetBrains decompiler
// Type: TinyIoC.TinyIoCAutoRegistrationException
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyIoC
{
  public class TinyIoCAutoRegistrationException : Exception
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
