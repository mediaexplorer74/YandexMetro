// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheFileHeader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yandex.Common;
using Yandex.IO;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class CacheFileHeader : IDisposable
  {
    private const ushort Version = 1;
    private const double BlockFreeSpaceThreshold = 0.4;
    private const int BufferBitTableLength = 8192;
    private const int BufferTempTableLength = 512;
    private const ushort DefaultRegularBlockSize = 32;
    private readonly string _fileName;
    private readonly object _fileStreamSync = new object();
    private readonly IFileStorage _fileStorage;
    private readonly ITileFactory<ITile> _tileFactory;
    private readonly ITileFileNameConstructor _tileFileNameConstructor;
    private readonly IHeaderBlockSerializer _headerBlockSerializer;
    private readonly IPath _path;
    private byte[] _offsetTableBuffer = new byte[32768];
    private readonly byte[] _bufferBitTable = new byte[8192];
    private readonly List<ITile> _listCacheSaveElements = new List<ITile>();
    private bool _disposed;
    private volatile Stream _fileStream;
    private ushort _headerSize = 32;
    private readonly int _magic = CacheStorageAsync.MAGIC_CACHE;
    private readonly int _platformInfo = CacheStorageAsync.CLIENT;
    private ushort _regularBlockSize = 32;
    private ushort _residualBlockSize = 23;
    private readonly byte[] _bufferTempTable = new byte[512];
    private readonly int _i;
    private readonly int _j;
    private readonly BaseLayers _type;
    private readonly byte _zoom;
    private bool _fileOperationsEnabled;
    private bool _flushPending;
    private IComparer<ITile> IdSortComparer;

    public CacheFileHeader(
      string fileName,
      IFileStorage fileStorage,
      ITileFactory<ITile> tileFactory,
      ITileFileNameConstructor tileFileNameConstructor,
      [NotNull] IHeaderBlockSerializer headerBlockSerializer,
      [NotNull] IPath path)
    {
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      if (tileFactory == null)
        throw new ArgumentNullException(nameof (tileFactory));
      if (tileFileNameConstructor == null)
        throw new ArgumentNullException(nameof (tileFileNameConstructor));
      if (headerBlockSerializer == null)
        throw new ArgumentNullException(nameof (headerBlockSerializer));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this._fileStorage = fileStorage;
      this._tileFactory = tileFactory;
      this._tileFileNameConstructor = tileFileNameConstructor;
      this._headerBlockSerializer = headerBlockSerializer;
      this._path = path;
      this._fileName = fileName;
      this._fileOperationsEnabled = true;
      int i;
      int j;
      byte zoom;
      BaseLayers type;
      this._tileFileNameConstructor.ParseFileName(this._fileName, out i, out j, out zoom, out type);
      this._i = i;
      this._j = j;
      this._zoom = zoom;
      this._type = type;
      this._flushPending = false;
      this.IdSortComparer = (IComparer<ITile>) new Yandex.Maps.Repository.MapLoader.IdSortComparer();
    }

    public bool TryDeserialize()
    {
      if (!this._fileOperationsEnabled)
        return false;
      try
      {
        lock (this._fileStreamSync)
        {
          Stream fileStream = this.GetFileStream(Yandex.PAL.IO.FileAccess.Read);
          if (fileStream == null)
            return false;
          fileStream.Seek(0L, SeekOrigin.Begin);
          BinaryReader binaryReader = new BinaryReader(fileStream);
          int num1 = binaryReader.ReadInt32();
          this._headerSize = binaryReader.ReadUInt16();
          ushort num2 = binaryReader.ReadUInt16();
          if (CacheStorageAsync.MAGIC_CACHE != num1 || num2 != (ushort) 1)
            return false;
          binaryReader.ReadInt32();
          this._regularBlockSize = binaryReader.ReadUInt16();
          this._residualBlockSize = binaryReader.ReadUInt16();
          if (fileStream.Length < (long) ((int) this._regularBlockSize << 10))
            return false;
          binaryReader.Read(this._bufferBitTable, 0, 8192);
          binaryReader.Read(this._bufferTempTable, 0, 512);
          if (binaryReader.ReadByte() == (byte) 0)
          {
            fileStream.Seek(495L, SeekOrigin.Current);
            this.ReadResidualBlock(binaryReader.ReadBytes((int) this._residualBlockSize << 10));
          }
          this._offsetTableBuffer = new byte[(int) this._regularBlockSize << 10];
          this.ReadBlock(fileStream, this._offsetTableBuffer, 0);
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
        return false;
      }
    }

    [CanBeNull]
    private Stream GetFileStream(Yandex.PAL.IO.FileAccess fileAccess)
    {
      if (this._disposed)
        return (Stream) null;
      bool flag1 = false;
      if (this._fileStream != null)
      {
        switch (fileAccess)
        {
          case Yandex.PAL.IO.FileAccess.Read:
            flag1 = this._fileStream.CanRead;
            break;
          case Yandex.PAL.IO.FileAccess.Write:
            flag1 = this._fileStream.CanWrite;
            break;
          case Yandex.PAL.IO.FileAccess.ReadWrite:
            flag1 = this._fileStream.CanWrite && this._fileStream.CanRead;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (fileAccess));
        }
        if (!flag1)
          this.CloseFileStream();
      }
      if (!flag1)
      {
        try
        {
          bool flag2 = fileAccess == Yandex.PAL.IO.FileAccess.Read;
          if (this._fileStorage.DirectoryExists(this._path.GetDirectoryName(this._fileName)))
          {
            if (flag2)
            {
              if (!this._fileStorage.FileExists(this._fileName))
                goto label_16;
            }
            Yandex.PAL.IO.FileAccess fileAccess1 = fileAccess == Yandex.PAL.IO.FileAccess.Write ? Yandex.PAL.IO.FileAccess.ReadWrite : fileAccess;
            this._fileStream = this._fileStorage.OpenFile(this._fileName, flag2 ? Yandex.IO.FileMode.Open : Yandex.IO.FileMode.OpenOrCreate, fileAccess1, Yandex.PAL.IO.FileShare.None);
          }
        }
        catch (Exception ex)
        {
          this._fileOperationsEnabled = false;
        }
      }
label_16:
      return this._fileStream;
    }

    private void FlushCacheFileHeader(IList<ITile> residualTiles)
    {
      if (!this._fileOperationsEnabled)
        return;
      lock (this._fileStreamSync)
      {
        Stream fileStream = this.GetFileStream(Yandex.PAL.IO.FileAccess.Write);
        if (fileStream == null)
          return;
        this.WriteCacheFileHeader(fileStream, residualTiles);
        fileStream.Flush();
      }
    }

    private void WriteCacheFileHeader([NotNull] Stream stream, IList<ITile> residualTiles)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      stream.Seek(0L, SeekOrigin.Begin);
      BinaryWriter binaryWriter = new BinaryWriter(stream);
      binaryWriter.Write(this._magic);
      binaryWriter.Write(this._headerSize);
      binaryWriter.Write((ushort) 1);
      binaryWriter.Write(this._platformInfo);
      binaryWriter.Write(this._regularBlockSize);
      binaryWriter.Write(this._residualBlockSize);
      binaryWriter.Write(this._bufferBitTable, 0, 8192);
      binaryWriter.Write(this._bufferTempTable, 0, 512);
      binaryWriter.Write((byte) 0);
      binaryWriter.Seek(495, SeekOrigin.Current);
      binaryWriter.Write(this.SerializeResidualBlock(residualTiles), 0, (int) this._residualBlockSize << 10);
      binaryWriter.Flush();
      this.Write(stream, this._offsetTableBuffer, 0);
    }

    private bool TryWriteRegularBlock(bool forceWrite)
    {
      bool flag1 = forceWrite;
      byte length = 0;
      int num1 = ((int) this._regularBlockSize << 10) - 10;
      int num2 = num1;
      List<ITile> source = new List<ITile>();
      foreach (ITile cacheSaveElement in this._listCacheSaveElements)
      {
        int num3 = cacheSaveElement.DataLength + 10;
        source.Add(cacheSaveElement);
        for (num2 -= num3; num2 < 0; num2 += num1)
          ++length;
        if ((double) num2 < (double) num1 * 0.4)
        {
          flag1 = true;
          break;
        }
      }
      if (!this._fileOperationsEnabled || !flag1 || !source.Any<ITile>())
        return false;
      int bitFree = Utils.GetBitFree(this._bufferBitTable, 0);
      Utils.SetBit(this._bufferBitTable, 1, bitFree);
      HeaderBlock cacheBlockHead = new HeaderBlock((int) this._regularBlockSize, BlockType.GroupHead)
      {
        BlockNumber = bitFree,
        ObjectsCount = (ushort) source.Count
      };
      IList<ObjectSize> tableObject = cacheBlockHead.TableObject;
      foreach (ITile tile in source)
        tableObject.Add(new ObjectSize(tile.DataLength, tile.TileInfo.Index));
      cacheBlockHead.NextBlockCount = length;
      short[] numArray = cacheBlockHead.TableNextBlock = new short[(int) length];
      for (int index = 0; index < (int) length; ++index)
      {
        numArray[index] = (short) Utils.GetBitFree(this._bufferBitTable, 0);
        Utils.SetBit(this._bufferBitTable, 1, (int) numArray[index]);
      }
      byte[] data = this._headerBlockSerializer.Serialize(cacheBlockHead);
      cacheBlockHead.Data = data;
      bool flag2 = false;
      try
      {
        int num4 = 0;
        lock (this._fileStreamSync)
        {
          Stream fileStream = this.GetFileStream(Yandex.PAL.IO.FileAccess.Write);
          if (fileStream == null)
            return false;
          foreach (ITile tile in source)
          {
            this._listCacheSaveElements.Remove(tile);
            int dataLength = tile.DataLength;
            int dataOffset = tile.DataOffset;
            int num5;
            while ((num5 = cacheBlockHead.SetNextObject(tile.Bytes, dataOffset, dataLength)) != dataLength)
            {
              this.Write(fileStream, data, cacheBlockHead.BlockNumber);
              dataOffset += num5;
              dataLength -= num5;
              cacheBlockHead = new HeaderBlock((int) this._regularBlockSize, BlockType.Regular)
              {
                BlockNumber = (int) numArray[num4++]
              };
              data = this._headerBlockSerializer.Serialize(cacheBlockHead);
              cacheBlockHead.Data = data;
            }
            byte[] bytes = BitConverter.GetBytes(bitFree);
            this._offsetTableBuffer[(int) tile.TileInfo.Index << 1] = bytes[0];
            this._offsetTableBuffer[((int) tile.TileInfo.Index << 1) + 1] = bytes[1];
            flag2 = true;
          }
          if (flag2)
            this.Write(fileStream, data, cacheBlockHead.BlockNumber);
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
        Utils.SetBit(this._bufferBitTable, 1, bitFree);
        for (int index = 0; index < (int) length; ++index)
          Utils.SetBit(this._bufferBitTable, 0, (int) numArray[index]);
        return false;
      }
      return flag2;
    }

    private byte[] SerializeResidualBlock(IList<ITile> saveElements)
    {
      HeaderBlock cacheBlockHead = new HeaderBlock((int) this._residualBlockSize, BlockType.Residual)
      {
        ObjectsCount = (ushort) saveElements.Count
      };
      int length = (int) (cacheBlockHead.NextBlockCount = (byte) 0);
      cacheBlockHead.TableNextBlock = new short[length];
      IList<ObjectSize> tableObject = cacheBlockHead.TableObject;
      foreach (ITile saveElement in (IEnumerable<ITile>) saveElements)
        tableObject.Add(new ObjectSize(saveElement.DataLength, saveElement.TileInfo.Index));
      byte[] numArray = this._headerBlockSerializer.Serialize(cacheBlockHead);
      cacheBlockHead.Data = numArray;
      foreach (ITile saveElement in (IEnumerable<ITile>) saveElements)
        cacheBlockHead.SetNextObject(saveElement.Bytes, saveElement.DataOffset, saveElement.DataLength);
      return numArray;
    }

    private void Write(Stream fileStream, byte[] data, int blockIndex)
    {
      int blockSize = this.GetBlockSize();
      int blockOffset = CacheFileHeader.GetBlockOffset(blockIndex, blockSize);
      fileStream.Seek((long) blockOffset, SeekOrigin.Begin);
      fileStream.Write(data, 0, blockSize);
    }

    private static int GetBlockOffset(int blockIndex, int blockSize) => (blockSize << 1) + (blockIndex - 1) * blockSize;

    private int GetBlockSize() => (int) this._regularBlockSize << 10;

    public IEnumerable<ITile> ReadRegularBlock(IList<ITileInfo> listTiles)
    {
      List<ITile> source = new List<ITile>(listTiles.Count);
      List<int> intList = new List<int>(listTiles.Count);
      try
      {
        lock (this._fileStreamSync)
        {
          Stream fileStream = this.GetFileStream(Yandex.PAL.IO.FileAccess.Read);
          foreach (ITileInfo listTile in (IEnumerable<ITileInfo>) listTiles)
          {
            ITileInfo tile = listTile;
            if (tile != null && !source.Any<ITile>((Func<ITile, bool>) (item => item != null && item.TileInfo == tile)))
            {
              ITile tile1 = this._listCacheSaveElements.FirstOrDefault<ITile>((Func<ITile, bool>) (t => t.TileInfo.Equals((object) tile)));
              if (tile1 != null)
              {
                source.Add(tile1);
              }
              else
              {
                ushort uint16 = BitConverter.ToUInt16(this._offsetTableBuffer, (int) tile.Index << 1);
                if (uint16 != (ushort) 0 && Utils.getBit(this._bufferBitTable, uint16) == 1 && !intList.Contains((int) uint16) && fileStream != null)
                {
                  List<ITile> tileList = new List<ITile>();
                  this.ReadTilesFromBlock(fileStream, (int) uint16, tileList);
                  intList.Add((int) uint16);
                  source.AddRange((IEnumerable<ITile>) tileList);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
      return (IEnumerable<ITile>) source;
    }

    private void ReadObjectsFromRegularBlockAndMarkAsDeleted(int blockIndex)
    {
      try
      {
        lock (this._fileStreamSync)
        {
          Stream fileStream = this.GetFileStream(Yandex.PAL.IO.FileAccess.Read);
          if (fileStream == null)
            return;
          this.ReadTilesFromBlock(fileStream, blockIndex, this._listCacheSaveElements);
        }
        Utils.SetBit(this._bufferBitTable, 0, blockIndex);
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }

    private void ReadTilesFromBlock(Stream fileStream, int blockIndex, List<ITile> vector)
    {
      byte[] numArray = new byte[(int) this._regularBlockSize << 10];
      this.ReadBlock(fileStream, numArray, blockIndex);
      HeaderBlock headerBlock = this._headerBlockSerializer.DeserializeHeaderBlock(numArray, (int) this._regularBlockSize);
      short[] tableNextBlock = headerBlock.TableNextBlock;
      IList<ObjectSize> tableObject = headerBlock.TableObject;
      int objectsCount = (int) headerBlock.ObjectsCount;
      int num = 0;
      for (int index = 0; index < objectsCount; ++index)
      {
        int offset1 = 0;
        byte[] buf = new byte[tableObject[index].Size];
        int offset2;
        for (int nextObject = headerBlock.GetNextObject(buf, offset1, tableObject[index].Size); nextObject != 0; nextObject = headerBlock.GetNextObject(buf, offset2, nextObject))
        {
          blockIndex = (int) tableNextBlock[num++];
          this.ReadBlock(fileStream, numArray, blockIndex);
          headerBlock = this._headerBlockSerializer.DeserializeHeaderBlock(numArray, (int) this._regularBlockSize);
          offset2 = tableObject[index].Size - nextObject;
        }
        vector.Add(this.DeserializeObject(tableObject[index].Index, buf));
      }
    }

    private void ReadBlock(Stream fileStream, byte[] data, int indexBlock)
    {
      int offset = (indexBlock + 1) * ((int) this._regularBlockSize << 10);
      fileStream.Seek((long) offset, SeekOrigin.Begin);
      fileStream.Read(data, 0, (int) this._regularBlockSize << 10);
    }

    private void ReadResidualBlock(byte[] data)
    {
      HeaderBlock headerBlock = this._headerBlockSerializer.DeserializeHeaderBlock(data, (int) this._residualBlockSize);
      IList<ObjectSize> tableObject = headerBlock.TableObject;
      int objectsCount = (int) headerBlock.ObjectsCount;
      for (int index = 0; index < objectsCount; ++index)
      {
        byte[] buf = new byte[tableObject[index].Size];
        headerBlock.GetNextObject(buf, 0, tableObject[index].Size);
        this.AddListCacheSaveElement(this.DeserializeObject(tableObject[index].Index, buf), true);
      }
    }

    public void AddListCacheSaveElement(ITile tile, bool residual)
    {
      if (!residual)
      {
        this._flushPending = true;
        ushort uint16 = BitConverter.ToUInt16(this._offsetTableBuffer, (int) tile.TileInfo.Index << 1);
        if (uint16 != (ushort) 0 && Utils.getBit(this._bufferBitTable, uint16) == 1)
          this.ReadObjectsFromRegularBlockAndMarkAsDeleted((int) uint16);
      }
      ITile[] array = this._listCacheSaveElements.Where<ITile>((Func<ITile, bool>) (item => item.TileInfo.EqualsCoordinates(tile.TileInfo))).ToArray<ITile>();
      if (((IEnumerable<ITile>) array).Any<ITile>())
      {
        foreach (ITile tile1 in array)
          this._listCacheSaveElements.Remove(tile1);
      }
      this._listCacheSaveElements.Add(tile);
    }

    private int GetListCacheSaveElementsSize(IEnumerable<ITile> tiles) => tiles.Sum<ITile>((Func<ITile, int>) (item => item.DataLength));

    private void SortList(List<ITile> tiles) => tiles.Sort(this.IdSortComparer);

    private ITile DeserializeObject(short hash, byte[] buf)
    {
      int num1 = (int) hash & (int) sbyte.MaxValue;
      int num2 = (int) hash >> 7;
      ITile tile = this._tileFactory.CreateTile(this._i + num1, this._j + num2, this._zoom, this._type, buf, 0, buf.Length, TileStatus.Ok);
      tile.TileInfo.Index = hash;
      tile.TileInfo.IdSort = Utils.CalcMortonNumber(num1 >> 1, num2 >> 1);
      return tile;
    }

    public void Dispose()
    {
      this.OnDispose();
      GC.SuppressFinalize((object) this);
    }

    ~CacheFileHeader() => this.OnDispose();

    private void OnDispose()
    {
      if (this._disposed)
        return;
      this.TryFlush(true);
      this.CloseFileStream();
      this._disposed = true;
    }

    private void CloseFileStream()
    {
      lock (this._fileStreamSync)
      {
        if (this._fileStream == null)
          return;
        this._fileStream.Dispose();
        this._fileStream = (Stream) null;
      }
    }

    public bool TryFlush(bool forceWrite = false)
    {
      try
      {
        if (!this._flushPending)
          return false;
        bool flag = false;
        List<ITile> cacheSaveElements = this._listCacheSaveElements;
        this.SortList(cacheSaveElements);
        if (this.GetListCacheSaveElementsSize((IEnumerable<ITile>) cacheSaveElements) > (int) this._residualBlockSize << 10)
        {
          while (this.TryWriteRegularBlock(forceWrite))
            flag = true;
        }
        if (!forceWrite)
        {
          if (!flag)
            goto label_12;
        }
        if (forceWrite)
        {
          this.FlushCacheFileHeader((IList<ITile>) cacheSaveElements.ToArray());
          this._listCacheSaveElements.Clear();
        }
        else
          this.FlushCacheFileHeader((IList<ITile>) new ITile[0]);
        return true;
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
label_12:
      return false;
    }

    internal DateTime LastAccessTime { get; set; }
  }
}
