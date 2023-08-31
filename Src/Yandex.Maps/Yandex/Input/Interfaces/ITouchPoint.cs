// Decompiled with JetBrains decompiler
// Type: Yandex.Input.Interfaces.ITouchPoint
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Input.Interfaces
{
  [ComVisible(true)]
  public interface ITouchPoint
  {
    TouchAction Action { get; set; }

    Point Position { get; set; }

    [Obsolete("Is not supported on WP8")]
    Size Size { get; set; }

    object DirectlyOver { get; set; }
  }
}
