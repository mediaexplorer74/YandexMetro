// Decompiled with JetBrains decompiler
// Type: Yandex.IO.IPath
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Collections.Generic;

namespace Yandex.IO
{
  internal interface IPath
  {
    char DirectorySeparatorChar { get; }

    string GetDirectoryName(string path);

    string Combine(string path1, string path2);

    [NotNull]
    IList<string> GetDirectories(string path);

    [NotNull]
    IList<string> Split(string path);

    string GetFileName(string fileName);
  }
}
