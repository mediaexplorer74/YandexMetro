// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.FileNameMapper`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Controls.Images.Interfaces;

namespace Yandex.Controls.Images
{
  internal class FileNameMapper<T> : IFileNameMapper<T>
  {
    public string GetFileName(T key) => key.GetHashCode().ToString();
  }
}
