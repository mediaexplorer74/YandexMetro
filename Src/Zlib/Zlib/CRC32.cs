// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.CRC32
// Assembly: Zlib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 235FDED2-38DA-4349-9C02-D4B9C65CF362
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Zlib.dll

using System;
using System.IO;

namespace Ionic.Zlib
{
  public class CRC32
  {
    private const int BUFFER_SIZE = 8192;
    private long _TotalBytesRead;
    private static readonly uint[] crc32Table;
    private uint _RunningCrc32Result = uint.MaxValue;

    public long TotalBytesRead => this._TotalBytesRead;

    public int Crc32Result => ~(int) this._RunningCrc32Result;

    public int GetCrc32(Stream input) => this.GetCrc32AndCopy(input, (Stream) null);

    public int GetCrc32AndCopy(Stream input, Stream output)
    {
      if (input == null)
        throw new ZlibException("The input stream must not be null.");
      byte[] numArray = new byte[8192];
      int count1 = 8192;
      this._TotalBytesRead = 0L;
      int count2 = input.Read(numArray, 0, count1);
      output?.Write(numArray, 0, count2);
      this._TotalBytesRead += (long) count2;
      while (count2 > 0)
      {
        this.SlurpBlock(numArray, 0, count2);
        count2 = input.Read(numArray, 0, count1);
        output?.Write(numArray, 0, count2);
        this._TotalBytesRead += (long) count2;
      }
      return ~(int) this._RunningCrc32Result;
    }

    public int ComputeCrc32(int W, byte B) => this._InternalComputeCrc32((uint) W, B);

    internal int _InternalComputeCrc32(uint W, byte B) => (int) CRC32.crc32Table[(IntPtr) (uint) (((int) W ^ (int) B) & (int) byte.MaxValue)] ^ (int) (W >> 8);

    public void SlurpBlock(byte[] block, int offset, int count)
    {
      if (block == null)
        throw new ZlibException("The data buffer must not be null.");
      for (int index1 = 0; index1 < count; ++index1)
      {
        int index2 = offset + index1;
        this._RunningCrc32Result = this._RunningCrc32Result >> 8 ^ CRC32.crc32Table[(IntPtr) ((uint) block[index2] ^ this._RunningCrc32Result & (uint) byte.MaxValue)];
      }
      this._TotalBytesRead += (long) count;
    }

    static CRC32()
    {
      uint num1 = 3988292384;
      CRC32.crc32Table = new uint[256];
      for (uint index1 = 0; index1 < 256U; ++index1)
      {
        uint num2 = index1;
        for (uint index2 = 8; index2 > 0U; --index2)
        {
          if (((int) num2 & 1) == 1)
            num2 = num2 >> 1 ^ num1;
          else
            num2 >>= 1;
        }
        CRC32.crc32Table[(IntPtr) index1] = num2;
      }
    }

    private uint gf2_matrix_times(uint[] matrix, uint vec)
    {
      uint num = 0;
      int index = 0;
      while (vec != 0U)
      {
        if (((int) vec & 1) == 1)
          num ^= matrix[index];
        vec >>= 1;
        ++index;
      }
      return num;
    }

    private void gf2_matrix_square(uint[] square, uint[] mat)
    {
      for (int index = 0; index < 32; ++index)
        square[index] = this.gf2_matrix_times(mat, mat[index]);
    }

    public void Combine(int crc, int length)
    {
      uint[] numArray1 = new uint[32];
      uint[] numArray2 = new uint[32];
      if (length == 0)
        return;
      uint vec = ~this._RunningCrc32Result;
      uint num1 = (uint) crc;
      numArray2[0] = 3988292384U;
      uint num2 = 1;
      for (int index = 1; index < 32; ++index)
      {
        numArray2[index] = num2;
        num2 <<= 1;
      }
      this.gf2_matrix_square(numArray1, numArray2);
      this.gf2_matrix_square(numArray2, numArray1);
      uint num3 = (uint) length;
      do
      {
        this.gf2_matrix_square(numArray1, numArray2);
        if (((int) num3 & 1) == 1)
          vec = this.gf2_matrix_times(numArray1, vec);
        uint num4 = num3 >> 1;
        if (num4 != 0U)
        {
          this.gf2_matrix_square(numArray2, numArray1);
          if (((int) num4 & 1) == 1)
            vec = this.gf2_matrix_times(numArray2, vec);
          num3 = num4 >> 1;
        }
        else
          break;
      }
      while (num3 != 0U);
      this._RunningCrc32Result = ~(vec ^ num1);
    }
  }
}
