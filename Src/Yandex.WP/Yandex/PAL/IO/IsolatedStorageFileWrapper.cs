// Decompiled with JetBrains decompiler
// Type: Yandex.PAL.IO.IsolatedStorageFileWrapper
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Yandex.Common;
using Yandex.DevUtils;
using Yandex.IO;

namespace Yandex.PAL.IO
{
  public class IsolatedStorageFileWrapper : ILimitedFileStorage, IFileStorage, IDisposable
  {
    private const long QuotaIncrease = 5242880;
    private const string IsostoreScheme = "isostore:";
    private const string SharedContentPath = "Shared\\ShellContent";
    private readonly IDictionary<string, bool> _directoryExistence;
    private readonly IPath _path;
    private readonly IsolatedStorageFile _file;

    public IsolatedStorageFileWrapper([NotNull] IDictionary<string, bool> directoryExistence, [NotNull] IPath path)
    {
      if (directoryExistence == null)
        throw new ArgumentNullException(nameof (directoryExistence));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this._directoryExistence = directoryExistence;
      this._path = path;
      if (DesignerProperties.IsInDesignMode)
        return;
      try
      {
        this._file = IsolatedStorageFile.GetUserStoreForApplication();
      }
      catch (IsolatedStorageException ex)
      {
        Logger.TrackException((Exception) ex);
      }
    }

    public event EventHandler StorageQuotaReached;

    public bool FileExists(string fileName) => !DesignerProperties.IsInDesignMode && this._file.FileExists(fileName);

    public Stream CreateFile(string fileName, long minimumAvailableFreeSpace)
    {
      string directoryName = this._path.GetDirectoryName(fileName);
      try
      {
        if (!string.IsNullOrEmpty(directoryName) && !this._file.DirectoryExists(directoryName))
          this._file.CreateDirectory(directoryName);
        return (Stream) this._file.OpenFile(fileName, (System.IO.FileMode) 1, (System.IO.FileAccess) 2, (System.IO.FileShare) 0);
      }
      catch (IsolatedStorageException ex)
      {
        return (Stream) null;
      }
    }

    public bool IsFreeSpaceAvailable(long minimumAvailableFreeSpace)
    {
      if (minimumAvailableFreeSpace <= this._file.AvailableFreeSpace)
        return true;
      this.OnStorageQuotaReached();
      return false;
    }

    public void DeleteFile(string fileName) => this._file.DeleteFile(fileName);

    public string[] GetFileNames(string directoryName) => this._file.GetFileNames(this._path.Combine(directoryName, "*.*"));

    public string[] GetDirectoryNames(string directoryName) => this._file.GetDirectoryNames(this._path.Combine(directoryName, "*.*"));

    public bool DirectoryExists(string directoryName)
    {
      bool flag = this._file.DirectoryExists(directoryName);
      if (flag)
        this._directoryExistence[directoryName] = true;
      return flag;
    }

    public bool DirectoryExistsCached(string directoryName)
    {
      bool flag;
      if (!this._directoryExistence.TryGetValue(directoryName, out flag))
      {
        flag = this._file.DirectoryExists(directoryName);
        if (flag)
          this._directoryExistence[directoryName] = true;
      }
      return flag;
    }

    public void CreateDirectory(string path)
    {
      IList<string> directories = this._path.GetDirectories(path);
      for (int index = directories.Count - 1; index >= 0; --index)
      {
        this._file.CreateDirectory(directories[index]);
        this._directoryExistence[directories[index]] = true;
      }
    }

    public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName) => this._file.MoveDirectory(sourceDirectoryName, destinationDirectoryName);

    public void CreateDirectoryNative(string path) => this._file.CreateDirectory(path);

    public void MoveFile(string pathFrom, string pathTo) => this._file.MoveFile(pathFrom, pathTo);

    public Stream OpenFile(
      string fileName,
      Yandex.IO.FileMode fileMode,
      FileAccess fileAccess,
      FileShare fileShare)
    {
      return (Stream) this._file.OpenFile(fileName, (System.IO.FileMode) fileMode, (System.IO.FileAccess) fileAccess, (System.IO.FileShare) fileShare);
    }

    public Stream OpenFile(string fileName, Yandex.IO.FileMode fileMode) => this.OpenFile(fileName, fileMode, FileAccess.Read, FileShare.Read);

    public void Dispose() => this._file.Dispose();

    public bool TryIncreaseQuota()
    {
      if (this._file.AvailableFreeSpace < 5242880L)
      {
        try
        {
          this._file.IncreaseQuotaTo(this._file.Quota + 5242880L);
        }
        catch (Exception ex)
        {
          return false;
        }
      }
      return true;
    }

    public void DirectoryDelete(string directoryName, bool recursive)
    {
      if (recursive)
      {
        if (!this.DirectoryExists(directoryName))
          return;
        foreach (string directoryName1 in this.GetDirectoryNames(directoryName))
          this.DirectoryDelete(this._path.Combine(directoryName, directoryName1), true);
        foreach (string fileName in this.GetFileNames(directoryName))
          this._file.DeleteFile(this._path.Combine(directoryName, fileName));
        this.DirectoryDelete(directoryName, false);
      }
      else
      {
        this._directoryExistence.Remove(directoryName);
        if (!this._file.DirectoryExists(directoryName))
          return;
        this._file.DeleteDirectory(directoryName.TrimEnd(this._path.DirectorySeparatorChar));
      }
    }

    public string GetFileUri(string name) => "isostore:/" + name.Replace(this._path.DirectorySeparatorChar, '/');

    public string GetSharedContentPath() => "Shared\\ShellContent";

    protected virtual void OnStorageQuotaReached()
    {
      if (this.StorageQuotaReached == null)
        return;
      this.StorageQuotaReached((object) this, EventArgs.Empty);
    }
  }
}
