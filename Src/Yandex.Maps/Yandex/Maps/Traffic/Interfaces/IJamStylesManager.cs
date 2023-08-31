// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamStylesManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.Maps.Traffic.DTO.Styles;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.Traffic.Interfaces
{
  internal interface IJamStylesManager : ICommunicator<object, JamStyles>
  {
    [CanBeNull]
    JamStyles Styles { get; set; }
  }
}
