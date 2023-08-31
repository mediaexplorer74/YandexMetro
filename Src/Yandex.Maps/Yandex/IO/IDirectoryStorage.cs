// Decompiled with JetBrains decompiler
// Type: Yandex.IO.IDirectoryStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;

namespace Yandex.IO
{
  internal interface IDirectoryStorage
  {
    bool DirectoryExists(string dir);

    IEnumerable<string> GetDirectoryNames(string directoryName);

    void DeleteDirectory(string path, bool recursive = true);

    void CreateDirectory(string path);

    void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);
  }
}
