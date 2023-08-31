// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.PhoneHelper
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Phone.Controls
{
  internal static class PhoneHelper
  {
    public const double SipLandscapeHeight = 259.0;
    public const double SipPortraitHeight = 339.0;
    public const double SipTextCompletionHeight = 62.0;

    public static bool TryGetPhoneApplicationFrame(out PhoneApplicationFrame phoneApplicationFrame)
    {
      phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;
      return phoneApplicationFrame != null;
    }

    public static bool IsPortrait(this PhoneApplicationFrame phoneApplicationFrame) => ((PageOrientation) 13 & phoneApplicationFrame.Orientation) == phoneApplicationFrame.Orientation;

    public static double GetUsefulWidth(this PhoneApplicationFrame phoneApplicationFrame) => !phoneApplicationFrame.IsPortrait() ? ((FrameworkElement) phoneApplicationFrame).ActualHeight : ((FrameworkElement) phoneApplicationFrame).ActualWidth;

    public static double GetUsefulHeight(this PhoneApplicationFrame phoneApplicationFrame) => !phoneApplicationFrame.IsPortrait() ? ((FrameworkElement) phoneApplicationFrame).ActualWidth : ((FrameworkElement) phoneApplicationFrame).ActualHeight;

    public static Size GetUsefulSize(this PhoneApplicationFrame phoneApplicationFrame) => new Size(phoneApplicationFrame.GetUsefulWidth(), phoneApplicationFrame.GetUsefulHeight());

    private static bool TryGetFocusedTextBox(out TextBox textBox)
    {
      textBox = FocusManager.GetFocusedElement() as TextBox;
      return textBox != null;
    }

    public static bool IsSipShown() => PhoneHelper.TryGetFocusedTextBox(out TextBox _);

    public static bool IsSipTextCompletionShown(this TextBox textBox)
    {
      if (textBox.InputScope == null)
        return false;
      foreach (InputScopeName name in (IEnumerable) textBox.InputScope.Names)
      {
        switch (name.NameValue - 49)
        {
          case 0:
          case 1:
            return true;
          default:
            continue;
        }
      }
      return false;
    }

    public static Size GetSipCoveredSize(this PhoneApplicationFrame phoneApplicationFrame)
    {
      if (!PhoneHelper.IsSipShown())
        return new Size(0.0, 0.0);
      double usefulWidth = phoneApplicationFrame.GetUsefulWidth();
      double num = phoneApplicationFrame.IsPortrait() ? 339.0 : 259.0;
      TextBox textBox;
      if (PhoneHelper.TryGetFocusedTextBox(out textBox) && textBox.IsSipTextCompletionShown())
        num += 62.0;
      return new Size(usefulWidth, num);
    }

    public static Size GetSipUncoveredSize(this PhoneApplicationFrame phoneApplicationFrame) => new Size(phoneApplicationFrame.GetUsefulWidth(), phoneApplicationFrame.GetUsefulHeight() - phoneApplicationFrame.GetSipCoveredSize().Height);
  }
}
