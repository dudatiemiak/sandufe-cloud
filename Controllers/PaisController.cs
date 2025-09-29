using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pais>>> Get()
        {
            return await _context.Paises.ToListAsync();
        }

        [HttpGet("{idPais}")]
        public async Task<ActionResult<Pais>> Get(int idPais)
        {
            var pais = await _context.Paises.FindAsync(idPais);
            if (pais == null)
                return NotFound();
            return pais;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Pais pais)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pais.NmPais))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do país é obrigatório." });

                if (pais.NmPais.Length > 50)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do país não pode exceder 50 caracteres." });

                _context.Paises.Add(pais);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idPais = pais.IdPais }, pais);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar país: {ex.Message}" });
            }
        }

        [HttpPut("{idPais}")]
        public async Task<ActionResult> Put(int idPais, Pais pais)
        {
            if (idPais != pais.IdPais)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(pais).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idPais}")]
        public async Task<ActionResult> Delete(int idPais)
        {
            var pais = await _context.Paises.FindAsync(idPais);
            if (pais == null) return NotFound();

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
