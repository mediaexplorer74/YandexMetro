// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultReferenceResolver
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
  internal class DefaultReferenceResolver : IReferenceResolver
  {
    private int _referenceCount;

    private BidirectionalDictionary<string, object> GetMappings(object context)
    {
      JsonSerializerInternalBase serializerInternalBase;
      switch (context)
      {
        case JsonSerializerInternalBase _:
          serializerInternalBase = (JsonSerializerInternalBase) context;
          break;
        case JsonSerializerProxy _:
          serializerInternalBase = ((JsonSerializerProxy) context).GetInternalSerializer();
          break;
        default:
          throw new Exception("The DefaultReferenceResolver can only be used internally.");
      }
      return serializerInternalBase.DefaultReferenceMappings;
    }

    public object ResolveReference(object context, string reference)
    {
      object second;
      this.GetMappings(context).TryGetByFirst(reference, out second);
      return second;
    }

    public string GetReference(object context, object value)
    {
      BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
      string first;
      if (!mappings.TryGetBySecond(value, out first))
      {
        ++this._referenceCount;
        first = this._referenceCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        mappings.Add(first, value);
      }
      return first;
    }

    public void AddReference(object context, string reference, object value) => this.GetMappings(context).Add(reference, value);

    public bool IsReferenced(object context, object value) => this.GetMappings(context).TryGetBySecond(value, out string _);
  }
}
