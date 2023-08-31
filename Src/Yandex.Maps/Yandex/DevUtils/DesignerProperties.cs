// Decompiled with JetBrains decompiler
// Type: Yandex.DevUtils.DesignerProperties
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.ComponentModel;

namespace Yandex.DevUtils
{
  internal static class DesignerProperties
  {
    private static readonly bool _isInDesignMode = DesignerProperties.GetIsInDesignMode();

    private static bool GetIsInDesignMode()
    {
      try
      {
        return DesignerProperties.IsInDesignTool;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static bool IsInDesignMode => DesignerProperties._isInDesignMode;
  }
}
