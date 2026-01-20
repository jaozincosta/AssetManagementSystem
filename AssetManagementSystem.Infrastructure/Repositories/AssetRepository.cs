using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Enums;
using AssetManagementSystem.Domain.Interfaces;
using AssetManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.Allocations)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Assets.FirstOrDefaultAsync(a => a.SerialNumber == serialNumber);
        }

        public async Task<IEnumerable<Asset>> GetByStatusAsync(AssetStatus status)
        {
            return await _context.Assets.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<Asset> AddAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Assets.AnyAsync(a => a.Id == id);
        }
    }
}