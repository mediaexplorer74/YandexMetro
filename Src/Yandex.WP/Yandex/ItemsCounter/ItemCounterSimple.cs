// Decompiled with JetBrains decompiler
// Type: Yandex.ItemsCounter.ItemCounterSimple
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

namespace Yandex.ItemsCounter
{
  public class ItemCounterSimple : IItemCounter
  {
    private long _count;

    public void Increment() => ++this._count;

    public void Decrement() => --this._count;

    public long Count => this._count;
  }
}
