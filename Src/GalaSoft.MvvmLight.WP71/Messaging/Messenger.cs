// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.Messenger
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaSoft.MvvmLight.Messaging
{
  public class Messenger : IMessenger
  {
    private static Messenger _defaultInstance;
    private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsOfSubclassesAction;
    private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsStrictAction;

    public static Messenger Default
    {
      get
      {
        if (Messenger._defaultInstance == null)
          Messenger._defaultInstance = new Messenger();
        return Messenger._defaultInstance;
      }
    }

    public static void OverrideDefault(Messenger newMessenger) => Messenger._defaultInstance = newMessenger;

    public static void Reset() => Messenger._defaultInstance = (Messenger) null;

    public virtual void Register<TMessage>(object recipient, Action<TMessage> action) => this.Register<TMessage>(recipient, (object) null, false, action);

    public virtual void Register<TMessage>(
      object recipient,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action)
    {
      this.Register<TMessage>(recipient, (object) null, receiveDerivedMessagesToo, action);
    }

    public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action) => this.Register<TMessage>(recipient, token, false, action);

    public virtual void Register<TMessage>(
      object recipient,
      object token,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action)
    {
      Type key = typeof (TMessage);
      Dictionary<Type, List<Messenger.WeakActionAndToken>> dictionary;
      if (receiveDerivedMessagesToo)
      {
        if (this._recipientsOfSubclassesAction == null)
          this._recipientsOfSubclassesAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
        dictionary = this._recipientsOfSubclassesAction;
      }
      else
      {
        if (this._recipientsStrictAction == null)
          this._recipientsStrictAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
        dictionary = this._recipientsStrictAction;
      }
      List<Messenger.WeakActionAndToken> weakActionAndTokenList;
      if (!dictionary.ContainsKey(key))
      {
        weakActionAndTokenList = new List<Messenger.WeakActionAndToken>();
        dictionary.Add(key, weakActionAndTokenList);
      }
      else
        weakActionAndTokenList = dictionary[key];
      WeakAction<TMessage> weakAction = new WeakAction<TMessage>(recipient, action);
      Messenger.WeakActionAndToken weakActionAndToken = new Messenger.WeakActionAndToken()
      {
        Action = (WeakAction) weakAction,
        Token = token
      };
      weakActionAndTokenList.Add(weakActionAndToken);
      this.Cleanup();
    }

    public virtual void Send<TMessage>(TMessage message) => this.SendToTargetOrType<TMessage>(message, (Type) null, (object) null);

    public virtual void Send<TMessage, TTarget>(TMessage message) => this.SendToTargetOrType<TMessage>(message, typeof (TTarget), (object) null);

    public virtual void Send<TMessage>(TMessage message, object token) => this.SendToTargetOrType<TMessage>(message, (Type) null, token);

    public virtual void Unregister(object recipient)
    {
      Messenger.UnregisterFromLists(recipient, this._recipientsOfSubclassesAction);
      Messenger.UnregisterFromLists(recipient, this._recipientsStrictAction);
    }

    public virtual void Unregister<TMessage>(object recipient) => this.Unregister<TMessage>(recipient, (Action<TMessage>) null);

    public virtual void Unregister<TMessage>(object recipient, object token) => this.Unregister<TMessage>(recipient, token, (Action<TMessage>) null);

    public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
    {
      Messenger.UnregisterFromLists<TMessage>(recipient, action, this._recipientsStrictAction);
      Messenger.UnregisterFromLists<TMessage>(recipient, action, this._recipientsOfSubclassesAction);
      this.Cleanup();
    }

    public virtual void Unregister<TMessage>(
      object recipient,
      object token,
      Action<TMessage> action)
    {
      Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsStrictAction);
      Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsOfSubclassesAction);
      this.Cleanup();
    }

    private static void CleanupList(
      IDictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      if (lists == null)
        return;
      List<Type> typeList = new List<Type>();
      foreach (KeyValuePair<Type, List<Messenger.WeakActionAndToken>> list in (IEnumerable<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>) lists)
      {
        List<Messenger.WeakActionAndToken> weakActionAndTokenList = new List<Messenger.WeakActionAndToken>();
        foreach (Messenger.WeakActionAndToken weakActionAndToken in list.Value)
        {
          if (weakActionAndToken.Action == null || !weakActionAndToken.Action.IsAlive)
            weakActionAndTokenList.Add(weakActionAndToken);
        }
        foreach (Messenger.WeakActionAndToken weakActionAndToken in weakActionAndTokenList)
          list.Value.Remove(weakActionAndToken);
        if (list.Value.Count == 0)
          typeList.Add(list.Key);
      }
      foreach (Type key in typeList)
        lists.Remove(key);
    }

    private static bool Implements(Type instanceType, Type interfaceType)
    {
      if ((object) interfaceType == null || (object) instanceType == null)
        return false;
      foreach (object obj in instanceType.GetInterfaces())
      {
        if (obj == (object) interfaceType)
          return true;
      }
      return false;
    }

    private static void SendToList<TMessage>(
      TMessage message,
      IEnumerable<Messenger.WeakActionAndToken> list,
      Type messageTargetType,
      object token)
    {
      if (list == null)
        return;
      foreach (Messenger.WeakActionAndToken weakActionAndToken in list.Take<Messenger.WeakActionAndToken>(list.Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>())
      {
        if (weakActionAndToken.Action is IExecuteWithObject action && weakActionAndToken.Action.IsAlive && weakActionAndToken.Action.Target != null && ((object) messageTargetType == null || (object) weakActionAndToken.Action.Target.GetType() == (object) messageTargetType || Messenger.Implements(weakActionAndToken.Action.Target.GetType(), messageTargetType)) && (weakActionAndToken.Token == null && token == null || weakActionAndToken.Token != null && weakActionAndToken.Token.Equals(token)))
          action.ExecuteWithObject((object) message);
      }
    }

    private static void UnregisterFromLists(
      object recipient,
      Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      if (recipient == null || lists == null || lists.Count == 0)
        return;
      lock (lists)
      {
        foreach (Type key in lists.Keys)
        {
          foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[key])
          {
            WeakAction action = weakActionAndToken.Action;
            if (action != null && recipient == action.Target)
              action.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(
      object recipient,
      Action<TMessage> action,
      Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      Type key = typeof (TMessage);
      if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(key))
        return;
      lock (lists)
      {
        foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[key])
        {
          if (weakActionAndToken.Action is WeakAction<TMessage> action1 && recipient == action1.Target && (action == null || action == action1.Action))
            weakActionAndToken.Action.MarkForDeletion();
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(
      object recipient,
      object token,
      Action<TMessage> action,
      Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      Type key = typeof (TMessage);
      if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(key))
        return;
      lock (lists)
      {
        foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[key])
        {
          if (weakActionAndToken.Action is WeakAction<TMessage> action1 && recipient == action1.Target && (action == null || action == action1.Action) && (token == null || token.Equals(weakActionAndToken.Token)))
            weakActionAndToken.Action.MarkForDeletion();
        }
      }
    }

    private void Cleanup()
    {
      Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>) this._recipientsOfSubclassesAction);
      Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>) this._recipientsStrictAction);
    }

    private void SendToTargetOrType<TMessage>(
      TMessage message,
      Type messageTargetType,
      object token)
    {
      Type type1 = typeof (TMessage);
      if (this._recipientsOfSubclassesAction != null)
      {
        foreach (Type type2 in this._recipientsOfSubclassesAction.Keys.Take<Type>(this._recipientsOfSubclassesAction.Count<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>()).ToList<Type>())
        {
          List<Messenger.WeakActionAndToken> list = (List<Messenger.WeakActionAndToken>) null;
          if ((object) type1 == (object) type2 || type1.IsSubclassOf(type2) || Messenger.Implements(type1, type2))
            list = this._recipientsOfSubclassesAction[type2];
          Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>) list, messageTargetType, token);
        }
      }
      if (this._recipientsStrictAction != null && this._recipientsStrictAction.ContainsKey(type1))
      {
        List<Messenger.WeakActionAndToken> list = this._recipientsStrictAction[type1];
        Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>) list, messageTargetType, token);
      }
      this.Cleanup();
    }

    private struct WeakActionAndToken
    {
      public WeakAction Action;
      public object Token;
    }
  }
}
