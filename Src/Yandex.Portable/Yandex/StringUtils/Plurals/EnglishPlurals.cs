﻿// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Plurals.EnglishPlurals
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using Yandex.StringUtils.Plurals.Interfaces;

namespace Yandex.StringUtils.Plurals
{
  public class EnglishPlurals : IPlurals
  {
    public string GetPluralForm(string[] forms, long count) => forms[count == 0L ? 0 : 1];
  }
}
