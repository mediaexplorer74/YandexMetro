// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.CollapseAllPushPinsAction
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;

namespace Yandex.Maps
{
  [DefaultTrigger(typeof (UIElement), typeof (EventTrigger), "Tap")]
  [ComVisible(false)]
  public class CollapseAllPushPinsAction : TriggerAction<MapBase>
  {
    protected override void Invoke(object parameter) => this.AssociatedObject?.PushPinManager.CollapseAllPushpins();
  }
}
