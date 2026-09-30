using CommunityToolkit.Mvvm.ComponentModel;
using ScreanTranslate.Model;
using System.Windows.Input;

namespace ScreanTranslate.ViewModel
{
    public partial class MainWindowModel : ObservableObject
    {
        public MainWindowModel(MainWindow mainWindow)
        { 
            globalHotkey = new GlobalHotkey();
            globalHotkey.StartHook();

            Properties.Settings.Default.Reload();

            IsSleshChecked = Properties.Settings.Default.Slesh;
            IsAltChecked = Properties.Settings.Default.Alt;
            IsCtrlChecked = Properties.Settings.Default.Ctrl;

            HistoryCommand = new HistoryCommand();
            ExitCommand = new ExitCommand();
            SleshCommand = new SleshCommand(() => globalHotkey, mainWindow);
            AltCommand = new AltCommand(() => globalHotkey, mainWindow);
            CtrlCommand = new CtrlCommand(() => globalHotkey, mainWindow);
        }

        [ObservableProperty]
        private bool _isSleshChecked;

        [ObservableProperty]
        private bool _isAltChecked;

        [ObservableProperty]
        private bool _isCtrlChecked;
        public GlobalHotkey globalHotkey { get; }
        public ICommand HistoryCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SleshCommand { get; }
        public ICommand AltCommand { get; }
        public ICommand CtrlCommand { get; }

        private bool _disposed = false;

        // Обработка очистки при закрытии
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    globalHotkey?.StopHook();
                    globalHotkey?.Dispose();
                }
                _disposed = true;
            }
        }
        ~MainWindowModel()
        {
            Dispose(false);
        }
    }
}
