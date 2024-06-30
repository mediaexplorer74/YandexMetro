// Y.UI.Common.BaseApplicationService`2


using GalaSoft.MvvmLight.Messaging;
//using Microsoft.Phone.Shell;
using System;
using System.Windows;
//using System.Windows.Navigation;
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

        /*
    public void ApplicationLaunching(LaunchingEventArgs e) 
            => this.OnApplicationLoading();

    public void ApplicationActivated(ActivatedEventArgs e) 
            => this.OnApplicationActivated(e.IsApplicationInstancePreserved);

    public void ApplicationDeactivated(DeactivatedEventArgs e) 
            => this.OnApplicationDeactivated();

    public void ApplicationClosing(ClosingEventArgs e) 
            => this.OnApplicationClosing();

    public void HandleException(ApplicationUnhandledExceptionEventArgs e) 
            => e.Handled = this.OnHandleException(e.ExceptionObject);

    public void HandleException(NavigationFailedEventArgs e) 
            => e.Handled = this.OnHandleException(e.Exception);
        */

    public void HandleException(Exception ex) => this.OnHandleException(ex);

    protected /*override*/ void OnApplicationLoading()
    {
      this._isAppActive = true;
      this.LoadSettings();
      Messenger.Default.Send<ApplicationActivatedMessage>(new ApplicationActivatedMessage());
    }

    protected /*override*/ void OnApplicationClosing()
    {
      if (this._appClosingFinished)
        return;
      this.SaveSettings();
      this._appClosingFinished = true;
      this._isAppActive = false;
    }

    protected /*override*/ void OnApplicationActivated(bool isInstancePreserved)
    {
      this._isAppActive = true;
      if (!isInstancePreserved)
        this.LoadSettings();
      this.LoadState();
      Messenger.Default.Send<ApplicationActivatedMessage>(new ApplicationActivatedMessage());
    }

    protected /*override*/ void OnApplicationDeactivated()
    {
      this.SaveState();
      this.SaveSettings();
      this._isAppActive = false;
    }

    protected /*override*/ void LoadSettings()
    {
    }

    protected /*override*/ void SaveSettings()
    {
    }

    protected /*override*/ void LoadState() 
            => this.AppState = StateKeeper.Load<TAppState>("ApplicationState");

    protected /*override*/ void SaveState() 
            => StateKeeper.Save<TAppState>(this.AppState, "ApplicationState");

    protected /*override*/ bool OnHandleException(Exception exception) 
            => this.ProcessException(exception);

    private bool ProcessException(Exception exception)
    {
      this.OnApplicationClosing();
      return false;
    }
  }
}
