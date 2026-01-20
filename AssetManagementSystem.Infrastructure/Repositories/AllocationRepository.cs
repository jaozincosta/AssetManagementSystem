using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Entities;
using AssetManagementSystem.Domain.Interfaces;
using AssetManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Infrastructure.Repositories
{
    public class AllocationRepository : IAllocationRepository
    {
        private readonly AppDbContext _context;

        public AllocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Allocation>> GetAllAsync()
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .OrderByDescending(a => a.AllocatedAt)
                .ToListAsync();
        }

        public async Task<Allocation?> GetByIdAsync(int id)
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Allocation>> GetByAssetIdAsync(int assetId)
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .Where(a => a.AssetId == assetId)
                .OrderByDescending(a => a.AllocatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Allocation>> GetByUserIdAsync(int userId)
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AllocatedAt)
                .ToListAsync();
        }

        public async Task<Allocation?> GetActiveAllocationByAssetIdAsync(int assetId)
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedAt == null);
        }

        public async Task<Allocation> AddAsync(Allocation allocation)
        {
            _context.Allocations.Add(allocation);
            await _context.SaveChangesAsync();
            return allocation;
        }

        public async Task UpdateAsync(Allocation allocation)
        {
            _context.Entry(allocation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Allocation>> GetAllocationHistoryAsync(int assetId)
        {
            return await _context.Allocations
                .Include(a => a.Asset)
                .Include(a => a.User)
                .Where(a => a.AssetId == assetId)
                .OrderByDescending(a => a.AllocatedAt)
                .ToListAsync();
        }
    }
}