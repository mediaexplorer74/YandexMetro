// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PerfLog
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using Microsoft.Phone.Logging;

namespace Microsoft.Phone.Controls
{
  internal class PerfLog
  {
    internal static string Panorama = nameof (Panorama);
    internal static string PanoramaPanel = nameof (PanoramaPanel);
    internal static string PanoramaItem = nameof (PanoramaItem);
    internal static string PanningLayer = nameof (PanningLayer);
    internal static string Pivot = nameof (Pivot);
    internal static string PivotItem = nameof (PivotItem);
    internal static string PivotHeadersControl = nameof (PivotHeadersControl);

    internal static void BeginLogMarker(PerfMarkerEvents EventCode, string Message) => PerfLog.LogPerfMarker((LogFlags) 65793, EventCode, Message);

    internal static void EndLogMarker(PerfMarkerEvents EventCode, string Message) => PerfLog.LogPerfMarker((LogFlags) 131329, EventCode, Message);

    internal static void InfoLogMarker(PerfMarkerEvents EventCode, string Message) => PerfLog.LogPerfMarker((LogFlags) 257, EventCode, Message);

    private static void LogPerfMarker(LogFlags logFlag, PerfMarkerEvents EventCode, string Message) => Logger.YLogEvent(2147483654U, (uint) EventCode, logFlag, Message);
  }
}
