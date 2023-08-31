// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.SelectedIndexChangedEventArgs
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;

namespace Microsoft.Phone.Controls.Primitives
{
  internal class SelectedIndexChangedEventArgs : EventArgs
  {
    public SelectedIndexChangedEventArgs(int index) => this.SelectedIndex = index;

    public int SelectedIndex { get; private set; }
  }
}
