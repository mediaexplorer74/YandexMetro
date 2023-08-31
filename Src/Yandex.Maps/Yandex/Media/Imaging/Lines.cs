// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.Lines
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Media.Imaging
{
  internal static class Lines
  {
    private const double RoughCeiling = 0.99999999;

    public static void DrawSolidLine(
      int[] pixels,
      int pixelWidth,
      int pixelHeight,
      double x1,
      double y1,
      double x2,
      double y2,
      uint intColor,
      double thickness)
    {
      double num1 = x2 - x1;
      double num2 = y2 - y1;
      if (num1 == 0.0 && num2 == 0.0)
        return;
      double num3 = 0.0;
      double num4 = 0.0;
      if (thickness > 1.0)
      {
        double num5 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2);
        double num6 = num2 * num5;
        num3 = num1 * num5 * 0.5 * thickness;
        num4 = -num6 * 0.5 * thickness;
      }
      double num7 = x1 - num4;
      double num8 = y1 - num3;
      double num9 = x1 + num4;
      double num10 = y1 + num3;
      double num11 = x2 - num4;
      double num12 = y2 - num3;
      double num13 = x2 + num4;
      double num14 = y2 + num3;
      Polygons.DrawFilledPolygon(pixels, pixelWidth, pixelHeight, new double[10]
      {
        num7,
        num8,
        num9,
        num10,
        num13,
        num14,
        num11,
        num12,
        num7,
        num8
      }, intColor);
    }

    public static void DrawLineWu(
      int[] pixels,
      int pixelWidth,
      int pixelHeight,
      double x1,
      double y1,
      double x2,
      double y2,
      uint intColor,
      double thickness)
    {
      double num1 = x2 - x1;
      double num2 = y2 - y1;
      if (num1 == 0.0 && num2 == 0.0)
        return;
      uint sa = intColor >> 24;
      uint sg = intColor >> 8 & (uint) byte.MaxValue;
      uint srb = intColor & 16711935U;
      bool flag1 = Math.Abs(num1) < Math.Abs(num2);
      if (flag1)
      {
        double num3 = x1;
        x1 = y1;
        y1 = num3;
        double num4 = x2;
        x2 = y2;
        y2 = num4;
        double num5 = num1;
        num1 = num2;
        num2 = num5;
      }
      if (x2 < x1)
      {
        double num6 = x1;
        x1 = x2;
        x2 = num6;
        double num7 = y1;
        y1 = y2;
        y2 = num7;
        num1 = -num1;
        num2 = -num2;
      }
      double num8 = num2 / num1;
      double num9 = 0.0;
      double num10 = 0.0;
      int length = pixels.Length;
      if (thickness > 1.0)
      {
        double num11 = 1.0 / Math.Sqrt(num1 * num1 + num2 * num2);
        double num12 = num2 * num11;
        num9 = num1 * num11 * 0.5 * thickness;
        num10 = -num12 * 0.5 * thickness;
      }
      double val1_1 = x1 - num10;
      double num13 = y1 - num9;
      double val2_1 = x1 + num10;
      double num14 = y1 + num9;
      double val1_2 = x2 - num10;
      double num15 = y2 - num9;
      double val2_2 = x2 + num10;
      double num16 = y2 + num9;
      double num17 = -num1 / num2;
      bool flag2 = num17 >= 0.0;
      double num18 = Math.Min(val1_1, val2_1);
      double num19 = Math.Max(val1_2, val2_2);
      int num20 = (int) (num18 + 0.99999999);
      double num21 = num13 + num8 * ((double) num20 - val1_1) + 1.0;
      double num22 = num14 + num8 * ((double) num20 - val2_1);
      double num23 = Math.Max(val1_1, val2_1);
      double num24 = Math.Min(val1_2, val2_2);
      int num25 = (int) (val1_1 + 0.99999999);
      double d1 = num13 + num8 * ((double) num25 - val1_1);
      int num26 = (int) Math.Floor(d1);
      int num27 = (int) (val2_1 + 0.99999999);
      double d2 = num14 + num8 * ((double) num27 - val2_1) + 1.0;
      int num28 = (int) Math.Floor(d2);
      int num29 = (int) (num13 + 0.99999999);
      double d3 = val1_1 - num8 * ((double) num29 - num13);
      int num30 = (int) Math.Floor(d3);
      int num31 = (int) (num15 + 0.99999999);
      double d4 = val1_2 - num8 * ((double) num31 - num15) + 1.0;
      int num32 = (int) Math.Floor(d4);
      if (!flag1)
      {
        double num33 = num13 + num17 * ((double) num20 - val1_1);
        double num34 = num15 + num17 * ((double) num20 - val1_2);
        int num35 = Math.Min((int) (num19 + 0.99999999), pixelWidth);
        for (int index1 = num20; index1 < num35; ++index1)
        {
          if (index1 >= 0)
          {
            int index2 = index1 + pixelWidth * (int) num21;
            int num36 = Math.Min((int) num22 + 1, pixelHeight);
            for (int index3 = (int) num21; index3 < num36; ++index3)
            {
              if (index3 >= 0 && ((double) index1 >= num23 || (!flag2 || (double) index3 <= num33) && (flag2 || (double) index3 >= num33)) && ((double) index1 <= num24 || (!flag2 || (double) index3 >= num34) && (flag2 || (double) index3 <= num34)))
                pixels[index2] = (int) intColor;
              index2 += pixelWidth;
            }
          }
          num21 += num8;
          num22 += num8;
          num33 += num17;
          num34 += num17;
        }
        for (int y = num29; (double) y < num14; ++y)
        {
          Lines.Plot(num30, y, pixels, sa, sg, srb, 1.0 - d3 + (double) num30, pixelWidth, pixelHeight);
          d3 -= num8;
          num30 = (int) Math.Floor(d3);
        }
        for (int y = num31; (double) y < num16; ++y)
        {
          Lines.Plot(num32, y, pixels, sa, sg, srb, d4 - (double) num32, pixelWidth, pixelHeight);
          d4 -= num8;
          num32 = (int) Math.Floor(d4);
        }
        Lines.Plot(num25 - 1, num26, pixels, sa, sg, srb, (1.0 - d1 + (double) num26) * ((double) num25 - val1_1), pixelWidth, pixelHeight);
        int x3;
        for (x3 = num25; (double) x3 < val1_2; ++x3)
        {
          Lines.Plot(x3, num26, pixels, sa, sg, srb, 1.0 - d1 + (double) num26, pixelWidth, pixelHeight);
          d1 += num8;
          num26 = (int) Math.Floor(d1);
        }
        Lines.Plot(x3, num26, pixels, sa, sg, srb, (1.0 - d1 + (double) num26) * ((double) (1 - x3) + val1_2), pixelWidth, pixelHeight);
        Lines.Plot(num27 - 1, num28, pixels, sa, sg, srb, (d2 - (double) num28) * ((double) num27 - val2_1), pixelWidth, pixelHeight);
        int x4;
        for (x4 = num27; (double) x4 < val2_2; ++x4)
        {
          Lines.Plot(x4, num28, pixels, sa, sg, srb, d2 - (double) num28, pixelWidth, pixelHeight);
          d2 += num8;
          num28 = (int) Math.Floor(d2);
        }
        Lines.Plot(x4, num28, pixels, sa, sg, srb, (d2 - (double) num28) * ((double) (1 - x4) + val2_2), pixelWidth, pixelHeight);
      }
      else
      {
        double num37 = num13 + num17 * ((double) num20 - val1_1);
        double num38 = num15 + num17 * ((double) num20 - val1_2);
        int num39 = num20 * pixelWidth;
        for (int index4 = num20; (double) index4 < num19; ++index4)
        {
          if (index4 >= 0 && index4 <= pixelHeight)
          {
            int num40 = Math.Max(num39 + (int) num21, 0);
            int num41 = Math.Min(num39 + (int) num22 + 1, length);
            int num42 = num40 - num39;
            for (int index5 = num40; index5 < num41; ++index5)
            {
              if (num42 >= 0 && num42 < pixelWidth && ((double) index4 >= num23 || (!flag2 || (double) num42 <= num37) && (flag2 || (double) num42 >= num37)) && ((double) index4 <= num24 || (!flag2 || (double) num42 >= num38) && (flag2 || (double) num42 <= num38)))
                pixels[index5] = (int) intColor;
              ++num42;
            }
          }
          num21 += num8;
          num22 += num8;
          num37 += num17;
          num38 += num17;
          num39 += pixelWidth;
        }
        for (int x = num29; (double) x < num14; ++x)
        {
          Lines.Plot(x, num30, pixels, sa, sg, srb, 1.0 - d3 + (double) num30, pixelWidth, pixelHeight);
          d3 -= num8;
          num30 = (int) Math.Floor(d3);
        }
        for (int x = num31; (double) x < num16; ++x)
        {
          Lines.Plot(x, num32, pixels, sa, sg, srb, d4 - (double) num32, pixelWidth, pixelHeight);
          d4 -= num8;
          num32 = (int) Math.Floor(d4);
        }
        Lines.Plot(num26, num25 - 1, pixels, sa, sg, srb, (1.0 - d1 + (double) num26) * ((double) num25 - val1_1), pixelWidth, pixelHeight);
        int y3;
        for (y3 = num25; (double) y3 <= val1_2; ++y3)
        {
          Lines.Plot(num26, y3, pixels, sa, sg, srb, 1.0 - d1 + (double) num26, pixelWidth, pixelHeight);
          d1 += num8;
          num26 = (int) Math.Floor(d1);
        }
        Lines.Plot(num26, y3, pixels, sa, sg, srb, (1.0 - d1 + (double) num26) * ((double) (1 - y3) + val1_2), pixelWidth, pixelHeight);
        Lines.Plot(num28, num27 - 1, pixels, sa, sg, srb, (d2 - (double) num28) * ((double) num27 - val2_1), pixelWidth, pixelHeight);
        int y4;
        for (y4 = num27; (double) y4 <= val2_2; ++y4)
        {
          Lines.Plot(num28, y4, pixels, sa, sg, srb, d2 - (double) num28, pixelWidth, pixelHeight);
          d2 += num8;
          num28 = (int) Math.Floor(d2);
        }
        Lines.Plot(num28, y4, pixels, sa, sg, srb, (d2 - (double) num28) * ((double) (1 - y4) + val2_2), pixelWidth, pixelHeight);
      }
    }

    private static void Plot(
      int x,
      int y,
      int[] pixels,
      uint sa,
      uint sg,
      uint srb,
      double brightness,
      int pixelWidth,
      int pixelHeight)
    {
      if (x < 0 || x >= pixelWidth || y < 0 || y >= pixelHeight)
        return;
      int index = y * pixelWidth + x;
      uint pixel = (uint) pixels[index];
      sa = (uint) ((double) sa * brightness);
      uint num1 = pixel >> 24;
      uint num2 = pixel >> 8 & (uint) byte.MaxValue;
      uint num3 = pixel & 16711935U;
      pixels[index] = (int) sa + ((int) num1 * ((int) byte.MaxValue - (int) sa) * 32897 >>> 23) << 24 | ((int) sg - (int) num2) * (int) sa + ((int) num2 << 8) & 65280 | (int) ((srb - num3) * sa >> 8) + (int) num3 & 16711935;
    }

    public static void Plot(
      int index,
      int[] pixels,
      uint sa,
      uint sg,
      uint srb,
      double brightness)
    {
      if (index < 0 || index >= pixels.Length)
        return;
      uint pixel = (uint) pixels[index];
      sa = (uint) ((double) sa * brightness);
      uint num1 = pixel >> 24;
      uint num2 = pixel >> 8 & (uint) byte.MaxValue;
      uint num3 = pixel & 16711935U;
      pixels[index] = (int) sa + ((int) num1 * ((int) byte.MaxValue - (int) sa) * 32897 >>> 23) << 24 | ((int) sg - (int) num2) * (int) sa + ((int) num2 << 8) & 65280 | (int) ((srb - num3) * sa >> 8) + (int) num3 & 16711935;
    }

    public static void DrawCircle(
      double radius,
      double centerX,
      double centerY,
      int[] pixels,
      uint colorInt,
      int pixelWidth,
      int pixelHeight)
    {
      centerX += 0.5;
      centerY += 0.5;
      int num1 = (int) (radius + 0.99999999);
      double num2 = radius * radius;
      for (int index1 = -num1; index1 <= num1; ++index1)
      {
        int num3 = index1 * index1;
        for (int index2 = -num1; index2 <= num1; ++index2)
        {
          if ((double) (index2 * index2 + num3) <= num2)
          {
            int num4 = (int) ((double) index2 + centerX);
            int num5 = (int) ((double) index1 + centerY);
            if (num4 >= 0 && num4 < pixelWidth && num5 >= 0 && num5 < pixelHeight)
            {
              int index3 = num5 * pixelWidth + num4;
              pixels[index3] = (int) colorInt;
            }
          }
        }
      }
    }
  }
}
