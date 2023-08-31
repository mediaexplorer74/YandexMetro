// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.DictionaryExtensions
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Y.Common.Extensions
{
  public static class DictionaryExtensions
  {
    public static string ToPostData(this Dictionary<string, string> values) => string.Join("&", values.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => string.Format("{0}={1}", (object) HttpUtility.UrlEncode(kvp.Key), (object) HttpUtility.UrlEncode(kvp.Value)))));

    public static string ToQueryString(this Dictionary<string, string> source, string baseUrl)
    {
      if (source.Count == 0)
        return baseUrl;
      string str = string.Join("&", source.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kvp => string.Format("{0}={1}", (object) HttpUtility.UrlEncode(kvp.Key), (object) HttpUtility.UrlEncode(kvp.Value)))).ToArray<string>());
      return string.Format(baseUrl.Contains("?") ? "{0}&{1}" : "{0}?{1}", (object) baseUrl, (object) str);
    }

    public static Dictionary<string, string> ToDictionary(this string postData)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (!string.IsNullOrWhiteSpace(postData))
      {
        string str1 = postData;
        string[] separator1 = new string[1]{ "&" };
        foreach (string str2 in str1.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
        {
          string[] separator2 = new string[1]{ "=" };
          string[] strArray = str2.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length == 2)
            dictionary.Add(strArray[0], strArray[1]);
        }
      }
      return dictionary;
    }
  }
}
