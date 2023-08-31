// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.LinkUnlinkEventArgs
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows.Controls;

namespace Microsoft.Phone.Controls
{
  public class LinkUnlinkEventArgs : EventArgs
  {
    public LinkUnlinkEventArgs(ContentPresenter cp) => this.ContentPresenter = cp;

    public ContentPresenter ContentPresenter { get; private set; }
  }
}
