// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaModel
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
  internal class JsonSchemaModel
  {
    public bool Required { get; set; }

    public JsonSchemaType Type { get; set; }

    public int? MinimumLength { get; set; }

    public int? MaximumLength { get; set; }

    public double? DivisibleBy { get; set; }

    public double? Minimum { get; set; }

    public double? Maximum { get; set; }

    public bool ExclusiveMinimum { get; set; }

    public bool ExclusiveMaximum { get; set; }

    public int? MinimumItems { get; set; }

    public int? MaximumItems { get; set; }

    public IList<string> Patterns { get; set; }

    public IList<JsonSchemaModel> Items { get; set; }

    public IDictionary<string, JsonSchemaModel> Properties { get; set; }

    public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

    public JsonSchemaModel AdditionalProperties { get; set; }

    public bool AllowAdditionalProperties { get; set; }

    public IList<JToken> Enum { get; set; }

    public JsonSchemaType Disallow { get; set; }

    public JsonSchemaModel()
    {
      this.Type = JsonSchemaType.Any;
      this.AllowAdditionalProperties = true;
      this.Required = false;
    }

    public static JsonSchemaModel Create(IList<JsonSchema> schemata)
    {
      JsonSchemaModel model = new JsonSchemaModel();
      foreach (JsonSchema schema in (IEnumerable<JsonSchema>) schemata)
        JsonSchemaModel.Combine(model, schema);
      return model;
    }

    private static void Combine(JsonSchemaModel model, JsonSchema schema)
    {
      model.Required = model.Required || ((int) schema.Required ?? 0) != 0;
      model.Type &= (JsonSchemaType) ((int) schema.Type ?? (int) sbyte.MaxValue);
      model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
      model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
      model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
      model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
      model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
      model.ExclusiveMinimum = model.ExclusiveMinimum || ((int) schema.ExclusiveMinimum ?? 0) != 0;
      model.ExclusiveMaximum = model.ExclusiveMaximum || ((int) schema.ExclusiveMaximum ?? 0) != 0;
      model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
      model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
      model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
      if (schema.Enum != null)
      {
        if (model.Enum == null)
          model.Enum = (IList<JToken>) new List<JToken>();
        model.Enum.AddRangeDistinct<JToken>((IEnumerable<JToken>) schema.Enum, (IEqualityComparer<JToken>) new JTokenEqualityComparer());
      }
      model.Disallow |= (JsonSchemaType) ((int) schema.Disallow ?? 0);
      if (schema.Pattern == null)
        return;
      if (model.Patterns == null)
        model.Patterns = (IList<string>) new List<string>();
      model.Patterns.AddDistinct<string>(schema.Pattern);
    }
  }
}
