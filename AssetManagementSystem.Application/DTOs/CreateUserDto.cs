using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Application.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departamento é obrigatório")]
        [StringLength(50, ErrorMessage = "Departamento não pode exceder 50 caracteres")]
        public string Department { get; set; } = string.Empty;
    }
}