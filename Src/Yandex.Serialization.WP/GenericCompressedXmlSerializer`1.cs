// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.GenericCompressedXmlSerializer`1
// Assembly: Yandex.Serialization.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92CA2ACD-4B0D-4692-89EF-43480E0E9364
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Serialization.WP.dll

namespace Yandex.Serialization
{
  public class GenericCompressedXmlSerializer<T> : GenericXmlSerializer<T> where T : class
  {
    protected override bool DataIsCompressed => true;
  }
}
