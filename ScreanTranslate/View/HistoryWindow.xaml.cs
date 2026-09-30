using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ScreanTranslate.Model;
using ScreanTranslate.ViewModel;

namespace ScreanTranslate.View
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private HistoryWindowModel? _viewModel;

        public HistoryWindow()
        {
            InitializeComponent();

            _viewModel = new HistoryWindowModel();
            DataContext = _viewModel;

            // При закритии окна будет вызов метода с очисткой
            this.Closed += OnClosed;
        }

        // Очистка при закрытии
        private void OnClosed(object sender, EventArgs e)
        {
            HistoryListBox.ItemsSource = null;

            if (_viewModel != null)
            {
                _viewModel.Dispose();
                _viewModel = null;
            }

            DataContext = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}