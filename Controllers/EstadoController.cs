using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> Get()
        {
            return await _context.Estados.ToListAsync();
        }

        [HttpGet("{idEstado}")]
        public async Task<ActionResult<Estado>> Get(int idEstado)
        {
            var estado = await _context.Estados.FindAsync(idEstado);

            if (estado == null)
                return NotFound();
            return estado;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Estado estado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(estado.NmEstado))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do estado é obrigatório." });

                _context.Estados.Add(estado);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idEstado = estado.IdEstado }, estado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar estado: {ex.Message}" });
            }
        }

        [HttpPut("{idEstado}")]
        public async Task<ActionResult> Put(int idEstado, Estado estado)
        {
            if (idEstado != estado.IdEstado)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(estado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idEstado}")]
        public async Task<ActionResult> Delete(int idEstado)
        {
            var estado = await _context.Estados.FindAsync(idEstado);
            if (estado == null) return NotFound();

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
