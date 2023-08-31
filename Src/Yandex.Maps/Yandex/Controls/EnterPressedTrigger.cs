// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.EnterPressedTrigger
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Yandex.Controls
{
  internal class EnterPressedTrigger : EventTrigger
  {
    protected override void OnEvent(EventArgs eventArgs)
    {
      if (!(eventArgs is KeyEventArgs) || ((KeyEventArgs) eventArgs).Key != 3)
        return;
      base.OnEvent(eventArgs);
    }
  }
}
