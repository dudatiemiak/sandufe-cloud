using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> Get()
        {
            return await _context.Motos.ToListAsync();
        }

        [HttpGet("{idMoto}")]
        public async Task<ActionResult<Moto>> Get(int idMoto)
        {
            var moto = await _context.Motos.FindAsync(idMoto);

            if (moto == null)
                return NotFound();

            return moto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Moto moto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(moto.NmPlaca))
                    return BadRequest(new { StatusCode = 400, Message = "A placa é obrigatória." });

                if (moto.NmPlaca.Length > 10)
                    return BadRequest(new { StatusCode = 400, Message = "A placa não pode ter mais que 10 caracteres." });

                if (string.IsNullOrWhiteSpace(moto.StMoto))
                    return BadRequest(new { StatusCode = 400, Message = "O status da moto é obrigatório." });

                if (moto.KmRodado < 0)
                    return BadRequest(new { StatusCode = 400, Message = "O km rodado não pode ser negativo." });

                _context.Motos.Add(moto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idMoto = moto.IdMoto }, moto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar moto: {ex.Message}" });
            }
        }

        [HttpPut("{idMoto}")]
        public async Task<ActionResult> Put(int idMoto, Moto moto)
        {
            if (idMoto != moto.IdMoto)
                return BadRequest(new { StatusCode = 400, Message = "ID da moto não corresponde ao objeto enviado." });

            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idMoto}")]
        public async Task<ActionResult> Delete(int idMoto)
        {
            var moto = await _context.Motos.FindAsync(idMoto);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Filtra motos pela placa exata.
        /// </summary>
        [HttpGet("placa/{placa}")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetByPlaca(string placa)
        {
            var motos = await _context.Motos
                .Where(m => m.NmPlaca.ToLower() == placa.ToLower())
                .ToListAsync();

            return motos;
        }

        /// <summary>
        /// Filtra motos pelo status.
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetByStatus(string status)
        {
            var motos = await _context.Motos
                .Where(m => m.StMoto.ToLower() == status.ToLower())
                .ToListAsync();

            return motos;
        }

        /// <summary>
        /// Filtra motos por id de FilialDepartamento.
        /// </summary>
        [HttpGet("filialdepartamento/{idFilialDepartamento}")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetByFilialDepartamento(int idFilialDepartamento)
        {
            var motos = await _context.Motos
                .Where(m => m.IdFilialDepartamento == idFilialDepartamento)
                .ToListAsync();

            return motos;
        }
    }
}
