﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.Interfaces.IFileStorage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.IO;

namespace Yandex.Controls.Images.Interfaces
{
  internal interface IFileStorage
  {
    void CreateDirectory(string directoryName);

    bool FileExists(string fileName);

    Stream OpenFile(string fileName);

    Stream CreateFile(string fileName);

    void DeleteFile(string fileName);
  }
}
