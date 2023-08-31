// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.ICompositeTransformation
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Maps.Interfaces
{
  internal interface ICompositeTransformation
  {
    double Rotation { get; set; }

    double ScaleX { get; set; }

    double ScaleY { get; set; }

    double TranslateX { get; set; }

    double TranslateY { get; set; }
  }
}
