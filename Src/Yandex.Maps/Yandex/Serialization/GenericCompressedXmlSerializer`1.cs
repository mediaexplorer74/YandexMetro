// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.GenericCompressedXmlSerializer`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Serialization
{
  internal class GenericCompressedXmlSerializer<T> : GenericXmlSerializer<T> where T : class
  {
    protected override bool DataIsCompressed => true;
  }
}
