using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //Lista todos os usuários
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        //Busca um usuário pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            return Ok(user);
        }

        //Cria um novo usuário
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Atualiza um usuário existente
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, dto);
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Remove um usuário
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _userService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Ativa ou inativa um usuário
        [HttpPatch("{id}/toggle-active")]
        public async Task<ActionResult> ToggleActive(int id)
        {
            var success = await _userService.ToggleActiveAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            return Ok(new { message = "Status do usuário alterado com sucesso." });
        }
    }
}