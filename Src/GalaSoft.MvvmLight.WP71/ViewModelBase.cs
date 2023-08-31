// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.ViewModelBase
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GalaSoft.MvvmLight
{
  public abstract class ViewModelBase : INotifyPropertyChanged, ICleanup, IDisposable
  {
    private static bool? _isInDesignMode;

    protected ViewModelBase()
      : this((IMessenger) null)
    {
    }

    protected ViewModelBase(IMessenger messenger) => this.MessengerInstance = messenger;

    public event PropertyChangedEventHandler PropertyChanged;

    public static bool IsInDesignModeStatic
    {
      get
      {
        if (!ViewModelBase._isInDesignMode.HasValue)
          ViewModelBase._isInDesignMode = new bool?(DesignerProperties.IsInDesignTool);
        return ViewModelBase._isInDesignMode.Value;
      }
    }

    public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

    protected IMessenger MessengerInstance { get; set; }

    protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
    {
      PropertyChangedMessage<T> message = new PropertyChangedMessage<T>((object) this, oldValue, newValue, propertyName);
      if (this.MessengerInstance != null)
        this.MessengerInstance.Send<PropertyChangedMessage<T>>(message);
      else
        Messenger.Default.Send<PropertyChangedMessage<T>>(message);
    }

    [Obsolete("This interface will be removed from ViewModelBase in a future version, use ICleanup.Cleanup instead.")]
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Cleanup();
    }

    public void Dispose() => this.Dispose(true);

    public virtual void Cleanup() => Messenger.Default.Unregister((object) this);

    protected virtual void RaisePropertyChanged<T>(
      string propertyName,
      T oldValue,
      T newValue,
      bool broadcast)
    {
      this.RaisePropertyChanged(propertyName);
      if (!broadcast)
        return;
      this.Broadcast<T>(oldValue, newValue, propertyName);
    }

    protected virtual void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    [DebuggerStepThrough]
    [Conditional("DEBUG")]
    public void VerifyPropertyName(string propertyName)
    {
      if ((object) this.GetType().GetProperty(propertyName) == null)
        throw new ArgumentException("Property not found", propertyName);
    }
  }
}
