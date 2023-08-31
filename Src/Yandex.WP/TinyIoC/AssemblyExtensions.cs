// Decompiled with JetBrains decompiler
// Type: TinyIoC.AssemblyExtensions
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.IO;
using System.Reflection;

namespace TinyIoC
{
  public static class AssemblyExtensions
  {
    public static Type[] SafeGetTypes(this Assembly assembly)
    {
      try
      {
        return assembly.GetTypes();
      }
      catch (FileNotFoundException ex)
      {
        return new Type[0];
      }
      catch (NotSupportedException ex)
      {
        return new Type[0];
      }
      catch (ReflectionTypeLoadException ex)
      {
        return new Type[0];
      }
    }
  }
}
