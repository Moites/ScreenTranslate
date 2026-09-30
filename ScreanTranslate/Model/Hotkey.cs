using Gma.System.MouseKeyHook;
using Hardcodet.Wpf.TaskbarNotification;
using ScreanTranslate.View;
using ScreanTranslate.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;


namespace ScreanTranslate.Model
{
    public class GlobalHotkey : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IKeyboardEvents? _hook;
        private HashSet<Keys> _pressedKeys = new HashSet<Keys>();
        public List<Hotkey> Hotkeys { get; set; }

        private bool _disposed = false;

        public GlobalHotkey()
        {
            Hotkeys = new List<Hotkey>
            {
                new Hotkey { Key = Keys.Oem5, Alt = false, Ctrl = false, Status = Properties.Settings.Default.Slesh },
                new Hotkey { Key = Keys.Oem5, Alt = true, Ctrl = false, Status = Properties.Settings.Default.Alt },
                new Hotkey { Key = Keys.Oem5, Alt = false, Ctrl = true, Status = Properties.Settings.Default.Ctrl }
            };
        }
        public List<Hotkey> GetHotkey()
        {
            return Hotkeys;
        }

        public void StartHook()
        {
            _hook = Hook.GlobalEvents();

            _hook.KeyDown += OnKeyDown;
            _hook.KeyUp += OnKeyUp;
        }
        public void RefreshHotkeysFromSettings()
        {
            foreach (var hotkey in Hotkeys)
            {
                if (hotkey.Key == Keys.Oem5 && !hotkey.Ctrl && !hotkey.Alt)
                    hotkey.Status = Properties.Settings.Default.Slesh;
                else if (hotkey.Key == Keys.Oem5 && hotkey.Alt && !hotkey.Ctrl)
                    hotkey.Status = Properties.Settings.Default.Alt;
                else if (hotkey.Key == Keys.Oem5 && hotkey.Ctrl && !hotkey.Alt)
                    hotkey.Status = Properties.Settings.Default.Ctrl;
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _pressedKeys.Add(e.KeyCode);
            CheckHotKeys();
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _pressedKeys.Remove(e.KeyCode);
        }
        private bool IsKeyPressed(Keys key)
        {
            return _pressedKeys.Contains(key);
        }
        private void CheckHotKeys()
        {
            bool ctrl = IsKeyPressed(Keys.ControlKey) || IsKeyPressed(Keys.RControlKey) || IsKeyPressed(Keys.LControlKey);
            bool alt = IsKeyPressed(Keys.Menu) || IsKeyPressed(Keys.LMenu) || IsKeyPressed(Keys.RMenu);

            foreach (var hotkey in Hotkeys)
            {
                bool Pressed = _pressedKeys.Contains(hotkey.Key);

                if (Pressed && ctrl == hotkey.Ctrl && alt == hotkey.Alt)
                {
                    ExecuteHotkey(hotkey);
                    break;
                }
            }
        }
        private void ExecuteHotkey(Hotkey hotkey)
        {
            bool Check;

            var Window = System.Windows.Application.Current.Windows.OfType<ChoiceAreaWindow>().FirstOrDefault();
            if (Window != null)
            {
                Window.Close();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (hotkey.Key == Keys.Oem5 && hotkey.Ctrl == true && hotkey.Status == true)
                    {
                        Check = CopyScreen.MakeScreen();
                        if (Check)
                        {
                            ResultWindowModel resultWindowModel = new ResultWindowModel();
                            resultWindowModel.WtiteText();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (hotkey.Key == Keys.Oem5 && hotkey.Alt == true && hotkey.Status == true)
                    {
                        Check = CopyScreen.MakeScreen();
                        if (Check)
                        {
                            ResultWindowModel resultWindowModel = new ResultWindowModel();
                            resultWindowModel.WtiteText();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (hotkey.Key == Keys.Oem5 && hotkey.Status == true)
                    {
                        Check = CopyScreen.MakeScreen();
                        if (Check)
                        {
                            ResultWindowModel resultWindowModel = new ResultWindowModel();
                            resultWindowModel.WtiteText();
                        }
                        else
                        {
                            return;
                        }
                    }
                });
            }
        }
        public void StopHook()
        {
            if (_hook != null)
            {
                _hook.KeyDown -= OnKeyDown;
                _hook.KeyUp -= OnKeyUp;
                _hook = null;
            }

            _pressedKeys.Clear();
        }
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
                    StopHook();
                }

                _disposed = true;
            }
        }
        ~GlobalHotkey()
        {
            Dispose(false);
        }
    }
}
