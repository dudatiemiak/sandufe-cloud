using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModeloController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModeloController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modelo>>> Get()
        {
            return await _context.Modelos.ToListAsync();
        }

        [HttpGet("{idModelo}")]
        public async Task<ActionResult<Modelo>> Get(int idModelo)
        {
            var modelo = await _context.Modelos.FindAsync(idModelo);
            if (modelo == null)
                return NotFound();
            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Modelo modelo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelo.NmModelo))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do modelo é obrigatório." });

                if (modelo.NmModelo.Length > 50)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do modelo não pode exceder 50 caracteres." });

                _context.Modelos.Add(modelo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idModelo = modelo.IdModelo }, modelo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar modelo: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int idModelo, Modelo modelo)
        {
            if (idModelo != modelo.IdModelo)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(modelo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idModelo}")]
        public async Task<ActionResult> Delete(int idModelo)
        {
            var modelo = await _context.Modelos.FindAsync(idModelo);
            if (modelo == null)
                return NotFound();

            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
