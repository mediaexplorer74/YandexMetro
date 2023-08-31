// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Controls.Events.TrafficValueChangedArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;

namespace Yandex.Maps.Controls.Events
{
  [ComVisible(true)]
  public class TrafficValueChangedArgs : EventArgs
  {
    public byte? TrafficValue { get; private set; }

    public TrafficValueChangedArgs(byte? trafficValue) => this.TrafficValue = trafficValue;
  }
}
