using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CidadeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cidade>>> Get()
        {
            return await _context.Cidades.ToListAsync();
        }

        [HttpGet("{idCidade}")]
        public async Task<ActionResult<Cidade>> Get(int idCidade)
        {
            var cidade = await _context.Cidades.FindAsync(idCidade);

            if (cidade == null)
                return NotFound();
            return cidade;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cidade cidade)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cidade.NmCidade))
                    return BadRequest(new { StatusCode = 400, Message = "O nome da cidade é obrigatório." });

                if (cidade.NmCidade.Length > 50)
                    return BadRequest(new { StatusCode = 400, Message = "O nome da cidade não pode exceder 50 caracteres." });

                _context.Cidades.Add(cidade);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idCidade = cidade.IdCidade }, cidade);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar cidade: {ex.Message}" });
            }
        }

        [HttpPut("{idCidade}")]
        public async Task<ActionResult> Put(int idCidade, Cidade cidade)
        {
            if (idCidade != cidade.IdCidade)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(cidade).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idCidade}")]
        public async Task<ActionResult> Delete(int idCidade)
        {
            var cidade = await _context.Cidades.FindAsync(idCidade);
            if (cidade == null) return NotFound();

            _context.Cidades.Remove(cidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
