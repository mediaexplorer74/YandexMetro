﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.IJoinObserver
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Joins
{
  internal interface IJoinObserver : IDisposable
  {
    void Subscribe(object gate);

    void Dequeue();
  }
}