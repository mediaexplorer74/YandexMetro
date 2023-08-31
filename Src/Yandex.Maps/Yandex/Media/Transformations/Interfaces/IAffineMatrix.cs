// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Transformations.Interfaces.IAffineMatrix
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Media.Transformations.Interfaces
{
  internal interface IAffineMatrix
  {
    double M11 { get; }

    double M12 { get; }

    double M21 { get; }

    double M22 { get; }

    double OffsetX { get; }

    double OffsetY { get; }
  }
}
