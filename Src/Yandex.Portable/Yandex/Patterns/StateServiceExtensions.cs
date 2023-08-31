// Decompiled with JetBrains decompiler
// Type: Yandex.Patterns.StateServiceExtensions
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;

namespace Yandex.Patterns
{
  public static class StateServiceExtensions
  {
    public static void ExecuteWhenReady(this IStateService stateService, Action action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      EventHandler<StateChangedEventArgs> stateServiceOnStateChanged = (EventHandler<StateChangedEventArgs>) null;
      stateServiceOnStateChanged = (EventHandler<StateChangedEventArgs>) ((sender, e) =>
      {
        if (e.State != ServiceState.Ready)
          return;
        stateService.StateChanged -= stateServiceOnStateChanged;
        action();
      });
      stateService.StateChanged += stateServiceOnStateChanged;
      stateServiceOnStateChanged((object) stateService, new StateChangedEventArgs(stateService.State));
    }
  }
}
