// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.EventArgs.TileBlocksReadyEventArgs
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.EventArgs
{
  internal class TileBlocksReadyEventArgs : System.EventArgs
  {
    public TileBlocksReadyEventArgs(IEnumerable<ITileBlock> requestedBlocks) => this.RequestedBlocks = requestedBlocks != null ? requestedBlocks : throw new ArgumentNullException(nameof (requestedBlocks));

    public IEnumerable<ITileBlock> RequestedBlocks { get; private set; }
  }
}
