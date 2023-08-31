// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.DataAdapters.VersionStringAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.PrinterClient.DataAdapters
{
  internal class VersionStringAdapter : IVersionStringAdapter
  {
    public string Process(Version version)
    {
      if (version == (Version) null)
        throw new ArgumentNullException(nameof (version));
      return version.Major.ToString("##") + version.Minor.ToString("00").Substring(0, 2);
    }
  }
}
