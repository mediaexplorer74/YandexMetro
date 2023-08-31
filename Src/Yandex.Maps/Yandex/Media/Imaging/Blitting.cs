// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.Blitting
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Media.Imaging
{
  internal class Blitting
  {
    public static void Blit(
      int[] destPixels,
      int destWidth,
      int destHeight,
      Rect destRect,
      int[] sourcePixels,
      int sourceWidth,
      int sourceHeight,
      Rect sourceRect,
      bool blendPixels)
    {
      int width1 = (int) destRect.Width;
      int height = (int) destRect.Height;
      if (!new Rect(0.0, 0.0, (double) destWidth, (double) destHeight).Intersects(destRect))
        return;
      int length = sourcePixels.Length;
      int x1 = (int) destRect.X;
      int y1 = (int) destRect.Y;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int width2 = (int) sourceRect.Width;
      double num5 = sourceRect.Width / destRect.Width;
      double num6 = sourceRect.Height / destRect.Height;
      int x2 = (int) sourceRect.X;
      double y2 = (double) (int) sourceRect.Y;
      int num7 = y1;
      for (int index1 = 0; index1 < height; ++index1)
      {
        if (num7 >= 0 && num7 < destHeight)
        {
          double num8 = (double) x2;
          int index2 = x1 + num7 * destWidth;
          int num9 = x1;
          int sourcePixel = sourcePixels[0];
          if (!blendPixels)
          {
            int num10 = (int) num8 + (int) y2 * sourceWidth;
            int num11 = num9 < 0 ? -num9 : 0;
            int num12 = num9 + num11;
            int num13 = sourceWidth - num11;
            int num14 = num12 + num13 < destWidth ? num13 : destWidth - num12;
            if (num14 > width2)
              num14 = width2;
            if (num14 > width1)
              num14 = width1;
            Buffer.BlockCopy((Array) sourcePixels, (num10 + num11) * 4, (Array) destPixels, (index2 + num11) * 4, num14 * 4);
          }
          else
          {
            for (int index3 = 0; index3 < width1; ++index3)
            {
              if (num9 >= 0 && num9 < destWidth)
              {
                if ((int) num8 != -1 || (int) y2 != -1)
                {
                  int index4 = (int) num8 + (int) y2 * sourceWidth;
                  if (index4 >= 0 && index4 < length)
                  {
                    sourcePixel = sourcePixels[index4];
                    num4 = sourcePixel >> 24 & (int) byte.MaxValue;
                    num1 = sourcePixel >> 16 & (int) byte.MaxValue;
                    num2 = sourcePixel >> 8 & (int) byte.MaxValue;
                    num3 = sourcePixel & (int) byte.MaxValue;
                  }
                  else
                    num4 = 0;
                }
                if (num4 == (int) byte.MaxValue)
                  destPixels[index2] = sourcePixel;
                else if (num4 > 0)
                {
                  int destPixel = destPixels[index2];
                  int num15 = destPixel >> 24 & (int) byte.MaxValue;
                  if (num15 == 0)
                  {
                    destPixels[index2] = sourcePixel;
                  }
                  else
                  {
                    int num16 = destPixel >> 16 & (int) byte.MaxValue;
                    int num17 = destPixel >> 8 & (int) byte.MaxValue;
                    int num18 = destPixel & (int) byte.MaxValue;
                    int num19 = num15 + (num4 * (num4 - num15) >> 8) << 24 | num16 + (num4 * (num1 - num16) >> 8) << 16 | num17 + (num4 * (num2 - num17) >> 8) << 8 | num18 + (num4 * (num3 - num18) >> 8);
                    destPixels[index2] = num19;
                  }
                }
              }
              ++num9;
              ++index2;
              num8 += num5;
            }
          }
        }
        y2 += num6;
        ++num7;
      }
    }
  }
}
