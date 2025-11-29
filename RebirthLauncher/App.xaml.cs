// RebirthLauncher/App.xaml.cs
using System;
using System.Net.Http;
using System.Windows;

namespace RebirthLauncher
{
    public partial class App : Application
    {
        // Shared HttpClient for the entire application
        public HttpClient HttpClient { get; }

        public App()
        {
            // Create a single HttpClient instance to be reused across the app.
            // Do not modify default headers here; callers will only read response bodies.
            HttpClient = new HttpClient();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Dispose the shared HttpClient on application exit.
            try
            {
                HttpClient?.Dispose();
            }
            catch
            {
                // Swallow disposal exceptions; nothing we can do at shutdown.
            }

            base.OnExit(e);
        }
    }
}