using Newtonsoft.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace KooliProjekt.WindowsForms.Api
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
            try
            {
                var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new OperationResult<PagedResult<User>> 
                    { 
                        Errors = new List<string> { $"Server returned status code: {response.StatusCode}" } 
                    };
                }

                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                {
                    return new OperationResult<PagedResult<User>> 
                    { 
                        Errors = new List<string> { "Empty response from server" } 
                    };
                }

                var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<User>>>(body);
                if (result == null)
                {
                    return new OperationResult<PagedResult<User>> 
                    { 
                        Errors = new List<string> { $"Failed to deserialize response: {body}" } 
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new OperationResult<PagedResult<User>> 
                { 
                    Errors = new List<string> { $"Error occurred: {ex.Message}" } 
                };
            }
        }

        public async Task<OperationResult> Save(User list)
        {
            try
            {
                var url = _baseUrl + "Save";
                var json = JsonConvert.SerializeObject(list);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                using var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { $"Server returned status code: {response.StatusCode}" } 
                    };
                }

                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { "Empty response from server" } 
                    };
                }

                var result = JsonConvert.DeserializeObject<OperationResult>(body);
                if (result == null)
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { $"Failed to deserialize response: {body}" } 
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new OperationResult 
                { 
                    Errors = new List<string> { $"Error occurred: {ex.Message}" } 
                };
            }
        }

        public async Task<OperationResult> Delete(int id)
        {
            try
            {
                var url = _baseUrl + $"Delete?id={id}";

                using var request = new HttpRequestMessage(HttpMethod.Delete, url);
                using var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { $"Server returned status code: {response.StatusCode}" } 
                    };
                }

                var body = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(body))
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { "Empty response from server" } 
                    };
                }

                var result = JsonConvert.DeserializeObject<OperationResult>(body);
                if (result == null)
                {
                    return new OperationResult 
                    { 
                        Errors = new List<string> { $"Failed to deserialize response: {body}" } 
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new OperationResult 
                { 
                    Errors = new List<string> { $"Error occurred: {ex.Message}" } 
                };
            }
        }
    }
}
