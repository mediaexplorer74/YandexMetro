// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.QueryStringUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Yandex.WebUtils
{
  public class QueryStringUtil
  {
    public static string ToQueryString(Dictionary<string, object> parameters)
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, object> keyValuePair in parameters.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (p => null != p.Value)))
      {
        string stringValue = QueryStringUtil.GetStringValue(keyValuePair.Value);
        stringList.Add(string.Format("{0}={1}", (object) Uri.EscapeDataString(keyValuePair.Key), (object) Uri.EscapeDataString(stringValue)));
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
        stringList.Add(string.Format("{0}={1}", (object) Uri.EscapeDataString(keyValuePair.Key), (object) stringValue));
      }
      string str = string.Join("&", stringList.ToArray());
      if (sb.Length > 0)
        sb.Append("&");
      sb.Append(str);
    }

    public static string GetStringValue(object o) => !(o is IConvertible convertible) ? o.ToString() : convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
