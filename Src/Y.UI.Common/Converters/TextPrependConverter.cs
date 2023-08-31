// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Converters.TextPrependConverter
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Y.UI.Common.Converters
{
  public class TextPrependConverter : IValueConverter
  {
    public string PrependFontFamilyResourceName { get; set; }

    public string MainFontFamilyResourceName { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      List<Inline> inlineList = new List<Inline>();
      Run run1 = new Run();
      run1.Text = parameter.ToString();
      if (this.PrependFontFamilyResourceName != null)
        ((TextElement) run1).FontFamily = Application.Current.Resources[(object) this.PrependFontFamilyResourceName] as FontFamily;
      inlineList.Add((Inline) run1);
      if (value != null)
      {
        Run run2 = new Run();
        run2.Text = value.ToString();
        if (this.MainFontFamilyResourceName != null)
          ((TextElement) run2).FontFamily = Application.Current.Resources[(object) this.MainFontFamilyResourceName] as FontFamily;
        inlineList.Add((Inline) run2);
      }
      return (object) inlineList;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
