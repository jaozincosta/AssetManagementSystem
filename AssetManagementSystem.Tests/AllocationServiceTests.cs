using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Services;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Enums;
using AssetManagementSystem.Domain.Interfaces;
using Moq;

namespace AssetManagementSystem.Tests
{
    public class AllocationServiceTests
    {
        private readonly Mock<IAllocationRepository> _allocationRepositoryMock;
        private readonly Mock<IAssetRepository> _assetRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AllocationService _allocationService;

        public AllocationServiceTests()
        {
            _allocationRepositoryMock = new Mock<IAllocationRepository>();
            _assetRepositoryMock = new Mock<IAssetRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _allocationService = new AllocationService(
                _allocationRepositoryMock.Object,
                _assetRepositoryMock.Object,
                _userRepositoryMock.Object
            );
        }

        //Não deve permitir alocar ativo que está "Em Uso"
        [Fact]
        public async Task AllocateAsync_Should_ThrowException_When_Asset_Is_InUse()
        {
            var asset = new Asset
            {
                Id = 1,
                Name = "Notebook Dell",
                SerialNumber = "ABC123",
                Status = AssetStatus.InUse // Ativo já está em uso
            };

            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(asset);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _allocationService.AllocateAsync(dto)
            );

            Assert.Equal("Este ativo já está alocado para outro usuário.", exception.Message);
        }

        //Não deve permitir alocar ativo que está "Em Manutenção"
        [Fact]
        public async Task AllocateAsync_Should_ThrowException_When_Asset_Is_InMaintenance()
        {
            var asset = new Asset
            {
                Id = 1,
                Name = "Notebook Dell",
                SerialNumber = "ABC123",
                Status = AssetStatus.Maintenance // Ativo em manutenção
            };

            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(asset);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _allocationService.AllocateAsync(dto)
            );

            Assert.Equal("Este ativo está em manutenção e não pode ser alocado.", exception.Message);
        }

        //Não deve permitir alocar para usuário inativo
        [Fact]
        public async Task AllocateAsync_Should_ThrowException_When_User_Is_Inactive()
        {
            var asset = new Asset
            {
                Id = 1,
                Name = "Notebook Dell",
                SerialNumber = "ABC123",
                Status = AssetStatus.Available
            };

            var user = new User
            {
                Id = 1,
                Name = "João",
                Email = "joao@teste.com",
                IsActive = false // Usuário inativo
            };

            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(asset);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _allocationService.AllocateAsync(dto)
            );

            Assert.Equal("Não é possível alocar ativos para usuários inativos.", exception.Message);
        }

        //Deve permitir alocar ativo disponível para usuário ativo
        [Fact]
        public async Task AllocateAsync_Should_Succeed_When_Asset_Is_Available_And_User_Is_Active()
        {
            var asset = new Asset
            {
                Id = 1,
                Name = "Notebook Dell",
                SerialNumber = "ABC123",
                Status = AssetStatus.Available
            };

            var user = new User
            {
                Id = 1,
                Name = "João",
                Email = "joao@teste.com",
                IsActive = true
            };

            var allocation = new Allocation
            {
                Id = 1,
                AssetId = 1,
                UserId = 1,
                AllocatedAt = DateTime.UtcNow
            };

            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(asset);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _allocationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Allocation>())).ReturnsAsync(allocation);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste de alocação"
            };

            var result = await _allocationService.AllocateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.AssetId);
            Assert.Equal(1, result.UserId);

            // Verifica se o status do ativo foi alterado para "Em Uso"
            _assetRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Asset>(a => a.Status == AssetStatus.InUse)), Times.Once);
        }

        //Deve lançar exceção quando ativo não existe
        [Fact]
        public async Task AllocateAsync_Should_ThrowException_When_Asset_NotFound()
        {
            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Asset?)null);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _allocationService.AllocateAsync(dto)
            );

            Assert.Equal("Ativo não encontrado.", exception.Message);
        }

        //Deve lançar exceção quando usuário não existe
        [Fact]
        public async Task AllocateAsync_Should_ThrowException_When_User_NotFound()
        {
            var asset = new Asset
            {
                Id = 1,
                Name = "Notebook Dell",
                SerialNumber = "ABC123",
                Status = AssetStatus.Available
            };

            _assetRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(asset);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User?)null);

            var dto = new CreateAllocationDto
            {
                AssetId = 1,
                UserId = 1,
                Notes = "Teste"
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _allocationService.AllocateAsync(dto)
            );

            Assert.Equal("Usuário não encontrado.", exception.Message);
        }
    }
}