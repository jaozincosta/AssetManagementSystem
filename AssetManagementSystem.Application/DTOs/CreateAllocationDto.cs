using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Application.DTOs
{
    public class CreateAllocationDto
    {
        [Required(ErrorMessage = "ID do ativo é obrigatório")]
        public int AssetId { get; set; }

        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public int UserId { get; set; }

        [StringLength(500, ErrorMessage = "Observações não podem exceder 500 caracteres")]
        public string? Notes { get; set; }
    }
}