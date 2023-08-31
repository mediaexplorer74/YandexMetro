// Decompiled with JetBrains decompiler
// Type: Yandex.IO.Path
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Collections.Generic;

namespace Yandex.IO
{
  public class Path : IPath
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

    public IList<string> Split(string path) => (IList<string>) path.TrimEnd(this.DirectorySeparatorChar).Split(this.DirectorySeparatorChar);

    public string GetFileName(string fileName) => System.IO.Path.GetFileName(fileName);
  }
}
