// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonArrayContract
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonArrayContract : JsonContract
  {
    private readonly bool _isCollectionItemTypeNullableType;
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private MethodCall<object, object> _genericWrapperCreator;

    internal Type CollectionItemType { get; private set; }

    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType))
        this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
      else if (underlyingType.IsGenericType && (object) underlyingType.GetGenericTypeDefinition() == (object) typeof (IEnumerable<>))
      {
        this._genericCollectionDefinitionType = typeof (IEnumerable<>);
        this.CollectionItemType = underlyingType.GetGenericArguments()[0];
      }
      else
        this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.UnderlyingType);
      if ((object) this.CollectionItemType != null)
        this._isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(this.CollectionItemType);
      if (!this.IsTypeGenericCollectionInterface(this.UnderlyingType))
        return;
      this.CreatedType = ReflectionUtils.MakeGenericType(typeof (List<>), this.CollectionItemType);
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if (list is IList && ((object) this.CollectionItemType == null || !this._isCollectionItemTypeNullableType) || this.UnderlyingType.IsArray)
        return (IWrappedCollection) new CollectionWrapper<object>((IList) list);
      if ((object) this._genericCollectionDefinitionType != null)
      {
        this.EnsureGenericWrapperCreator();
        return (IWrappedCollection) this._genericWrapperCreator((object) null, new object[1]
        {
          list
        });
      }
      IList list1 = (IList) ((IEnumerable) list).Cast<object>().ToList<object>();
      if ((object) this.CollectionItemType != null)
      {
        Array instance = Array.CreateInstance(this.CollectionItemType, list1.Count);
        for (int index = 0; index < list1.Count; ++index)
          instance.SetValue(list1[index], index);
        list1 = (IList) instance;
      }
      return (IWrappedCollection) new CollectionWrapper<object>(list1);
    }

    private void EnsureGenericWrapperCreator()
    {
      if ((object) this._genericWrapperType != null)
        return;
      this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof (CollectionWrapper<>), this.CollectionItemType);
      Type type;
      if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof (List<>)) || (object) this._genericCollectionDefinitionType.GetGenericTypeDefinition() == (object) typeof (IEnumerable<>))
        type = ReflectionUtils.MakeGenericType(typeof (ICollection<>), this.CollectionItemType);
      else
        type = this._genericCollectionDefinitionType;
      this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
      {
        type
      }));
    }

    private bool IsTypeGenericCollectionInterface(Type type)
    {
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      return (object) genericTypeDefinition == (object) typeof (IList<>) || (object) genericTypeDefinition == (object) typeof (ICollection<>) || (object) genericTypeDefinition == (object) typeof (IEnumerable<>);
    }
  }
}
