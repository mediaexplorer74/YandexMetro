// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.IWrappedList
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System.Collections;

namespace Newtonsoft.Json.Utilities
{
  internal interface IWrappedList : IList, ICollection, IEnumerable
  {
    object UnderlyingList { get; }
  }
}
