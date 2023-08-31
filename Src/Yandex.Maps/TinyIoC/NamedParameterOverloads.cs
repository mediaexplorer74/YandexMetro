// Decompiled with JetBrains decompiler
// Type: TinyIoC.NamedParameterOverloads
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;

namespace TinyIoC
{
  internal sealed class NamedParameterOverloads : Dictionary<string, object>
  {
    private static readonly NamedParameterOverloads _Default = new NamedParameterOverloads();

    public static NamedParameterOverloads FromIDictionary(IDictionary<string, object> data) => data is NamedParameterOverloads parameterOverloads ? parameterOverloads : new NamedParameterOverloads(data);

    public NamedParameterOverloads()
    {
    }

    public NamedParameterOverloads(IDictionary<string, object> data)
      : base(data)
    {
    }

    public static NamedParameterOverloads Default => NamedParameterOverloads._Default;
  }
}
