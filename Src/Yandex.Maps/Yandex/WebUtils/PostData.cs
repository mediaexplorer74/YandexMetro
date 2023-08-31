// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.PostData
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Yandex.Globalization;

namespace Yandex.WebUtils
{
  internal class PostData
  {
    public string Boundary { get; set; }

    public Dictionary<string, object> PlainTextParams { get; private set; }

    public Dictionary<string, byte[]> CompressedParams { get; private set; }

    public string CompressedContentType { get; set; }

    public PostData()
    {
      this.PlainTextParams = new Dictionary<string, object>();
      this.CompressedParams = new Dictionary<string, byte[]>();
    }

    public void WriteToStream(Stream postStream)
    {
      this.PlainTextParams["lang"] = (object) CultureInfo.CurrentUICulture.GetSpecificName();
      bool flag1 = false;
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (KeyValuePair<string, object> plainTextParam in this.PlainTextParams)
      {
        stringBuilder1.Append("--").AppendLine(this.Boundary).Append("Content-Disposition: form-data; name=\"").Append(plainTextParam.Key).Append("\"");
        if (!flag1)
        {
          stringBuilder1.Append("\r\nContent-Type:text/plain");
          flag1 = true;
        }
        string stringValue = QueryStringUtil.GetStringValue(plainTextParam.Value);
        stringBuilder1.Append("\r\n\r\n").AppendLine(stringValue);
      }
      byte[] bytes1 = Encoding.UTF8.GetBytes(stringBuilder1.ToString());
      postStream.Write(bytes1, 0, bytes1.Length);
      bool flag2 = false;
      foreach (KeyValuePair<string, byte[]> compressedParam in this.CompressedParams)
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("--").AppendLine(this.Boundary).Append("Content-Disposition: form-data; name=\"").Append(compressedParam.Key).Append("\"");
        if (!flag2)
        {
          stringBuilder2.Append("\r\nContent-Type:").Append(this.CompressedContentType);
          flag2 = true;
        }
        stringBuilder2.Append("\r\n\r\n");
        byte[] bytes2 = Encoding.UTF8.GetBytes(stringBuilder2.ToString());
        postStream.Write(bytes2, 0, bytes2.Length);
        postStream.Write(compressedParam.Value, 0, compressedParam.Value.Length);
      }
      byte[] bytes3 = Encoding.UTF8.GetBytes("\r\n--" + this.Boundary + "--\r\n");
      postStream.Write(bytes3, 0, bytes3.Length);
    }
  }
}
