// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MappingTileQueue
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository
{
  internal class MappingTileQueue : IMappingTileQueue, IStateQueue<ITileInfo, TileQueueEntry>
  {
    private readonly IDictionary<ITileInfo, TileQueueEntry> _internalDictionary;
    private readonly object _internalDictionaryLock = new object();
    private readonly ITileInfoNormalizer _tileInfoConverter;

    public MappingTileQueue(
      ITileInfoNormalizer tileInfoNormalizer,
      IDictionary<ITileInfo, TileQueueEntry> internalDictionary)
    {
      if (tileInfoNormalizer == null)
        throw new ArgumentNullException(nameof (tileInfoNormalizer));
      if (internalDictionary == null)
        throw new ArgumentNullException(nameof (internalDictionary));
      this._tileInfoConverter = tileInfoNormalizer;
      this._internalDictionary = internalDictionary;
    }

    public void Clear()
    {
      lock (this._internalDictionaryLock)
        this._internalDictionary.Clear();
    }

    public bool TryGetEntry(ITileInfo tileInfo, [CanBeNull] out TileQueueEntry entry)
    {
      if (this._internalDictionary.TryGetValue(this._tileInfoConverter.ConvertToFiniteCoordinates(tileInfo), out entry))
        return true;
      entry = (TileQueueEntry) null;
      return false;
    }

    public IEnumerable<ITileInfo> Keys => (IEnumerable<ITileInfo>) this._internalDictionary.Keys;

    public IEnumerable<TileQueueEntry> Values => (IEnumerable<TileQueueEntry>) this._internalDictionary.Values;

    public bool Contains(ITileInfo tileInfo)
    {
      lock (this._internalDictionaryLock)
        return this._internalDictionary.ContainsKey(tileInfo);
    }

    public bool TryEnqueue([NotNull] ITileInfo infiniteTileInfo, out TileQueueEntry entry)
    {
      ITileInfo key = infiniteTileInfo != null ? this._tileInfoConverter.ConvertToFiniteCoordinates(infiniteTileInfo) : throw new ArgumentNullException(nameof (infiniteTileInfo));
      lock (this._internalDictionaryLock)
      {
        if (this._internalDictionary.TryGetValue(key, out entry))
        {
          if (entry == null || entry.MappedTileInfos.Contains(infiniteTileInfo))
            return false;
          entry.MappedTileInfos.Add(infiniteTileInfo);
          return false;
        }
        entry = new TileQueueEntry(key);
        entry.MappedTileInfos.Add(infiniteTileInfo);
        this._internalDictionary.Add(key, entry);
        return true;
      }
    }

    public TileQueueEntry Dequeue([NotNull] ITileInfo infiniteTileInfo)
    {
      ITileInfo key = infiniteTileInfo != null ? this._tileInfoConverter.ConvertToFiniteCoordinates(infiniteTileInfo) : throw new ArgumentNullException(nameof (infiniteTileInfo));
      lock (this._internalDictionaryLock)
      {
        TileQueueEntry tileQueueEntry;
        if (!this._internalDictionary.TryGetValue(key, out tileQueueEntry))
          return (TileQueueEntry) null;
        this._internalDictionary.Remove(key);
        return tileQueueEntry;
      }
    }
  }
}
