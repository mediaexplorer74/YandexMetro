// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Adapters.LayerAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Yandex.Maps.API.Adapters
{
  [ComVisible(true)]
  public class LayerAdapter
  {
    public static BaseLayers StringToLayer(string value) => ((IEnumerable<string>) value.Split(',')).Select<string, string>((Func<string, string>) (t => t.Trim())).Aggregate<string, BaseLayers>(BaseLayers.none, (Func<BaseLayers, string, BaseLayers>) ((current, layer) => current | (BaseLayers) Enum.Parse(typeof (BaseLayers), layer, true)));

    public static string LayerToString(BaseLayers value)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 1;
      bool flag = true;
      for (int index = 0; index < 32; ++index)
      {
        if ((value & (BaseLayers) num) == (BaseLayers) num)
        {
          string name = Enum.GetName(typeof (BaseLayers), (object) num);
          if (!string.IsNullOrEmpty(name))
          {
            if (flag)
              flag = false;
            else
              stringBuilder.Append(",");
            stringBuilder.Append(name);
          }
        }
        num <<= 1;
      }
      return stringBuilder.ToString();
    }
  }
}
