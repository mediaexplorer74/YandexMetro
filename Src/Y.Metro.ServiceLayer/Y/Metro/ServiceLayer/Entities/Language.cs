// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.Entities.Language
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Y.Metro.ServiceLayer.Entities
{
  public class Language
  {
    public static readonly List<Language> Languages = new List<Language>()
    {
      new Language()
      {
        Name = "English",
        NativeName = "English",
        Code = "en",
        UseCode = "en-US"
      },
      new Language()
      {
        Name = "Russian",
        NativeName = "русский",
        Code = "ru",
        UseCode = "ru-RU"
      },
      new Language()
      {
        Name = "Turkish",
        NativeName = "Türkçe",
        Code = "tr",
        UseCode = "tr-TR"
      },
      new Language()
      {
        Name = "Ukrainian",
        NativeName = "україньска",
        Code = "uk",
        UseCode = "uk-UA"
      }
    };

    public string Name { get; set; }

    public string NativeName { get; set; }

    public string Code { get; set; }

    public string UseCode { get; set; }

    public bool IsDefault { get; set; }

    public Language()
    {
    }

    public Language(string name, string code, bool isDefault = false)
    {
      this.Name = name;
      this.NativeName = name;
      this.Code = code;
      this.IsDefault = isDefault;
      try
      {
        this.NativeName = new CultureInfo(code).NativeName;
      }
      catch (ArgumentException ex)
      {
      }
    }

    public override string ToString() => string.Format("{0} ({1})", (object) this.Name, (object) this.NativeName);

    public override bool Equals(object obj) => obj != null && obj is Language && ((Language) obj).Code.Equals(this.Code);

    public override int GetHashCode() => this.Code == null ? base.GetHashCode() : this.Code.GetHashCode();
  }
}
