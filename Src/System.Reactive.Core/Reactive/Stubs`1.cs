// Decompiled with JetBrains decompiler
// Type: System.Reactive.Stubs`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal static class Stubs<T>
  {
    public static readonly Action<T> Ignore = (Action<T>) (_ => { });
    public static readonly Func<T, T> I = (Func<T, T>) (_ => _);
  }
}
