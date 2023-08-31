// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.ITransition
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  public interface ITransition
  {
    event EventHandler Completed;

    ClockState GetCurrentState();

    TimeSpan GetCurrentTime();

    void Pause();

    void Resume();

    void Seek(TimeSpan offset);

    void SeekAlignedToLastTick(TimeSpan offset);

    void SkipToFill();

    void Begin();

    void Stop();
  }
}
