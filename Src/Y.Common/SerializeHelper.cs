// Decompiled with JetBrains decompiler
// Type: Y.Common.SerializeHelper
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Y.Common
{
  public static class SerializeHelper
  {
    private static readonly Regex ParseException = new Regex("There is an error in XML document \\((\\d+), (\\d+)\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex InternetException = new Regex("html", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

    public static bool CanDeserialize<T>(string data)
    {
      try
      {
        using (StringReader input = new StringReader(data))
          return new XmlSerializer(typeof (T)).CanDeserialize(XmlReader.Create((TextReader) input));
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static T DeserializeAndValidate<T>(string data) where T : class
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      using (StringReader stringReader = new StringReader(data))
      {
        try
        {
          return (T) xmlSerializer.Deserialize((TextReader) stringReader);
        }
        catch (Exception ex)
        {
          return default (T);
        }
      }
    }

    public static T Deserialize<T>(string data)
    {
      using (StringReader input = new StringReader(data))
        return (T) new XmlSerializer(typeof (T)).Deserialize(XmlReader.Create((TextReader) input));
    }

    public static T Deserialize<T>(Stream stream) => (T) new XmlSerializer(typeof (T)).Deserialize(stream);

    public static T Deserialize<T>(string data, string dataUrl)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      using (StringReader stringReader = new StringReader(data))
      {
        try
        {
          return (T) xmlSerializer.Deserialize((TextReader) stringReader);
        }
        catch (InvalidOperationException ex)
        {
          string message = ex.Message;
          Match match = SerializeHelper.ParseException.Match(message);
          if (match.Success)
          {
            string str = data;
            int int32 = Convert.ToInt32(match.Groups[2].Value);
            int startIndex = int32 - 100;
            if (startIndex < 0)
              startIndex = 0;
            int num = int32 + 100;
            if (num > str.Length)
              num = str.Length - 1;
            int length = num - startIndex;
            if (length < 0)
              length = 0;
            string input = string.Empty;
            if (startIndex + length < str.Length)
              input = str.Substring(startIndex, length);
            if (SerializeHelper.InternetException.Match(input).Success)
              throw new InternetConnectionException(message, (Exception) ex);
            throw new InvalidOperationException(message + string.Format("| Details: from {0} to {1},|{2}|", (object) startIndex, (object) num, (object) input) + string.Format("| Data Url: {0}|", (object) dataUrl), (Exception) ex);
          }
          throw;
        }
      }
    }
  }
}
