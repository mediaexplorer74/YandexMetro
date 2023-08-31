// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonSerializer
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
  public class JsonSerializer
  {
    private TypeNameHandling _typeNameHandling;
    private FormatterAssemblyStyle _typeNameAssemblyFormat;
    private PreserveReferencesHandling _preserveReferencesHandling;
    private ReferenceLoopHandling _referenceLoopHandling;
    private MissingMemberHandling _missingMemberHandling;
    private ObjectCreationHandling _objectCreationHandling;
    private NullValueHandling _nullValueHandling;
    private DefaultValueHandling _defaultValueHandling;
    private ConstructorHandling _constructorHandling;
    private JsonConverterCollection _converters;
    private IContractResolver _contractResolver;
    private IReferenceResolver _referenceResolver;
    private SerializationBinder _binder;
    private StreamingContext _context;

    public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

    public virtual IReferenceResolver ReferenceResolver
    {
      get
      {
        if (this._referenceResolver == null)
          this._referenceResolver = (IReferenceResolver) new DefaultReferenceResolver();
        return this._referenceResolver;
      }
      set => this._referenceResolver = value != null ? value : throw new ArgumentNullException(nameof (value), "Reference resolver cannot be null.");
    }

    public virtual SerializationBinder Binder
    {
      get => this._binder;
      set => this._binder = value != null ? value : throw new ArgumentNullException(nameof (value), "Serialization binder cannot be null.");
    }

    public virtual TypeNameHandling TypeNameHandling
    {
      get => this._typeNameHandling;
      set => this._typeNameHandling = value >= TypeNameHandling.None && value <= TypeNameHandling.Auto ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get => this._typeNameAssemblyFormat;
      set => this._typeNameAssemblyFormat = value >= FormatterAssemblyStyle.Simple && value <= FormatterAssemblyStyle.Full ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual PreserveReferencesHandling PreserveReferencesHandling
    {
      get => this._preserveReferencesHandling;
      set => this._preserveReferencesHandling = value >= PreserveReferencesHandling.None && value <= PreserveReferencesHandling.All ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._referenceLoopHandling;
      set => this._referenceLoopHandling = value >= ReferenceLoopHandling.Error && value <= ReferenceLoopHandling.Serialize ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual MissingMemberHandling MissingMemberHandling
    {
      get => this._missingMemberHandling;
      set => this._missingMemberHandling = value >= MissingMemberHandling.Ignore && value <= MissingMemberHandling.Error ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual NullValueHandling NullValueHandling
    {
      get => this._nullValueHandling;
      set => this._nullValueHandling = value >= NullValueHandling.Include && value <= NullValueHandling.Ignore ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual DefaultValueHandling DefaultValueHandling
    {
      get => this._defaultValueHandling;
      set => this._defaultValueHandling = value >= DefaultValueHandling.Include && value <= DefaultValueHandling.IgnoreAndPopulate ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual ObjectCreationHandling ObjectCreationHandling
    {
      get => this._objectCreationHandling;
      set => this._objectCreationHandling = value >= ObjectCreationHandling.Auto && value <= ObjectCreationHandling.Replace ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual ConstructorHandling ConstructorHandling
    {
      get => this._constructorHandling;
      set => this._constructorHandling = value >= ConstructorHandling.Default && value <= ConstructorHandling.AllowNonPublicDefaultConstructor ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    public virtual JsonConverterCollection Converters
    {
      get
      {
        if (this._converters == null)
          this._converters = new JsonConverterCollection();
        return this._converters;
      }
    }

    public virtual IContractResolver ContractResolver
    {
      get
      {
        if (this._contractResolver == null)
          this._contractResolver = DefaultContractResolver.Instance;
        return this._contractResolver;
      }
      set => this._contractResolver = value;
    }

    public virtual StreamingContext Context
    {
      get => this._context;
      set => this._context = value;
    }

    public JsonSerializer()
    {
      this._referenceLoopHandling = ReferenceLoopHandling.Error;
      this._missingMemberHandling = MissingMemberHandling.Ignore;
      this._nullValueHandling = NullValueHandling.Include;
      this._defaultValueHandling = DefaultValueHandling.Include;
      this._objectCreationHandling = ObjectCreationHandling.Auto;
      this._preserveReferencesHandling = PreserveReferencesHandling.None;
      this._constructorHandling = ConstructorHandling.Default;
      this._typeNameHandling = TypeNameHandling.None;
      this._context = JsonSerializerSettings.DefaultContext;
      this._binder = (SerializationBinder) DefaultSerializationBinder.Instance;
    }

    public static JsonSerializer Create(JsonSerializerSettings settings)
    {
      JsonSerializer jsonSerializer = new JsonSerializer();
      if (settings != null)
      {
        if (!CollectionUtils.IsNullOrEmpty<JsonConverter>((ICollection<JsonConverter>) settings.Converters))
          jsonSerializer.Converters.AddRange<JsonConverter>((IEnumerable<JsonConverter>) settings.Converters);
        jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
        jsonSerializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
        jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
        jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
        jsonSerializer.MissingMemberHandling = settings.MissingMemberHandling;
        jsonSerializer.ObjectCreationHandling = settings.ObjectCreationHandling;
        jsonSerializer.NullValueHandling = settings.NullValueHandling;
        jsonSerializer.DefaultValueHandling = settings.DefaultValueHandling;
        jsonSerializer.ConstructorHandling = settings.ConstructorHandling;
        jsonSerializer.Context = settings.Context;
        if (settings.Error != null)
          jsonSerializer.Error += settings.Error;
        if (settings.ContractResolver != null)
          jsonSerializer.ContractResolver = settings.ContractResolver;
        if (settings.ReferenceResolver != null)
          jsonSerializer.ReferenceResolver = settings.ReferenceResolver;
        if (settings.Binder != null)
          jsonSerializer.Binder = settings.Binder;
      }
      return jsonSerializer;
    }

    public void Populate(TextReader reader, object target) => this.Populate((JsonReader) new JsonTextReader(reader), target);

    public void Populate(JsonReader reader, object target) => this.PopulateInternal(reader, target);

    internal virtual void PopulateInternal(JsonReader reader, object target)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      ValidationUtils.ArgumentNotNull(target, nameof (target));
      new JsonSerializerInternalReader(this).Populate(reader, target);
    }

    public object Deserialize(JsonReader reader) => this.Deserialize(reader, (Type) null);

    public object Deserialize(TextReader reader, Type objectType) => this.Deserialize((JsonReader) new JsonTextReader(reader), objectType);

    public T Deserialize<T>(JsonReader reader) => (T) this.Deserialize(reader, typeof (T));

    public object Deserialize(JsonReader reader, Type objectType) => this.DeserializeInternal(reader, objectType);

    internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      return new JsonSerializerInternalReader(this).Deserialize(reader, objectType);
    }

    public void Serialize(TextWriter textWriter, object value) => this.Serialize((JsonWriter) new JsonTextWriter(textWriter), value);

    public void Serialize(JsonWriter jsonWriter, object value) => this.SerializeInternal(jsonWriter, value);

    internal virtual void SerializeInternal(JsonWriter jsonWriter, object value)
    {
      ValidationUtils.ArgumentNotNull((object) jsonWriter, nameof (jsonWriter));
      new JsonSerializerInternalWriter(this).Serialize(jsonWriter, value);
    }

    internal JsonConverter GetMatchingConverter(Type type) => JsonSerializer.GetMatchingConverter((IList<JsonConverter>) this._converters, type);

    internal static JsonConverter GetMatchingConverter(
      IList<JsonConverter> converters,
      Type objectType)
    {
      ValidationUtils.ArgumentNotNull((object) objectType, nameof (objectType));
      if (converters != null)
      {
        for (int index = 0; index < converters.Count; ++index)
        {
          JsonConverter converter = converters[index];
          if (converter.CanConvert(objectType))
            return converter;
        }
      }
      return (JsonConverter) null;
    }

    internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
      EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
      if (error == null)
        return;
      error((object) this, e);
    }
  }
}
