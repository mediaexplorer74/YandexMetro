// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.HistoricalSchedulerBase
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Concurrency
{
  public abstract class HistoricalSchedulerBase : VirtualTimeSchedulerBase<DateTimeOffset, TimeSpan>
  {
    protected HistoricalSchedulerBase()
      : base(DateTimeOffset.MinValue, (IComparer<DateTimeOffset>) System.Collections.Generic.Comparer<DateTimeOffset>.Default)
    {
    }

    protected HistoricalSchedulerBase(DateTimeOffset initialClock)
      : base(initialClock, (IComparer<DateTimeOffset>) System.Collections.Generic.Comparer<DateTimeOffset>.Default)
    {
    }

    protected HistoricalSchedulerBase(
      DateTimeOffset initialClock,
      IComparer<DateTimeOffset> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override DateTimeOffset Add(DateTimeOffset absolute, TimeSpan relative) => absolute.Add(relative);

    protected override DateTimeOffset ToDateTimeOffset(DateTimeOffset absolute) => absolute;

    protected override TimeSpan ToRelative(TimeSpan timeSpan) => timeSpan;
  }
}
