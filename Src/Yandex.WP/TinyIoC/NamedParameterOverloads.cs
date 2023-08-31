// Decompiled with JetBrains decompiler
// Type: TinyIoC.NamedParameterOverloads
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Collections.Generic;

namespace TinyIoC
{
  public sealed class NamedParameterOverloads : Dictionary<string, object>
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
