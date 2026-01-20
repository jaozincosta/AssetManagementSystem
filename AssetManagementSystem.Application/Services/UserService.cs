using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Interfaces;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Interfaces;

namespace AssetManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => MapToDto(u));
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            // Verifica se já existe usuário com este e-mail
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Department = dto.Department,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdUser = await _userRepository.AddAsync(user);
            return MapToDto(createdUser);
        }

        public async Task<UserDto?> UpdateAsync(int id, CreateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            // Verifica se o e-mail já está em uso por outro usuário
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new InvalidOperationException("Este e-mail já está em uso por outro usuário.");
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Department = dto.Department;

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            // Verifica se o usuário tem qualquer alocação (histórico)
            if (user.Allocations != null && user.Allocations.Any())
            {
                throw new InvalidOperationException("Não é possível excluir este usuário pois ele possui histórico de alocações. Você pode desativá-lo se necessário.");
            }

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = !user.IsActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Department = user.Department,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }
    }
}