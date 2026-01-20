using System.Net.Http.Json;

namespace AssetManagementSystem.Web.Services
{
    public class ApiService
    {
        protected readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<T>(url);
            }
            catch (Exception)
            {
                return default;
            }
        }

        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            return default;
        }

        protected async Task<HttpResponseMessage> PostAsync<TRequest>(string url, TRequest data)
        {
            return await _httpClient.PostAsJsonAsync(url, data);
        }

        protected async Task<HttpResponseMessage> PutAsync<TRequest>(string url, TRequest data)
        {
            return await _httpClient.PutAsJsonAsync(url, data);
        }

        protected async Task<HttpResponseMessage> PatchAsync(string url)
        {
            return await _httpClient.PatchAsync(url, null);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }
    }
}