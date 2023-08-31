// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.SerializationBinder
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  public abstract class SerializationBinder
  {
    public abstract Type BindToType(string assemblyName, string typeName);

    public virtual void BindToName(
      Type serializedType,
      out string assemblyName,
      out string typeName)
    {
      assemblyName = (string) null;
      typeName = (string) null;
    }
  }
}
