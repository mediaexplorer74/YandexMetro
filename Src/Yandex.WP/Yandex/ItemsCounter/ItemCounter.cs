// Decompiled with JetBrains decompiler
// Type: Yandex.ItemsCounter.ItemCounter
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Threading;

namespace Yandex.ItemsCounter
{
  public class ItemCounter : IItemCounter
  {
    private int _count;

    public void Increment() => Interlocked.Increment(ref this._count);

    public void Decrement() => Interlocked.Decrement(ref this._count);

    public long Count => (long) this._count;
  }
}
