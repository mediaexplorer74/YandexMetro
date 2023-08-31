// Decompiled with JetBrains decompiler
// Type: Yandex.App.ApplicationIdentity
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Yandex.App.Interfaces;

namespace Yandex.App
{
  internal class ApplicationIdentity : IApplicationIdentity
  {
    private static Version GetVersion(Assembly assembly) => new Version(Regex.Match(assembly.FullName, "\\d+\\.\\d+\\.\\d+\\.\\d+").ToString());

    public ApplicationIdentity(string platform, [NotNull] Assembly assembly)
    {
      if (string.IsNullOrEmpty(platform))
        throw new ArgumentOutOfRangeException(nameof (platform));
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      this.Platform = platform;
      this.ApplicationVersion = ApplicationIdentity.GetVersion(assembly);
    }

    public ApplicationIdentity(string platform, Version applicationVersion)
    {
      if (string.IsNullOrEmpty(platform))
        throw new ArgumentNullException(nameof (platform));
      if ((Version) null == applicationVersion)
        throw new ArgumentNullException(nameof (applicationVersion));
      this.Platform = platform;
      this.ApplicationVersion = applicationVersion;
    }

    public string Platform { get; private set; }

    public Version ApplicationVersion { get; private set; }
  }
}
