// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.SchemeReader
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.IO;
using System.Xml.Serialization;

namespace Y.Metro.ServiceLayer
{
  public class SchemeReader
  {
    public static T ReadTest<T>()
    {
      using (Stream resourceStream = AppResourcesHelper.GetResourceStream("/Y.Metro.ServiceLayer;component/Generated/MoscowMetroSchemeSample.xml"))
        return (T) new XmlSerializer(typeof (T)).Deserialize(resourceStream);
    }
  }
}
