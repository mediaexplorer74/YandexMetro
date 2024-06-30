// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.RouteHelper
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Y.Metro.ServiceLayer.FastScheme;
using Yandex.Metro.Logic.FastScheme;

namespace Yandex.Metro.Logic
{
  public static class RouteHelper
  {
    private const int MaxVertexLabel = 2147483646;

    public static string CurrentRouteAsString { get; set; }

    public static List<Route> ConstructRoutes(MetroStation startStation, MetroStation endStation)
    {
      List<Route> routeList = new List<Route>();
      List<MetroRoute> metroRouteList = RouteHelper.ConstructRoutes(startStation.Id, endStation.Id);
      int num = 1;
      foreach (MetroRoute shortestRoute in metroRouteList)
      {
        Route routeObject = RouteHelper.ConvertToRouteObject(shortestRoute);
        routeObject.RouteNumber = num++;
        routeList.Add(routeObject);
      }
      return routeList;
    }

    private static List<MetroRoute> ConstructRoutes(int startStation, int endStation)
    {
      RouteHelper.CurrentRouteAsString = string.Format("startId: {0}, endId: {1}, city: {2}, lang: {3}", (object) startStation, (object) endStation, (object) FastKeeper.Scheme.CityId, (object) FastKeeper.Scheme.Language);
      List<MetroRoute> source = new List<MetroRoute>();
      RouteHelper.CheckIfStationExists(endStation);
      RouteHelper.CheckIfStationExists(startStation);
      GRWeightType[] grWeightTypeArray = new GRWeightType[2]
      {
        GRWeightType.GRWeightTypeTime,
        GRWeightType.GRWeightTypeTransfer
      };
      MetroRoute nodeRoute = RouteHelper.MakeRouteFromPoint(FastKeeper.Scheme.SuperGraph, startStation, endStation, grWeightTypeArray);
      source.Add(nodeRoute);
      MetroRoute metroRoute = (MetroRoute) null;
      if (nodeRoute.Weights.Transfer > 0)
      {
        if (nodeRoute.Weights.Transfer > 1)
        {
          GRWeightType[] sort = new GRWeightType[2]
          {
            GRWeightType.GRWeightTypeTransfer,
            GRWeightType.GRWeightTypeTime
          };
          metroRoute = RouteHelper.MakeRouteFromPoint(FastKeeper.Scheme.SuperGraph, startStation, endStation, sort);
        }
        int num1 = RouteHelper.RealNumberOfTransfers(nodeRoute);
        int time1 = nodeRoute.Weights.Time;
        while (nodeRoute != null && source.Count < 10)
        {
          int time2 = nodeRoute.Weights.Time;
          nodeRoute = RouteHelper.FindAnotherRouteFromRoute(nodeRoute, grWeightTypeArray, startStation, endStation, FastKeeper.Scheme.SuperGraph);
          int num2 = RouteHelper.RealNumberOfTransfers(nodeRoute);
          if (nodeRoute == null || num2 > num1 || (double) nodeRoute.Weights.Time > (double) time1 * 1.25)
          {
            if (nodeRoute == null || nodeRoute.Weights.Time != time2)
              break;
          }
          else
            source.Add(nodeRoute);
        }
        if (metroRoute != null && metroRoute.Weights.Time > source.Last<MetroRoute>().Weights.Time)
          source.Add(metroRoute);
      }
      foreach (KeyValuePair<int, int> removedEdge in FastKeeper.Scheme.RemovedEdges)
      {
        FastKeeper.Scheme.SuperGraph.AdjacentEdges[removedEdge.Key].Add(removedEdge.Value);
        FastKeeper.Scheme.SuperGraph.AdjacentEdges[removedEdge.Value].Add(removedEdge.Key);
      }
      FastKeeper.Scheme.RemovedEdges.Clear();
      foreach (KeyValuePair<int, int> addedEdge in FastKeeper.Scheme.AddedEdges)
      {
        if (FastKeeper.Scheme.SuperGraph.AdjacentEdges.ContainsKey(addedEdge.Key))
          FastKeeper.Scheme.SuperGraph.AdjacentEdges[addedEdge.Key].Remove(addedEdge.Value);
        if (FastKeeper.Scheme.SuperGraph.AdjacentEdges.ContainsKey(addedEdge.Value))
          FastKeeper.Scheme.SuperGraph.AdjacentEdges[addedEdge.Value].Remove(addedEdge.Key);
        FastKeeper.Scheme.SuperGraph.Remove(addedEdge.Key, addedEdge.Value);
        int hash = IntPair.Hash(addedEdge.Key, addedEdge.Value);
        GroupEdge groupEdge = FastKeeper.Scheme.RawSuperEdges.FirstOrDefault<GroupEdge>((Func<GroupEdge, bool>) (s => s.Hash == hash));
        FastKeeper.Scheme.RawSuperEdges.Remove(groupEdge);
      }
      FastKeeper.Scheme.AddedEdges.Clear();
      foreach (int addedNode in FastKeeper.Scheme.AddedNodes)
      {
        int i = addedNode;
        MetroStation metroStation = FastKeeper.Scheme.SuperGraph.Nodes.First<MetroStation>((Func<MetroStation, bool>) (s => s.Id == i));
        FastKeeper.Scheme.SuperGraph.Nodes.Remove(metroStation);
        FastKeeper.Scheme.SuperGraph.AdjacentEdges.Remove(i);
      }
      FastKeeper.Scheme.AddedNodes.Clear();
      foreach (GroupEdge removedSuperEdge in FastKeeper.Scheme.RemovedSuperEdges)
      {
        if (!removedSuperEdge.IsFake)
          FastKeeper.Scheme.RawSuperEdges.Add(removedSuperEdge);
      }
      FastKeeper.Scheme.RemovedSuperEdges.Clear();
      return source;
    }

    private static MetroRoute FindAnotherRouteFromRoute(
      MetroRoute nodeRoute,
      GRWeightType[] defaultSort,
      int startStation,
      int endStation,
      Graph superGraph)
    {
      int count = nodeRoute.Stations.Count;
      Dictionary<KeyValuePair<int, int>, MetroRoute> dictionary = new Dictionary<KeyValuePair<int, int>, MetroRoute>();
      for (int index = 1; index < count; ++index)
      {
        int station1 = nodeRoute.Stations[index];
        int station2 = nodeRoute.Stations[index - 1];
        if (superGraph[station2, station1].Transfer <= 0)
        {
          superGraph.AdjacentEdges[station2].Remove(station1);
          superGraph.AdjacentEdges[station1].Remove(station2);
          MetroRoute metroRoute = RouteHelper.MakeRouteFromPoint(superGraph, startStation, endStation, defaultSort);
          if (metroRoute != null)
            dictionary.Add(new KeyValuePair<int, int>(station2, station1), metroRoute);
          superGraph.AdjacentEdges[station2].Add(station1);
          superGraph.AdjacentEdges[station1].Add(station2);
        }
      }
      MetroRoute anotherRouteFromRoute = (MetroRoute) null;
      KeyValuePair<int, int> keyValuePair1 = new KeyValuePair<int, int>();
      int num1 = int.MaxValue;
      foreach (KeyValuePair<KeyValuePair<int, int>, MetroRoute> keyValuePair2 in dictionary)
      {
        MetroRoute metroRoute = keyValuePair2.Value;
        int num2 = metroRoute.Weights.Sorted(defaultSort);
        if (num2 < num1)
        {
          anotherRouteFromRoute = metroRoute;
          keyValuePair1 = keyValuePair2.Key;
          num1 = num2;
        }
      }
      if (anotherRouteFromRoute != null)
      {
        superGraph.AdjacentEdges[keyValuePair1.Key].Remove(keyValuePair1.Value);
        superGraph.AdjacentEdges[keyValuePair1.Value].Remove(keyValuePair1.Key);
        FastKeeper.Scheme.RemovedEdges.Add(keyValuePair1);
      }
      return anotherRouteFromRoute;
    }

    private static int RealNumberOfTransfers(MetroRoute nodeRoute)
    {
      if (nodeRoute == null)
        return int.MaxValue;
      int transfer = nodeRoute.Weights.Transfer;
      if (transfer > 0 && FastKeeper.Scheme.SuperGraph[nodeRoute.Stations[0], nodeRoute.Stations[1]].Transfer > 0)
        --transfer;
      if (transfer > 0 && FastKeeper.Scheme.SuperGraph[nodeRoute.Stations[nodeRoute.Stations.Count - 1], nodeRoute.Stations[nodeRoute.Stations.Count - 2]].Transfer > 0)
        --transfer;
      int index = 0;
      bool flag = false;
      if (transfer > 0)
      {
        for (; index + 1 < nodeRoute.Stations.Count; ++index)
        {
          if (FastKeeper.Scheme.SuperGraph[nodeRoute.Stations[index], nodeRoute.Stations[index + 1]].Transfer > 0)
          {
            if (flag)
              --transfer;
            else
              flag = true;
          }
          else
            flag = false;
        }
      }
      return transfer;
    }

    private static MetroRoute MakeRouteFromPoint(
      Graph graph,
      int startPoint,
      int endPoint,
      GRWeightType[] sort)
    {
      if (!RouteHelper.CalculatePathLengthsFromPoint(graph, startPoint, endPoint, sort))
        return (MetroRoute) null;
      List<int> source = new List<int>();
      MetroStation metroStation1 = graph.Nodes.First<MetroStation>((Func<MetroStation, bool>) (n => n.Id == endPoint));
      source.Add(endPoint);
      while (metroStation1.Id != startPoint)
      {
        foreach (int num1 in graph.AdjacentEdges[metroStation1.Id])
        {
          int next = num1;
          MetroStation metroStation2 = graph.Nodes.First<MetroStation>((Func<MetroStation, bool>) (n => n.Id == next));
          if (metroStation2.constMark >= 0)
          {
            int num2 = metroStation2.constMark + graph[metroStation1.Id, next].Sorted(sort) + metroStation1.Weights.Sorted(sort);
            if (metroStation1.constMark == num2)
            {
              source.Insert(0, next);
              metroStation1 = metroStation2;
              break;
            }
          }
        }
      }
      MetroRoute metroRoute = new MetroRoute();
      metroRoute.Stations = new List<int>();
      metroRoute.AllStations = new List<int>();
      for (int index = 0; index < source.Count - 1; ++index)
      {
        metroRoute.Stations.Add(source[index]);
        metroRoute.Weights.Time += graph[source[index], source[index + 1]].Time;
        metroRoute.Weights.Transfer += graph[source[index], source[index + 1]].Transfer;
        metroRoute.Weights.Time += FastKeeper.Scheme.Stations[source[index + 1]].Weights.Time;
        metroRoute.Weights.Transfer += FastKeeper.Scheme.Stations[source[index + 1]].Weights.Transfer;
        int edgeKey = IntPair.Hash(source[index], source[index + 1]);
        GroupEdge groupEdge = FastKeeper.Scheme.RawSuperEdges.First<GroupEdge>((Func<GroupEdge, bool>) (s => s.Hash == edgeKey));
        if (source[index] == groupEdge.SubNodes[0])
        {
          metroRoute.AllStations.AddRange((IEnumerable<int>) groupEdge.SubNodes);
        }
        else
        {
          List<int> list = groupEdge.SubNodes.ToList<int>();
          list.Reverse();
          metroRoute.AllStations.AddRange((IEnumerable<int>) list);
        }
      }
      metroRoute.Stations.Add(source.Last<int>());
      metroRoute.AllStations = metroRoute.AllStations.Distinct<int>().ToList<int>();
      return metroRoute;
    }

    private static bool CalculatePathLengthsFromPoint(
      Graph graph,
      int startPoint,
      int endPoint,
      GRWeightType[] sort)
    {
      foreach (MetroStation node in graph.Nodes)
      {
        node.tempMark = 2147483646;
        node.constMark = -1;
      }
      MetroStation metroStation1 = graph.Nodes.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (n => n.Id == startPoint));
      metroStation1.tempMark = 0;
      while (metroStation1 != null)
      {
        metroStation1.constMark = metroStation1.tempMark;
        metroStation1.tempMark = -1;
        if (metroStation1.Id != endPoint)
        {
          foreach (int num1 in graph.AdjacentEdges[metroStation1.Id])
          {
            int next = num1;
            MetroStation metroStation2 = graph.Nodes.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (n => n.Id == next));
            if (metroStation2.tempMark >= 0)
            {
              int num2 = metroStation1.constMark + graph[metroStation1.Id, next].Sorted(sort) + metroStation2.Weights.Sorted(sort);
              if (num2 < metroStation2.tempMark)
                metroStation2.tempMark = num2;
            }
          }
          metroStation1 = graph.Nodes.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (n => n.Id == endPoint));
          foreach (MetroStation node in graph.Nodes)
          {
            if (node.tempMark >= 0 && node.tempMark < metroStation1.tempMark)
              metroStation1 = node;
          }
        }
        else
          break;
      }
      return graph.Nodes.FirstOrDefault<MetroStation>((Func<MetroStation, bool>) (n => n.Id == endPoint)).constMark < 2147483646;
    }

    private static void CheckIfStationExists(int startStation)
    {
      MetroScheme scheme = FastKeeper.Scheme;
      if (FastKeeper.Scheme.SuperGraph.AdjacentEdges.Keys.ToList<int>().Contains(startStation))
        return;
      GroupEdge groupEdge1 = scheme.RawSuperEdges.First<GroupEdge>((Func<GroupEdge, bool>) (e => e.SubNodes.Contains(startStation)));
      List<int> subNodes = groupEdge1.SubNodes;
      List<int> list1 = subNodes.TakeWhile<int>((Func<int, bool>) (i => i != startStation)).ToList<int>();
      list1.Add(startStation);
      List<int> list2 = subNodes.SkipWhile<int>((Func<int, bool>) (i => i != startStation)).ToList<int>();
      int num1 = subNodes[0];
      int num2 = subNodes.Last<int>();
      GroupEdge groupEdge2 = new GroupEdge(num1, startStation)
      {
        SubNodes = list1,
        IsFake = true
      };
      GroupEdge groupEdge3 = new GroupEdge(num2, startStation)
      {
        SubNodes = list2,
        IsFake = true
      };
      for (int index = 0; index < list1.Count - 1; ++index)
        groupEdge2.Weight.Time += scheme.Graph[list1[index], list1[index + 1]].Time;
      for (int index = 0; index < list2.Count - 1; ++index)
        groupEdge3.Weight.Time += scheme.Graph[list2[index], list2[index + 1]].Time;
      scheme.RemovedEdges.Add(new KeyValuePair<int, int>(num1, num2));
      scheme.SuperGraph.AdjacentEdges[num1].Remove(num2);
      scheme.SuperGraph.AdjacentEdges[num2].Remove(num1);
      scheme.RemovedSuperEdges.Add(groupEdge1);
      scheme.RawSuperEdges.Remove(groupEdge1);
      scheme.RawSuperEdges.Add(groupEdge2);
      scheme.RawSuperEdges.Add(groupEdge3);
      scheme.SuperGraph[num1, startStation] = groupEdge2.Weight;
      scheme.SuperGraph[startStation, num2] = groupEdge3.Weight;
      scheme.SuperGraph.AdjacentEdges[startStation] = new List<int>()
      {
        num1,
        num2
      };
      scheme.SuperGraph.AdjacentEdges[num1].Add(startStation);
      scheme.SuperGraph.AdjacentEdges[num2].Add(startStation);
      scheme.SuperGraph.Nodes.Add(scheme.Stations[startStation]);
      scheme.AddedNodes.Add(startStation);
      scheme.AddedEdges.Add(new KeyValuePair<int, int>(num1, startStation));
      scheme.AddedEdges.Add(new KeyValuePair<int, int>(num2, startStation));
    }

    private static void BuildFromVertex(
      int p,
      List<int> visited,
      List<int> listSuperNodes,
      List<int> nodesForSuperEdge,
      List<GroupEdge> superEdges,
      MetroScheme scheme,
      PointWeights totalWeight,
      bool superEdgeForce = false)
    {
      visited.Add(p);
      List<int> list = scheme.Graph.AdjacentEdges[p].Where<int>((Func<int, bool>) (s => !visited.Contains(s))).OrderByDescending<int, int>((Func<int, int>) (k => scheme.Graph[p, k].Transfer)).ToList<int>();
      if (list.Count == 0)
      {
        listSuperNodes.Add(p);
        if (nodesForSuperEdge.Count <= 1)
          return;
        superEdges.Add(new GroupEdge(nodesForSuperEdge[0], nodesForSuperEdge.Last<int>())
        {
          SubNodes = nodesForSuperEdge.Copy<int>(),
          Weight = totalWeight.Copy()
        });
      }
      else
      {
        bool flag = scheme.Graph[p, list[0]].Transfer > 0;
        if ((flag || superEdgeForce) && !listSuperNodes.Contains(p))
          listSuperNodes.Add(p);
        foreach (int num in list)
        {
          PointWeights pointWeights = scheme.Graph[p, num];
          MetroStation station = scheme.Stations[num];
          if (pointWeights.Transfer > 0)
          {
            if (!listSuperNodes.Contains(num))
              listSuperNodes.Add(num);
            superEdges.Add(new GroupEdge(p, num)
            {
              SubNodes = new List<int>() { p, num },
              Weight = pointWeights
            });
            if (nodesForSuperEdge.Count > 1)
              superEdges.Add(new GroupEdge(nodesForSuperEdge[0], nodesForSuperEdge.Last<int>())
              {
                SubNodes = nodesForSuperEdge.Copy<int>(),
                Weight = totalWeight.Copy()
              });
            RouteHelper.BuildFromVertex(num, visited, listSuperNodes, new List<int>()
            {
              num
            }, superEdges, scheme, new PointWeights(), true);
          }
          else
          {
            PointWeights totalWeight1 = totalWeight.Copy();
            totalWeight1.Time += pointWeights.Time;
            if (!flag)
            {
              nodesForSuperEdge.Add(num);
              totalWeight1.Time += station.Weights.Time;
              RouteHelper.BuildFromVertex(num, visited, listSuperNodes, nodesForSuperEdge, superEdges, scheme, totalWeight1);
              nodesForSuperEdge.Remove(num);
            }
            else
              RouteHelper.BuildFromVertex(num, visited, listSuperNodes, new List<int>()
              {
                p,
                num
              }, superEdges, scheme, pointWeights.Copy());
          }
        }
      }
    }

    public static void GenerateRouteGraph(MetroScheme scheme)
    {
      scheme.Graph = new Graph();
      Dictionary<int, List<int>> allAdjacentEdges = new Dictionary<int, List<int>>();
      foreach (MetroLink link in scheme.Links)
      {
        scheme.Graph[link.To, link.From] = link.Weights;
        if (!allAdjacentEdges.ContainsKey(link.From))
          allAdjacentEdges[link.From] = new List<int>();
        if (!allAdjacentEdges.ContainsKey(link.To))
          allAdjacentEdges[link.To] = new List<int>();
        allAdjacentEdges[link.From].Add(link.To);
        allAdjacentEdges[link.To].Add(link.From);
      }
      scheme.Graph.AdjacentEdges = allAdjacentEdges;
      List<int> intList1 = new List<int>();
      scheme.Stations.First<KeyValuePair<int, MetroStation>>();
      List<int> listSuperNodes = new List<int>();
      List<GroupEdge> superEdges = new List<GroupEdge>();
      List<int> intList2 = new List<int>();
      List<int> visited = new List<int>();
      KeyValuePair<int, MetroStation> keyValuePair = scheme.Stations.First<KeyValuePair<int, MetroStation>>((Func<KeyValuePair<int, MetroStation>, bool>) (s => allAdjacentEdges[s.Key].Count == 1));
      listSuperNodes.Add(keyValuePair.Key);
      RouteHelper.BuildFromVertex(keyValuePair.Key, visited, listSuperNodes, new List<int>()
      {
        keyValuePair.Key
      }, superEdges, scheme, new PointWeights());
      Graph graph = new Graph()
      {
        AdjacentEdges = new Dictionary<int, List<int>>()
      };
      foreach (GroupEdge groupEdge in superEdges)
      {
        int num1 = groupEdge.SubNodes.First<int>();
        int num2 = groupEdge.SubNodes.Last<int>();
        graph[num1, num2] = groupEdge.Weight;
        if (!graph.AdjacentEdges.ContainsKey(num1))
          graph.AdjacentEdges[num1] = new List<int>();
        if (!graph.AdjacentEdges.ContainsKey(num2))
          graph.AdjacentEdges[num2] = new List<int>();
        graph.AdjacentEdges[num1].Add(num2);
        graph.AdjacentEdges[num2].Add(num1);
      }
      graph.Nodes = new List<MetroStation>();
      graph.MakeAdjacentNodesUnique();
      foreach (int key in listSuperNodes)
        graph.Nodes.Add(scheme.Stations[key]);
      scheme.SuperGraph = graph;
      scheme.RawSuperEdges = superEdges;
    }

    private static Route ConvertToRouteObject(MetroRoute shortestRoute)
    {
      MetroScheme mapScheme = FastKeeper.Scheme;
      int allStation1 = shortestRoute.AllStations[0];
      int key = shortestRoute.AllStations.Last<int>();
      MetroStation station1 = mapScheme.Stations[allStation1];
      MetroStation station2 = mapScheme.Stations[key];
      MetroScheme metroScheme = new MetroScheme();
      List<MetroStation> list = shortestRoute.AllStations.Select<int, MetroStation>((Func<int, MetroStation>) (routeId => mapScheme.Stations[routeId])).ToList<MetroStation>();
      Dictionary<int, MetroStation> dictionary = list.OrderBy<MetroStation, int>((Func<MetroStation, int>) (s => s.OrderId)).ToDictionary<MetroStation, int>((Func<MetroStation, int>) (s => s.Id));
      IEnumerable<int> linesIds = list.Select<MetroStation, int>((Func<MetroStation, int>) (s => s.LineId)).Distinct<int>();
      MetroLine[] array1 = ((IEnumerable<MetroLine>) mapScheme.Lines).Where<MetroLine>((Func<MetroLine, bool>) (l => linesIds.Contains<int>(l.Id))).ToArray<MetroLine>();
      int[][] array2 = ((IEnumerable<int[]>) ((IEnumerable<int[]>) mapScheme.Transfers).Where<int[]>((Func<int[], bool>) (t1 => ((IEnumerable<int>) t1).Any<int>((Func<int, bool>) (t2 => shortestRoute.AllStations.Contains(t2))))).Select<int[], int[]>((Func<int[], int[]>) (t =>
      {
        int[] array3 = ((IEnumerable<int>) t).Where<int>((Func<int, bool>) (t1 => shortestRoute.AllStations.Contains(t1))).ToArray<int>();
        if (array3.Length != 2)
          return array3;
        int first = array3[0];
        int second = array3[1];
        return !((IEnumerable<MetroLink>) mapScheme.Links).Any<MetroLink>((Func<MetroLink, bool>) (r =>
        {
          if (r.From == first && r.To == second)
            return true;
          return r.From == second && r.To == first;
        })) ? (int[]) null : array3;
      })).ToArray<int[]>()).Where<int[]>((Func<int[], bool>) (r => r != null)).OrderBy<int[], int>((Func<int[], int>) (t => ((IEnumerable<int>) t).Min<int>((Func<int, int>) (stationId => shortestRoute.AllStations.IndexOf(stationId))))).ToArray<int[]>();
      metroScheme.Transfers = array2;
      metroScheme.Stations = dictionary;
      metroScheme.Links = mapScheme.Links;
      metroScheme.Lines = array1;
      DateTime now = DateTime.Now;
      int num1 = 0;
      List<int> intList = new List<int>() { 0 };
      for (int index = 1; index < shortestRoute.AllStations.Count; ++index)
      {
        int allStation2 = shortestRoute.AllStations[index - 1];
        int allStation3 = shortestRoute.AllStations[index];
        PointWeights pointWeights = FastKeeper.Scheme.Graph[allStation2, allStation3];
        int num2 = num1 + pointWeights.Time;
        intList.Add(num2);
        DateTime dateTime = now.AddSeconds((double) num2);
        list[index].ArrivalTime = dateTime.ToString("HH:mm");
        list[index].IntervalTime = string.Format("+{0}", (object) (int) Math.Round((double) num2 / 60.0));
        num1 = num2 + FastKeeper.Scheme.Stations[allStation3].Weights.Time;
      }
      return new Route()
      {
        SortStations = list,
        EstimatedDuration = shortestRoute.Weights.Time,
        StartStation = station1,
        EndStation = station2,
        RouteScheme = metroScheme,
        Timings = intList
      };
    }
  }
}
