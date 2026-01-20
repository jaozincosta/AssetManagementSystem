using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Application.DTOs
{
    public class CreateAssetDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número de série é obrigatório")]
        [StringLength(50, ErrorMessage = "Número de série não pode exceder 50 caracteres")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [Range(1, 3, ErrorMessage = "Tipo deve ser 1 (Notebook), 2 (Monitor) ou 3 (Periférico)")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Value { get; set; }
    }
}