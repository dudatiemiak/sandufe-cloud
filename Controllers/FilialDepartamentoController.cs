using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilialDepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilialDepartamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilialDepartamento>>> Get()
        {
            return await _context.FilialDepartamentos.ToListAsync();
        }

        [HttpGet("{idFilialDepartamento}")]
        public async Task<ActionResult<FilialDepartamento>> Get(int idFilialDepartamento)
        {
            var filialDepartamento = await _context.FilialDepartamentos.FindAsync(idFilialDepartamento);

            if (filialDepartamento == null)
                return NotFound();
            return filialDepartamento;
        }

        [HttpPost]
        public async Task<ActionResult> Post(FilialDepartamento filialDepartamento)
        {
            try
            {
                if (filialDepartamento.DtEntrada == default)
                    return BadRequest(new { StatusCode = 400, Message = "A data de entrada é obrigatória." });

                _context.FilialDepartamentos.Add(filialDepartamento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idFilialDepartamento = filialDepartamento.IdFilialDepartamento }, filialDepartamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar relação filial-departamento: {ex.Message}" });
            }
        }

        [HttpPut("{idFilialDepartamento}")]
        public async Task<ActionResult> Put(int idFilialDepartamento, FilialDepartamento filialDepartamento)
        {
            if (idFilialDepartamento != filialDepartamento.IdFilialDepartamento)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(filialDepartamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idFilialDepartamento}")]
        public async Task<ActionResult> Delete(int idFilialDepartamento)
        {
            var filialDepartamento = await _context.FilialDepartamentos.FindAsync(idFilialDepartamento);
            if (filialDepartamento == null) return NotFound();

            _context.FilialDepartamentos.Remove(filialDepartamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
