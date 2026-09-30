using ScreanTranslate.View;
using ScreanTranslate.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ScreanTranslate.Model
{
    public class DeleteHistoryCommand : ICommand
    {
        private HistoryWindowModel historyWindowModel;
        public event EventHandler? CanExecuteChanged;

        public DeleteHistoryCommand(HistoryWindowModel historyWindow)
        {
            historyWindowModel = historyWindow;
        }
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            historyWindowModel.Delete();
        }
    }
    public class HistoryCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var existingWindow = System.Windows.Application.Current.Windows.OfType<HistoryWindow>().FirstOrDefault();

            if (existingWindow != null)
            {
                existingWindow.Activate();
            }
            else
            {
                var historyWindow = new HistoryWindow();
                historyWindow.Show();
            }
        }
    }
    public class ExitCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => System.Windows.Application.Current.Shutdown();
    }
    public class SleshCommand : ICommand
    {
        private readonly MainWindow _mainWindow;
        private readonly Func<GlobalHotkey> _getHotkey;
        public SleshCommand(Func<GlobalHotkey> getHotkey, MainWindow mainWindow)
        {
            _getHotkey = getHotkey;
            _mainWindow = mainWindow;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var model = _mainWindow.DataContext as MainWindowModel;
            if (model == null) return;

            model.IsSleshChecked = true;
            model.IsAltChecked = false;
            model.IsCtrlChecked = false;

            Properties.Settings.Default.Slesh = true;
            Properties.Settings.Default.Alt = false;
            Properties.Settings.Default.Ctrl = false;
            Properties.Settings.Default.Save();

            var hotkey = _getHotkey();
            hotkey.RefreshHotkeysFromSettings();
        }
    }
    public class AltCommand : ICommand
    {
        private readonly MainWindow _mainWindow;
        private readonly Func<GlobalHotkey> _getHotkey;
        public AltCommand(Func<GlobalHotkey> getHotkey, MainWindow mainWindow)
        {
            _getHotkey = getHotkey;
            _mainWindow = mainWindow;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var model = _mainWindow.DataContext as MainWindowModel;
            if (model == null) return;

            model.IsSleshChecked = false;
            model.IsAltChecked = true;
            model.IsCtrlChecked = false;

            Properties.Settings.Default.Slesh = false;
            Properties.Settings.Default.Alt = true;
            Properties.Settings.Default.Ctrl = false;
            Properties.Settings.Default.Save();

            var hotkey = _getHotkey();
            hotkey.RefreshHotkeysFromSettings();
        }
    }
    public class CtrlCommand : ICommand
    {
        private readonly MainWindow _mainWindow;
        private readonly Func<GlobalHotkey> _getHotkey;
        public CtrlCommand(Func<GlobalHotkey> getHotkey, MainWindow mainWindow)
        {
            _getHotkey = getHotkey;
            _mainWindow = mainWindow;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var model = _mainWindow.DataContext as MainWindowModel;
            if (model == null) return;

            model.IsSleshChecked = false;
            model.IsAltChecked = false;
            model.IsCtrlChecked = true;

            Properties.Settings.Default.Slesh = false;
            Properties.Settings.Default.Alt = false;
            Properties.Settings.Default.Ctrl = true;
            Properties.Settings.Default.Save();

            var hotkey = _getHotkey();
            hotkey.RefreshHotkeysFromSettings();
        }
    }
}
