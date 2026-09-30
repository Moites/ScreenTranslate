using ScreanTranslate.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;

namespace ScreanTranslate.View
{
    /// <summary>
    /// Логика взаимодействия для ChoiceAreaWindow.xaml
    /// </summary>
    public partial class ChoiceAreaWindow : Window
    {
        private ChoiceAreaWindowModel? _viewModel;
        public ChoiceAreaWindow()
        {
            InitializeComponent();

            _viewModel = new ChoiceAreaWindowModel();
            DataContext = _viewModel;
            _viewModel.SelectionCompleted += OnSelectionCompleted;
        }

        // Нажатие левой кнопки мыши (начало выделения)
        private void CanvasDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(ChoiceCanvas);
            var viewModel = (ChoiceAreaWindowModel)DataContext;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                viewModel.StartCommand.Execute(position);
            }
        }

        // Изменение положения мыши (изменение области выделения)
        private void CanvasMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(ChoiceCanvas);
            var viewModel = (ChoiceAreaWindowModel)DataContext;

            viewModel.UpdateSelectionCommand.Execute(position);
        }

        // Отжатие кнопки (окончание выделения)
        private void CanvasUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(ChoiceCanvas);
            var viewModel = (ChoiceAreaWindowModel)DataContext;

            if (e.ChangedButton == MouseButton.Left)
            {
                viewModel.CompleteSelectionCommand.Execute(position);
            }
        }

        // Закрытие окна
        private void OnSelectionCompleted(System.Drawing.Rectangle area)
        {
            Close();
        }

        // Очистка при закрытии
        protected override void OnClosed(System.EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SelectionCompleted -= OnSelectionCompleted;
                _viewModel.Dispose();
                _viewModel = null;
            }

            DataContext = null;

            ChoiceCanvas.Children.Clear();

            base.OnClosed(e);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
