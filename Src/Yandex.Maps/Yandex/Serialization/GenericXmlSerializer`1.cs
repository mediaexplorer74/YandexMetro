// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.GenericXmlSerializer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Ionic.Zlib;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  internal class GenericXmlSerializer<T> : IGenericXmlSerializer<T> where T : class
  {
    private XmlSerializer _xmlSerializer;

    public string Serialize(T value)
    {
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
      {
        Encoding = (Encoding) new UTF8Encoding()
      };
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add("", "");
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream, xmlWriterSettings))
          this.GetXmlSerializer().Serialize(xmlWriter, (object) value, namespaces);
        return Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int) memoryStream.Length);
      }
    }

    public void Serialize(T value, Stream outStream)
    {
      if ((object) value == null)
        throw new ArgumentNullException(nameof (value));
      if (outStream == null)
        throw new ArgumentNullException(nameof (outStream));
      if (!outStream.CanWrite)
        throw new ArgumentException("outStream should support writing.");
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
      {
        Encoding = (Encoding) new UTF8Encoding()
      };
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add("", "");
      using (XmlWriter xmlWriter = XmlWriter.Create(outStream, xmlWriterSettings))
        this.GetXmlSerializer().Serialize(xmlWriter, (object) value, namespaces);
    }

    public T Deserialize(string inputString)
    {
      if (string.IsNullOrEmpty(inputString))
        throw new ArgumentException("inputString is null or empty.");
      using (StringReader stringReader = new StringReader(inputString))
        return (T) this.GetXmlSerializer().Deserialize((TextReader) stringReader);
    }

    public T Deserialize(XmlReader xmlReader) => xmlReader != null ? (T) this.GetXmlSerializer().Deserialize(xmlReader) : throw new ArgumentNullException(nameof (xmlReader));

    public T Deserialize(Stream inputStream, bool leaveOpen = false)
    {
      if (inputStream == null)
        throw new ArgumentNullException(nameof (inputStream));
      if (!inputStream.CanRead)
        throw new ArgumentException("inputStream should support reading.");
      XmlSerializer xmlSerializer = this.GetXmlSerializer();
      XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
      {
        IgnoreWhitespace = true
      };
      if (!this.DataIsCompressed)
        return GenericXmlSerializer<T>.DeserializeFromStream(inputStream, xmlSerializer, xmlReaderSettings);
      using (GZipStream inputStream1 = new GZipStream(inputStream, CompressionMode.Decompress, leaveOpen))
        return GenericXmlSerializer<T>.DeserializeFromStream((Stream) inputStream1, xmlSerializer, xmlReaderSettings);
    }

    private static T DeserializeFromStream(
      Stream inputStream,
      XmlSerializer ser,
      XmlReaderSettings xmlReaderSettings)
    {
      using (StreamReader streamReader = new StreamReader(inputStream, Encoding.UTF8))
      {
        using (XmlReader xmlReader = XmlReader.Create((TextReader) streamReader, xmlReaderSettings))
          return (T) ser.Deserialize(xmlReader);
      }
    }

    protected virtual bool DataIsCompressed => false;

    private XmlSerializer GetXmlSerializer() => this._xmlSerializer ?? (this._xmlSerializer = new XmlSerializer(typeof (T)));
  }
}
