// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.IConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.ComponentModel;

namespace System.Reactive.Concurrency
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public interface IConcurrencyAbstractionLayer
  {
    IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime);

    IDisposable StartPeriodicTimer(Action action, TimeSpan period);

    IDisposable QueueUserWorkItem(Action<object> action, object state);

    void Sleep(TimeSpan timeout);

    IStopwatch StartStopwatch();

    bool SupportsLongRunning { get; }

    void StartThread(Action<object> action, object state);
  }
}
