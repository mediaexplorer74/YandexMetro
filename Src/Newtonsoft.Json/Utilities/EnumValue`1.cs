// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumValue`1
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal class EnumValue<T> where T : struct
  {
    private string _name;
    private T _value;

    public string Name => this._name;

    public T Value => this._value;

    public EnumValue(string name, T value)
    {
      this._name = name;
      this._value = value;
    }
  }
}
