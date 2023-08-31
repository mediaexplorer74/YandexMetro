// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.CollectionUtils
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class CollectionUtils
  {
    public static IEnumerable<T> CastValid<T>(this IEnumerable enumerable)
    {
      ValidationUtils.ArgumentNotNull((object) enumerable, nameof (enumerable));
      return enumerable.Cast<object>().Where<object>((Func<object, bool>) (o => o is T)).Cast<T>();
    }

    public static List<T> CreateList<T>(params T[] values) => new List<T>((IEnumerable<T>) values);

    public static bool IsNullOrEmpty(ICollection collection) => collection == null || collection.Count == 0;

    public static bool IsNullOrEmpty<T>(ICollection<T> collection) => collection == null || collection.Count == 0;

    public static bool IsNullOrEmptyOrDefault<T>(IList<T> list) => CollectionUtils.IsNullOrEmpty<T>((ICollection<T>) list) || ReflectionUtils.ItemsUnitializedValue<T>(list);

    public static IList<T> Slice<T>(IList<T> list, int? start, int? end) => CollectionUtils.Slice<T>(list, start, end, new int?());

    public static IList<T> Slice<T>(IList<T> list, int? start, int? end, int? step)
    {
      if (list == null)
        throw new ArgumentNullException(nameof (list));
      int? nullable = step;
      if ((nullable.GetValueOrDefault() != 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        throw new ArgumentException("Step cannot be zero.", nameof (step));
      List<T> objList = new List<T>();
      if (list.Count == 0)
        return (IList<T>) objList;
      int num1 = step ?? 1;
      int num2 = start ?? 0;
      int num3 = end ?? list.Count;
      int val1_1 = num2 < 0 ? list.Count + num2 : num2;
      int val1_2 = num3 < 0 ? list.Count + num3 : num3;
      int num4 = Math.Max(val1_1, 0);
      int num5 = Math.Min(val1_2, list.Count - 1);
      for (int index = num4; index < num5; index += num1)
        objList.Add(list[index]);
      return (IList<T>) objList;
    }

    public static Dictionary<K, List<V>> GroupBy<K, V>(
      ICollection<V> source,
      Func<V, K> keySelector)
    {
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      Dictionary<K, List<V>> dictionary = new Dictionary<K, List<V>>();
      foreach (V v in (IEnumerable<V>) source)
      {
        K key = keySelector(v);
        List<V> vList;
        if (!dictionary.TryGetValue(key, out vList))
        {
          vList = new List<V>();
          dictionary.Add(key, vList);
        }
        vList.Add(v);
      }
      return dictionary;
    }

    public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
    {
      if (initial == null)
        throw new ArgumentNullException(nameof (initial));
      if (collection == null)
        return;
      foreach (T obj in collection)
        initial.Add(obj);
    }

    public static void AddRange(this IList initial, IEnumerable collection)
    {
      ValidationUtils.ArgumentNotNull((object) initial, nameof (initial));
      new ListWrapper<object>(initial).AddRange<object>(collection.Cast<object>());
    }

    public static List<T> Distinct<T>(List<T> collection)
    {
      List<T> objList = new List<T>();
      foreach (T obj in collection)
      {
        if (!objList.Contains(obj))
          objList.Add(obj);
      }
      return objList;
    }

    public static List<List<T>> Flatten<T>(params IList<T>[] lists)
    {
      List<List<T>> flattenedResult = new List<List<T>>();
      Dictionary<int, T> currentSet = new Dictionary<int, T>();
      CollectionUtils.Recurse<T>((IList<IList<T>>) new List<IList<T>>((IEnumerable<IList<T>>) lists), 0, currentSet, flattenedResult);
      return flattenedResult;
    }

    private static void Recurse<T>(
      IList<IList<T>> global,
      int current,
      Dictionary<int, T> currentSet,
      List<List<T>> flattenedResult)
    {
      IList<T> objList1 = global[current];
      for (int index = 0; index < objList1.Count; ++index)
      {
        currentSet[current] = objList1[index];
        if (current == global.Count - 1)
        {
          List<T> objList2 = new List<T>();
          for (int key = 0; key < currentSet.Count; ++key)
            objList2.Add(currentSet[key]);
          flattenedResult.Add(objList2);
        }
        else
          CollectionUtils.Recurse<T>(global, current + 1, currentSet, flattenedResult);
      }
    }

    public static List<T> CreateList<T>(ICollection collection)
    {
      T[] collection1 = collection != null ? new T[collection.Count] : throw new ArgumentNullException(nameof (collection));
      collection.CopyTo((Array) collection1, 0);
      return new List<T>((IEnumerable<T>) collection1);
    }

    public static bool ListEquals<T>(IList<T> a, IList<T> b)
    {
      if (a == null || b == null)
        return a == null && b == null;
      if (a.Count != b.Count)
        return false;
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < a.Count; ++index)
      {
        if (!equalityComparer.Equals(a[index], b[index]))
          return false;
      }
      return true;
    }

    public static bool TryGetSingleItem<T>(IList<T> list, out T value) => CollectionUtils.TryGetSingleItem<T>(list, false, out value);

    public static bool TryGetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty, out T value) => MiscellaneousUtils.TryAction<T>((Creator<T>) (() => CollectionUtils.GetSingleItem<T>(list, returnDefaultIfEmpty)), out value);

    public static T GetSingleItem<T>(IList<T> list) => CollectionUtils.GetSingleItem<T>(list, false);

    public static T GetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty)
    {
      if (list.Count == 1)
        return list[0];
      if (returnDefaultIfEmpty && list.Count == 0)
        return default (T);
      throw new Exception("Expected single {0} in list but got {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T), (object) list.Count));
    }

    public static IList<T> Minus<T>(IList<T> list, IList<T> minus)
    {
      ValidationUtils.ArgumentNotNull((object) list, nameof (list));
      List<T> objList = new List<T>(list.Count);
      foreach (T obj in (IEnumerable<T>) list)
      {
        if (minus == null || !minus.Contains(obj))
          objList.Add(obj);
      }
      return (IList<T>) objList;
    }

    public static IList CreateGenericList(Type listType)
    {
      ValidationUtils.ArgumentNotNull((object) listType, nameof (listType));
      return (IList) ReflectionUtils.CreateGeneric(typeof (List<>), listType);
    }

    public static IDictionary CreateGenericDictionary(Type keyType, Type valueType)
    {
      ValidationUtils.ArgumentNotNull((object) keyType, nameof (keyType));
      ValidationUtils.ArgumentNotNull((object) valueType, nameof (valueType));
      return (IDictionary) ReflectionUtils.CreateGeneric(typeof (Dictionary<,>), keyType, (object) valueType);
    }

    public static bool IsListType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return type.IsArray || typeof (IList).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (IList<>));
    }

    public static bool IsCollectionType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return type.IsArray || typeof (ICollection).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (ICollection<>));
    }

    public static bool IsDictionaryType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return typeof (IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (IDictionary<,>));
    }

    public static IWrappedCollection CreateCollectionWrapper(object list)
    {
      ValidationUtils.ArgumentNotNull(list, nameof (list));
      Type collectionDefinition;
      if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof (ICollection<>), out collectionDefinition))
        return (IWrappedCollection) ReflectionUtils.CreateGeneric(typeof (CollectionWrapper<>), (IList<Type>) new Type[1]
        {
          ReflectionUtils.GetCollectionItemType(collectionDefinition)
        }, (Func<Type, IList<object>, object>) ((t, a) => t.GetConstructor(new Type[1]
        {
          collectionDefinition
        }).Invoke(new object[1]{ list })), list);
      return list is IList ? (IWrappedCollection) new CollectionWrapper<object>((IList) list) : throw new Exception("Can not create ListWrapper for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) list.GetType()));
    }

    public static IWrappedList CreateListWrapper(object list)
    {
      ValidationUtils.ArgumentNotNull(list, nameof (list));
      Type listDefinition;
      if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof (IList<>), out listDefinition))
        return (IWrappedList) ReflectionUtils.CreateGeneric(typeof (ListWrapper<>), (IList<Type>) new Type[1]
        {
          ReflectionUtils.GetCollectionItemType(listDefinition)
        }, (Func<Type, IList<object>, object>) ((t, a) => t.GetConstructor(new Type[1]
        {
          listDefinition
        }).Invoke(new object[1]{ list })), list);
      return list is IList ? (IWrappedList) new ListWrapper<object>((IList) list) : throw new Exception("Can not create ListWrapper for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) list.GetType()));
    }

    public static IWrappedDictionary CreateDictionaryWrapper(object dictionary)
    {
      ValidationUtils.ArgumentNotNull(dictionary, nameof (dictionary));
      Type dictionaryDefinition;
      if (ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof (IDictionary<,>), out dictionaryDefinition))
        return (IWrappedDictionary) ReflectionUtils.CreateGeneric(typeof (DictionaryWrapper<,>), (IList<Type>) new Type[2]
        {
          ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition),
          ReflectionUtils.GetDictionaryValueType(dictionaryDefinition)
        }, (Func<Type, IList<object>, object>) ((t, a) => t.GetConstructor(new Type[1]
        {
          dictionaryDefinition
        }).Invoke(new object[1]{ dictionary })), dictionary);
      return dictionary is IDictionary ? (IWrappedDictionary) new DictionaryWrapper<object, object>((IDictionary) dictionary) : throw new Exception("Can not create DictionaryWrapper for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) dictionary.GetType()));
    }

    public static object CreateAndPopulateList(Type listType, Action<IList, bool> populateList)
    {
      ValidationUtils.ArgumentNotNull((object) listType, nameof (listType));
      ValidationUtils.ArgumentNotNull((object) populateList, nameof (populateList));
      bool flag1 = false;
      IList andPopulateList;
      if (listType.IsArray)
      {
        andPopulateList = (IList) new List<object>();
        flag1 = true;
      }
      else
      {
        Type implementingType;
        if (ReflectionUtils.InheritsGenericDefinition(listType, typeof (ReadOnlyCollection<>), out implementingType))
        {
          Type genericArgument = implementingType.GetGenericArguments()[0];
          Type type = ReflectionUtils.MakeGenericType(typeof (IEnumerable<>), genericArgument);
          bool flag2 = false;
          foreach (MethodBase constructor in listType.GetConstructors())
          {
            IList<ParameterInfo> parameters = (IList<ParameterInfo>) constructor.GetParameters();
            if (parameters.Count == 1 && type.IsAssignableFrom(parameters[0].ParameterType))
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            throw new Exception("Read-only type {0} does not have a public constructor that takes a type that implements {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) listType, (object) type));
          andPopulateList = CollectionUtils.CreateGenericList(genericArgument);
          flag1 = true;
        }
        else
          andPopulateList = !typeof (IList).IsAssignableFrom(listType) ? (!ReflectionUtils.ImplementsGenericDefinition(listType, typeof (ICollection<>)) ? (IList) null : (!ReflectionUtils.IsInstantiatableType(listType) ? (IList) null : (IList) CollectionUtils.CreateCollectionWrapper(Activator.CreateInstance(listType)))) : (!ReflectionUtils.IsInstantiatableType(listType) ? ((object) listType != (object) typeof (IList) ? (IList) null : (IList) new List<object>()) : (IList) Activator.CreateInstance(listType));
      }
      if (andPopulateList == null)
        throw new Exception("Cannot create and populate list type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) listType));
      populateList(andPopulateList, flag1);
      if (flag1)
      {
        if (listType.IsArray)
          andPopulateList = (IList) CollectionUtils.ToArray((Array) ((List<object>) andPopulateList).ToArray(), ReflectionUtils.GetCollectionItemType(listType));
        else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof (ReadOnlyCollection<>)))
          andPopulateList = (IList) ReflectionUtils.CreateInstance(listType, (object) andPopulateList);
      }
      else if (andPopulateList is IWrappedCollection)
        return ((IWrappedCollection) andPopulateList).UnderlyingCollection;
      return (object) andPopulateList;
    }

    public static Array ToArray(Array initial, Type type)
    {
      if ((object) type == null)
        throw new ArgumentNullException(nameof (type));
      Array instance = Array.CreateInstance(type, initial.Length);
      Array.Copy(initial, 0, instance, 0, initial.Length);
      return instance;
    }

    public static bool AddDistinct<T>(this IList<T> list, T value) => list.AddDistinct<T>(value, (IEqualityComparer<T>) EqualityComparer<T>.Default);

    public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
    {
      if (list.ContainsValue<T>(value, comparer))
        return false;
      list.Add(value);
      return true;
    }

    public static bool ContainsValue<TSource>(
      this IEnumerable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      if (comparer == null)
        comparer = (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      foreach (TSource x in source)
      {
        if (comparer.Equals(x, value))
          return true;
      }
      return false;
    }

    public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values) => list.AddRangeDistinct<T>(values, (IEqualityComparer<T>) EqualityComparer<T>.Default);

    public static bool AddRangeDistinct<T>(
      this IList<T> list,
      IEnumerable<T> values,
      IEqualityComparer<T> comparer)
    {
      bool flag = true;
      foreach (T obj in values)
      {
        if (!list.AddDistinct<T>(obj, comparer))
          flag = false;
      }
      return flag;
    }

    public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
      int num = 0;
      foreach (T obj in collection)
      {
        if (predicate(obj))
          return num;
        ++num;
      }
      return -1;
    }

    public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value) where TSource : IEquatable<TSource> => list.IndexOf<TSource>(value, (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default);

    public static int IndexOf<TSource>(
      this IEnumerable<TSource> list,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      int num = 0;
      foreach (TSource x in list)
      {
        if (comparer.Equals(x, value))
          return num;
        ++num;
      }
      return -1;
    }
  }
}
