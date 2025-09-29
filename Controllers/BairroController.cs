using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BairroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BairroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bairro>>> Get()
        {
            return await _context.Bairros.ToListAsync();
        }

        [HttpGet("{idBairro}")]
        public async Task<ActionResult<Bairro>> Get(int idBairro)
        {
            var bairro = await _context.Bairros.FindAsync(idBairro);

            if (bairro == null) return NotFound();
            return bairro;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Bairro bairro)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bairro.NmBairro))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do bairro é obrigatório." });

                if (bairro.NmBairro.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do bairro não pode exceder 100 caracteres." });

                _context.Bairros.Add(bairro);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idBairro = bairro.IdBairro }, bairro);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar bairro: {ex.Message}" });
            }
        }

        [HttpPut("{idBairro}")]
        public async Task<ActionResult> Put(int idBairro, Bairro bairro)
        {
            if (idBairro != bairro.IdBairro)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(bairro).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{idBairro}")]
        public async Task<ActionResult> Delete(int idBairro)
        {
            var bairro = await _context.Bairros.FindAsync(idBairro);
            if (bairro == null) return NotFound();

            _context.Bairros.Remove(bairro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
