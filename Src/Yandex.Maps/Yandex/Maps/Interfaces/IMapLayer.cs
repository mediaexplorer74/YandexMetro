// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IMapLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Controls;
using Yandex.Positioning;

namespace Yandex.Maps.Interfaces
{
  internal interface IMapLayer
  {
    void AddChild(UIElement element, GeoCoordinate location);

    UIElementCollection Children { get; }
  }
}
