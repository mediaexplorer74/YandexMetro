// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.SharpXmlSerializer`1
// Assembly: Yandex.Serialization.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92CA2ACD-4B0D-4692-89EF-43480E0E9364
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Serialization.WP.dll

using Ionic.Zlib;
using Polenter.Serialization;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  public class SharpXmlSerializer<T> : IGenericXmlSerializer<T> where T : class
  {
    private SharpSerializer _sharpSerializer;

    public string Serialize(T value)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.GetSerializer().Serialize((object) value, (Stream) memoryStream);
        return Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int) memoryStream.Length);
      }
    }

    public T Deserialize(string inputString)
    {
      if (string.IsNullOrEmpty(inputString))
        throw new ArgumentException("inputString is null or empty.");
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(inputString)))
        return (T) new SharpSerializer().Deserialize((Stream) memoryStream);
    }

    public void Serialize(T value, Stream outStream)
    {
      if ((object) value == null)
        throw new ArgumentNullException(nameof (value));
      if (outStream == null)
        throw new ArgumentNullException(nameof (outStream));
      if (!outStream.CanWrite)
        throw new ArgumentException("outStream should support writing.");
      this.GetSerializer().Serialize((object) value, outStream);
    }

    public T Deserialize(Stream inputStream, bool leaveOpen = false)
    {
      if (inputStream == null)
        throw new ArgumentNullException(nameof (inputStream));
      if (!inputStream.CanRead)
        throw new ArgumentException("inputStream should support reading.");
      SharpSerializer deserializer = this.GetDeserializer();
      if (this.DataIsCompressed)
      {
        using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress, leaveOpen))
          return (T) deserializer.Deserialize((Stream) gzipStream);
      }
      else
      {
        try
        {
          return (T) deserializer.Deserialize(inputStream);
        }
        finally
        {
          if (!leaveOpen)
            inputStream.Dispose();
        }
      }
    }

    protected virtual bool DataIsCompressed => false;

    protected virtual SharpSerializer GetSerializer() => this._sharpSerializer ?? (this._sharpSerializer = SharpXmlSerializer<T>.CreateAndInitSharpSerializer());

    protected virtual SharpSerializer GetDeserializer() => SharpXmlSerializer<T>.CreateAndInitSharpSerializer();

    private static SharpSerializer CreateAndInitSharpSerializer()
    {
      SharpSerializer initSharpSerializer = new SharpSerializer(true);
      initSharpSerializer.PropertyProvider.AttributesToIgnore.Clear();
      initSharpSerializer.PropertyProvider.AttributesToIgnore.Add(typeof (XmlIgnoreAttribute));
      return initSharpSerializer;
    }
  }
}
