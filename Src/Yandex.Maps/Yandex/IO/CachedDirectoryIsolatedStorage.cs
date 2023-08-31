// Decompiled with JetBrains decompiler
// Type: Yandex.IO.CachedDirectoryIsolatedStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Yandex.DevUtils;

namespace Yandex.IO
{
  [UsedImplicitly]
  internal class CachedDirectoryIsolatedStorage : 
    CachedDirectoryStorage,
    ILimitedFileStorage,
    IFileStorage,
    IDirectoryStorage,
    IDisposable
  {
    private const long QuotaIncrease = 5242880;
    private const string IsostoreScheme = "isostore:";
    private const string SharedContentPath = "Shared\\ShellContent";
    private readonly IPath _path;
    private readonly IsolatedStorageFile _file;

    public CachedDirectoryIsolatedStorage([NotNull] IDictionary<string, bool> directoryExistence, [NotNull] IPath path)
      : base(directoryExistence, path)
    {
      if (directoryExistence == null)
        throw new ArgumentNullException(nameof (directoryExistence));
      this._path = path != null ? path : throw new ArgumentNullException(nameof (path));
      if (DesignerProperties.IsInDesignMode)
        return;
      this._file = IsolatedStorageFile.GetUserStoreForApplication();
    }

    public event EventHandler StorageQuotaReached;

    public bool FileExists(string fileName) => !DesignerProperties.IsInDesignMode && this._file.FileExists(fileName);

    public bool IsFreeSpaceAvailable(long minimumAvailableFreeSpace)
    {
      if (minimumAvailableFreeSpace <= this._file.AvailableFreeSpace)
        return true;
      this.OnStorageQuotaReached();
      return false;
    }

    public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
    {
      this._file.MoveDirectory(sourceDirectoryName, destinationDirectoryName);
      base.MoveDirectory(sourceDirectoryName, destinationDirectoryName);
    }

    public void MoveFile(string pathFrom, string pathTo) => this._file.MoveFile(pathFrom, pathTo);

    public Stream OpenFile(
      string fileName,
      FileMode fileMode,
      Yandex.PAL.IO.FileAccess fileAccess,
      Yandex.PAL.IO.FileShare fileShare)
    {
      try
      {
        return (Stream) this._file.OpenFile(fileName, (System.IO.FileMode) fileMode, (System.IO.FileAccess) fileAccess, (System.IO.FileShare) fileShare);
      }
      catch (IsolatedStorageException ex)
      {
        throw new FileNotFoundException("IsolatedStorageException inside CachedDirectoryIsolatedStorage.OpenFile", (Exception) ex);
      }
    }

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

    public string GetFileUri(string name) => "isostore:/" + name.Replace(this._path.DirectorySeparatorChar, '/');

    public string GetSharedContentPath() => "Shared\\ShellContent";

    protected virtual void OnStorageQuotaReached()
    {
      EventHandler storageQuotaReached = this.StorageQuotaReached;
      if (storageQuotaReached == null)
        return;
      storageQuotaReached((object) this, EventArgs.Empty);
    }

    public override void DeleteFile(string fileName) => this._file.DeleteFile(fileName);

    public override string[] GetFileNames(string directoryName) => this._file.GetFileNames(this._path.Combine(directoryName, "*"));

    public override IEnumerable<string> GetDirectoryNames(string directoryName) => (IEnumerable<string>) this._file.GetDirectoryNames(this._path.Combine(directoryName, "*"));

    protected override bool DirectoryExistsNative(string directoryName) => this._file.DirectoryExists(directoryName);

    protected override void CreateDirectoryNative(string directoryName) => this._file.CreateDirectory(directoryName);

    protected override void DirectoryDeleteNative(string directoryName) => this._file.DeleteDirectory(directoryName);
  }
}
