using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace KooliProjekt.WpfApplication
{
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Users/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<User>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<User>>>(body);
            if (result == null)
            {
                result = new OperationResult<PagedResult<User>>();
                result.AddError($"Failed to deserialize response. Response body: {body}");
            }
            return result;
        }

        public async Task<OperationResult> Save(User list)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(list)
            };            
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            if (result == null)
            {
                result = new OperationResult();
                result.AddError($"Failed to deserialize response. Response body: {body}");
            }
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete?id=" + id;

            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            if (result == null)
            {
                result = new OperationResult();
                result.AddError($"Failed to deserialize response. Response body: {body}");
            }
            return result;
        }
    }
}
