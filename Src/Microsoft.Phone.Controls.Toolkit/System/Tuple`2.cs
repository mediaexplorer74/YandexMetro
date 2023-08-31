// Decompiled with JetBrains decompiler
// Type: System.Tuple`2
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

namespace System
{
  internal class Tuple<T1, T2>
  {
    public T1 Item1 { get; private set; }

    public T2 Item2 { get; private set; }

    public Tuple(T1 item1, T2 item2)
    {
      this.Item1 = item1;
      this.Item2 = item2;
    }
  }
}
