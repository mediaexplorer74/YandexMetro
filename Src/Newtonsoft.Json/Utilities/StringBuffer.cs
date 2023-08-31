// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringBuffer
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal class StringBuffer
  {
    private char[] _buffer;
    private int _position;
    private static readonly char[] _emptyBuffer = new char[0];

    public int Position
    {
      get => this._position;
      set => this._position = value;
    }

    public StringBuffer() => this._buffer = StringBuffer._emptyBuffer;

    public StringBuffer(int initalSize) => this._buffer = new char[initalSize];

    public void Append(char value)
    {
      if (this._position == this._buffer.Length)
        this.EnsureSize(1);
      this._buffer[this._position++] = value;
    }

    public void Clear()
    {
      this._buffer = StringBuffer._emptyBuffer;
      this._position = 0;
    }

    private void EnsureSize(int appendLength)
    {
      char[] destinationArray = new char[(this._position + appendLength) * 2];
      Array.Copy((Array) this._buffer, (Array) destinationArray, this._position);
      this._buffer = destinationArray;
    }

    public override string ToString() => this.ToString(0, this._position);

    public string ToString(int start, int length) => new string(this._buffer, start, length);

    public char[] GetInternalBuffer() => this._buffer;
  }
}
