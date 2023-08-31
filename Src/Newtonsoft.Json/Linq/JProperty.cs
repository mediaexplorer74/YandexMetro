// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JProperty
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public class JProperty : JContainer
  {
    private readonly List<JToken> _content = new List<JToken>();
    private readonly string _name;

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._content;

    public string Name
    {
      [DebuggerStepThrough] get => this._name;
    }

    public JToken Value
    {
      [DebuggerStepThrough] get => this.ChildrenTokens.Count <= 0 ? (JToken) null : this.ChildrenTokens[0];
      set
      {
        this.CheckReentrancy();
        JToken jtoken = value ?? (JToken) new JValue((object) null);
        if (this.ChildrenTokens.Count == 0)
          this.InsertItem(0, jtoken);
        else
          this.SetItem(0, jtoken);
      }
    }

    public JProperty(JProperty other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    internal override JToken GetItem(int index)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      return this.Value;
    }

    internal override void SetItem(int index, JToken item)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      if (JContainer.IsTokenUnchanged(this.Value, item))
        return;
      if (this.Parent != null)
        ((JObject) this.Parent).InternalPropertyChanging(this);
      base.SetItem(0, item);
      if (this.Parent == null)
        return;
      ((JObject) this.Parent).InternalPropertyChanged(this);
    }

    internal override bool RemoveItem(JToken item) => throw new Exception("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));

    internal override void RemoveItemAt(int index) => throw new Exception("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));

    internal override void InsertItem(int index, JToken item)
    {
      if (this.Value != null)
        throw new Exception("{0} cannot have multiple values.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
      base.InsertItem(0, item);
    }

    internal override bool ContainsItem(JToken item) => this.Value == item;

    internal override void ClearItems() => throw new Exception("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));

    internal override bool DeepEquals(JToken node) => node is JProperty container && this._name == container.Name && this.ContentsEqual((JContainer) container);

    internal override JToken CloneToken() => (JToken) new JProperty(this);

    public override JTokenType Type
    {
      [DebuggerStepThrough] get => JTokenType.Property;
    }

    internal JProperty(string name)
    {
      ValidationUtils.ArgumentNotNull((object) name, nameof (name));
      this._name = name;
    }

    public JProperty(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JProperty(string name, object content)
    {
      ValidationUtils.ArgumentNotNull((object) name, nameof (name));
      this._name = name;
      this.Value = this.IsMultiContent(content) ? (JToken) new JArray(content) : this.CreateFromContent(content);
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WritePropertyName(this._name);
      this.Value.WriteTo(writer, converters);
    }

    internal override int GetDeepHashCode() => this._name.GetHashCode() ^ (this.Value != null ? this.Value.GetDeepHashCode() : 0);

    public static JProperty Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw new Exception("Error reading JProperty from JsonReader.");
      JProperty jproperty = reader.TokenType == JsonToken.PropertyName ? new JProperty((string) reader.Value) : throw new Exception("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      jproperty.SetLineInfo(reader as IJsonLineInfo);
      jproperty.ReadTokenFrom(reader);
      return jproperty;
    }
  }
}
