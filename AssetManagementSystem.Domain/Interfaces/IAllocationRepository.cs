using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Entities;

namespace AssetManagementSystem.Domain.Interfaces
{
    public interface IAllocationRepository
    {
        Task<IEnumerable<Allocation>> GetAllAsync();
        Task<Allocation?> GetByIdAsync(int id);
        Task<IEnumerable<Allocation>> GetByAssetIdAsync(int assetId);
        Task<IEnumerable<Allocation>> GetByUserIdAsync(int userId);
        Task<Allocation?> GetActiveAllocationByAssetIdAsync(int assetId);
        Task<Allocation> AddAsync(Allocation allocation);
        Task UpdateAsync(Allocation allocation);
        Task<IEnumerable<Allocation>> GetAllocationHistoryAsync(int assetId);
    }
}
