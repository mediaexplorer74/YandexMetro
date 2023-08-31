// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileReader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository
{
  [UsedImplicitly]
  internal class TileReader : ITileSerializer
  {
    public static readonly int TileMagic = BitConverter.ToInt32(new byte[4]
    {
      (byte) 89,
      (byte) 84,
      (byte) 76,
      (byte) 68
    }, 0);

    public bool DeserializeTileDetails(ITile tile, byte[] data, int dataOffset, int dataLength)
    {
      if (dataOffset + dataLength > data.Length)
        return false;
      using (MemoryStream input = new MemoryStream(data, dataOffset, dataLength, false))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          if (binaryReader.ReadInt32() != TileReader.TileMagic)
            return false;
          ushort length = binaryReader.ReadUInt16();
          tile.Version = binaryReader.ReadUInt16();
          tile.TileInfo.Checksum = binaryReader.ReadBytes(16);
          tile.TileInfo.MapVersion = binaryReader.ReadUInt16();
          tile.Time = (ulong) binaryReader.ReadUInt32();
          uint num1 = 0;
          if (length > (ushort) 0)
          {
            TileRecord[] tileRecordArray = new TileRecord[(int) length];
            for (int index = 0; index < (int) length; ++index)
            {
              TileRecord tileRecord = new TileRecord()
              {
                Scale = binaryReader.ReadInt16()
              };
              int num2 = (int) binaryReader.ReadInt16();
              tileRecord.DataLength = binaryReader.ReadUInt32();
              tileRecord.DataOffset = num1;
              tileRecordArray[index] = tileRecord;
              num1 += tileRecord.DataLength;
            }
            tile.Scale = tileRecordArray[0].Scale;
            tile.Records = tileRecordArray;
          }
          tile.Bytes = data;
          tile.BitmapSource = (object) null;
          tile.HeaderLength = (int) input.Position;
          tile.DataOffset = dataOffset;
          tile.DataLength = dataLength;
        }
      }
      return true;
    }

    public void SerializeTileDetails(ITile tile, byte[] data, int dataOffset, int dataLength)
    {
      if (dataOffset + dataLength > data.Length || 32 > data.Length)
        return;
      using (MemoryStream output = new MemoryStream(data, dataOffset, dataLength, true))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Seek(30, SeekOrigin.Current);
          binaryWriter.Write(tile.Scale);
        }
      }
      if (tile.Records == null || tile.Records.Length <= 0)
        return;
      tile.Records[0].Scale = tile.Scale;
    }
  }
}
