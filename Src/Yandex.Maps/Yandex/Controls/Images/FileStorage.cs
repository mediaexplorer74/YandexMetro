// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.FileStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.IO;
using System.IO.IsolatedStorage;
using Yandex.Controls.Images.Interfaces;

namespace Yandex.Controls.Images
{
  internal class FileStorage : IFileStorage
  {
    private readonly IsolatedStorageFile _file = IsolatedStorageFile.GetUserStoreForApplication();

    public void CreateDirectory(string directoryName) => this._file.CreateDirectory(directoryName);

    public bool FileExists(string fileName) => this._file.FileExists(fileName);

    public Stream OpenFile(string fileName) => (Stream) this._file.OpenFile(fileName, (FileMode) 3, (FileAccess) 1);

    public Stream CreateFile(string fileName) => (Stream) this._file.CreateFile(fileName);

    public void DeleteFile(string fileName) => this._file.DeleteFile(fileName);
  }
}
