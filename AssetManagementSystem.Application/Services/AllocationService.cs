using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Interfaces;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Enums;
using AssetManagementSystem.Domain.Interfaces;

namespace AssetManagementSystem.Application.Services
{
    public class AllocationService : IAllocationService
    {
        private readonly IAllocationRepository _allocationRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserRepository _userRepository;

        public AllocationService(
            IAllocationRepository allocationRepository,
            IAssetRepository assetRepository,
            IUserRepository userRepository)
        {
            _allocationRepository = allocationRepository;
            _assetRepository = assetRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<AllocationDto>> GetAllAsync()
        {
            var allocations = await _allocationRepository.GetAllAsync();
            return allocations.Select(a => MapToDto(a));
        }

        public async Task<AllocationDto?> GetByIdAsync(int id)
        {
            var allocation = await _allocationRepository.GetByIdAsync(id);
            return allocation == null ? null : MapToDto(allocation);
        }

        public async Task<IEnumerable<AllocationDto>> GetHistoryByAssetIdAsync(int assetId)
        {
            var allocations = await _allocationRepository.GetAllocationHistoryAsync(assetId);
            return allocations.Select(a => MapToDto(a));
        }

        public async Task<IEnumerable<AllocationDto>> GetByUserIdAsync(int userId)
        {
            var allocations = await _allocationRepository.GetByUserIdAsync(userId);
            return allocations.Select(a => MapToDto(a));
        }


        // Aloca um ativo para um usuário.
        // Não permite alocar ativos com status "Em Uso" ou "Manutenção".
        public async Task<AllocationDto> AllocateAsync(CreateAllocationDto dto)
        {
            // Verifica se o ativo existe
            var asset = await _assetRepository.GetByIdAsync(dto.AssetId);
            if (asset == null)
            {
                throw new InvalidOperationException("Ativo não encontrado.");
            }

            // Verifica se o ativo está disponível
            if (asset.Status == AssetStatus.InUse)
            {
                throw new InvalidOperationException("Este ativo já está alocado para outro usuário.");
            }

            if (asset.Status == AssetStatus.Maintenance)
            {
                throw new InvalidOperationException("Este ativo está em manutenção e não pode ser alocado.");
            }

            // Verifica se o usuário existe
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            // Verifica se o usuário está ativo
            if (!user.IsActive)
            {
                throw new InvalidOperationException("Não é possível alocar ativos para usuários inativos.");
            }

            // Cria a alocação
            var allocation = new Allocation
            {
                AssetId = dto.AssetId,
                UserId = dto.UserId,
                AllocatedAt = DateTime.UtcNow,
                ReturnedAt = null,
                Notes = dto.Notes
            };

            // Atualiza o status do ativo para "Em Uso"
            asset.Status = AssetStatus.InUse;
            await _assetRepository.UpdateAsync(asset);

            // Salva a alocação
            var createdAllocation = await _allocationRepository.AddAsync(allocation);

            // Carrega as navigation properties para o DTO
            createdAllocation.Asset = asset;
            createdAllocation.User = user;

            return MapToDto(createdAllocation);
        }

        // Registra a devolução de um ativo.
        public async Task<AllocationDto?> ReturnAssetAsync(int allocationId)
        {
            var allocation = await _allocationRepository.GetByIdAsync(allocationId);
            if (allocation == null)
            {
                return null;
            }

            // Verifica se já foi devolvido
            if (allocation.ReturnedAt != null)
            {
                throw new InvalidOperationException("Este ativo já foi devolvido.");
            }

            // Registra a data de devolução
            allocation.ReturnedAt = DateTime.UtcNow;
            await _allocationRepository.UpdateAsync(allocation);

            // Atualiza o status do ativo para "Disponível"
            var asset = await _assetRepository.GetByIdAsync(allocation.AssetId);
            if (asset != null)
            {
                asset.Status = AssetStatus.Available;
                await _assetRepository.UpdateAsync(asset);
                allocation.Asset = asset;
            }

            return MapToDto(allocation);
        }

        private static AllocationDto MapToDto(Allocation allocation)
        {
            return new AllocationDto
            {
                Id = allocation.Id,
                AssetId = allocation.AssetId,
                AssetName = allocation.Asset?.Name ?? string.Empty,
                AssetSerialNumber = allocation.Asset?.SerialNumber ?? string.Empty,
                UserId = allocation.UserId,
                UserName = allocation.User?.Name ?? string.Empty,
                AllocatedAt = allocation.AllocatedAt,
                ReturnedAt = allocation.ReturnedAt,
                Notes = allocation.Notes
            };
        }
    }
}