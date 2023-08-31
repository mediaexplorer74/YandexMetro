// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Behaviors.LandscapeHideSystemTrayIndicatorBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Yandex.Controls.Behaviors
{
  internal class LandscapeHideSystemTrayIndicatorBehavior : PageOrientationBehavior
  {
    protected override void OnUpdatePageOrientation(PageOrientation currentPageOrientation)
    {
      PageOrientation pageOrientation = currentPageOrientation;
      bool flag = pageOrientation != 2 && pageOrientation != 18 && pageOrientation != 34;
      ProgressIndicator progressIndicator = SystemTray.ProgressIndicator;
      if (progressIndicator == null)
        return;
      progressIndicator.IsVisible = flag;
    }
  }
}
