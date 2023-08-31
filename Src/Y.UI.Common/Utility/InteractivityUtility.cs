// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.InteractivityUtility
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Interactivity;

namespace Y.UI.Common.Utility
{
  public static class InteractivityUtility
  {
    public static void UnwireBehaviorsAndTriggers(DependencyObject currentControl)
    {
      if (currentControl == null)
        return;
      Interaction.GetTriggers(currentControl).Clear();
      Interaction.GetBehaviors(currentControl).Clear();
    }
  }
}
