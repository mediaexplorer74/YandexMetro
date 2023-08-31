// Decompiled with JetBrains decompiler
// Type: Yandex.App.EntryPointAssemblyIdentity
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Yandex.App
{
  internal class EntryPointAssemblyIdentity : ApplicationIdentity
  {
    private static Assembly GetEntryPointAssembly() => ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Single<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.StartsWith(Deployment.Current.EntryPointAssembly + ",")));

    public EntryPointAssemblyIdentity(string platform)
      : base(platform, EntryPointAssemblyIdentity.GetEntryPointAssembly())
    {
    }
  }
}
