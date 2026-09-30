using ScreanTranslate.Model;
using ScreanTranslate.ViewModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreanTranslate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if(!Properties.Settings.Default.FirstRun)
            {
                AutoLoad.AutoRunning = true;

                Properties.Settings.Default.FirstRun = true;
                Properties.Settings.Default.Reset();
            }
            Properties.Settings.Default.Reload();
            InitializeComponent();
            DataContext = new MainWindowModel(this);
        }

        // Отработка закрытия окна
        protected override void OnClosed(EventArgs e)
        {
            var model = DataContext as MainWindowModel;

            if (model != null)
            {
                model.Dispose();
            }
            DataContext = null;

            if (MyNotifyIcon != null)
            {
                MyNotifyIcon.Dispose();
            }

            base.OnClosed(e);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}