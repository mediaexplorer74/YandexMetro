// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PushPinManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Maps.Events;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public class PushPinManager : IPushPinManager
  {
    private readonly Dictionary<string, List<PushPin>> _groups = new Dictionary<string, List<PushPin>>();

    public IDictionary<string, List<PushPin>> Groups => (IDictionary<string, List<PushPin>>) this._groups;

    public void SetPushPinGroups(PushPin pushpin, string[] groupKeys)
    {
      if (groupKeys == null)
        groupKeys = new string[0];
      groupKeys = ((IEnumerable<string>) groupKeys).Where<string>((Func<string, bool>) (key => key != null)).ToArray<string>();
      string[] array = this.Groups.Where<KeyValuePair<string, List<PushPin>>>((Func<KeyValuePair<string, List<PushPin>>, bool>) (group => group.Value.Contains(pushpin))).Select<KeyValuePair<string, List<PushPin>>, string>((Func<KeyValuePair<string, List<PushPin>>, string>) (group => group.Key)).ToArray<string>();
      foreach (string groupKey in ((IEnumerable<string>) array).Except<string>((IEnumerable<string>) groupKeys))
        this.RemovePushPin(pushpin, groupKey);
      foreach (string groupKey in ((IEnumerable<string>) groupKeys).Except<string>((IEnumerable<string>) array))
        this.AddPushPin(pushpin, groupKey);
    }

    public void CollapseAllPushpins()
    {
      foreach (PushPin pushPin in this.Groups.SelectMany<KeyValuePair<string, List<PushPin>>, PushPin>((Func<KeyValuePair<string, List<PushPin>>, IEnumerable<PushPin>>) (group => (IEnumerable<PushPin>) group.Value)))
        pushPin.ContentVisibility = (Visibility) 1;
    }

    private void AddPushPin(PushPin pushpin, [NotNull] string groupKey)
    {
      if (groupKey == null)
        throw new ArgumentNullException(nameof (groupKey));
      List<PushPin> pushPinList;
      if (!this.Groups.TryGetValue(groupKey, out pushPinList))
        pushPinList = this.Groups[groupKey] = new List<PushPin>();
      if (pushPinList.Contains(pushpin))
        throw new InvalidOperationException();
      pushPinList.Add(pushpin);
      this.OnPushPinAdded(pushpin, groupKey);
    }

    private void RemovePushPin(PushPin pushpin, [NotNull] string groupKey)
    {
      if (groupKey == null)
        throw new ArgumentNullException(nameof (groupKey));
      List<PushPin> pushPinList;
      if (!this.Groups.TryGetValue(groupKey, out pushPinList) || !pushPinList.Remove(pushpin))
        throw new InvalidOperationException();
      if (pushPinList.Count == 0)
        this.Groups.Remove(groupKey);
      this.OnPushPinRemoved(pushpin, groupKey);
    }

    public event EventHandler<GroupKeyArgs> PushPinAddedToGroup;

    public event EventHandler<GroupKeyArgs> PushPinRemovedFromGroup;

    private void OnPushPinAdded(PushPin pushpin, string groupKey)
    {
      EventHandler<GroupKeyArgs> pushPinAddedToGroup = this.PushPinAddedToGroup;
      if (pushPinAddedToGroup == null)
        return;
      pushPinAddedToGroup((object) pushpin, new GroupKeyArgs(groupKey));
    }

    private void OnPushPinRemoved(PushPin pushpin, string groupKey)
    {
      EventHandler<GroupKeyArgs> removedFromGroup = this.PushPinRemovedFromGroup;
      if (removedFromGroup == null)
        return;
      removedFromGroup((object) pushpin, new GroupKeyArgs(groupKey));
    }
  }
}
