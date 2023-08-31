// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    public Type ConverterType => this._converterType;

    public JsonConverterAttribute(Type converterType) => this._converterType = (object) converterType != null ? converterType : throw new ArgumentNullException(nameof (converterType));

    internal static JsonConverter CreateJsonConverterInstance(Type converterType)
    {
      try
      {
        return (JsonConverter) Activator.CreateInstance(converterType);
      }
      catch (Exception ex)
      {
        throw new Exception("Error creating {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) converterType), ex);
      }
    }
  }
}
