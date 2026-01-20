using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManagementSystem.Domain.Entities
{
    public class Allocation
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int UserId { get; set; }
        public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }  // null = ainda alocado
        public string? Notes { get; set; }

        //Navegação de propriedades
        public Asset Asset { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}