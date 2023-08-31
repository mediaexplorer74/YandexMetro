using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Yandex.Metro
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая выполняемая строка разрабатываемого
        /// кода, поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем. Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.App
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll
/*
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Y.UI.Common;
using Y.UI.Common.Control;
using Y.UI.Common.Utility;
using Yandex.Metro.Logic;

namespace Yandex.Metro
{
    public class App : Application
    {
        private bool phoneApplicationInitialized;

        public PhoneApplicationFrame RootFrame { get; private set; }

        public App()
        {
            this.UnhandledException +=
                      new EventHandler<ApplicationUnhandledExceptionEventArgs>(
                    this.Application_UnhandledException);
            DispatcherHelper.Initialize();
            this.InitializeComponent();
            this.InitializePhoneApplication();
            PageNavigationService.Initialize(this.RootFrame);

            TiltEffect.TiltableItems.Add(typeof(TapButton));

            TiltEffect.TiltableItems.Add(typeof(RoundButton));
            if (!Debugger.IsAttached)
                return;
            Application.Current.Host.Settings.EnableFrameRateCounter = true;
            PhoneApplicationService.Current.UserIdleDetectionMode = (IdleDetectionMode)1;
        }

        private void Application_Launching(object sender, LaunchingEventArgs e) 
=> MetroService.Instance.ApplicationLaunching(e);

        private void Application_Activated(object sender, ActivatedEventArgs e) 
=> MetroService.Instance.ApplicationActivated(e);

        private void Application_Deactivated(object sender, DeactivatedEventArgs e) 
=> MetroService.Instance.ApplicationDeactivated(e);

        private void Application_Closing(object sender, ClosingEventArgs e) 
=> MetroService.Instance.ApplicationClosing(e);

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            MetroService.Instance.HandleException(e);
            if (!Debugger.IsAttached)
                return;
            Debugger.Break();
        }

        private void Application_UnhandledException(
          object sender,
          ApplicationUnhandledExceptionEventArgs e)
        {
            MetroService.Instance.HandleException(e);
            if (!Debugger.IsAttached)
                return;
            Debugger.Break();
        }

        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
                return;
            this.RootFrame = new PhoneApplicationFrame();
            ((Frame)this.RootFrame).Navigated += 
new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
            ((Frame)this.RootFrame).NavigationFailed += 
new NavigationFailedEventHandler(this.RootFrame_NavigationFailed);
            this.phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (this.RootVisual != this.RootFrame)
                this.RootVisual = (UIElement)this.RootFrame;
            ((Frame)this.RootFrame).Navigated -= new NavigatedEventHandler(
this.CompleteInitializePhoneApplication);
        }

      
    }
}
*/

