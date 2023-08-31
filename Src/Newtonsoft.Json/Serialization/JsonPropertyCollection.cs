// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPropertyCollection
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
  {
    private readonly Type _type;

    public JsonPropertyCollection(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      this._type = type;
    }

    protected override string GetKeyForItem(JsonProperty item) => item.PropertyName;

    public void AddProperty(JsonProperty property)
    {
      if (this.Contains(property.PropertyName))
      {
        if (property.Ignored)
          return;
        JsonProperty jsonProperty = this[property.PropertyName];
        if (!jsonProperty.Ignored)
          throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) this._type));
        this.Remove(jsonProperty);
      }
      this.Add(property);
    }

    public JsonProperty GetClosestMatchProperty(string propertyName) => this.GetProperty(propertyName, StringComparison.Ordinal) ?? this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);

    public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
    {
      foreach (JsonProperty property in (Collection<JsonProperty>) this)
      {
        if (string.Equals(propertyName, property.PropertyName, comparisonType))
          return property;
      }
      return (JsonProperty) null;
    }
  }
}
