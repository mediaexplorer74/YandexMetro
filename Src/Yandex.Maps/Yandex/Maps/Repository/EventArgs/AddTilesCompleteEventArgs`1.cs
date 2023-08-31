// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.EventArgs.AddTilesCompleteEventArgs`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.EventArgs
{
  internal class AddTilesCompleteEventArgs<T> : System.EventArgs where T : ITile
  {
    public AddTilesCompleteEventArgs(IEnumerable<T> tiles) => this.Tiles = tiles != null ? tiles : throw new ArgumentNullException(nameof (tiles));

    public IEnumerable<T> Tiles { get; private set; }
  }
}
