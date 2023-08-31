// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.QueryStringUtil
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Yandex.WebUtils
{
  internal class QueryStringUtil
  {
    private static string EscapeDataString(string stringToEscape) => Uri.EscapeDataString(stringToEscape).Replace("%2C", ",");

    public static string ToQueryString(Dictionary<string, object> parameters)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, object> keyValuePair in parameters.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (p => null != p.Value)))
      {
        string stringValue = QueryStringUtil.GetStringValue(keyValuePair.Value);
        stringList.Add(string.Format("{0}={1}", (object) QueryStringUtil.EscapeDataString(keyValuePair.Key), (object) QueryStringUtil.EscapeDataString(stringValue)));
      }
      return "?" + string.Join("&", stringList.ToArray());
    }

    public static void ToQueryStringNoUrlEncoding(
      StringBuilder sb,
      Dictionary<string, object> parameters)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, object> keyValuePair in parameters.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (p => null != p.Value)))
      {
        string stringValue = QueryStringUtil.GetStringValue(keyValuePair.Value);
        stringList.Add(string.Format("{0}={1}", (object) QueryStringUtil.EscapeDataString(keyValuePair.Key), (object) stringValue));
      }
      string str = string.Join("&", stringList.ToArray());
      if (sb.Length > 0)
        sb.Append("&");
      sb.Append(str);
    }

    public static string GetStringValue(object o) => !(o is IConvertible convertible) ? o.ToString() : convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
