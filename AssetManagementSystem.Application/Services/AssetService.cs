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
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IEnumerable<AssetDto>> GetAllAsync()
        {
            var assets = await _assetRepository.GetAllAsync();
            return assets.Select(a => MapToDto(a));
        }

        public async Task<AssetDto?> GetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            return asset == null ? null : MapToDto(asset);
        }

        public async Task<IEnumerable<AssetDto>> GetAvailableAsync()
        {
            var assets = await _assetRepository.GetByStatusAsync(AssetStatus.Available);
            return assets.Select(a => MapToDto(a));
        }

        public async Task<AssetDto> CreateAsync(CreateAssetDto dto)
        {
            // Verifica se já existe ativo com este número de série
            var existingAsset = await _assetRepository.GetBySerialNumberAsync(dto.SerialNumber);
            if (existingAsset != null)
            {
                throw new InvalidOperationException("Já existe um ativo cadastrado com este número de série.");
            }

            var asset = new Asset
            {
                Name = dto.Name,
                SerialNumber = dto.SerialNumber,
                Type = (AssetType)dto.Type,
                Value = dto.Value,
                Status = AssetStatus.Available,
                CreatedAt = DateTime.UtcNow
            };

            var createdAsset = await _assetRepository.AddAsync(asset);
            return MapToDto(createdAsset);
        }

        public async Task<AssetDto?> UpdateAsync(int id, CreateAssetDto dto)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return null;
            }

            // Verifica se o número de série já está em uso por outro ativo
            var existingAsset = await _assetRepository.GetBySerialNumberAsync(dto.SerialNumber);
            if (existingAsset != null && existingAsset.Id != id)
            {
                throw new InvalidOperationException("Este número de série já está em uso por outro ativo.");
            }

            asset.Name = dto.Name;
            asset.SerialNumber = dto.SerialNumber;
            asset.Type = (AssetType)dto.Type;
            asset.Value = dto.Value;

            await _assetRepository.UpdateAsync(asset);
            return MapToDto(asset);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return false;
            }

            // Não permite excluir ativo que está em uso
            if (asset.Status == AssetStatus.InUse)
            {
                throw new InvalidOperationException("Não é possível excluir um ativo que está em uso. Devolva o ativo primeiro.");
            }

            // Verifica se o ativo tem histórico de alocações
            if (asset.Allocations != null && asset.Allocations.Any())
            {
                throw new InvalidOperationException("Não é possível excluir este ativo pois ele possui histórico de alocações.");
            }

            await _assetRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> SetMaintenanceAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return false;
            }

            if (asset.Status == AssetStatus.InUse)
            {
                throw new InvalidOperationException("Não é possível enviar para manutenção um ativo que está em uso. Devolva o ativo primeiro.");
            }

            asset.Status = AssetStatus.Maintenance;
            await _assetRepository.UpdateAsync(asset);
            return true;
        }

        public async Task<bool> SetAvailableAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return false;
            }

            if (asset.Status == AssetStatus.InUse)
            {
                throw new InvalidOperationException("Não é possível liberar um ativo que está em uso. Devolva o ativo primeiro.");
            }

            asset.Status = AssetStatus.Available;
            await _assetRepository.UpdateAsync(asset);
            return true;
        }

        private static AssetDto MapToDto(Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                SerialNumber = asset.SerialNumber,
                Type = asset.Type.ToString(),
                Value = asset.Value,
                Status = GetStatusInPortuguese(asset.Status),
                CreatedAt = asset.CreatedAt
            };
        }

        private static string GetStatusInPortuguese(AssetStatus status)
        {
            return status switch
            {
                AssetStatus.Available => "Disponível",
                AssetStatus.InUse => "Em Uso",
                AssetStatus.Maintenance => "Manutenção",
                _ => status.ToString()
            };
        }
    }
}