// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JObject
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
  public class JObject : 
    JContainer,
    IDictionary<string, JToken>,
    ICollection<KeyValuePair<string, JToken>>,
    IEnumerable<KeyValuePair<string, JToken>>,
    IEnumerable,
    INotifyPropertyChanged
  {
    private JObject.JPropertKeyedCollection _properties = new JObject.JPropertKeyedCollection((IEqualityComparer<string>) StringComparer.Ordinal);

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._properties;

    public event PropertyChangedEventHandler PropertyChanged;

    public JObject()
    {
    }

    public JObject(JObject other)
      : base((JContainer) other)
    {
    }

    public JObject(params object[] content)
      : this((object) content)
    {
    }

    public JObject(object content) => this.Add(content);

    internal override bool DeepEquals(JToken node) => node is JObject container && this.ContentsEqual((JContainer) container);

    internal override void InsertItem(int index, JToken item)
    {
      if (item != null && item.Type == JTokenType.Comment)
        return;
      base.InsertItem(index, item);
    }

    internal override void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      JProperty jproperty1 = o.Type == JTokenType.Property ? (JProperty) o : throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
      if (existing != null)
      {
        JProperty jproperty2 = (JProperty) existing;
        if (jproperty1.Name == jproperty2.Name)
          return;
      }
      if (this._properties.Dictionary != null && this._properties.Dictionary.TryGetValue(jproperty1.Name, out existing))
        throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jproperty1.Name, (object) this.GetType()));
    }

    internal void InternalPropertyChanged(JProperty childProperty)
    {
      this.OnPropertyChanged(childProperty.Name);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object) childProperty, (object) childProperty, this.IndexOfItem((JToken) childProperty)));
    }

    internal void InternalPropertyChanging(JProperty childProperty)
    {
    }

    internal override JToken CloneToken() => (JToken) new JObject(this);

    public override JTokenType Type => JTokenType.Object;

    public IEnumerable<JProperty> Properties() => this.ChildrenTokens.Cast<JProperty>();

    public JProperty Property(string name)
    {
      if (this._properties.Dictionary == null)
        return (JProperty) null;
      if (name == null)
        return (JProperty) null;
      JToken jtoken;
      this._properties.Dictionary.TryGetValue(name, out jtoken);
      return (JProperty) jtoken;
    }

    public JEnumerable<JToken> PropertyValues() => new JEnumerable<JToken>(this.Properties().Select<JProperty, JToken>((Func<JProperty, JToken>) (p => p.Value)));

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        return key is string propertyName ? this[propertyName] : throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is string propertyName))
          throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this[propertyName] = value;
      }
    }

    public JToken this[string propertyName]
    {
      get
      {
        ValidationUtils.ArgumentNotNull((object) propertyName, nameof (propertyName));
        return this.Property(propertyName)?.Value;
      }
      set
      {
        JProperty jproperty = this.Property(propertyName);
        if (jproperty != null)
        {
          jproperty.Value = value;
        }
        else
        {
          this.Add((object) new JProperty(propertyName, (object) value));
          this.OnPropertyChanged(propertyName);
        }
      }
    }

    public static JObject Load(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw new Exception("Error reading JObject from JsonReader.");
      if (reader.TokenType != JsonToken.StartObject)
        throw new Exception("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JObject jobject = new JObject();
      jobject.SetLineInfo(reader as IJsonLineInfo);
      jobject.ReadTokenFrom(reader);
      return jobject;
    }

    public static JObject Parse(string json) => JObject.Load((JsonReader) new JsonTextReader((TextReader) new StringReader(json)));

    public static JObject FromObject(object o) => JObject.FromObject(o, new JsonSerializer());

    public static JObject FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      return jtoken == null || jtoken.Type == JTokenType.Object ? (JObject) jtoken : throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartObject();
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
        childrenToken.WriteTo(writer, converters);
      writer.WriteEndObject();
    }

    public void Add(string propertyName, JToken value) => this.Add((object) new JProperty(propertyName, (object) value));

    bool IDictionary<string, JToken>.ContainsKey(string key) => this._properties.Dictionary != null && this._properties.Dictionary.ContainsKey(key);

    ICollection<string> IDictionary<string, JToken>.Keys => this._properties.Dictionary.Keys;

    public bool Remove(string propertyName)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
        return false;
      jproperty.Remove();
      return true;
    }

    public bool TryGetValue(string propertyName, out JToken value)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
      {
        value = (JToken) null;
        return false;
      }
      value = jproperty.Value;
      return true;
    }

    ICollection<JToken> IDictionary<string, JToken>.Values => this._properties.Dictionary.Values;

    void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item) => this.Add((object) new JProperty(item.Key, (object) item.Value));

    void ICollection<KeyValuePair<string, JToken>>.Clear() => this.RemoveAll();

    bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
    {
      JProperty jproperty = this.Property(item.Key);
      return jproperty != null && jproperty.Value == item.Value;
    }

    void ICollection<KeyValuePair<string, JToken>>.CopyTo(
      KeyValuePair<string, JToken>[] array,
      int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JProperty childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        array[arrayIndex + num] = new KeyValuePair<string, JToken>(childrenToken.Name, childrenToken.Value);
        ++num;
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly => false;

    bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
    {
      if (!((ICollection<KeyValuePair<string, JToken>>) this).Contains(item))
        return false;
      this.Remove(item.Key);
      return true;
    }

    internal override int GetDeepHashCode() => this.ContentsHashCode();

    public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
    {
      foreach (JProperty property in (IEnumerable<JToken>) this.ChildrenTokens)
        yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    private class JPropertKeyedCollection : KeyedCollection<string, JToken>
    {
      public JPropertKeyedCollection(IEqualityComparer<string> comparer)
        : base(comparer)
      {
      }

      protected override string GetKeyForItem(JToken item) => ((JProperty) item).Name;

      protected override void InsertItem(int index, JToken item)
      {
        if (this.Dictionary == null)
        {
          base.InsertItem(index, item);
        }
        else
        {
          this.Dictionary[this.GetKeyForItem(item)] = item;
          this.Items.Insert(index, item);
        }
      }

      public new IDictionary<string, JToken> Dictionary => base.Dictionary;
    }
  }
}
