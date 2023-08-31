// Decompiled with JetBrains decompiler
// Type: Yandex.Log.CapptainLogger
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Capptain.Agent;
using JetBrains.Annotations;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Yandex.Log.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Log
{
  internal class CapptainLogger : ILogger
  {
    private readonly IUiDispatcher _uiDispatcher;
    private readonly object _initSyncObj = new object();
    private readonly Queue<Exception> _notSentExceptions = new Queue<Exception>();
    private string _currentActivity;
    private bool _inited;
    private string _appId;
    private string _appKey;

    public CapptainLogger([NotNull] IUiDispatcher uiDispatcher) => this._uiDispatcher = uiDispatcher != null ? uiDispatcher : throw new ArgumentNullException(nameof (uiDispatcher));

    public void Configure([CanBeNull] string uri, string ipToCountryUrl)
    {
      CapptainHttpConfig.LoggerUrl = uri;
      CapptainHttpConfig.IpToCountryUrl = ipToCountryUrl;
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        try
        {
          CapptainAgent.Init(this._appId, this._appKey, false);
        }
        catch
        {
        }
        lock (this._initSyncObj)
        {
          if (this._inited)
            return;
          this._inited = true;
          this.ResendNotSentInformation();
        }
      }));
    }

    public void Init(string appId, string appKey)
    {
      this._appKey = appKey;
      this._appId = appId;
    }

    public void SendActivity(string activityName)
    {
      lock (this._initSyncObj)
      {
        if (!this._inited)
        {
          this._currentActivity = activityName;
          return;
        }
      }
      try
      {
        CapptainAgent.StartActivity(activityName, (Dictionary<object, object>) null);
      }
      catch
      {
      }
    }

    public void SendError(Exception ex)
    {
      if (ex is ThreadAbortException)
        return;
      lock (this._initSyncObj)
      {
        if (!this._inited)
        {
          this._notSentExceptions.Enqueue(ex);
          return;
        }
      }
      try
      {
        string name;
        string message;
        string stackTrace;
        CapptainLoggerHelper.PrepareExceptionForCapptain(ex, out name, out message, out stackTrace);
        Dictionary<object, object> dictionary = new Dictionary<object, object>()
        {
          {
            (object) message,
            (object) stackTrace
          }
        };
        while ((ex = ex.InnerException) != null)
        {
          CapptainLoggerHelper.PrepareExceptionForCapptain(ex, out message, out stackTrace);
          dictionary[(object) message] = (object) stackTrace;
        }
        CapptainAgent.SendError(name, dictionary);
      }
      catch
      {
      }
    }

    public void SendCrash(object args)
    {
      lock (this._initSyncObj)
      {
        if (!this._inited)
          return;
      }
      try
      {
        CapptainAgent.SendCrash(args as ApplicationUnhandledExceptionEventArgs);
      }
      catch
      {
      }
    }

    public void OnAplicationActivated(object args)
    {
      try
      {
        CapptainAgent.OnTombstoned(args as ActivatedEventArgs);
      }
      catch
      {
      }
    }

    private void ResendNotSentInformation()
    {
      if (this._currentActivity != null)
      {
        this.SendActivity(this._currentActivity);
        this._currentActivity = (string) null;
      }
      while (this._notSentExceptions.Count > 0)
        this.SendError(this._notSentExceptions.Dequeue());
    }
  }
}
