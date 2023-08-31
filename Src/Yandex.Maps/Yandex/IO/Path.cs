// Decompiled with JetBrains decompiler
// Type: Yandex.IO.Path
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;

namespace Yandex.IO
{
  internal class Path : IPath
  {
    public char DirectorySeparatorChar => '\\';

    public string GetDirectoryName(string path) => System.IO.Path.GetDirectoryName(path);

    public string Combine(string path1, string path2) => System.IO.Path.Combine(path1, path2);

    public IList<string> GetDirectories(string path)
    {
      string path1 = path.TrimEnd(this.DirectorySeparatorChar);
      List<string> directories = new List<string>()
      {
        path1
      };
      while (!string.IsNullOrEmpty(path1 = this.GetDirectoryName(path1)))
        directories.Add(path1);
      return (IList<string>) directories;
    }

    public IList<string> Split(string path) => (IList<string>) path.TrimEnd(this.DirectorySeparatorChar).Split(new char[1]
    {
      this.DirectorySeparatorChar
    }, StringSplitOptions.RemoveEmptyEntries);

    public string GetFileName(string fileName) => System.IO.Path.GetFileName(fileName);
  }
}
