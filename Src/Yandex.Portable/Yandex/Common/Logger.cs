// Decompiled with JetBrains decompiler
// Type: Yandex.Common.Logger
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using Yandex.Log.Interfaces;

namespace Yandex.Common
{
  public static class Logger
  {
    private static readonly List<ILogger> _Listeners = new List<ILogger>();

    public static void AddListener([NotNull] ILogger listener)
    {
      if (listener == null)
        throw new ArgumentNullException(nameof (listener));
      lock (Logger._Listeners)
      {
        if (Logger._Listeners.Contains(listener))
          return;
        Logger._Listeners.Add(listener);
      }
    }

    public static void RemoveListener([NotNull] ILogger listener)
    {
      if (listener == null)
        throw new ArgumentNullException(nameof (listener));
      lock (Logger._Listeners)
        Logger._Listeners.Remove(listener);
    }

    public static void ClearListeners()
    {
      lock (Logger._Listeners)
        Logger._Listeners.Clear();
    }

    public static void TrackFeature(string featureName)
    {
    }

    public static void TrackException(Exception exception) => Logger.SendError(exception);

    public static void SendActivity(string activityName)
    {
      ILogger[] listenersCopy;
      lock (Logger._Listeners)
        listenersCopy = Logger._Listeners.ToArray();
      ThreadPool.QueueUserWorkItem((WaitCallback) (nothing =>
      {
        foreach (ILogger logger in listenersCopy)
        {
          try
          {
            logger.SendActivity(activityName);
          }
          catch
          {
          }
        }
      }));
    }

    public static void SendError([NotNull] Exception ex)
    {
      if (ex == null)
        throw new ArgumentNullException(nameof (ex));
      ILogger[] listenersCopy;
      lock (Logger._Listeners)
        listenersCopy = Logger._Listeners.ToArray();
      ThreadPool.QueueUserWorkItem((WaitCallback) (nothing =>
      {
        foreach (ILogger logger in listenersCopy)
        {
          try
          {
            logger.SendError(ex);
          }
          catch
          {
          }
        }
      }));
    }

    public static void SendCrash(object args)
    {
      ILogger[] listenersCopy;
      lock (Logger._Listeners)
        listenersCopy = Logger._Listeners.ToArray();
      ThreadPool.QueueUserWorkItem((WaitCallback) (nothing =>
      {
        foreach (ILogger logger in listenersCopy)
        {
          try
          {
            logger.SendCrash(args);
          }
          catch
          {
          }
        }
      }));
    }
  }
}
