// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.AppResourcesHelper
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace Y.Metro.ServiceLayer
{
  public class AppResourcesHelper
  {
    public static string ReadTextFile(string fileName)
    {
      StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(fileName, UriKind.Relative));
      if (resourceStream == null)
        throw new ArgumentNullException(nameof (fileName));
      using (resourceStream.Stream)
      {
        using (StreamReader streamReader = new StreamReader(resourceStream.Stream))
          return streamReader.ReadToEnd();
      }
    }

    public static Stream GetResourceStream(string fileName) => (Application.GetResourceStream(new Uri(fileName, UriKind.Relative)) ?? throw new ArgumentNullException(nameof (fileName))).Stream;
  }
}
