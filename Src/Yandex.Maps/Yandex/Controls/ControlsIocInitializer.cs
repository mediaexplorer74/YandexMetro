// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ControlsIocInitializer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Controls.Images;
using Yandex.Controls.Images.Interfaces;
using Yandex.Ioc;
using Yandex.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Serialization;
using Yandex.Serialization.Interfaces;
using Yandex.StringUtils;
using Yandex.StringUtils.Interfaces;
using Yandex.Threading;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Controls
{
  internal class ControlsIocInitializer : IocInitializer
  {
    protected override void Initialize() => this.Container.Register<IUiDispatcher, UiDispatcher>(isSingleton: true).Register<IThreadPool, ThreadPool>(isSingleton: true).Register<IBitmapRepository<Uri>, BitmapRepository<Uri>>(isSingleton: true).Register<IFileStorage, FileStorage>(isSingleton: true).Register<IFileNameMapper<Uri>, FileNameMapper<Uri>>(isSingleton: true).Register<IBitmapFactory, BitmapFactory>(isSingleton: true).Register<IGenericXmlSerializer<RepositoryState>, SharpXmlSerializer<RepositoryState>>(isSingleton: true).Register<IWebClientFactory, WebClientFactory>(isSingleton: true).Register<IDistanceFormatterUtil, MetricDistanceFormatterUtil>(isSingleton: true).Register<ITimeFormatterUtil, TimeFormatterUtil>(isSingleton: true);
  }
}
