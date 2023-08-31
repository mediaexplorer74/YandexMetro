// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Navigation.NavigationService
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Yandex.Serialization.Interfaces;

namespace Yandex.Controls.Navigation
{
  internal class NavigationService : INavigationService
  {
    private const string SettingsKey = "navigation";
    private readonly IGenericXmlSerializer<NavigationState[]> _modelSerializer;
    private NavigationState _stateToPass;
    private bool _isActivating;
    private bool _isRecursiveNavigateBack;
    private double _opacityBeforeRecursiveNavigateBack;
    private PhoneApplicationFrame _rootFrame;
    private Stack<NavigationState> _states = new Stack<NavigationState>();
    private UriMapper _uriMapper;

    public NavigationService(
      IGenericXmlSerializer<NavigationState[]> modelSerializer)
    {
      this._modelSerializer = modelSerializer;
    }

    private UriMapper UriMapper => this._uriMapper ?? (this._uriMapper = (UriMapper) Application.Current.Resources[(object) nameof (UriMapper)]);

    internal PhoneApplicationPage Page => ((DependencyObject) this.RootVisual).GetVisualDescendants().OfType<PhoneApplicationPage>().FirstOrDefault<PhoneApplicationPage>();

    internal Frame RootVisual => Application.Current.RootVisual as Frame;

    private NavigationState CurrentState => this._states.Peek();

    public virtual void Init(PhoneApplicationFrame rootFrame, NavigationState initialState)
    {
      this._rootFrame = rootFrame;
      this._states.Push(initialState);
      ((Frame) rootFrame).Navigated += new NavigatedEventHandler(this.Navigated);
    }

    public void LoadStates()
    {
      this._isActivating = true;
      NavigationState[] source = (NavigationState[]) null;
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        if (storeForApplication.FileExists("navigation"))
        {
          using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream("navigation", (FileMode) 3, (FileAccess) 1, (FileShare) 0, storeForApplication))
          {
            try
            {
              source = this._modelSerializer.Deserialize((Stream) storageFileStream);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      if (source == null)
        return;
      this._states = new Stack<NavigationState>(((IEnumerable<NavigationState>) source).Reverse<NavigationState>());
    }

    public virtual void Navigate(NavigationState state)
    {
      if (this.Page == null || !NavigationService.CheckStateUri(this.CurrentState, this.TryGetMappedUri(((System.Windows.Controls.Page) this.Page).NavigationService.Source)))
      {
        this._stateToPass = state;
      }
      else
      {
        if (this._states.Any<NavigationState>((Func<NavigationState, bool>) (s => s.Uri == state.Uri)))
        {
          if (this.CurrentState.Uri == state.Uri)
          {
            state.TryInitialize(this.Page, NavigationType.Default);
          }
          else
          {
            this._opacityBeforeRecursiveNavigateBack = ((UIElement) this._rootFrame).Opacity;
            NavigationState navigationState = this._states.ElementAtOrDefault<NavigationState>(1);
            if (navigationState != null && navigationState.Uri != state.Uri)
              ((UIElement) this._rootFrame).Opacity = 0.0;
            this._isRecursiveNavigateBack = true;
            this.RemoveStatesTill(state.Uri);
            try
            {
              if (((Frame) this._rootFrame).CanGoBack)
                ((Frame) this._rootFrame).GoBack();
            }
            catch (InvalidOperationException ex)
            {
            }
          }
          this._states.Pop();
        }
        else
        {
          try
          {
            if (!((System.Windows.Controls.Page) this.Page).NavigationService.Navigate(state.Uri))
              return;
          }
          catch (Exception ex)
          {
            return;
          }
        }
        this._states.Push(state);
      }
    }

    public void SaveStates()
    {
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        using (IsolatedStorageFileStream outStream = new IsolatedStorageFileStream("navigation", (FileMode) 2, (FileAccess) 2, (FileShare) 0, storeForApplication))
        {
          try
          {
            this._modelSerializer.Serialize(this._states.ToArray(), (Stream) outStream);
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    public bool CanGoBack => ((System.Windows.Controls.Page) this.Page).NavigationService.CanGoBack;

    public void GoBack() => ((System.Windows.Controls.Page) this.Page).NavigationService.GoBack();

    public void RemoveBackEntry()
    {
      JournalEntry removedEntry = this._rootFrame.RemoveBackEntry();
      NavigationState navigationState = this._states.SingleOrDefault<NavigationState>((Func<NavigationState, bool>) (s => NavigationService.CheckStateUri(s, removedEntry.Source)));
      if (navigationState == null)
        return;
      List<NavigationState> list = this._states.ToList<NavigationState>();
      list.Remove(navigationState);
      this._states = new Stack<NavigationState>(list.Reverse<NavigationState>());
    }

    private void Navigated(object sender, NavigationEventArgs e)
    {
      Uri uri = this.TryGetMappedUri(e.Uri);
      if (uri.IsAbsoluteUri)
        return;
      PhoneApplicationPage content = (PhoneApplicationPage) e.Content;
      if (content == null)
        return;
      bool flag = ((FrameworkElement) content).DataContext == null || e.NavigationMode != 1 || this._isRecursiveNavigateBack;
      if (this._isRecursiveNavigateBack)
      {
        if (NavigationService.CheckStateUri(this.CurrentState, uri))
        {
          this._isRecursiveNavigateBack = false;
          ((UIElement) this._rootFrame).Opacity = this._opacityBeforeRecursiveNavigateBack;
        }
        else
        {
          ((Frame) this._rootFrame).GoBack();
          return;
        }
      }
      if (this._states.Any<NavigationState>((Func<NavigationState, bool>) (s => NavigationService.CheckStateUri(s, uri))))
        this.RemoveStatesTill(uri);
      NavigationType navigationType = e.NavigationMode == null || e.NavigationMode == 3 ? NavigationType.Reset : (!this._isActivating ? NavigationType.Default : NavigationType.Restore);
      if (flag && !this.CurrentState.TryInitialize(content, navigationType) && ((Frame) this._rootFrame).CanGoBack)
        ((Frame) this._rootFrame).GoBack();
      else if (this._stateToPass != null)
      {
        NavigationState stateToPass = this._stateToPass;
        this._stateToPass = (NavigationState) null;
        this.Navigate(stateToPass);
      }
      this._isActivating = false;
    }

    private static bool CheckStateUri(NavigationState state, Uri uri) => state.Uri == uri || uri.OriginalString.Contains(state.Uri.OriginalString);

    private Uri TryGetMappedUri(Uri uri) => this.UriMapper == null ? uri : ((UriMapperBase) this.UriMapper).MapUri(uri);

    private void RemoveStatesTill(Uri uri)
    {
      while (this._states.Count > 0 && !NavigationService.CheckStateUri(this._states.Peek(), uri))
      {
        if (this._states.Pop() is IDisposable disposable)
          disposable.Dispose();
      }
    }
  }
}
