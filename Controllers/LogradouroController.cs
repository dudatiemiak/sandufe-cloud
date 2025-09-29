using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogradouroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogradouroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logradouro>>> Get()
        {
            return await _context.Logradouros.ToListAsync();
        }

        [HttpGet("{idLogradouro}")]
        public async Task<ActionResult<Logradouro>> Get(int idLogradouro)
        {
            var logradouro = await _context.Logradouros.FindAsync(idLogradouro);

            if (logradouro == null)
                return NotFound();

            return logradouro;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Logradouro logradouro)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(logradouro.NmLogradouro))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do logradouro é obrigatório." });

                if (logradouro.NmLogradouro.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do logradouro não pode exceder 100 caracteres." });

                if (string.IsNullOrWhiteSpace(logradouro.NrLogradouro))
                    return BadRequest(new { StatusCode = 400, Message = "O número do logradouro é obrigatório." });

                if (logradouro.NrLogradouro.Length > 10)
                    return BadRequest(new { StatusCode = 400, Message = "O número do logradouro não pode exceder 10 caracteres." });

                if (logradouro.NmComplemento?.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O complemento não pode exceder 100 caracteres." });

                _context.Logradouros.Add(logradouro);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idLogradouro = logradouro.IdLogradouro }, logradouro);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar logradouro: {ex.Message}" });
            }
        }

        [HttpPut("{idLogradouro}")]
        public async Task<ActionResult> Put(int idLogradouro, Logradouro logradouro)
        {
            if (idLogradouro != logradouro.IdLogradouro)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(logradouro).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idLogradouro}")]
        public async Task<ActionResult> Delete(int idLogradouro)
        {
            var logradouro = await _context.Logradouros.FindAsync(idLogradouro);
            if (logradouro == null) return NotFound();

            _context.Logradouros.Remove(logradouro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
