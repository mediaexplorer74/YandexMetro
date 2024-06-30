// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.ViewModel.ProgressViewModel
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Y.UI.Common.Utility;
using Yandex.Metro.Resources;

namespace Yandex.Metro.ViewModel
{
  public class ProgressViewModel : ViewModelBase
  {
    private readonly ObservableCollection<string> _jobs = new ObservableCollection<string>();
    private bool _isMaploaded;
    private bool _progressIndicatorIsActive;
    private string _progressTitle;

    public bool IsMapLoading
    {
      get => this._isMaploaded;
      set
      {
        if (this._isMaploaded == value)
          return;
        this._isMaploaded = value;
        this.RaisePropertyChanged(nameof (IsMapLoading));
      }
    }

    public bool ProgressIndicatorIsActive
    {
      get => this._progressIndicatorIsActive;
      private set
      {
        if (this._progressIndicatorIsActive == value)
          return;
        this._progressIndicatorIsActive = value;
        this.RaisePropertyChanged(nameof (ProgressIndicatorIsActive));
      }
    }

    public string ProgressTitle
    {
      get => this._progressTitle;
      private set
      {
        if (this._progressTitle == value)
          return;
        this._progressTitle = value;
        this.RaisePropertyChanged(nameof (ProgressTitle));
      }
    }

    public ProgressViewModel() => this._jobs.CollectionChanged += (NotifyCollectionChangedEventHandler) ((s, e) =>
    {
      this.ProgressIndicatorIsActive = ((Collection<string>) this._jobs).Count > 0;
      if (((Collection<string>) this._jobs).Count <= 0)
        return;
      this.ProgressTitle = ((Collection<string>) this._jobs)[((Collection<string>) this._jobs).Count - 1];
    });

    public void StartJob(string jobName) => DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
    {
      if (!((Collection<string>) this._jobs).Contains(jobName))
        ((Collection<string>) this._jobs).Add(jobName);
      if (!(jobName == Localization.Progress_Map))
        return;
      this.IsMapLoading = true;
    }));

    public bool IsJobActive(string jobName) => ((Collection<string>) this._jobs).Contains(jobName);

    public void StopJob(string jobName) => DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
    {
      if (((Collection<string>) this._jobs).Contains(jobName))
        ((Collection<string>) this._jobs).Remove(jobName);
      if (!(jobName == Localization.Progress_Map))
        return;
      this.IsMapLoading = false;
    }));

    public void StopAllJobs() => DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => ((Collection<string>) this._jobs).Clear()));
  }
}
