using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManagementSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        //Navegação de propriedades
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
    }
}