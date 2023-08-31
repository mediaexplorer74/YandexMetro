// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonDictionaryContract
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonDictionaryContract : JsonContract
  {
    private readonly bool _isDictionaryValueTypeNullableType;
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private MethodCall<object, object> _genericWrapperCreator;

    public Func<string, string> PropertyNameResolver { get; set; }

    internal Type DictionaryKeyType { get; private set; }

    internal Type DictionaryValueType { get; private set; }

    public JsonDictionaryContract(Type underlyingType)
      : base(underlyingType)
    {
      Type keyType;
      Type valueType;
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IDictionary<,>), out this._genericCollectionDefinitionType))
      {
        keyType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        valueType = this._genericCollectionDefinitionType.GetGenericArguments()[1];
      }
      else
        ReflectionUtils.GetDictionaryKeyValueTypes(this.UnderlyingType, out keyType, out valueType);
      this.DictionaryKeyType = keyType;
      this.DictionaryValueType = valueType;
      if ((object) this.DictionaryValueType != null)
        this._isDictionaryValueTypeNullableType = ReflectionUtils.IsNullableType(this.DictionaryValueType);
      if (this.IsTypeGenericDictionaryInterface(this.UnderlyingType))
      {
        this.CreatedType = ReflectionUtils.MakeGenericType(typeof (Dictionary<,>), keyType, valueType);
      }
      else
      {
        if ((object) this.UnderlyingType != (object) typeof (IDictionary))
          return;
        this.CreatedType = typeof (Dictionary<object, object>);
      }
    }

    internal IWrappedDictionary CreateWrapper(object dictionary)
    {
      if (dictionary is IDictionary && ((object) this.DictionaryValueType == null || !this._isDictionaryValueTypeNullableType))
        return (IWrappedDictionary) new DictionaryWrapper<object, object>((IDictionary) dictionary);
      if ((object) this._genericWrapperType == null)
      {
        this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof (DictionaryWrapper<,>), this.DictionaryKeyType, this.DictionaryValueType);
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          this._genericCollectionDefinitionType
        }));
      }
      return (IWrappedDictionary) this._genericWrapperCreator((object) null, new object[1]
      {
        dictionary
      });
    }

    private bool IsTypeGenericDictionaryInterface(Type type) => type.IsGenericType && (object) type.GetGenericTypeDefinition() == (object) typeof (IDictionary<,>);
  }
}
