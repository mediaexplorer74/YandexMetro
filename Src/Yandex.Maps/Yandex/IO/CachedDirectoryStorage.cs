// Decompiled with JetBrains decompiler
// Type: Yandex.IO.CachedDirectoryStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Yandex.IO
{
  internal abstract class CachedDirectoryStorage : IDirectoryStorage
  {
    private readonly IDictionary<string, bool> _directoryExistence;
    private readonly IPath _path;

    public CachedDirectoryStorage([NotNull] IDictionary<string, bool> directoryExistence, [NotNull] IPath path)
    {
      if (directoryExistence == null)
        throw new ArgumentNullException(nameof (directoryExistence));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this._directoryExistence = directoryExistence;
      this._path = path;
    }

    protected abstract bool DirectoryExistsNative(string directoryName);

    protected abstract void CreateDirectoryNative(string directoryName);

    protected abstract void DirectoryDeleteNative(string directoryName);

    public abstract void DeleteFile(string fileName);

    public abstract string[] GetFileNames(string directoryName);

    public abstract IEnumerable<string> GetDirectoryNames(string directoryName);

    public virtual void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
    {
      if (!this._directoryExistence.ContainsKey(sourceDirectoryName))
        return;
      this._directoryExistence.Remove(sourceDirectoryName);
    }

    public void CreateDirectory(string path)
    {
      IList<string> directories = this._path.GetDirectories(path);
      for (int index = directories.Count - 1; index >= 0; --index)
      {
        this.CreateDirectoryNative(directories[index]);
        this._directoryExistence[directories[index]] = true;
      }
    }

    public bool DirectoryExists(string directoryName)
    {
      bool flag;
      if (!this._directoryExistence.TryGetValue(directoryName, out flag))
      {
        flag = this.DirectoryExistsNative(directoryName);
        if (flag)
          this._directoryExistence[directoryName] = true;
      }
      return flag;
    }

    public void DeleteDirectory(string directoryName, bool recursive)
    {
      if (recursive)
      {
        if (!this.DirectoryExists(directoryName))
          return;
        foreach (string directoryName1 in this.GetDirectoryNames(directoryName))
          this.DeleteDirectory(this._path.Combine(directoryName, directoryName1), true);
        foreach (string fileName in this.GetFileNames(directoryName))
          this.DeleteFile(this._path.Combine(directoryName, fileName));
        this.DeleteDirectory(directoryName, false);
      }
      else
      {
        this._directoryExistence.Remove(directoryName);
        if (!this.DirectoryExistsNative(directoryName))
          return;
        this.DirectoryDeleteNative(directoryName.TrimEnd(this._path.DirectorySeparatorChar));
      }
    }
  }
}
