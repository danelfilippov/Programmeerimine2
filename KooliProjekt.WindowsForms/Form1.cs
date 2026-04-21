using System.Collections;
using System.Net.Http.Json;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Data;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadUsers();
        }

        private void LoadUsers()
        {
            var url = "http://localhost:5086/api/Users/List";
            url += "?page=1&pageSize=10";

            using var client = new HttpClient();
            var response = client.GetFromJsonAsync<OperationResult<PagedResult<User>>>(url).Result;
            dataGridView1.DataSource = response.Value.Results;
        }

    }
}
