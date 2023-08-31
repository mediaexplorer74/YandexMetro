// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.Extensions
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
  public static class Extensions
  {
    public static bool IsValid(this JToken source, JsonSchema schema)
    {
      bool valid = true;
      source.Validate(schema, (ValidationEventHandler) ((sender, args) => valid = false));
      return valid;
    }

    public static void Validate(this JToken source, JsonSchema schema) => source.Validate(schema, (ValidationEventHandler) null);

    public static void Validate(
      this JToken source,
      JsonSchema schema,
      ValidationEventHandler validationEventHandler)
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      ValidationUtils.ArgumentNotNull((object) schema, nameof (schema));
      using (JsonValidatingReader validatingReader = new JsonValidatingReader(source.CreateReader()))
      {
        validatingReader.Schema = schema;
        if (validationEventHandler != null)
          validatingReader.ValidationEventHandler += validationEventHandler;
        do
          ;
        while (validatingReader.Read());
      }
    }
  }
}
