// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.Interfaces.IBitmapRepository`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Yandex.Controls.Images.Interfaces
{
  internal interface IBitmapRepository<TKey> where TKey : class
  {
    BitmapSource this[TKey key] { get; }

    Uri WriteItem(TKey key, TimeSpan timeout, Stream source);

    Uri WriteItem(TKey key, TimeSpan timeout, byte[] sourceBuffer);

    void Flush();

    void SaveState();
  }
}
