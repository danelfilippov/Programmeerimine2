using System.Collections;
using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;

        public Form1(IApiClient apiClient)
        {
            _apiClient = apiClient;

            InitializeComponent();

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            saveCommand.Click += SaveCommand_Click;
            addCommand.Click += AddCommand_Click;
            deleteCommand.Click += DeleteCommand_Click;
        }

        private async void DeleteCommand_Click(object sender, EventArgs e)
        {
            var message = "Oled kindel, et soovid kustutada " + titleField.Text + "?";
            var answer = MessageBox.Show(message, "Kustutamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            var id = int.Parse(idField.Text);
            var result = await _apiClient.Delete(id);
            if (result.HasErrors)
            {
                ShowError("Viga kustutamisel", result);
            }

            await LoadTodoLists();
        }

        private void AddCommand_Click(object sender, EventArgs e)
        {
            idField.Text = "0";
            titleField.Text = "";
        }

        private async void SaveCommand_Click(object sender, EventArgs e)
        {
            var todoList = new User();
            todoList.Id = int.Parse(idField.Text);
            todoList.Title = titleField.Text;

            var result = await _apiClient.Save(todoList);
            if (result.HasErrors)
            {
                ShowError("Viga salvestamisel", result);
            }
            await LoadTodoLists();
        }

        // Koosta etteantud veateatest ja OperationResult sees olevatest vigadest
        // veateade ja näita seda kasutajale
        private void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            MessageBox.Show(error, "Viga!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            var selectedList = (User)dataGridView1.CurrentRow.DataBoundItem;
            if (selectedList == null)
            {
                return;
            }

            idField.Text = selectedList.Id.ToString();
            titleField.Text = selectedList.Title ?? "";
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadTodoLists();
        }

        private async Task LoadTodoLists()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                ShowError("Viga andmete laadimisel", response);
                dataGridView1.DataSource = null;
                return;
            }

            dataGridView1.DataSource = response.Value.Results;
        }
    }
}
