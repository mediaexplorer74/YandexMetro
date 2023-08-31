// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerProxy
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerProxy : JsonSerializer
  {
    private readonly JsonSerializerInternalReader _serializerReader;
    private readonly JsonSerializerInternalWriter _serializerWriter;
    private readonly JsonSerializer _serializer;

    public override event EventHandler<ErrorEventArgs> Error
    {
      add => this._serializer.Error += value;
      remove => this._serializer.Error -= value;
    }

    public override IReferenceResolver ReferenceResolver
    {
      get => this._serializer.ReferenceResolver;
      set => this._serializer.ReferenceResolver = value;
    }

    public override JsonConverterCollection Converters => this._serializer.Converters;

    public override DefaultValueHandling DefaultValueHandling
    {
      get => this._serializer.DefaultValueHandling;
      set => this._serializer.DefaultValueHandling = value;
    }

    public override IContractResolver ContractResolver
    {
      get => this._serializer.ContractResolver;
      set => this._serializer.ContractResolver = value;
    }

    public override MissingMemberHandling MissingMemberHandling
    {
      get => this._serializer.MissingMemberHandling;
      set => this._serializer.MissingMemberHandling = value;
    }

    public override NullValueHandling NullValueHandling
    {
      get => this._serializer.NullValueHandling;
      set => this._serializer.NullValueHandling = value;
    }

    public override ObjectCreationHandling ObjectCreationHandling
    {
      get => this._serializer.ObjectCreationHandling;
      set => this._serializer.ObjectCreationHandling = value;
    }

    public override ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._serializer.ReferenceLoopHandling;
      set => this._serializer.ReferenceLoopHandling = value;
    }

    public override PreserveReferencesHandling PreserveReferencesHandling
    {
      get => this._serializer.PreserveReferencesHandling;
      set => this._serializer.PreserveReferencesHandling = value;
    }

    public override TypeNameHandling TypeNameHandling
    {
      get => this._serializer.TypeNameHandling;
      set => this._serializer.TypeNameHandling = value;
    }

    public override FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get => this._serializer.TypeNameAssemblyFormat;
      set => this._serializer.TypeNameAssemblyFormat = value;
    }

    public override ConstructorHandling ConstructorHandling
    {
      get => this._serializer.ConstructorHandling;
      set => this._serializer.ConstructorHandling = value;
    }

    public override Newtonsoft.Json.SerializationBinder Binder
    {
      get => this._serializer.Binder;
      set => this._serializer.Binder = value;
    }

    public override StreamingContext Context
    {
      get => this._serializer.Context;
      set => this._serializer.Context = value;
    }

    internal JsonSerializerInternalBase GetInternalSerializer() => this._serializerReader != null ? (JsonSerializerInternalBase) this._serializerReader : (JsonSerializerInternalBase) this._serializerWriter;

    public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
    {
      ValidationUtils.ArgumentNotNull((object) serializerReader, nameof (serializerReader));
      this._serializerReader = serializerReader;
      this._serializer = serializerReader.Serializer;
    }

    public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
    {
      ValidationUtils.ArgumentNotNull((object) serializerWriter, nameof (serializerWriter));
      this._serializerWriter = serializerWriter;
      this._serializer = serializerWriter.Serializer;
    }

    internal override object DeserializeInternal(JsonReader reader, Type objectType) => this._serializerReader != null ? this._serializerReader.Deserialize(reader, objectType) : this._serializer.Deserialize(reader, objectType);

    internal override void PopulateInternal(JsonReader reader, object target)
    {
      if (this._serializerReader != null)
        this._serializerReader.Populate(reader, target);
      else
        this._serializer.Populate(reader, target);
    }

    internal override void SerializeInternal(JsonWriter jsonWriter, object value)
    {
      if (this._serializerWriter != null)
        this._serializerWriter.Serialize(jsonWriter, value);
      else
        this._serializer.Serialize(jsonWriter, value);
    }
  }
}
