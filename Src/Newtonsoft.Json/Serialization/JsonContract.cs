// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonContract
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public abstract class JsonContract
  {
    public Type UnderlyingType { get; private set; }

    public Type CreatedType { get; set; }

    public bool? IsReference { get; set; }

    public JsonConverter Converter { get; set; }

    internal JsonConverter InternalConverter { get; set; }

    public MethodInfo OnDeserialized { get; set; }

    public MethodInfo OnDeserializing { get; set; }

    public MethodInfo OnSerialized { get; set; }

    public MethodInfo OnSerializing { get; set; }

    public Func<object> DefaultCreator { get; set; }

    public bool DefaultCreatorNonPublic { get; set; }

    public MethodInfo OnError { get; set; }

    internal void InvokeOnSerializing(object o, StreamingContext context)
    {
      if ((object) this.OnSerializing == null)
        return;
      this.OnSerializing.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnSerialized(object o, StreamingContext context)
    {
      if ((object) this.OnSerialized == null)
        return;
      this.OnSerialized.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnDeserializing(object o, StreamingContext context)
    {
      if ((object) this.OnDeserializing == null)
        return;
      this.OnDeserializing.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnDeserialized(object o, StreamingContext context)
    {
      if ((object) this.OnDeserialized == null)
        return;
      this.OnDeserialized.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
    {
      if ((object) this.OnError == null)
        return;
      this.OnError.Invoke(o, new object[2]
      {
        (object) context,
        (object) errorContext
      });
    }

    internal JsonContract(Type underlyingType)
    {
      ValidationUtils.ArgumentNotNull((object) underlyingType, nameof (underlyingType));
      this.UnderlyingType = underlyingType;
      this.CreatedType = underlyingType;
    }
  }
}
