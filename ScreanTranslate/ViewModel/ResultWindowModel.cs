using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.ApplicationServices;
using ScreanTranslate.Model;
using ScreanTranslate.View;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScreanTranslate.ViewModel
{
    internal partial class ResultWindowModel : ObservableObject
    {
        [ObservableProperty]
        private string? _textContent;
        [ObservableProperty]
        private bool _isPopupOpen;

        private ResultWindow resultWindow;
        private bool _disposed = false;
        public ResultWindowModel()
        {
            CloseWindow();
        }
        partial void OnIsPopupOpenChanged(bool value)
        {
            if (!value)
            {
                CloseWindow();
            }
        }
        public async void WtiteText()
        {
            var conversionResult = await ConvertationText();

            if (string.IsNullOrEmpty(conversionResult))
            {
                CloseWindow();
            }
            else
            {
                Task[] tasks = { TranslateText(conversionResult), ShowWindow() };

                await Task.WhenAll(tasks);

                IsPopupOpen = true;

                _ = Task.Run(async () =>
                {
                    await SQLite.InsertDB(ConvertationTextAPI.Text, TranslateAPI.TranslateText, ConvertationTextAPI.Base64, DateTime.Now);
                });
            }
        }
        private Task ShowWindow()
        {
            if (resultWindow != null)
            {
                CloseWindow();
            }

            resultWindow = new ResultWindow();
            resultWindow.DataContext = this;

            resultWindow.Closed += (s, e) => {
                IsPopupOpen = false;
                CloseWindow();
            };

            resultWindow.Show();
            return Task.CompletedTask;
        }
        private void CloseWindow()
        {
            if (resultWindow != null)
            {
                resultWindow.DataContext = null;
                resultWindow.Close();
                resultWindow = null;
            }
        }
        private Task<string> ConvertationText()
        {
            return Task.FromResult(ConvertationTextAPI.Vision().Result);
        }
        private async Task TranslateText(string text)
        {
            var result = await TranslateAPI.Translate(text);
            TextContent = result;
        }
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
                    CloseWindow();
                    TextContent = null;
                }
                _disposed = true;
            }
        }
        ~ResultWindowModel()
        {
            Dispose(false);
        }
    }
}
