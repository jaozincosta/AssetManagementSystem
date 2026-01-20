using AssetManagementSystem.Application.DTOs;

namespace AssetManagementSystem.Web.Services
{
    public class UserService : ApiService
    {
        public UserService(HttpClient httpClient) : base(httpClient) { }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var result = await GetAsync<List<UserDto>>("api/users");
            return result ?? new List<UserDto>();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await GetAsync<UserDto>($"api/users/{id}");
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateUserDto dto)
        {
            return await PostAsync("api/users", dto);
        }

        public async Task<HttpResponseMessage> UpdateAsync(int id, CreateUserDto dto)
        {
            return await PutAsync($"api/users/{id}", dto);
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            return await base.DeleteAsync($"api/users/{id}");
        }

        public async Task<HttpResponseMessage> ToggleActiveAsync(int id)
        {
            return await PatchAsync($"api/users/{id}/toggle-active");
        }
    }
}