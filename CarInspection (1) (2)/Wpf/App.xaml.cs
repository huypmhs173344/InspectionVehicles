using System;
using System.Windows;
using System.Windows.Threading;

namespace Wpf
{
    public partial class App : Application
    {
        public App()
        {
            // Handle any unhandled exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Đã xảy ra lỗi không mong muốn: {e.Exception.Message}",
                "Lỗi",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                MessageBox.Show(
                    $"Đã xảy ra lỗi nghiêm trọng: {exception.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up global exception handling for background threads
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(
                $"Đã xảy ra lỗi trong tiến trình nền: {e.Exception.Message}",
                "Lỗi",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            e.SetObserved();
        }
    }
}
