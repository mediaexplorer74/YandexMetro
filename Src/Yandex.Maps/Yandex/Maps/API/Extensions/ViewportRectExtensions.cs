// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.Extensions.ViewportRectExtensions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Media;

namespace Yandex.Maps.API.Extensions
{
  [ComVisible(true)]
  public static class ViewportRectExtensions
  {
    public static ViewportRect Expand(this ViewportRect viewportRect, double extendRatio)
    {
      if (extendRatio == 1.0)
        return viewportRect;
      double width = viewportRect.Rect.Width;
      double height = viewportRect.Rect.Height;
      double num = (extendRatio - 1.0) * 0.5;
      return new ViewportRect(new Rect(viewportRect.Rect.X - width * num, viewportRect.Rect.Y - height * num, width * extendRatio, height * extendRatio), viewportRect.ZoomLevel);
    }
  }
}
