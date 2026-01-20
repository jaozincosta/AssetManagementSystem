using AssetManagementSystem.Application.DTOs;
using AssetManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllocationsController : ControllerBase
    {
        private readonly IAllocationService _allocationService;

        public AllocationsController(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }

        //Lista todas as alocações
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllocationDto>>> GetAll()
        {
            var allocations = await _allocationService.GetAllAsync();
            return Ok(allocations);
        }

        //Busca uma alocação pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AllocationDto>> GetById(int id)
        {
            var allocation = await _allocationService.GetByIdAsync(id);
            if (allocation == null)
            {
                return NotFound(new { message = "Alocação não encontrada." });
            }
            return Ok(allocation);
        }

        // Busca o histórico de alocações de um ativo
        [HttpGet("asset/{assetId}/history")]
        public async Task<ActionResult<IEnumerable<AllocationDto>>> GetHistoryByAssetId(int assetId)
        {
            var allocations = await _allocationService.GetHistoryByAssetIdAsync(assetId);
            return Ok(allocations);
        }

        //Busca as alocações de um usuário
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AllocationDto>>> GetByUserId(int userId)
        {
            var allocations = await _allocationService.GetByUserIdAsync(userId);
            return Ok(allocations);
        }

        //Aloca um ativo para um usuário (REGRA DE OURO: não permite alocar ativo Em Uso ou Em Manutenção)
        [HttpPost]
        public async Task<ActionResult<AllocationDto>> Allocate([FromBody] CreateAllocationDto dto)
        {
            try
            {
                var allocation = await _allocationService.AllocateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = allocation.Id }, allocation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Registra a devolução de um ativo
        [HttpPatch("{id}/return")]
        public async Task<ActionResult<AllocationDto>> ReturnAsset(int id)
        {
            try
            {
                var allocation = await _allocationService.ReturnAssetAsync(id);
                if (allocation == null)
                {
                    return NotFound(new { message = "Alocação não encontrada." });
                }
                return Ok(allocation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}