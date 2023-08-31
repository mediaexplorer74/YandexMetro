// Decompiled with JetBrains decompiler
// Type: TinyIoC.AssemblyExtensions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.IO;
using System.Reflection;

namespace TinyIoC
{
  internal static class AssemblyExtensions
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
