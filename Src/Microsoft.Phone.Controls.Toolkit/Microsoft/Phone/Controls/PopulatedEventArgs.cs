// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PopulatedEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Collections;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class PopulatedEventArgs : RoutedEventArgs
  {
    public IEnumerable Data { get; private set; }

    public PopulatedEventArgs(IEnumerable data) => this.Data = data;
  }
}
