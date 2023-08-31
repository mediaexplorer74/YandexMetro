// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Gestures.DragEventArgs
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System.Windows;

namespace Microsoft.Phone.Gestures
{
  internal class DragEventArgs : GestureEventArgs
  {
    public bool IsTouchComplete { get; private set; }

    public Point DeltaDistance { get; private set; }

    public Point CumulativeDistance { get; internal set; }

    public DragEventArgs()
    {
    }

    public DragEventArgs(InputDeltaArgs args)
    {
      if (args == null)
        return;
      this.CumulativeDistance = args.CumulativeTranslation;
      this.DeltaDistance = args.DeltaTranslation;
    }

    public void MarkAsFinalTouchManipulation() => this.IsTouchComplete = true;
  }
}
