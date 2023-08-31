// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Helpers.DraggableControlBehaviorHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows;
using Yandex.Media;

namespace Yandex.Maps.Helpers
{
  internal class DraggableControlBehaviorHelper
  {
    private readonly int _maxSpaceToShiftMap;
    private readonly int _oneStepShiftInScreenPixels;
    private readonly int _shiftGearsCount;
    private readonly int _spaceToShiftMapStep;

    public DraggableControlBehaviorHelper(
      int oneStepShiftInScreenPixels,
      int spaceToShiftMapStep,
      int shiftGearsCount)
    {
      if (oneStepShiftInScreenPixels == 0)
        throw new ArgumentOutOfRangeException(nameof (oneStepShiftInScreenPixels));
      if (spaceToShiftMapStep == 0)
        throw new ArgumentOutOfRangeException(nameof (spaceToShiftMapStep));
      if (shiftGearsCount == 0)
        throw new ArgumentOutOfRangeException(nameof (shiftGearsCount));
      this._oneStepShiftInScreenPixels = oneStepShiftInScreenPixels;
      this._spaceToShiftMapStep = spaceToShiftMapStep;
      this._shiftGearsCount = shiftGearsCount;
      this._maxSpaceToShiftMap = this._spaceToShiftMapStep * this._shiftGearsCount;
    }

    internal bool UpdateCurrentPosition(
      Thickness contentPadding,
      Rect viewport,
      ref Point currentPosition)
    {
      bool flag = false;
      if (this.UpdatePosition(currentPosition.X - viewport.Left - contentPadding.Left, ref currentPosition, (DraggableControlBehaviorHelper.UpdatePoint) ((ref Point point, int shift) => point.X -= (double) shift)))
        flag = true;
      else if (this.UpdatePosition(viewport.Right - currentPosition.X - contentPadding.Right, ref currentPosition, (DraggableControlBehaviorHelper.UpdatePoint) ((ref Point point, int shift) => point.X += (double) shift)))
        flag = true;
      if (this.UpdatePosition(currentPosition.Y - viewport.Top - contentPadding.Top, ref currentPosition, (DraggableControlBehaviorHelper.UpdatePoint) ((ref Point point, int shift) => point.Y -= (double) shift)))
        flag = true;
      else if (this.UpdatePosition(viewport.Bottom - currentPosition.Y - contentPadding.Bottom, ref currentPosition, (DraggableControlBehaviorHelper.UpdatePoint) ((ref Point point, int shift) => point.Y += (double) shift)))
        flag = true;
      return flag;
    }

    private bool UpdatePosition(
      double space,
      ref Point currentPositionPoint,
      DraggableControlBehaviorHelper.UpdatePoint update)
    {
      if (space > (double) this._maxSpaceToShiftMap)
        return false;
      int shift = this.GetShift(space);
      update(ref currentPositionPoint, shift);
      return true;
    }

    private int GetShift(double space)
    {
      double num = space / (double) this._spaceToShiftMapStep;
      if (num < 0.0)
        num = 0.0;
      return (int) ((double) this._oneStepShiftInScreenPixels * Math.Ceiling((double) this._shiftGearsCount - num));
    }

    private delegate void UpdatePoint(ref Point point, int shift);
  }
}
