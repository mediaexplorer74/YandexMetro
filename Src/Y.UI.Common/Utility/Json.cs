// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.Json
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Y.UI.Common.Utility
{
  public static class Json
  {
    public static T Deserialize<T>(string json) => (T) Y.UI.Common.Utility.Json.Deserialize(json, typeof (T));

    public static object Deserialize(string json, Type objectType)
    {
      using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
        return new DataContractJsonSerializer(objectType).ReadObject((Stream) memoryStream);
    }

    public static string Serialize<T>(T objectForSerialization)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, (object) objectForSerialization);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        string str = (string) null;
        using (StreamReader streamReader = new StreamReader((Stream) memoryStream))
          str = streamReader.ReadToEnd();
        return str;
      }
    }
  }
}
