// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ResSource
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Reflection;

namespace Yandex.Controls
{
  internal class ResSource
  {
    public Assembly Assembly { get; private set; }

    public string BaseName { get; private set; }

    public string ResourceName { get; private set; }

    public ResSource(Assembly assembly, string baseName, string resourceName)
    {
      if (assembly == null)
        throw new ArgumentNullException(nameof (assembly));
      if (string.IsNullOrEmpty(baseName))
        throw new ArgumentOutOfRangeException("baseName is null or empty");
      if (string.IsNullOrEmpty(resourceName))
        throw new ArgumentOutOfRangeException("resourceName is null or empty");
      this.Assembly = assembly;
      this.BaseName = baseName;
      this.ResourceName = resourceName;
    }
  }
}
