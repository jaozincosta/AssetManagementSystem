using System;
using System.Collections.Generic;
using System.Text;
using AssetManagementSystem.Domain.Enums;

namespace AssetManagementSystem.Domain.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public AssetType Type { get; set; }
        public decimal Value { get; set; }
        public AssetStatus Status { get; set; } = AssetStatus.Available;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navegação de propriedades
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
    }
}