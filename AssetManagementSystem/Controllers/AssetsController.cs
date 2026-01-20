using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        //Lista todos os ativos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAll()
        {
            var assets = await _assetService.GetAllAsync();
            return Ok(assets);
        }

        //Lista apenas os ativos disponíveis
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAvailable()
        {
            var assets = await _assetService.GetAvailableAsync();
            return Ok(assets);
        }

        //Busca um ativo pelo 
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetById(int id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound(new { message = "Ativo não encontrado." });
            }
            return Ok(asset);
        }

        //Cria um novo ativo
        [HttpPost]
        public async Task<ActionResult<AssetDto>> Create([FromBody] CreateAssetDto dto)
        {
            try
            {
                var asset = await _assetService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Atualiza um ativo existente
        [HttpPut("{id}")]
        public async Task<ActionResult<AssetDto>> Update(int id, [FromBody] CreateAssetDto dto)
        {
            try
            {
                var asset = await _assetService.UpdateAsync(id, dto);
                if (asset == null)
                {
                    return NotFound(new { message = "Ativo não encontrado." });
                }
                return Ok(asset);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Remove um ativo
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _assetService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new { message = "Ativo não encontrado." });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Envia um ativo para manutenção
        [HttpPatch("{id}/maintenance")]
        public async Task<ActionResult> SetMaintenance(int id)
        {
            try
            {
                var success = await _assetService.SetMaintenanceAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Ativo não encontrado." });
                }
                return Ok(new { message = "Ativo enviado para manutenção." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Libera um ativo da manutenção (torna disponível)
        [HttpPatch("{id}/available")]
        public async Task<ActionResult> SetAvailable(int id)
        {
            try
            {
                var success = await _assetService.SetAvailableAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Ativo não encontrado." });
                }
                return Ok(new { message = "Ativo disponibilizado." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}