using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Application.DTOs;

namespace AssetManagementSystem.Application.Interfaces
{
    public interface IAllocationService
    {
        Task<IEnumerable<AllocationDto>> GetAllAsync();
        Task<AllocationDto?> GetByIdAsync(int id);
        Task<IEnumerable<AllocationDto>> GetHistoryByAssetIdAsync(int assetId);
        Task<IEnumerable<AllocationDto>> GetByUserIdAsync(int userId);
        Task<AllocationDto> AllocateAsync(CreateAllocationDto dto);
        Task<AllocationDto?> ReturnAssetAsync(int allocationId);
    }
}