// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Gestures.ManipulationGestureHelper
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Phone.Gestures
{
  internal class ManipulationGestureHelper : GestureHelper
  {
    public ManipulationGestureHelper(UIElement target, bool shouldHandleAllDrags)
      : base(target, shouldHandleAllDrags)
    {
    }

    protected override void HookEvents()
    {
      this.Target.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.Target_ManipulationStarted);
      this.Target.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.Target_ManipulationDelta);
      this.Target.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.Target_ManipulationCompleted);
    }

    private void Target_ManipulationStarted(object sender, ManipulationStartedEventArgs e) => this.NotifyDown((InputBaseArgs) new ManipulationGestureHelper.ManipulationBaseArgs(e));

    private void Target_ManipulationDelta(object sender, ManipulationDeltaEventArgs e) => this.NotifyMove((InputDeltaArgs) new ManipulationGestureHelper.ManipulationDeltaArgs(e));

    private void Target_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e) => this.NotifyUp((InputCompletedArgs) new ManipulationGestureHelper.ManiulationCompletedArgs(e));

    private class ManipulationBaseArgs : InputBaseArgs
    {
      public ManipulationBaseArgs(ManipulationStartedEventArgs args)
        : base(args.ManipulationContainer, args.ManipulationOrigin)
      {
      }
    }

    private class ManipulationDeltaArgs : InputDeltaArgs
    {
      private ManipulationDeltaEventArgs _args;

      public ManipulationDeltaArgs(ManipulationDeltaEventArgs args)
        : base(args.ManipulationContainer, args.ManipulationOrigin)
      {
        this._args = args;
      }

      public override Point DeltaTranslation => this._args.DeltaManipulation.Translation;

      public override Point CumulativeTranslation => this._args.CumulativeManipulation.Translation;

      public override Point ExpansionVelocity => this._args.Velocities.ExpansionVelocity;

      public override Point LinearVelocity => this._args.Velocities.LinearVelocity;
    }

    private class ManiulationCompletedArgs : InputCompletedArgs
    {
      private ManipulationCompletedEventArgs _args;

      public ManiulationCompletedArgs(ManipulationCompletedEventArgs args)
        : base(args.ManipulationContainer, args.ManipulationOrigin)
      {
        this._args = args;
      }

      public override Point TotalTranslation => this._args.TotalManipulation.Translation;

      public override Point FinalLinearVelocity => this._args.FinalVelocities != null ? this._args.FinalVelocities.LinearVelocity : new Point(0.0, 0.0);

      public override bool IsInertial => this._args.IsInertial;
    }
  }
}
