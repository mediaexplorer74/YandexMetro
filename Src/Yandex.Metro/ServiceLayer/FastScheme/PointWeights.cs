// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.PointWeights
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Runtime.Serialization;
using Yandex.Metro.Logic.FastScheme;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public struct PointWeights
  {
    [DataMember]
    public int Time;
    [DataMember]
    public int Transfer;

    public PointWeights Copy() => new PointWeights()
    {
      Time = this.Time,
      Transfer = this.Transfer
    };

    internal int Sorted(GRWeightType[] sort)
    {
      int num1 = 0;
      int num2 = 1;
      for (int index = sort.Length - 1; index > -1; --index)
      {
        int num3 = this.weightWithType(sort[index]);
        num1 += num3 * num2;
        num2 *= 10000;
      }
      return num1;
    }

    public int weightWithType(GRWeightType type)
    {
      int num = 0;
      switch (type)
      {
        case GRWeightType.GRWeightTypeTime:
          num = this.Time;
          break;
        case GRWeightType.GRWeightTypeTimeInMinutes:
          double d = (double) this.Time / 60.0;
          num = (int) Math.Round(Math.Floor((d < Math.Floor(d) + 0.1 ? Math.Floor(d) : Math.Floor(d + 1.0)) + 0.1) * 60.0);
          break;
        case GRWeightType.GRWeightTypeTransfer:
          num = this.Transfer;
          break;
      }
      return num;
    }
  }
}
