// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.ObjectSize
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Repository.MapLoader
{
  internal class ObjectSize
  {
    public ObjectSize(int size, short index)
    {
      this.Size = size;
      this.Index = index;
    }

    public int Size { get; private set; }

    public short Index { get; private set; }
  }
}
