// Decompiled with JetBrains decompiler
// Type: Yandex.DevUtils.DesignerProperties
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.ComponentModel;

namespace Yandex.DevUtils
{
  public static class DesignerProperties
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
