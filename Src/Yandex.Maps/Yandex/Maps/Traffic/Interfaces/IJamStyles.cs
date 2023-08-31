// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamStyles
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Collections.Generic;

namespace Yandex.Maps.Traffic.Interfaces
{
  internal interface IJamStyles : 
    IDictionary<int, IJamStyle>,
    ICollection<KeyValuePair<int, IJamStyle>>,
    IEnumerable<KeyValuePair<int, IJamStyle>>,
    IEnumerable
  {
  }
}
