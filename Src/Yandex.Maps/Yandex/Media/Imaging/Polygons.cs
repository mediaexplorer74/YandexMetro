// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.Polygons
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Media.Imaging
{
  internal class Polygons
  {
    public static void DrawFilledPolygon(int[] pixels, int w, int h, double[] points, uint color)
    {
      int length1 = points.Length;
      int length2 = points.Length >> 1;
      double[] numArray1 = new double[length2];
      double[] numArray2 = new double[length2];
      double num1 = (double) w;
      double num2 = 0.0;
      double a = (double) h;
      double num3 = 0.0;
      for (int index = 0; index < points.Length; index += 2)
      {
        double point1 = points[index];
        if (point1 < num1)
          num1 = point1;
        if (point1 > num2)
          num2 = point1;
        double point2 = points[index + 1];
        if (point2 < a)
          a = point2;
        if (point2 > num3)
          num3 = point2;
      }
      if (num1 < 0.0)
        num1 = 0.0;
      if (num2 >= (double) w)
        num2 = (double) (w - 1);
      if (a < 0.0)
        a = 0.0;
      if (num3 >= (double) h)
        num3 = (double) (h - 1);
      uint sa = color >> 24;
      uint sg = color >> 8 & (uint) byte.MaxValue;
      uint srb = color & 16711935U;
      double num4 = points[0];
      double num5 = points[1];
      int num6 = 0;
      for (int index = 2; index < length1; index += 2)
      {
        double point3 = points[index];
        double point4 = points[index + 1];
        numArray2[num6++] = (point3 - num4) / (point4 - num5);
        num4 = point3;
        num5 = point4;
      }
      for (int index1 = (int) Math.Ceiling(a); (double) index1 <= num3; ++index1)
      {
        int index2 = 0;
        int index3 = 0;
        double[] numArray3 = new double[2];
        for (int index4 = 2; index4 < length1; index4 += 2)
        {
          double point5 = points[index4];
          double point6 = points[index4 + 1];
          if (num5 < (double) index1 && point6 >= (double) index1 || point6 < (double) index1 && num5 >= (double) index1)
          {
            numArray3[index2] = numArray2[index3];
            numArray1[index2++] = num4 + ((double) index1 - num5) * numArray2[index3];
          }
          num4 = point5;
          num5 = point6;
          ++index3;
        }
        for (int index5 = 1; index5 < index2; ++index5)
        {
          double num7 = numArray1[index5];
          double num8 = numArray3[index5];
          int index6;
          for (index6 = index5; index6 > 0 && numArray1[index6 - 1] > num7; --index6)
          {
            numArray1[index6] = numArray1[index6 - 1];
            numArray3[index6] = numArray3[index6 - 1];
          }
          numArray1[index6] = num7;
          numArray3[index6] = num8;
        }
        for (int index7 = 0; index7 < index2 - 1; index7 += 2)
        {
          int num9 = (int) Math.Ceiling(numArray1[index7]);
          double num10 = numArray1[index7 + 1];
          double num11 = numArray3[index7];
          double num12 = numArray3[index7 + 1];
          if (num10 > 0.0 && num9 < w)
          {
            if (num9 < 0)
              num9 = 0;
            if (num10 >= (double) w)
              num10 = (double) (w - 1);
            double num13 = 1.0 / num11;
            int num14 = num9 - 1;
            for (double brightness = 1.0 - Math.Abs((double) (num14 - num9) * num13); brightness > 0.0 && (double) num14 >= num1; brightness = 1.0 - Math.Abs((double) (num14 - num9) * num13))
            {
              Lines.Plot(index1 * w + num14, pixels, sa, sg, srb, brightness);
              --num14;
            }
            if (num14 >= 0)
              Lines.Plot(index1 * w + num9 - 1, pixels, sa, sg, srb, (double) num9 - numArray1[0]);
            int num15;
            for (num15 = num9; (double) num15 < num10; ++num15)
              pixels[index1 * w + num15] = (int) color;
            if (num15 < w)
              Lines.Plot(index1 * w + num15, pixels, sa, sg, srb, 1.0 - (double) num15 + num10);
            double num16 = 1.0 / num12;
            for (double brightness = 1.0 - Math.Abs(((double) num15 - num10) * num16); brightness > 0.0 && (double) num15 < num2; brightness = 1.0 - Math.Abs(((double) num15 - num10) * num16))
            {
              Lines.Plot(index1 * w + num15, pixels, sa, sg, srb, brightness);
              ++num15;
            }
          }
        }
      }
    }

    public static void DrawTriangle(int[] pixels, int w, int h, double[] points, uint color)
    {
      double num1 = (double) w;
      double num2 = 0.0;
      double a = (double) h;
      double num3 = 0.0;
      for (int index = 0; index < points.Length; index += 2)
      {
        double point1 = points[index];
        if (point1 < num1)
          num1 = point1;
        if (point1 > num2)
          num2 = point1;
        double point2 = points[index + 1];
        if (point2 < a)
          a = point2;
        if (point2 > num3)
          num3 = point2;
      }
      if (num1 < 0.0)
        num1 = 0.0;
      if (num2 >= (double) w)
        num2 = (double) (w - 1);
      if (a < 0.0)
        a = 0.0;
      if (num3 >= (double) h)
        num3 = (double) (h - 1);
      double[] numArray1 = new double[8];
      for (int index = 0; index < 6; ++index)
        numArray1[index] = points[index];
      numArray1[6] = points[0];
      numArray1[7] = points[1];
      int length1 = numArray1.Length;
      int length2 = numArray1.Length >> 1;
      double[] numArray2 = new double[length2];
      double[] numArray3 = new double[length2];
      double[] numArray4 = new double[length2];
      uint sa = color >> 24;
      uint sg = color >> 8 & (uint) byte.MaxValue;
      uint srb = color & 16711935U;
      double num4 = numArray1[0];
      double num5 = numArray1[1];
      int num6 = 0;
      for (int index = 2; index < length1; index += 2)
      {
        double num7 = numArray1[index];
        double num8 = numArray1[index + 1];
        numArray3[num6++] = (num7 - num4) / (num8 - num5);
        num4 = num7;
        num5 = num8;
      }
      for (int index1 = (int) Math.Ceiling(a); (double) index1 <= num3; ++index1)
      {
        int index2 = 0;
        int index3 = 0;
        for (int index4 = 2; index4 < length1; index4 += 2)
        {
          double num9 = numArray1[index4];
          double num10 = numArray1[index4 + 1];
          if (num5 < (double) index1 && num10 >= (double) index1 || num10 < (double) index1 && num5 >= (double) index1)
          {
            numArray4[index2] = numArray3[index3];
            numArray2[index2++] = num4 + ((double) index1 - num5) * numArray3[index3];
          }
          num4 = num9;
          num5 = num10;
          ++index3;
        }
        double num11 = numArray2[0];
        double num12;
        double num13;
        if (num11 > numArray2[1])
        {
          numArray2[0] = numArray2[1];
          numArray2[1] = num11;
          num12 = numArray4[1];
          num13 = numArray4[0];
        }
        else
        {
          num12 = numArray4[0];
          num13 = numArray4[1];
        }
        int num14 = (int) Math.Ceiling(numArray2[0]);
        double num15 = numArray2[1];
        if (num15 > 0.0 && num14 < w)
        {
          if (num14 < 0)
            num14 = 0;
          if (num15 >= (double) w)
            num15 = (double) (w - 1);
          double num16 = 1.0 / num12;
          int num17 = num14 - 1;
          for (double brightness = 1.0 - Math.Abs((double) (num17 - num14) * num16); brightness > 0.0 && (double) num17 >= num1; brightness = 1.0 - Math.Abs((double) (num17 - num14) * num16))
          {
            Lines.Plot(index1 * w + num17, pixels, sa, sg, srb, brightness);
            --num17;
          }
          if (num17 > 0)
            Lines.Plot(index1 * w + num14 - 1, pixels, sa, sg, srb, (double) num14 - numArray2[0]);
          int num18;
          for (num18 = num14; (double) num18 < num15; ++num18)
            pixels[index1 * w + num18] = (int) color;
          if (num18 < w)
            Lines.Plot(index1 * w + num18, pixels, sa, sg, srb, 1.0 - (double) num18 + num15);
          double num19 = 1.0 / num13;
          for (double brightness = 1.0 - Math.Abs(((double) num18 - num15) * num19); brightness > 0.0 && (double) num18 < num2; brightness = 1.0 - Math.Abs(((double) num18 - num15) * num19))
          {
            Lines.Plot(index1 * w + num18, pixels, sa, sg, srb, brightness);
            ++num18;
          }
        }
      }
    }
  }
}
