// Decompiled with JetBrains decompiler
// Type: Yandex.App.DummyApplicationIdentity
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.App.Interfaces;

namespace Yandex.App
{
  internal class DummyApplicationIdentity : IApplicationIdentity
  {
    public string Platform => "winrt";

    public Version ApplicationVersion => new Version(1, 0);
  }
}
