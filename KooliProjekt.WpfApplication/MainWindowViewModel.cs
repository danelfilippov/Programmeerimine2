using System.Collections.ObjectModel;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private readonly ObservableCollection<User> _data;

        private User _selectedItem;

        public MainWindowViewModel() : this(new ApiClient())
        {

        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _data = new ObservableCollection<User>();
            _apiClient = apiClient;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var data = await _apiClient.List(1, 100);

                if (data.HasErrors || data.Value == null || data.Value.Results == null)
                {
                    return;
                }

                _data.Clear();
                foreach (var item in data.Value.Results)
                {
                    _data.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public ObservableCollection<User> Data
        {
            get
            {
                return _data;
            }
        }

        public User SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }
    }
}