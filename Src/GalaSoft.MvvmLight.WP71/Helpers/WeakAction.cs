// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakAction
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakAction
  {
    private readonly Action _action;
    private WeakReference _reference;

    public WeakAction(object target, Action action)
    {
      this._reference = new WeakReference(target);
      this._action = action;
    }

    public Action Action => this._action;

    public bool IsAlive => this._reference != null && this._reference.IsAlive;

    public object Target => this._reference == null ? (object) null : this._reference.Target;

    public void Execute()
    {
      if (this._action == null || !this.IsAlive)
        return;
      this._action();
    }

    public void MarkForDeletion() => this._reference = (WeakReference) null;
  }
}
