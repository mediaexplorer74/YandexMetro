﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.AutoCompleteSearch
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;

namespace Microsoft.Phone.Controls
{
  internal static class AutoCompleteSearch
  {
    public static AutoCompleteFilterPredicate<string> GetFilter(AutoCompleteFilterMode FilterMode)
    {
      switch (FilterMode)
      {
        case AutoCompleteFilterMode.StartsWith:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWith);
        case AutoCompleteFilterMode.StartsWithCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithCaseSensitive);
        case AutoCompleteFilterMode.StartsWithOrdinal:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithOrdinal);
        case AutoCompleteFilterMode.StartsWithOrdinalCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.StartsWithOrdinalCaseSensitive);
        case AutoCompleteFilterMode.Contains:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.Contains);
        case AutoCompleteFilterMode.ContainsCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsCaseSensitive);
        case AutoCompleteFilterMode.ContainsOrdinal:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsOrdinal);
        case AutoCompleteFilterMode.ContainsOrdinalCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.ContainsOrdinalCaseSensitive);
        case AutoCompleteFilterMode.Equals:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.Equals);
        case AutoCompleteFilterMode.EqualsCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsCaseSensitive);
        case AutoCompleteFilterMode.EqualsOrdinal:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsOrdinal);
        case AutoCompleteFilterMode.EqualsOrdinalCaseSensitive:
          return new AutoCompleteFilterPredicate<string>(AutoCompleteSearch.EqualsOrdinalCaseSensitive);
        default:
          return (AutoCompleteFilterPredicate<string>) null;
      }
    }

    public static bool StartsWith(string text, string value) => value.StartsWith(text, StringComparison.CurrentCultureIgnoreCase);

    public static bool StartsWithCaseSensitive(string text, string value) => value.StartsWith(text, StringComparison.CurrentCulture);

    public static bool StartsWithOrdinal(string text, string value) => value.StartsWith(text, StringComparison.OrdinalIgnoreCase);

    public static bool StartsWithOrdinalCaseSensitive(string text, string value) => value.StartsWith(text, StringComparison.Ordinal);

    public static bool Contains(string text, string value) => value.Contains(text, StringComparison.CurrentCultureIgnoreCase);

    public static bool ContainsCaseSensitive(string text, string value) => value.Contains(text, StringComparison.CurrentCulture);

    public static bool ContainsOrdinal(string text, string value) => value.Contains(text, StringComparison.OrdinalIgnoreCase);

    public static bool ContainsOrdinalCaseSensitive(string text, string value) => value.Contains(text, StringComparison.Ordinal);

    public static bool Equals(string text, string value) => value.Equals(text, StringComparison.CurrentCultureIgnoreCase);

    public static bool EqualsCaseSensitive(string text, string value) => value.Equals(text, StringComparison.CurrentCulture);

    public static bool EqualsOrdinal(string text, string value) => value.Equals(text, StringComparison.OrdinalIgnoreCase);

    public static bool EqualsOrdinalCaseSensitive(string text, string value) => value.Equals(text, StringComparison.Ordinal);
  }
}
