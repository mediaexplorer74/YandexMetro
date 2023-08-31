// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileRectangleBreaker
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository
{
  internal class TileRectangleBreaker : ITileRectangleBreaker
  {
    public IEnumerable<IEnumerable<ITileInfo>> GetRectangles(IEnumerable<ITileInfo> queryTiles)
    {
      List<IEnumerable<ITileInfo>> rectangles = new List<IEnumerable<ITileInfo>>();
      List<ITileInfo> list = queryTiles.ToList<ITileInfo>();
      switch (list.Count<ITileInfo>())
      {
        case 0:
          return (IEnumerable<IEnumerable<ITileInfo>>) rectangles;
        case 1:
          rectangles.Add(queryTiles);
          return (IEnumerable<IEnumerable<ITileInfo>>) rectangles;
        default:
          int num1 = list.Select<ITileInfo, int>((Func<ITileInfo, int>) (t => t.X)).Min();
          int num2 = list.Select<ITileInfo, int>((Func<ITileInfo, int>) (t => t.Y)).Min();
          int num3 = list.Select<ITileInfo, int>((Func<ITileInfo, int>) (t => t.X)).Max();
          int num4 = list.Select<ITileInfo, int>((Func<ITileInfo, int>) (t => t.Y)).Max();
          int num5 = num3 - num1 + 1;
          int num6 = num4 - num2 + 1;
          Dictionary<int, Dictionary<int, ITileInfo>> dictionary = new Dictionary<int, Dictionary<int, ITileInfo>>();
          foreach (ITileInfo tileInfo in list)
          {
            if (!dictionary.ContainsKey(tileInfo.Y - num2))
              dictionary[tileInfo.Y - num2] = new Dictionary<int, ITileInfo>();
            dictionary[tileInfo.Y - num2][tileInfo.X - num1] = tileInfo;
          }
          while (list.Count > 0)
          {
            List<ITileInfo> tileInfoList = new List<ITileInfo>();
            ITileInfo tileInfo1 = list.First<ITileInfo>();
            int num7 = tileInfo1.X - num1;
            int key1 = tileInfo1.Y - num2;
            int num8 = num5;
            bool flag = true;
            List<ITileInfo> collection = new List<ITileInfo>();
            for (; key1 < num6 && dictionary.ContainsKey(key1); ++key1)
            {
              int key2;
              for (key2 = num7; key2 < num8 && dictionary[key1].ContainsKey(key2); ++key2)
              {
                ITileInfo tileInfo2 = dictionary[key1][key2];
                if (tileInfo2 != null)
                {
                  if (flag)
                  {
                    list.Remove(tileInfo2);
                    tileInfoList.Add(tileInfo2);
                  }
                  else
                    collection.Add(tileInfo2);
                }
                else
                  break;
              }
              if (flag)
              {
                num8 = key2;
                flag = false;
              }
              else if (key2 == num8)
              {
                foreach (ITileInfo tileInfo3 in collection)
                  list.Remove(tileInfo3);
                tileInfoList.AddRange((IEnumerable<ITileInfo>) collection);
              }
              collection.Clear();
            }
            rectangles.Add((IEnumerable<ITileInfo>) tileInfoList);
          }
          return (IEnumerable<IEnumerable<ITileInfo>>) rectangles;
      }
    }
  }
}
