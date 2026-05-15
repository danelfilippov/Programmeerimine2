namespace KooliProjekt.WpfApplication
{
    public class User : NotifyPropertyChangedBase
    {
        private int _id;
        private string _title;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
    }
}
