using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManagementSystem.Application.DTOs
{
    public class AllocationDto
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string AssetSerialNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime AllocatedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string? Notes { get; set; }
        public bool IsActive => ReturnedAt == null;
    }
}