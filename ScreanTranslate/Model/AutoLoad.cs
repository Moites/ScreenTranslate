using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreanTranslate.Model
{
    internal class AutoLoad
    {
        private static RegistryKey GetBaseKey()
        {
            return Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }

        private static string GetAppName()
        {
            return Path.GetFileName(GetAppPath());
        }

        private static string GetAppPath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        private static bool GetRuns(string _AppName = "")
        {
            RegistryKey r = GetBaseKey();
            if (_AppName == "")
                _AppName = Process.GetCurrentProcess().ProcessName + ".exe";
            try
            {
                return r.GetValueNames().Contains(_AppName);
            }
            finally
            {
                r.Close();
            }
        }

        private static void SetToRun(bool _Auto)
        {
            SetToAutoRun(_Auto);
        }

        public static void SetToAutoRun(bool _Auto)
        {
            RegistryKey r = GetBaseKey();
            try
            {
                string appName = "ScreenTranslateApp";
                string appPath = Process.GetCurrentProcess().MainModule.FileName;
                string registryValue = "\"" + appPath + "\"";

                if (_Auto)
                {
                    r.SetValue(appName, registryValue);
                }
                else
                {
                    r.DeleteValue(appName, false);
                }
            }
            finally
            {
                r.Close();
            }
        }

        public static bool AutoRunning
        {
            get
            {
                return GetRuns();
            }
            set
            {
                SetToRun(value);
            }
        }
    }
}
