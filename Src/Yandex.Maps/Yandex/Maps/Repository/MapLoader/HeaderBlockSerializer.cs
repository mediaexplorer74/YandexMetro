// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.HeaderBlockSerializer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class HeaderBlockSerializer : IHeaderBlockSerializer
  {
    public byte[] Serialize(HeaderBlock cacheBlockHead)
    {
      byte[] buffer = new byte[cacheBlockHead.BlockSize];
      using (MemoryStream output = new MemoryStream(buffer))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Write(HeaderBlock.MagicBlock);
          binaryWriter.Write(cacheBlockHead.CurrentBlockVersion);
          binaryWriter.Write((byte) cacheBlockHead.BlockType);
          binaryWriter.Write(cacheBlockHead.NextBlockCount);
          binaryWriter.Write(cacheBlockHead.ObjectsCount);
          for (int index = 0; index < (int) cacheBlockHead.NextBlockCount; ++index)
            binaryWriter.Write(cacheBlockHead.TableNextBlock[index]);
          for (int index = 0; index < (int) cacheBlockHead.ObjectsCount; ++index)
          {
            binaryWriter.Write(cacheBlockHead.TableObject[index].Size);
            binaryWriter.Write(cacheBlockHead.TableObject[index].Index);
          }
        }
        output.Flush();
      }
      return buffer;
    }

    public HeaderBlock DeserializeHeaderBlock(byte[] buf, int blockSize)
    {
      using (MemoryStream input = new MemoryStream(buf))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          short num = binaryReader.ReadInt32() == HeaderBlock.MagicBlock ? binaryReader.ReadInt16() : throw new Exception("Wrong block magic");
          BlockType blockType = (BlockType) binaryReader.ReadByte();
          HeaderBlock headerBlock = new HeaderBlock(blockSize, blockType);
          headerBlock.Data = buf;
          headerBlock.CurrentBlockVersion = num;
          if (headerBlock.BlockType == BlockType.GroupHead || headerBlock.BlockType == BlockType.Residual)
          {
            headerBlock.NextBlockCount = binaryReader.ReadByte();
            headerBlock.TableNextBlock = new short[(int) headerBlock.NextBlockCount];
            headerBlock.ObjectsCount = binaryReader.ReadUInt16();
            for (int index = 0; index < (int) headerBlock.NextBlockCount; ++index)
              headerBlock.TableNextBlock[index] = binaryReader.ReadInt16();
            headerBlock.TableObject.Clear();
            for (int index = 0; index < (int) headerBlock.ObjectsCount; ++index)
              headerBlock.TableObject.Add(new ObjectSize(binaryReader.ReadInt32(), binaryReader.ReadInt16()));
          }
          return headerBlock;
        }
      }
    }
  }
}
