// Decompiled with JetBrains decompiler
// Type: System.Reactive.Unit
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Runtime.InteropServices;

namespace System.Reactive
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Unit : IEquatable<Unit>
  {
    private static readonly Unit _default = new Unit();

    public bool Equals(Unit other) => true;

    public override bool Equals(object obj) => obj is Unit;

    public override int GetHashCode() => 0;

    public override string ToString() => "()";

    public static bool operator ==(Unit first, Unit second) => true;

    public static bool operator !=(Unit first, Unit second) => false;

    public static Unit Default => Unit._default;
  }
}
