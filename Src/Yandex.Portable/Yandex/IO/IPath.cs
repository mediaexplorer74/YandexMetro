// Decompiled with JetBrains decompiler
// Type: Yandex.IO.IPath
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System.Collections.Generic;

namespace Yandex.IO
{
  public interface IPath
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
