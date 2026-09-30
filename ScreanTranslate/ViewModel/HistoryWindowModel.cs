using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScreanTranslate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreanTranslate.ViewModel
{
    public partial class HistoryWindowModel : ObservableObject
    {
        [ObservableProperty]
        private static ObservableCollection<DataDB> test;
        [ObservableProperty]
        private string? _selectedSortOption;
        public ObservableCollection<string> SortOptions { get; }
        public ICommand DeleteHistoryCommand { get; }
        private bool _disposed = false;

        public HistoryWindowModel()
        {
            Test = new ObservableCollection<DataDB>();
            Load();

            DeleteHistoryCommand = new DeleteHistoryCommand(this);

            SortOptions = new ObservableCollection<string>
        {
            "от новых",
            "от старых"
        };

            SelectedSortOption = SortOptions.FirstOrDefault();
        }

        private void Load()
        {
            var all = SQLite.SelectDB();

            foreach (DataDB db in all)
            {

                Test.Add(db);
            }
        }
        public void Delete()
        {
            SQLite sQLite = new SQLite();
            sQLite.DeleteDB();

            Test.Clear();
            Load();
        }

        public Visibility EmptyMessageVisibility => Test?.Count > 0 ? Visibility.Collapsed : Visibility.Visible;

        partial void OnTestChanged(ObservableCollection<DataDB> value)
        {
            OnPropertyChanged(nameof(EmptyMessageVisibility));
        }

        partial void OnSelectedSortOptionChanged(string? value)
        {
            if (value != null)
            {
                SortDateSelection();
            }
        }

        private void SortDateSelection()
        {
            if (string.IsNullOrEmpty(SelectedSortOption)) return;

            switch (SelectedSortOption)
            {
                case "от новых":
                    SQLite.DataSort = true;
                    Test.Clear();
                    Load();
                    break;
                case "от старых":
                    SQLite.DataSort = false;
                    Test.Clear();
                    Load();
                    break;
            }
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
                    Test?.Clear();
                    SortOptions?.Clear();
                }
                _disposed = true;
            }
        }

        ~HistoryWindowModel()
        {
            Dispose(false);
        }
    }
}
