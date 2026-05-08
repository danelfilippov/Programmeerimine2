namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<User> DataSource { get; set; }
        User SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentTitle { get; set; }
        bool ConfirmDelete();
    }
}
