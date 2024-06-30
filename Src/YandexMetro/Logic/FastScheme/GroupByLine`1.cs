// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.FastScheme.GroupByLine`1
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Yandex.Metro.Logic.FastScheme
{
  public class GroupByLine<T> : ObservableCollection<T>
  {
    public GroupByLine(LineForGroup line, IEnumerable<T> items)
      : base(items)
    {
      this.Line = line;
    }

    public override bool Equals(object obj) => obj is GroupByLine<T> groupByLine && this.Line.Equals((object) groupByLine.Line);

    public LineForGroup Line { get; set; }
  }
}
