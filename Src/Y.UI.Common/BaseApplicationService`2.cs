// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.BaseApplicationService`2
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Navigation;
using Y.UI.Common.Utility;

namespace Y.UI.Common
{
  public class BaseApplicationService<TSettings, TAppState>
    where TSettings : class, new()
    where TAppState : class, new()
  {
    private bool _isAppActive;
    private bool _appClosingFinished;

    public TSettings AppSettings { get; private set; }

    public TAppState AppState { get; private set; }

    public virtual string AppVersion { get; private set; }

    public bool IsAppActive => this._isAppActive;

    public BaseApplicationService()
    {
      this.AppSettings = new TSettings();
      this.AppState = new TAppState();
    }

    public void ApplicationLaunching(LaunchingEventArgs e) => this.OnApplicationLoading();

    public void ApplicationActivated(ActivatedEventArgs e) => this.OnApplicationActivated(e.IsApplicationInstancePreserved);

    public void ApplicationDeactivated(DeactivatedEventArgs e) => this.OnApplicationDeactivated();

    public void ApplicationClosing(ClosingEventArgs e) => this.OnApplicationClosing();

    public void HandleException(ApplicationUnhandledExceptionEventArgs e) => e.Handled = this.OnHandleException(e.ExceptionObject);

    public void HandleException(NavigationFailedEventArgs e) => e.Handled = this.OnHandleException(e.Exception);

    public void HandleException(Exception ex) => this.OnHandleException(ex);

    protected virtual void OnApplicationLoading()
    {
      this._isAppActive = true;
      this.LoadSettings();
      Messenger.Default.Send<ApplicationActivatedMessage>(new ApplicationActivatedMessage());
    }

    protected virtual void OnApplicationClosing()
    {
      if (this._appClosingFinished)
        return;
      this.SaveSettings();
      this._appClosingFinished = true;
      this._isAppActive = false;
    }

    protected virtual void OnApplicationActivated(bool isInstancePreserved)
    {
      this._isAppActive = true;
      if (!isInstancePreserved)
        this.LoadSettings();
      this.LoadState();
      Messenger.Default.Send<ApplicationActivatedMessage>(new ApplicationActivatedMessage());
    }

    protected virtual void OnApplicationDeactivated()
    {
      this.SaveState();
      this.SaveSettings();
      this._isAppActive = false;
    }

    protected virtual void LoadSettings()
    {
    }

    protected virtual void SaveSettings()
    {
    }

    protected virtual void LoadState() => this.AppState = StateKeeper.Load<TAppState>("ApplicationState");

    protected virtual void SaveState() => StateKeeper.Save<TAppState>(this.AppState, "ApplicationState");

    protected virtual bool OnHandleException(Exception exception) => this.ProcessException(exception);

    private bool ProcessException(Exception exception)
    {
      this.OnApplicationClosing();
      return false;
    }
  }
}
