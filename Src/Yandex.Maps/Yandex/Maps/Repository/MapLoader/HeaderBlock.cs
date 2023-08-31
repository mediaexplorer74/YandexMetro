// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.HeaderBlock
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class HeaderBlock
  {
    public const byte HeaderSize = 10;
    internal static readonly int MagicBlock = BitConverter.ToInt32(new byte[4]
    {
      (byte) 89,
      (byte) 66,
      (byte) 76,
      (byte) 75
    }, 0);
    public short[] TableNextBlock;
    internal short CurrentBlockVersion = 1;
    private int _offsetData;
    public byte NextBlockCount;
    public int BlockNumber;

    internal int BlockSize { get; private set; }

    internal byte[] Data { get; set; }

    public BlockType BlockType { get; private set; }

    public HeaderBlock(int blockSize, BlockType blockType)
    {
      this.BlockSize = blockSize << 10;
      this.TableObject = (IList<ObjectSize>) new List<ObjectSize>();
      this.BlockType = blockType;
    }

    public ushort ObjectsCount { get; set; }

    public IList<ObjectSize> TableObject { get; private set; }

    public int SetNextObject(byte[] data, int offset, int size)
    {
      int objectInternalOffset = this.GetObjectInternalOffset();
      int length = Math.Min(this.BlockSize - objectInternalOffset, size);
      Array.Copy((Array) data, offset, (Array) this.Data, objectInternalOffset, length);
      this._offsetData += length;
      return length;
    }

    public int GetNextObject(byte[] buf, int offset, int size)
    {
      int objectInternalOffset = this.GetObjectInternalOffset();
      int length = Math.Min(this.BlockSize - objectInternalOffset, size);
      Array.Copy((Array) this.Data, objectInternalOffset, (Array) buf, offset, length);
      this._offsetData += length;
      return size - length;
    }

    private int GetObjectInternalOffset() => 10 + ((int) this.NextBlockCount << 1) + (int) this.ObjectsCount * 6 + this._offsetData;
  }
}
