// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Resources.ResourceWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;

namespace Yandex.Maps.Resources
{
  [ComVisible(false)]
  public class ResourceWrapper
  {
    private readonly Yandex.Maps.Properties.Resources _appResources;

    public ResourceWrapper() => this._appResources = new Yandex.Maps.Properties.Resources();

    public Yandex.Maps.Properties.Resources AppResources => this._appResources;
  }
}
