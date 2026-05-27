using System.Windows;
using System;
using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            Loaded += async (s, e) => 
            {
                try
                {
                    await viewModel.LoadData();
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error loading data: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        errorMessage += $"\n\nInner Exception: {ex.InnerException.Message}";
                    }
                    errorMessage += $"\n\nStack Trace: {ex.StackTrace}";
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }
    }
}