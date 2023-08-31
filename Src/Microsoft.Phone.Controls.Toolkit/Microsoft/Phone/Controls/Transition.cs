// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Transition
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  public class Transition : ITransition
  {
    private CacheMode _cacheMode;
    private UIElement _element;
    private bool _isHitTestVisible;
    private Storyboard _storyboard;

    public event EventHandler Completed
    {
      add => ((Timeline) this._storyboard).Completed += value;
      remove => ((Timeline) this._storyboard).Completed -= value;
    }

    public Transition(UIElement element, Storyboard storyboard)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (storyboard == null)
        throw new ArgumentNullException(nameof (storyboard));
      this._element = element;
      this._storyboard = storyboard;
    }

    public void Begin()
    {
      this.Save();
      ((Timeline) this._storyboard).Completed += new EventHandler(this.OnCompletedRestore);
      this._storyboard.Begin();
    }

    private void OnCompletedRestore(object sender, EventArgs e) => this.Restore();

    public ClockState GetCurrentState() => this._storyboard.GetCurrentState();

    public TimeSpan GetCurrentTime() => this._storyboard.GetCurrentTime();

    public void Pause() => this._storyboard.Pause();

    private void Restore()
    {
      if (!(this._cacheMode is BitmapCache))
        this._element.CacheMode = this._cacheMode;
      if (this._isHitTestVisible)
        this._element.IsHitTestVisible = this._isHitTestVisible;
      else
        this._element.IsHitTestVisible = true;
    }

    public void Resume() => this._storyboard.Resume();

    private void Save()
    {
      this._cacheMode = this._element.CacheMode;
      if (!(this._cacheMode is BitmapCache))
        this._element.CacheMode = TransitionFrame.BitmapCacheMode;
      this._isHitTestVisible = this._element.IsHitTestVisible;
      if (!this._isHitTestVisible)
        return;
      this._element.IsHitTestVisible = false;
    }

    public void Seek(TimeSpan offset) => this._storyboard.Seek(offset);

    public void SeekAlignedToLastTick(TimeSpan offset) => this._storyboard.SeekAlignedToLastTick(offset);

    public void SkipToFill() => this._storyboard.SkipToFill();

    public void Stop()
    {
      this._storyboard.Stop();
      this.Restore();
    }
  }
}
