// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.ISystemClock
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;

namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public interface ISystemClock
  {
    DateTimeOffset UtcNow { get; }
  }
}
