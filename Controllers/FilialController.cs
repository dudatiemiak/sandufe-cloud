using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilialController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilialController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> Get()
        {
            return await _context.Filiais.ToListAsync();
        }

        [HttpGet("{idFilial}")]
        public async Task<ActionResult<Filial>> Get(int idFilial)
        {
            var filial = await _context.Filiais.FindAsync(idFilial);

            if (filial == null)
                return NotFound();

            return filial;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Filial filial)
        {
            try
            {
                if (string.IsNullOrEmpty(filial.NmFilial))
                    return BadRequest(new { StatusCode = 400, Message = "O nome da filial é obrigatório." });

                if (filial.NmFilial.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O nome da filial não pode ultrapassar 100 caracteres." });

                _context.Filiais.Add(filial);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idFilial = filial.IdFilial }, filial);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao salvar filial: {ex.Message}" });
            }
        }

        [HttpPut("{idFilial}")]
        public async Task<ActionResult> Put(int idFilial, Filial filial)
        {
            if (idFilial != filial.IdFilial)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não bate com o objeto enviado." });

            _context.Entry(filial).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idFilial}")]
        public async Task<ActionResult> Delete(int idFilial)
        {
            var filial = await _context.Filiais.FindAsync(idFilial);
            if (filial == null) return NotFound();

            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retorna todas as filiais cujo nome contenha o valor informado.
        /// </summary>
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Filial>>> GetByNome(string nome)
        {
            var filiais = await _context.Filiais
                .Where(f => f.NmFilial.Contains(nome))
                .ToListAsync();

            return filiais;
        }
    }    
}
