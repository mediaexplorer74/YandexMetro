// Decompiled with JetBrains decompiler
// Type: Yandex.IO.IFileStorage
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using System;
using System.IO;
using Yandex.PAL.IO;

namespace Yandex.IO
{
  public interface IFileStorage : IDisposable
  {
    bool FileExists(string fileName);

    Stream CreateFile(string fileName, long minimumAvailableFreeSpace);

    Stream OpenFile(string fileName, FileMode fileMode);

    Stream OpenFile(
      string fileName,
      FileMode fileMode,
      FileAccess fileAccess,
      FileShare fileShare);

    bool DirectoryExistsCached(string dir);

    bool DirectoryExists(string dir);

    void DirectoryDelete(string path, bool recursive = true);

    bool IsFreeSpaceAvailable(long minimumAvailableFreeSpace);

    void DeleteFile(string fileName);

    string[] GetFileNames(string directoryName);

    string[] GetDirectoryNames(string directoryName);

    void CreateDirectory(string path);

    void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);

    void MoveFile(string pathFrom, string pathTo);

    string GetFileUri(string name);

    string GetSharedContentPath();

    void CreateDirectoryNative(string path);
  }
}
