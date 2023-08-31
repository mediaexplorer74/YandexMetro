// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TimePicker
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Globalization;

namespace Microsoft.Phone.Controls
{
  public class TimePicker : DateTimePickerBase
  {
    private string _fallbackValueStringFormat;

    public TimePicker()
    {
      this.DefaultStyleKey = (object) typeof (TimePicker);
      this.Value = new DateTime?(DateTime.Now);
    }

    protected override string ValueStringFormatFallback
    {
      get
      {
        if (this._fallbackValueStringFormat == null)
          this._fallbackValueStringFormat = "{0:" + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Replace(":ss", "") + "}";
        return this._fallbackValueStringFormat;
      }
    }
  }
}
