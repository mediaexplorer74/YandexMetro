// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JArray
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  public class JArray : 
    JContainer,
    IList<JToken>,
    ICollection<JToken>,
    IEnumerable<JToken>,
    IEnumerable
  {
    private IList<JToken> _values = (IList<JToken>) new List<JToken>();

    protected override IList<JToken> ChildrenTokens => this._values;

    public override JTokenType Type => JTokenType.Array;

    public JArray()
    {
    }

    public JArray(JArray other)
      : base((JContainer) other)
    {
    }

    public JArray(params object[] content)
      : this((object) content)
    {
    }

    public JArray(object content) => this.Add(content);

    internal override bool DeepEquals(JToken node) => node is JArray container && this.ContentsEqual((JContainer) container);

    internal override JToken CloneToken() => (JToken) new JArray(this);

    public static JArray Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw new Exception("Error reading JArray from JsonReader.");
      if (reader.TokenType != JsonToken.StartArray)
        throw new Exception("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JArray jarray = new JArray();
      jarray.SetLineInfo(reader as IJsonLineInfo);
      jarray.ReadTokenFrom(reader);
      return jarray;
    }

    public static JArray Parse(string json) => JArray.Load((JsonReader) new JsonTextReader((TextReader) new StringReader(json)));

    public static JArray FromObject(object o) => JArray.FromObject(o, new JsonSerializer());

    public static JArray FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      return jtoken.Type == JTokenType.Array ? (JArray) jtoken : throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartArray();
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
        childrenToken.WriteTo(writer, converters);
      writer.WriteEndArray();
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        return key is int index ? this.GetItem(index) : throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int index))
          throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    public JToken this[int index]
    {
      get => this.GetItem(index);
      set => this.SetItem(index, value);
    }

    public int IndexOf(JToken item) => this.IndexOfItem(item);

    public void Insert(int index, JToken item) => this.InsertItem(index, item);

    public void RemoveAt(int index) => this.RemoveItemAt(index);

    public void Add(JToken item) => this.Add((object) item);

    public void Clear() => this.ClearItems();

    public bool Contains(JToken item) => this.ContainsItem(item);

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex) => this.CopyItemsTo((Array) array, arrayIndex);

    bool ICollection<JToken>.IsReadOnly => false;

    public bool Remove(JToken item) => this.RemoveItem(item);

    internal override int GetDeepHashCode() => this.ContentsHashCode();
  }
}
