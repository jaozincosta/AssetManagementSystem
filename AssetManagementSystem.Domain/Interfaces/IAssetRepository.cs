using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Enums;

namespace AssetManagementSystem.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(int id);
        Task<Asset?> GetBySerialNumberAsync(string serialNumber);
        Task<IEnumerable<Asset>> GetByStatusAsync(AssetStatus status);
        Task<Asset> AddAsync(Asset asset);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
