// Decompiled with JetBrains decompiler
// Type: DanielVaughan.WindowsPhone7Unleashed.BindingChangedEventArgs
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Windows;

namespace DanielVaughan.WindowsPhone7Unleashed
{
  public class BindingChangedEventArgs : System.EventArgs
  {
    public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e) => this.EventArgs = e;

    public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
  }
}
