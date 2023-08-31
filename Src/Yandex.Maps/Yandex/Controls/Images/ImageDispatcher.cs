// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Images.ImageDispatcher
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yandex.Controls.Images.Interfaces;
using Yandex.Ioc;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Controls.Images
{
  internal class ImageDispatcher : IImageDispatcher
  {
    private const int MaxRequestCount = 5;
    private static int _requestCount;
    private static readonly object RequestSyncObject = new object();
    private static readonly object SyncObject = new object();
    private static readonly Queue<ImageQueueItem> Requests = new Queue<ImageQueueItem>();
    private static bool _exit;
    private readonly IBitmapRepository<Uri> _bitmapRepository;
    private readonly IBitmapFactory _bitmapFactory;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IWebClientFactory _webClientFactory;

    public ImageDispatcher(
      IThread thread,
      IUiDispatcher uiDispatcher,
      IWebClientFactory webClientFactory,
      IBitmapRepository<Uri> bitmapRepository,
      IBitmapFactory bitmapFactory)
    {
      if (thread == null)
        throw new ArgumentNullException(nameof (thread));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      if (webClientFactory == null)
        throw new ArgumentNullException(nameof (webClientFactory));
      if (bitmapRepository == null)
        throw new ArgumentNullException(nameof (bitmapRepository));
      if (bitmapFactory == null)
        throw new ArgumentNullException(nameof (bitmapFactory));
      this._uiDispatcher = uiDispatcher;
      this._webClientFactory = webClientFactory;
      this._bitmapRepository = bitmapRepository;
      this._bitmapFactory = bitmapFactory;
      thread.Start(new Action(this.Worker));
      Application.Current.Exit += new EventHandler(ImageDispatcher.ApplicationExit);
    }

    public ImageDispatcher()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IThread>(), IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>(), (IWebClientFactory) IocSingleton<ControlsIocInitializer>.Resolve<IMapWebClientFactory>(), IocSingleton<ControlsIocInitializer>.Resolve<IBitmapRepository<Uri>>(), IocSingleton<ControlsIocInitializer>.Resolve<IBitmapFactory>())
    {
    }

    private static void ApplicationExit(object sender, EventArgs e)
    {
      ImageDispatcher._exit = true;
      ImageDispatcher.PulseMonitor();
    }

    public static void Enqueue(ImageQueueItem imageQueueItem)
    {
      lock (ImageDispatcher.RequestSyncObject)
        ImageDispatcher.Requests.Enqueue(imageQueueItem);
      ImageDispatcher.PulseMonitor();
    }

    private void Worker()
    {
      while (!ImageDispatcher._exit)
      {
        lock (ImageDispatcher.SyncObject)
        {
          int length = ImageDispatcher.Requests.Count;
          if (length == 0 || ImageDispatcher._requestCount >= 5)
            Monitor.Wait(ImageDispatcher.SyncObject);
          if (length > 0)
          {
            int num = 5 - ImageDispatcher._requestCount;
            if (num > 0)
            {
              if (length > num)
                length = num;
              ImageQueueItem[] imageQueueItemArray = new ImageQueueItem[length];
              lock (ImageDispatcher.RequestSyncObject)
              {
                for (int index = 0; index < length; ++index)
                  imageQueueItemArray[index] = ImageDispatcher.Requests.Dequeue();
              }
              foreach (ImageQueueItem imageQueueItem in imageQueueItemArray)
              {
                ImageQueueItem loopItem = imageQueueItem;
                this._uiDispatcher.BeginInvoke((Action) (() =>
                {
                  BitmapSource bitmap = this._bitmapRepository[loopItem.Uri];
                  if (bitmap == null)
                  {
                    this.LoadImage(loopItem);
                  }
                  else
                  {
                    if (loopItem.Image == null)
                      return;
                    ImageDispatcher.SetImageSource(loopItem.Image, (ImageSource) bitmap);
                  }
                }));
              }
            }
          }
        }
      }
    }

    private void LoadImage(ImageQueueItem item)
    {
      if (item.Uri.IsAbsoluteUri)
      {
        ++ImageDispatcher._requestCount;
        IHttpWebRequest getHttpWebRequest = this._webClientFactory.CreateGetHttpWebRequest(item.Uri);
        getHttpWebRequest.AllowReadStreamBuffering = true;
        getHttpWebRequest.BeginGetResponse(new AsyncCallback(this.ProcessImageResponse), (object) new ImageResponseItem(item, getHttpWebRequest));
      }
      else
      {
        Image image = item.Image;
        if (image == null)
          return;
        ImageQueueItem closureItem = item;
        this._uiDispatcher.BeginInvoke((Action) (() => ImageDispatcher.SetImageSource(image, (ImageSource) new BitmapImage(closureItem.Uri))));
      }
    }

    private void ProcessImageResponse(IAsyncResult result)
    {
      ImageResponseItem asyncState = result.AsyncState as ImageResponseItem;
      Image image = asyncState.Item.Image;
      if (image == null)
      {
        --ImageDispatcher._requestCount;
      }
      else
      {
        IHttpWebRequest webRequest = asyncState.WebRequest;
        byte[] imageBuffer;
        try
        {
          using (IWebResponse response = webRequest.EndGetResponse(result))
          {
            using (Stream responseStream = response.GetResponseStream())
            {
              using (MemoryStream memoryStream = new MemoryStream())
              {
                responseStream.CopyTo((Stream) memoryStream);
                imageBuffer = memoryStream.GetBuffer();
              }
            }
          }
        }
        catch (Exception ex)
        {
          --ImageDispatcher._requestCount;
          return;
        }
        this._uiDispatcher.Invoke((SendOrPostCallback) (data => ImageDispatcher.SetImageSource(image, (ImageSource) this._bitmapFactory.GetBitmap(imageBuffer, 0, imageBuffer.Length))), (object) null);
        --ImageDispatcher._requestCount;
        ImageDispatcher.PulseMonitor();
        this._bitmapRepository.WriteItem(asyncState.Item.Uri, TimeSpan.FromHours(12.0), imageBuffer);
      }
    }

    private static void SetImageSource(Image image, ImageSource bitmap)
    {
      image.Source = (ImageSource) null;
      image.Source = bitmap;
      ((UIElement) image).InvalidateArrange();
      ((UIElement) image).InvalidateMeasure();
    }

    private static void PulseMonitor()
    {
      if (!Monitor.TryEnter(ImageDispatcher.SyncObject))
        return;
      Monitor.Pulse(ImageDispatcher.SyncObject);
      Monitor.Exit(ImageDispatcher.SyncObject);
    }
  }
}
