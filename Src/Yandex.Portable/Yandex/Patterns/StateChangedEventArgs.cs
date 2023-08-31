// Decompiled with JetBrains decompiler
// Type: Yandex.Patterns.StateChangedEventArgs
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Patterns
{
  public class StateChangedEventArgs : EventArgs
  {
    public StateChangedEventArgs(ServiceState state) => this.State = state;

    public ServiceState State { get; private set; }
  }
}
