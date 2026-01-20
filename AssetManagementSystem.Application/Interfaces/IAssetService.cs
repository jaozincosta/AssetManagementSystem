using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Application.DTOs;

namespace AssetManagementSystem.Application.Interfaces
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetDto>> GetAllAsync();
        Task<AssetDto?> GetByIdAsync(int id);
        Task<IEnumerable<AssetDto>> GetAvailableAsync();
        Task<AssetDto> CreateAsync(CreateAssetDto dto);
        Task<AssetDto?> UpdateAsync(int id, CreateAssetDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SetMaintenanceAsync(int id);
        Task<bool> SetAvailableAsync(int id);
    }
}