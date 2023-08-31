// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ViewStateManager.Interfaces.IViewState
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.Controls.ViewStateManager.Interfaces
{
  internal interface IViewState
  {
    string VisualStateName { get; }

    void OnSet(bool restore, IViewState previousState);

    void OnLeave(IViewState nextState);
  }
}
