using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace ScreanTranslate.ViewModel
{
    public partial class ChoiceAreaWindowModel : ObservableObject
    {
        [ObservableProperty]
        private System.Windows.Point startPoint;
        [ObservableProperty]
        private System.Windows.Point currentPoint;
        [ObservableProperty]
        private bool isSelecting;
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private double _Left;
        [ObservableProperty]
        private double _Top;
        [ObservableProperty]
        private double _Width;
        [ObservableProperty]
        private double _Height;

        private bool _disposed = false;

        public event Action<Rectangle>? SelectionCompleted;

        [RelayCommand]
        private void Start(System.Windows.Point position)
        {
            StartPoint = position;
            CurrentPoint = position;
            IsSelecting = true;
            IsVisible = true;
            UpdateSelectionRectangle();
        }
        private void UpdateSelectionRectangle()
        {
            Left = Math.Min(StartPoint.X, CurrentPoint.X);
            Top = Math.Min(StartPoint.Y, CurrentPoint.Y);
            Width = Math.Abs(CurrentPoint.X - StartPoint.X);
            Height = Math.Abs(CurrentPoint.Y - StartPoint.Y);
        }

        [RelayCommand]
        private void UpdateSelection(System.Windows.Point position)
        {
            if (!IsSelecting)
            {
                return;
            }

            CurrentPoint = position;
            UpdateSelectionRectangle();
        }

        [RelayCommand]
        private void CompleteSelection(System.Windows.Point position)
        {
            if (!IsSelecting)
            {
                return;
            }

            CurrentPoint = position;
            UpdateSelectionRectangle();

            var Area = new Rectangle(
                (int)Left,
                (int)Top,
                (int)Width,
                (int)Height);

            SelectionCompleted?.Invoke(Area);

            ResetSelection();
        }
        private void ResetSelection()
        {
            IsSelecting = false;
            IsVisible = false;
            Width = 0; Height = 0;
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
                    SelectionCompleted = null;
                }
                _disposed = true;
            }
        }

        ~ChoiceAreaWindowModel()
        {
            Dispose(false);
        }
    }
}
