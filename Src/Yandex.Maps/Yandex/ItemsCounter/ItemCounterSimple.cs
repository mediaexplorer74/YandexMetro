// Decompiled with JetBrains decompiler
// Type: Yandex.ItemsCounter.ItemCounterSimple
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.ItemsCounter
{
  internal class ItemCounterSimple : IItemCounter
  {
    private long _count;

    public void Increment()
    {
      ++this._count;
      this.OnCountChanged();
    }

    public void Decrement()
    {
      --this._count;
      this.OnCountChanged();
    }

    public long Count => this._count;

    public event EventHandler CountChanged;

    protected virtual void OnCountChanged()
    {
      EventHandler countChanged = this.CountChanged;
      if (countChanged == null)
        return;
      countChanged((object) this, EventArgs.Empty);
    }
  }
}
