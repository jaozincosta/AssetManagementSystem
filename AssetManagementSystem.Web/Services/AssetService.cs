using AssetManagementSystem.Application.DTOs;

namespace AssetManagementSystem.Web.Services
{
    public class AssetService : ApiService
    {
        public AssetService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<AssetDto>> GetAllAsync()
        {
            var result = await GetAsync<List<AssetDto>>("api/assets");
            return result ?? new List<AssetDto>();
        }

        public async Task<List<AssetDto>> GetAvailableAsync()
        {
            var result = await GetAsync<List<AssetDto>>("api/assets/available");
            return result ?? new List<AssetDto>();
        }

        public async Task<AssetDto?> GetByIdAsync(int id)
        {
            return await GetAsync<AssetDto>($"api/assets/{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateAssetDto dto)
        {
            return await PostAsync("api/assets", dto);
        }

        public async Task<HttpResponseMessage> UpdateAsync(int id, CreateAssetDto dto)
        {
            return await PutAsync($"api/assets/{id}", dto);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"api/assets/{id}");
        }

        public async Task<HttpResponseMessage> SetMaintenanceAsync(int id)
        {
            return await PatchAsync($"api/assets/{id}/maintenance");
        }

        public async Task<HttpResponseMessage> SetAvailableAsync(int id)
        {
            return await PatchAsync($"api/assets/{id}/available");
        }
    }
}