// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.BitmapRepository`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Yandex.Common;
using Yandex.Controls.Images.Interfaces;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Serialization.Interfaces;

namespace Yandex.Controls.Images
{
  internal class BitmapRepository<TKey> : IBitmapRepository<TKey> where TKey : class
  {
    private const int BufferLength = 16384;
    private const string ImageManagerFolder = "\\ImageManager";
    private const string BitmapRepositoryContentsFileName = "\\ImageManager\\BitmapRepositoryContents.dat";
    private readonly IBitmapFactory _bitmapFactory;
    private readonly IGenericXmlSerializer<RepositoryState> _serializer;
    private readonly IFileNameMapper<TKey> _fileNameMapper;
    private readonly IFileStorage _fileStorage;
    private RepositoryState _state;

    public BitmapRepository(
      IFileStorage fileStorage,
      IFileNameMapper<TKey> fileNameMapper,
      IBitmapFactory bitmapFactory,
      IGenericXmlSerializer<RepositoryState> serializer)
    {
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      if (fileNameMapper == null)
        throw new ArgumentNullException(nameof (fileNameMapper));
      if (bitmapFactory == null)
        throw new ArgumentNullException(nameof (bitmapFactory));
      if (serializer == null)
        throw new ArgumentNullException(nameof (serializer));
      this._fileStorage = fileStorage;
      this._fileNameMapper = fileNameMapper;
      this._bitmapFactory = bitmapFactory;
      this._serializer = serializer;
      this.IntializeItems();
    }

    private void IntializeItems()
    {
      this._fileStorage.CreateDirectory("\\ImageManager");
      if (this._fileStorage.FileExists("\\ImageManager\\BitmapRepositoryContents.dat"))
      {
        try
        {
          using (Stream stream = this._fileStorage.OpenFile("\\ImageManager\\BitmapRepositoryContents.dat"))
            this._state = this._serializer.Deserialize(stream);
        }
        catch (Exception ex)
        {
          Logger.TrackException(ex);
        }
      }
      this._state = this._state ?? (this._state = new RepositoryState());
    }

    public void SaveState()
    {
      using (Stream file = this._fileStorage.CreateFile("\\ImageManager\\BitmapRepositoryContents.dat"))
        this._serializer.Serialize(this._state, file);
    }

    public BitmapSource this[TKey key]
    {
      get
      {
        string str = (object) key != null ? this.GetFileName(key) : throw new ArgumentNullException(nameof (key));
        if (this._state.Items.ContainsKey(str))
        {
          this._state.Items[str].LastAccessTime = DateTime.Now;
          if (this._fileStorage.FileExists(str))
            return this.ReadItem(str);
        }
        return (BitmapSource) null;
      }
    }

    public Uri WriteItem(TKey key, TimeSpan timeout, Stream source)
    {
      string str = (object) key != null ? this.GetFileName(key) : throw new ArgumentNullException(nameof (key));
      try
      {
        using (Stream file = this._fileStorage.CreateFile(str))
        {
          byte[] buffer = new byte[16384];
          source.Position = 0L;
          int count;
          while ((count = source.Read(buffer, 0, 16384)) > 0)
          {
            file.Write(buffer, 0, count);
            this._state.Items[str] = new RepositoryItem()
            {
              LastAccessTime = DateTime.Now,
              Timeout = timeout
            };
          }
        }
        return new Uri(str);
      }
      catch
      {
        return (Uri) null;
      }
    }

    [CanBeNull]
    public Uri WriteItem([NotNull] TKey key, TimeSpan timeout, [NotNull] byte[] sourceBuffer)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      if (sourceBuffer == null)
        throw new ArgumentNullException(nameof (sourceBuffer));
      string fileName = this.GetFileName(key);
      try
      {
        using (Stream file = this._fileStorage.CreateFile(fileName))
          file.Write(sourceBuffer, 0, sourceBuffer.Length);
        this._state.Items[fileName] = new RepositoryItem()
        {
          LastAccessTime = DateTime.Now,
          Timeout = timeout
        };
        return new Uri(fileName);
      }
      catch
      {
        return (Uri) null;
      }
    }

    public void Flush()
    {
      DateTime now = DateTime.Now;
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, RepositoryItem> keyValuePair in this._state.Items)
      {
        if (keyValuePair.Value == null)
        {
          stringList.Add(keyValuePair.Key);
        }
        else
        {
          TimeSpan timeout = keyValuePair.Value.Timeout;
          if (timeout != TimeSpan.MaxValue && keyValuePair.Value.LastAccessTime + timeout < now)
            stringList.Add(keyValuePair.Key);
        }
      }
      foreach (string str in stringList)
      {
        this._state.Items.Remove(str);
        this._fileStorage.DeleteFile(str);
      }
    }

    private BitmapSource ReadItem(string fileName)
    {
      try
      {
        using (Stream stream = this._fileStorage.OpenFile(fileName))
          return this._bitmapFactory.GetBitmap(stream);
      }
      catch
      {
        return (BitmapSource) null;
      }
    }

    private string GetFileName(TKey key) => Path.Combine("\\ImageManager", this._fileNameMapper.GetFileName(key));
  }
}
