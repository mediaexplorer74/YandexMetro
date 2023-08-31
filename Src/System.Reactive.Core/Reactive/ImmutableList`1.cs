// Decompiled with JetBrains decompiler
// Type: System.Reactive.ImmutableList`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal class ImmutableList<T>
  {
    private T[] data;

    public ImmutableList() => this.data = new T[0];

    public ImmutableList(T[] data) => this.data = data;

    public ImmutableList<T> Add(T value)
    {
      T[] objArray = new T[this.data.Length + 1];
      Array.Copy((Array) this.data, (Array) objArray, this.data.Length);
      objArray[this.data.Length] = value;
      return new ImmutableList<T>(objArray);
    }

    public ImmutableList<T> Remove(T value)
    {
      int num = this.IndexOf(value);
      if (num < 0)
        return this;
      T[] objArray = new T[this.data.Length - 1];
      Array.Copy((Array) this.data, 0, (Array) objArray, 0, num);
      Array.Copy((Array) this.data, num + 1, (Array) objArray, num, this.data.Length - num - 1);
      return new ImmutableList<T>(objArray);
    }

    public int IndexOf(T value)
    {
      for (int index = 0; index < this.data.Length; ++index)
      {
        if (this.data[index].Equals((object) value))
          return index;
      }
      return -1;
    }

    public T[] Data => this.data;
  }
}
