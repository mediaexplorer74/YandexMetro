// Decompiled with JetBrains decompiler
// Type: Yandex.IO.IFileStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;

namespace Yandex.IO
{
  internal interface IFileStorage : IDirectoryStorage, IDisposable
  {
    bool FileExists(string fileName);

    Stream OpenFile(
      string fileName,
      FileMode fileMode,
      Yandex.PAL.IO.FileAccess fileAccess,
      Yandex.PAL.IO.FileShare fileShare);

    bool IsFreeSpaceAvailable(long minimumAvailableFreeSpace);

    void DeleteFile(string fileName);

    string[] GetFileNames(string directoryName);

    void MoveFile(string pathFrom, string pathTo);

    string GetFileUri(string name);

    string GetSharedContentPath();
  }
}
