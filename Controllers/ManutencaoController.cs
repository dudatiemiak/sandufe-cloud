using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManutencaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ManutencaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manutencao>>> Get()
        {
            return await _context.Manutencoes.ToListAsync();
        }

        [HttpGet("{idManutencao}")]
        public async Task<ActionResult<Manutencao>> Get(int idManutencao)
        {
            var manutencao = await _context.Manutencoes.FindAsync(idManutencao);

            if (manutencao == null)
                return NotFound();

            return manutencao;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Manutencao manutencao)
        {
            try
            {
                if (manutencao.DtEntrada == default)
                    return BadRequest(new { StatusCode = 400, Message = "A data de entrada é obrigatória." });

                if (string.IsNullOrWhiteSpace(manutencao.DsManutencao))
                    return BadRequest(new { StatusCode = 400, Message = "A descrição da manutenção é obrigatória." });

                if (manutencao.DsManutencao.Length > 300)
                    return BadRequest(new { StatusCode = 400, Message = "A descrição da manutenção não pode ultrapassar 300 caracteres." });

                _context.Manutencoes.Add(manutencao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idManutencao = manutencao.IdManutencao }, manutencao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar manutenção: {ex.Message}" });
            }
        }

        [HttpPut("{idManutencao}")]
        public async Task<ActionResult> Put(int idManutencao, Manutencao manutencao)
        {
            if (idManutencao != manutencao.IdManutencao)
                return BadRequest(new { StatusCode = 400, Message = "ID da manutenção não corresponde ao objeto enviado." });

            _context.Entry(manutencao).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idManutencao}")]
        public async Task<ActionResult> Delete(int idManutencao)
        {
            var manutencao = await _context.Manutencoes.FindAsync(idManutencao);
            if (manutencao == null) return NotFound();

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retorna todas as manutenções de uma moto específica.
        /// </summary>
        [HttpGet("moto/{idMoto}")]
        public async Task<ActionResult<IEnumerable<Manutencao>>> GetByMoto(int idMoto)
        {
            var manutencoes = await _context.Manutencoes
                .Where(m => m.IdMoto == idMoto)
                .ToListAsync();

            return manutencoes;
        }

        /// <summary>
        /// Retorna manutenções realizadas a partir de uma determinada data.
        /// </summary>
        [HttpGet("dataentrada/{data}")]
        public async Task<ActionResult<IEnumerable<Manutencao>>> GetByDataEntrada(DateTime data)
        {
            var manutencoes = await _context.Manutencoes
                .Where(m => m.DtEntrada.Date >= data.Date)
                .ToListAsync();

            return manutencoes;
        }
    }
}
