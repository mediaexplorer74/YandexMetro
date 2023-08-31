// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.Interfaces.IBitmapRenderer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Media.Imaging.Interfaces
{
  internal interface IBitmapRenderer
  {
    void Render(byte[] source, int offset, int length, int[] destination, int width, int height);

    void Render(byte[] source, int offset, int length, out int[] destination);
  }
}
