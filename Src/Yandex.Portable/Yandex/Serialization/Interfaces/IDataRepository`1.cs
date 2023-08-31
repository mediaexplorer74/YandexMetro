// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.Interfaces.IDataRepository`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;

namespace Yandex.Serialization.Interfaces
{
  public interface IDataRepository<TData>
  {
    [NotNull]
    TData Data { get; }

    void Save();
  }
}
