// Decompiled with JetBrains decompiler
// Type: TinyIoC.ResolveOptions
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace TinyIoC
{
  internal sealed class ResolveOptions
  {
    private static readonly ResolveOptions _Default = new ResolveOptions();
    private static readonly ResolveOptions _FailUnregisteredAndNameNotFound = new ResolveOptions()
    {
      NamedResolutionFailureAction = NamedResolutionFailureActions.Fail,
      UnregisteredResolutionAction = UnregisteredResolutionActions.Fail
    };
    private static readonly ResolveOptions _FailUnregisteredOnly = new ResolveOptions()
    {
      NamedResolutionFailureAction = NamedResolutionFailureActions.AttemptUnnamedResolution,
      UnregisteredResolutionAction = UnregisteredResolutionActions.Fail
    };
    private static readonly ResolveOptions _FailNameNotFoundOnly = new ResolveOptions()
    {
      NamedResolutionFailureAction = NamedResolutionFailureActions.Fail,
      UnregisteredResolutionAction = UnregisteredResolutionActions.AttemptResolve
    };
    private UnregisteredResolutionActions _UnregisteredResolutionAction;
    private NamedResolutionFailureActions _NamedResolutionFailureAction = NamedResolutionFailureActions.Fail;

    public UnregisteredResolutionActions UnregisteredResolutionAction
    {
      get => this._UnregisteredResolutionAction;
      set => this._UnregisteredResolutionAction = value;
    }

    public NamedResolutionFailureActions NamedResolutionFailureAction
    {
      get => this._NamedResolutionFailureAction;
      set => this._NamedResolutionFailureAction = value;
    }

    public static ResolveOptions Default => ResolveOptions._Default;

    public static ResolveOptions FailNameNotFoundOnly => ResolveOptions._FailNameNotFoundOnly;

    public static ResolveOptions FailUnregisteredAndNameNotFound => ResolveOptions._FailUnregisteredAndNameNotFound;

    public static ResolveOptions FailUnregisteredOnly => ResolveOptions._FailUnregisteredOnly;
  }
}
