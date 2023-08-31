// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.SchemeToFastSchemeConverter
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Yandex.Metro.Logic;

namespace Y.Metro.ServiceLayer.FastScheme
{
  public static class SchemeToFastSchemeConverter
  {
    public static MetroScheme Convert(schemepack scheme)
    {
      schemepackScheme schemepackScheme = scheme.scheme[0];
      MetroScheme scheme1 = new MetroScheme();
      scheme1.CityId = scheme.id;
      scheme1.Language = schemepackScheme.locale;
      scheme1.Version = scheme.ver;
      schemepackSchemeLinesLine[] lines = schemepackScheme.lines;
      int length1 = lines.Length;
      MetroLine[] metroLineArray = scheme1.Lines = new MetroLine[length1];
      for (int index = 0; index < length1; ++index)
      {
        schemepackSchemeLinesLine schemepackSchemeLinesLine = lines[index];
        MetroLine metroLine = new MetroLine()
        {
          Id = schemepackSchemeLinesLine.id,
          Title = schemepackSchemeLinesLine.name
        };
        string color = schemepackSchemeLinesLine.color;
        byte num1 = byte.Parse(color.Substring(1, 2), NumberStyles.HexNumber);
        byte num2 = byte.Parse(color.Substring(3, 2), NumberStyles.HexNumber);
        byte num3 = byte.Parse(color.Substring(5, 2), NumberStyles.HexNumber);
        metroLine.Color = new MetroColor()
        {
          R = num1,
          G = num2,
          B = num3
        };
        metroLineArray[index] = metroLine;
      }
      schemepackSchemeStationsStation[] stations = schemepackScheme.stations;
      int length2 = stations.Length;
      Dictionary<int, MetroStation> dictionary = new Dictionary<int, MetroStation>();
      for (int index1 = 0; index1 < length2; ++index1)
      {
        schemepackSchemeStationsStation station = stations[index1];
        MetroStation metroStation = new MetroStation()
        {
          OrderId = index1
        };
        int key = metroStation.Id = station.id;
        metroStation.LineId = station.line;
        metroStation.IsTransfer = station.type == "transfer";
        if (station.geoCoordinates != null)
        {
          schemepackSchemeStationsStationGeoCoordinates geoCoordinates = station.geoCoordinates;
          metroStation.Coordinates = new GeoPoint(geoCoordinates.latitude, geoCoordinates.longitude);
        }
        schemepackSchemeStationsStationName stationsStationName = station.name.sameAsForStation == null ? station.name : ((IEnumerable<schemepackSchemeStationsStation>) stations).Where<schemepackSchemeStationsStation>((Func<schemepackSchemeStationsStation, bool>) (st => st.id == int.Parse(station.name.sameAsForStation))).First<schemepackSchemeStationsStation>().name;
        MetroStationName metroStationName = new MetroStationName();
        metroStationName.Alignment = stationsStationName.alignment == "left" ? NameAlignment.Left : NameAlignment.Right;
        metroStationName.Text = stationsStationName.text;
        if (stationsStationName.textLines != null)
        {
          schemepackSchemeStationsStationNameTextLinesTextLine[] textLines = stationsStationName.textLines;
          int length3 = textLines.Length;
          metroStationName.TextLines = new string[length3];
          for (int index2 = 0; index2 < length3; ++index2)
            metroStationName.TextLines[index2] = textLines[index2].Value;
        }
        if (stationsStationName.customSchemePosition != null)
        {
          schemepackSchemeStationsStationNameCustomSchemePosition customSchemePosition = stationsStationName.customSchemePosition;
          metroStationName.CustomPosition = new Point?(new Point(customSchemePosition.x, customSchemePosition.y));
        }
        metroStation.Name = metroStationName;
        if (station.oldNames != null)
        {
          schemepackSchemeStationsStationOldNamesOldName[] array = ((IEnumerable<schemepackSchemeStationsStationOldNamesOldName>) station.oldNames).Where<schemepackSchemeStationsStationOldNamesOldName>((Func<schemepackSchemeStationsStationOldNamesOldName, bool>) (o => !string.IsNullOrEmpty(o.Value))).ToArray<schemepackSchemeStationsStationOldNamesOldName>();
          int length4 = array.Length;
          OldName[] oldNameArray = new OldName[length4];
          for (int index3 = 0; index3 < length4; ++index3)
          {
            schemepackSchemeStationsStationOldNamesOldName stationOldNamesOldName = array[index3];
            oldNameArray[index3] = new OldName()
            {
              Name = stationOldNamesOldName.Value,
              Year = stationOldNamesOldName.usedBefore
            };
          }
          metroStation.OldNames = oldNameArray;
        }
        schemepackSchemeStationsStationWeightsWeight[] weights = station.weights;
        if (weights != null)
        {
          PointWeights pointWeights = new PointWeights();
          int length5 = weights.Length;
          for (int index4 = 0; index4 < length5; ++index4)
          {
            schemepackSchemeStationsStationWeightsWeight stationWeightsWeight = weights[index4];
            if (stationWeightsWeight.type == "time")
              pointWeights.Time = int.Parse(stationWeightsWeight.Value);
            else if (stationWeightsWeight.type == "transfer")
              pointWeights.Transfer = int.Parse(stationWeightsWeight.Value);
          }
          metroStation.Weights = pointWeights;
        }
        schemepackSchemeStationsStationSchemePosition schemePosition = station.schemePosition;
        metroStation.SchemePosition = new Point(schemePosition.x, schemePosition.y);
        dictionary[key] = metroStation;
        schemepackSchemeStationsStationBoardInfo boardInfo1 = station.boardInfo;
        if (boardInfo1 != null)
        {
          BoardInfo boardInfo2 = new BoardInfo();
          boardPositions[] exit = boardInfo1.exit;
          if (exit != null)
          {
            List<int> intList = boardInfo2.Exit = new List<int>();
            boardPositionsPos[] pos = exit[0].pos;
            int length6 = pos.Length;
            for (int index5 = 0; index5 < length6; ++index5)
              intList.Add(pos[index5].Value);
          }
          boardPositions[] transfer = boardInfo1.transfer;
          if (transfer != null)
          {
            int length7 = transfer.Length;
            TransferBoardPositions[] transferBoardPositionsArray = boardInfo2.Transfer = new TransferBoardPositions[length7];
            for (int index6 = 0; index6 < length7; ++index6)
            {
              boardPositions boardPositions = transfer[index6];
              boardPositionsPos[] pos = boardPositions.pos;
              int length8 = pos == null ? 0 : pos.Length;
              TransferBoardPositions transferBoardPositions = new TransferBoardPositions()
              {
                ToStation = boardPositions.toStation != null ? new int?(int.Parse(boardPositions.toStation)) : new int?(),
                PrevStation = boardPositions.prevStation != null ? new int?(int.Parse(boardPositions.prevStation)) : new int?(),
                NextStation = boardPositions.nextStation != null ? new int?(int.Parse(boardPositions.nextStation)) : new int?(),
                Positions = new List<int>()
              };
              for (int index7 = 0; index7 < length8; ++index7)
                transferBoardPositions.Positions.Add(pos[index7].Value);
              transferBoardPositionsArray[index6] = transferBoardPositions;
            }
          }
          metroStation.BoardInfo = boardInfo2;
        }
      }
      scheme1.Stations = dictionary;
      schemepackSchemeLinksLink[] links = schemepackScheme.links;
      int length9 = links.Length;
      MetroLink[] metroLinkArray = new MetroLink[length9];
      for (int index8 = 0; index8 < length9; ++index8)
      {
        schemepackSchemeLinksLink schemepackSchemeLinksLink = links[index8];
        MetroLink metroLink = new MetroLink();
        metroLink.From = schemepackSchemeLinksLink.fromStation;
        metroLink.To = schemepackSchemeLinksLink.toStation;
        metroLink.Type = schemepackSchemeLinksLink.type;
        schemepackSchemeLinksLinkWeightsWeight[] weights = schemepackSchemeLinksLink.weights;
        int length10 = weights.Length;
        PointWeights pointWeights = new PointWeights();
        for (int index9 = 0; index9 < length10; ++index9)
        {
          schemepackSchemeLinksLinkWeightsWeight linkWeightsWeight = weights[index9];
          if (linkWeightsWeight.type == "time")
            pointWeights.Time = int.Parse(linkWeightsWeight.Value);
          else if (linkWeightsWeight.type == "transfer")
            pointWeights.Transfer = int.Parse(linkWeightsWeight.Value);
        }
        metroLink.Weights = pointWeights;
        if (schemepackSchemeLinksLink.customDraw != null)
        {
          int length11 = schemepackSchemeLinksLink.customDraw.Length;
          metroLink.CustomDraws = new CustomLinkDraw[length11];
          for (int index10 = 0; index10 < length11; ++index10)
          {
            schemepackSchemeLinksLinkCustomDrawCustomDrawElement customDrawElement = schemepackSchemeLinksLink.customDraw[index10];
            CustomLinkDraw customLinkDraw = new CustomLinkDraw()
            {
              Type = customDrawElement.type,
              EndAngle = customDrawElement.endAngle,
              StartAngle = customDrawElement.startAngle,
              Radius = customDrawElement.radius,
              X = customDrawElement.x,
              Y = customDrawElement.y
            };
            metroLink.CustomDraws[index10] = customLinkDraw;
          }
        }
        metroLinkArray[index8] = metroLink;
      }
      scheme1.Links = metroLinkArray;
      schemepackSchemeTransfersTransferStation[][] transfers = schemepackScheme.transfers;
      int length12 = transfers.Length;
      int[][] numArray1 = new int[length12][];
      for (int index11 = 0; index11 < length12; ++index11)
      {
        schemepackSchemeTransfersTransferStation[] transfersTransferStationArray = transfers[index11];
        int length13 = transfersTransferStationArray.Length;
        int[] numArray2 = new int[length13];
        for (int index12 = 0; index12 < length13; ++index12)
          numArray2[index12] = transfersTransferStationArray[index12].Value;
        numArray1[index11] = numArray2;
      }
      scheme1.Transfers = numArray1;
      if (schemepackScheme.options != null && schemepackScheme.options.Length > 0)
      {
        schemepackSchemeOptions option = schemepackScheme.options[0];
        scheme1.Width = option.size[0].width;
        scheme1.Height = option.size[0].height;
        if (option.workTime != null)
        {
          schemepackSchemeOptionsWorkTime schemeOptionsWorkTime = ((IEnumerable<schemepackSchemeOptionsWorkTime>) option.workTime).FirstOrDefault<schemepackSchemeOptionsWorkTime>();
          if (schemeOptionsWorkTime != null)
            scheme1.WorkTime = new WorkTime()
            {
              CloseTime = schemeOptionsWorkTime.close,
              OpenTime = schemeOptionsWorkTime.open
            };
        }
      }
      RouteHelper.GenerateRouteGraph(scheme1);
      return scheme1;
    }
  }
}
