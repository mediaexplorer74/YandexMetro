﻿// Decompiled with JetBrains decompiler
// Type: DanielVaughan.WindowsPhone7Unleashed.BindingChangedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;

namespace DanielVaughan.WindowsPhone7Unleashed
{
  internal class BindingChangedEventArgs : System.EventArgs
  {
    public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e) => this.EventArgs = e;

    public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
  }
}
