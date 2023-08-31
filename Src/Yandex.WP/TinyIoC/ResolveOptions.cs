// Decompiled with JetBrains decompiler
// Type: TinyIoC.ResolveOptions
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

namespace TinyIoC
{
  public sealed class ResolveOptions
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
