using Hardcodet.Wpf.TaskbarNotification;
using ScreanTranslate.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ScreanTranslate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Mutex Mutex = new Mutex(true, "42ae83c2-03a0-472e-a2ea-41d69524a85b");

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }
    }
}
