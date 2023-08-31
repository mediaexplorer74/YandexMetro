// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Events.OperationStatusChangedEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.Maps.Events
{
  [ComVisible(true)]
  public class OperationStatusChangedEventArgs : EventArgs
  {
    public OperationStatusChangedEventArgs(OperationStatus operationStatus) => this.OperationStatus = operationStatus;

    public OperationStatus OperationStatus { get; private set; }
  }
}
