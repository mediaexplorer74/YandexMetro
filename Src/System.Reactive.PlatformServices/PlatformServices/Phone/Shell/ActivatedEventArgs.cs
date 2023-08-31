// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.Phone.Shell.ActivatedEventArgs
// Assembly: System.Reactive.PlatformServices, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC39E7C4-BCC5-4024-9B94-3702D2ED3C79
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.PlatformServices.dll

using System.Reflection;

namespace System.Reactive.PlatformServices.Phone.Shell
{
  internal class ActivatedEventArgs : EventArgs
  {
    private static readonly Type s_aeaType = PhoneApplicationService.s_phshAsm.GetType("Microsoft.Phone.Shell.ActivatedEventArgs");
    private static readonly PropertyInfo s_aipProp = ActivatedEventArgs.s_aeaType.GetProperty(nameof (IsApplicationInstancePreserved), BindingFlags.Instance | BindingFlags.Public);
    private readonly object _target;

    public ActivatedEventArgs(object target) => this._target = target;

    public bool IsApplicationInstancePreserved => (bool) ActivatedEventArgs.s_aipProp.GetValue(this._target, (object[]) null);
  }
}
