// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventPattern`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive
{
  public class EventPattern<TSender, TEventArgs> : 
    IEquatable<EventPattern<TSender, TEventArgs>>,
    IEventPattern<TSender, TEventArgs>
    where TEventArgs : System.EventArgs
  {
    public EventPattern(TSender sender, TEventArgs e)
    {
      this.Sender = sender;
      this.EventArgs = e;
    }

    public TSender Sender { get; private set; }

    public TEventArgs EventArgs { get; private set; }

    public bool Equals(EventPattern<TSender, TEventArgs> other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return EqualityComparer<TSender>.Default.Equals(this.Sender, other.Sender) && EqualityComparer<TEventArgs>.Default.Equals(this.EventArgs, other.EventArgs);
    }

    public override bool Equals(object obj) => this.Equals(obj as EventPattern<TSender, TEventArgs>);

    public override int GetHashCode()
    {
      int hashCode1 = EqualityComparer<TSender>.Default.GetHashCode(this.Sender);
      int hashCode2 = EqualityComparer<TEventArgs>.Default.GetHashCode(this.EventArgs);
      return (hashCode1 << 5) + (hashCode1 ^ hashCode2);
    }

    public static bool operator ==(
      EventPattern<TSender, TEventArgs> first,
      EventPattern<TSender, TEventArgs> second)
    {
      return object.Equals((object) first, (object) second);
    }

    public static bool operator !=(
      EventPattern<TSender, TEventArgs> first,
      EventPattern<TSender, TEventArgs> second)
    {
      return !object.Equals((object) first, (object) second);
    }
  }
}
