// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.Interfaces.IApplicationInformation
// Assembly: Yandex.App.Information.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1BBDB095-C38E-4D74-91B1-61B6F357D2E7
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.App.Information.WP.dll

using System;
using System.Windows.Media.Imaging;

namespace Yandex.App.Information.Interfaces
{
  public interface IApplicationInformation
  {
    Version Version { get; }

    DateTime DateCreated { get; }

    string LicenseAgreementUri { get; }

    int PublishYear { get; }

    BitmapImage Image { get; }

    string ApplicationName { get; }

    string Uuid { get; }

    string FeedbackEmail { get; }
  }
}
