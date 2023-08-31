// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamRender
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Traffic.DTO.Styles;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Media;
using Yandex.Media.Imaging;

namespace Yandex.Maps.Traffic
{
  internal class JamRender : IJamRender
  {
    private const double ArrowHeadScale = 1.5;
    private const double ArrowLengthMargin = 10.0;
    private const int TwilightModeStyleShift = 100;
    private readonly object _renderLock = new object();
    private readonly IJamStylesManager _jamStylesManager;
    private readonly IZoomInfo _zoomInfo;
    private Dictionary<int, Dictionary<byte, JamZoom>> _styles;
    private volatile OperationMode _operationMode;

    public JamRender(IJamStylesManager jamStylesManager, IZoomInfo zoomInfo)
    {
      if (jamStylesManager == null)
        throw new ArgumentNullException(nameof (jamStylesManager));
      if (zoomInfo == null)
        throw new ArgumentNullException(nameof (zoomInfo));
      this._jamStylesManager = jamStylesManager;
      this._zoomInfo = zoomInfo;
    }

    public OperationMode OperationMode
    {
      get => this._operationMode;
      set
      {
        this._operationMode = value;
        if (value != OperationMode.Full && value != OperationMode.Disabled)
          return;
        lock (this._renderLock)
          Monitor.PulseAll(this._renderLock);
      }
    }

    public bool Render(
      IList<Track> tracks,
      byte zoom,
      int[] destination,
      int destinationWidth,
      int destinationHeight,
      int offsetX,
      int offsetY,
      double renderScale,
      bool useTwilightModeStyles)
    {
      if (this.OperationMode == OperationMode.Disabled)
        return false;
      bool flag = false;
      if (this._styles == null)
        this._styles = this.PrepareStyles();
      byte key = (byte) ((uint) this._zoomInfo.MaxZoom - (uint) zoom);
      foreach (IGrouping<int, Track> grouping in tracks.Where<Track>((Func<Track, bool>) (track => track != null && track.PixelPoints != null && track.PixelPoints.Count > 0)).GroupBy<Track, int>((Func<Track, int>) (item => item.StyleId)))
      {
        Dictionary<byte, JamZoom> dictionary;
        if (useTwilightModeStyles)
        {
          if (!this._styles.TryGetValue(grouping.Key + 100, out dictionary) && !this._styles.TryGetValue(grouping.Key, out dictionary))
            continue;
        }
        else if (!this._styles.TryGetValue(grouping.Key, out dictionary))
          continue;
        JamZoom jamZoom;
        if (dictionary.TryGetValue(key, out jamZoom))
        {
          uint lineColorInt = (uint) jamZoom.LineColorInt;
          double thickness = jamZoom.LineWidth * renderScale;
          double radius = thickness * 0.5;
          foreach (Track track in (IEnumerable<Track>) grouping)
          {
            if (this._operationMode != OperationMode.Full)
            {
              lock (this._renderLock)
              {
                if (this.OperationMode == OperationMode.Disabled)
                  return false;
                Monitor.Wait(this._renderLock);
                if (this.OperationMode == OperationMode.Disabled)
                  return false;
              }
            }
            IEnumerator<Point> enumerator = track.PixelPoints.GetEnumerator();
            enumerator.MoveNext();
            Point current1 = enumerator.Current;
            double x1 = current1.X - (double) offsetX;
            double y1 = current1.Y - (double) offsetY;
            while (enumerator.MoveNext())
            {
              Point current2 = enumerator.Current;
              double num1 = current2.X - (double) offsetX;
              double num2 = current2.Y - (double) offsetY;
              flag = true;
              Lines.DrawSolidLine(destination, destinationWidth, destinationHeight, x1, y1, num1, num2, lineColorInt, thickness);
              Lines.DrawCircle(radius, num1, num2, destination, lineColorInt, destinationWidth, destinationHeight);
              x1 = num1;
              y1 = num2;
            }
          }
          if (jamZoom.Arrows != null)
          {
            foreach (Track track in (IEnumerable<Track>) grouping)
            {
              while (this._operationMode != OperationMode.Full)
              {
                lock (this._renderLock)
                {
                  if (this.OperationMode == OperationMode.Disabled)
                    return false;
                  Monitor.Wait(this._renderLock);
                  if (this.OperationMode == OperationMode.Disabled)
                    return false;
                }
              }
              double zDashProgress = 0.0;
              IEnumerator<Point> enumerator = track.PixelPoints.GetEnumerator();
              enumerator.MoveNext();
              Point current3 = enumerator.Current;
              double p1X = current3.X - (double) offsetX;
              double p1Y = current3.Y - (double) offsetY;
              while (enumerator.MoveNext())
              {
                Point current4 = enumerator.Current;
                double p2X = current4.X - (double) offsetX;
                double p2Y = current4.Y - (double) offsetY;
                JamRender.DrawArrowDashes(destination, destinationWidth, destinationHeight, p1X, p1Y, p2X, p2Y, jamZoom.Arrows, ref zDashProgress, renderScale);
                p1X = p2X;
                p1Y = p2Y;
              }
            }
          }
        }
      }
      return flag;
    }

    private Dictionary<int, Dictionary<byte, JamZoom>> PrepareStyles()
    {
      Dictionary<int, Dictionary<byte, JamZoom>> dictionary1 = new Dictionary<int, Dictionary<byte, JamZoom>>();
      foreach (int key in this._jamStylesManager.Styles.Keys)
      {
        Dictionary<byte, JamZoom> dictionary2 = new Dictionary<byte, JamZoom>();
        for (byte i = 0; (int) i < (int) this._zoomInfo.MaxZoom; ++i)
          dictionary2[i] = this._jamStylesManager.Styles[key].Zooms.Where<JamZoom>((Func<JamZoom, bool>) (z => z.From <= (int) i && (int) i <= z.To)).FirstOrDefault<JamZoom>();
        dictionary1[key] = dictionary2;
      }
      return dictionary1;
    }

    private static void DrawArrowDashes(
      int[] pixels,
      int pixelWidth,
      int pixelHeight,
      double p1X,
      double p1Y,
      double p2X,
      double p2Y,
      JamArrow style,
      ref double zDashProgress,
      double renderScale)
    {
      double num1 = p2X - p1X;
      double num2 = p2Y - p1Y;
      if (Math.Abs(num1 - 0.0) < double.Epsilon && Math.Abs(num2 - 0.0) < double.Epsilon)
        return;
      double x1 = p1X;
      double y1 = p1Y;
      double num3 = Math.Sqrt(num1 * num1 + num2 * num2);
      double thickness = style.Thickness * renderScale;
      double num4 = 1.0 / num3;
      double cos = num1 * num4;
      double sin = num2 * num4;
      double num5 = style.Dash * renderScale * 1.5;
      double num6 = style.Space * renderScale;
      double num7 = num6 + num5;
      double arrowHeadHeight = style.ArrowHeight * renderScale * 1.5;
      double arrowHeadLength = style.ArrowLength * renderScale * 1.5;
      double num8 = 0.0;
      while (zDashProgress > 0.0)
      {
        if (zDashProgress >= num5)
        {
          if (num7 - zDashProgress <= num3 - num8)
          {
            num8 += num7 - zDashProgress;
            zDashProgress = 0.0;
          }
          else
          {
            zDashProgress += num3 - num8;
            return;
          }
        }
        else
        {
          double num9 = Math.Max(arrowHeadLength + 10.0, num5 - zDashProgress);
          if (num9 <= num3 - num8)
          {
            JamRender.DrawArrow(arrowHeadLength, arrowHeadHeight, cos, sin, x1, y1, x1 + cos * num9, y1 + sin * num9, pixels, pixelWidth, pixelHeight, (uint) style.ColorInt, thickness);
            zDashProgress = num5;
            num8 += num9;
          }
          else
            break;
        }
      }
      if (zDashProgress == 0.0)
      {
        while (num3 - num8 >= num5)
        {
          JamRender.DrawArrow(arrowHeadLength, arrowHeadHeight, cos, sin, x1 + cos * num8, y1 + sin * num8, x1 + cos * (num8 + num5), y1 + sin * (num8 + num5), pixels, pixelWidth, pixelHeight, (uint) style.ColorInt, thickness);
          double num10 = num8 + num5;
          if (num3 - num10 >= num6)
          {
            num8 = num10 + num6;
            zDashProgress = 0.0;
          }
          else
          {
            zDashProgress = num5;
            return;
          }
        }
      }
      if (num8 >= num3)
        return;
      double num11 = num3 - num8;
      zDashProgress += num11;
      Lines.DrawSolidLine(pixels, pixelWidth, pixelHeight, x1 + cos * num8, y1 + sin * num8, p2X, p2Y, (uint) style.ColorInt, thickness);
    }

    private static void DrawArrow(
      double arrowHeadLength,
      double arrowHeadHeight,
      double cos,
      double sin,
      double x1,
      double y1,
      double arrowHeadX,
      double arrowHeadY,
      int[] pixels,
      int pixelWidth,
      int pixelHeight,
      uint intColor,
      double thickness)
    {
      double x2 = arrowHeadX - arrowHeadLength * cos;
      double y2 = arrowHeadY - arrowHeadLength * sin;
      double num1 = arrowHeadHeight;
      double num2 = x2 + num1 * sin;
      double num3 = y2 - num1 * cos;
      double num4 = x2 - num1 * sin;
      double num5 = y2 + num1 * cos;
      Lines.DrawSolidLine(pixels, pixelWidth, pixelHeight, x1, y1, x2, y2, intColor, thickness);
      Polygons.DrawTriangle(pixels, pixelWidth, pixelHeight, new double[8]
      {
        arrowHeadX,
        arrowHeadY,
        num2,
        num3,
        num4,
        num5,
        arrowHeadX,
        arrowHeadY
      }, intColor);
    }
  }
}
