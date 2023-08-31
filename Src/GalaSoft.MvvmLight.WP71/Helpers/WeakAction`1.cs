// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakAction`1
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakAction<T> : WeakAction, IExecuteWithObject
  {
    private readonly System.Action<T> _action;

    public WeakAction(object target, System.Action<T> action)
      : base(target, (System.Action) null)
    {
      this._action = action;
    }

    public System.Action<T> Action => this._action;

    public new void Execute()
    {
      if (this._action == null || !this.IsAlive)
        return;
      this._action(default (T));
    }

    public void Execute(T parameter)
    {
      if (this._action == null || !this.IsAlive)
        return;
      this._action(parameter);
    }

    public void ExecuteWithObject(object parameter) => this.Execute((T) parameter);
  }
}
