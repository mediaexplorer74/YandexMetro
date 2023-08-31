// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Gestures.InputCompletedArgs
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System.Windows;

namespace Microsoft.Phone.Gestures
{
  internal abstract class InputCompletedArgs : InputBaseArgs
  {
    public abstract Point TotalTranslation { get; }

    public abstract Point FinalLinearVelocity { get; }

    public abstract bool IsInertial { get; }

    protected InputCompletedArgs(UIElement source, Point origin)
      : base(source, origin)
    {
    }
  }
}
