using AssetManagementSystem.Application.DTOs;

namespace AssetManagementSystem.Web.Services
{
    public class AllocationService : ApiService
    {
        public AllocationService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<AllocationDto>> GetAllAsync()
        {
            var result = await GetAsync<List<AllocationDto>>("api/allocations");
            return result ?? new List<AllocationDto>();
        }

        public async Task<List<AllocationDto>> GetByAssetIdAsync(int assetId)
        {
            var result = await GetAsync<List<AllocationDto>>($"api/allocations/asset/{assetId}/history");
            return result ?? new List<AllocationDto>();
        }

        public async Task<List<AllocationDto>> GetByUserIdAsync(int userId)
        {
            var result = await GetAsync<List<AllocationDto>>($"api/allocations/user/{userId}");
            return result ?? new List<AllocationDto>();
        }

        public async Task<HttpResponseMessage> AllocateAsync(CreateAllocationDto dto)
        {
            return await PostAsync("api/allocations", dto);
        }

        public async Task<HttpResponseMessage> ReturnAssetAsync(int allocationId)
        {
            return await PatchAsync($"api/allocations/{allocationId}/return");
        }
    }
}