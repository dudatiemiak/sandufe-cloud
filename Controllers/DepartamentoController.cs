using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> Get()
        {
            return await _context.Departamentos.ToListAsync();
        }

        [HttpGet("{idDepartamento}")]
        public async Task<ActionResult<Departamento>> Get(int idDepartamento)
        {
            var departamento = await _context.Departamentos.FindAsync(idDepartamento);

            if (departamento == null)
                return NotFound();
            return departamento;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Departamento departamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departamento.NmDepartamento))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do departamento é obrigatório." });

                if (departamento.NmDepartamento.Length > 50)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do departamento não pode exceder 50 caracteres." });

                if (string.IsNullOrWhiteSpace(departamento.DsDepartamento))
                    return BadRequest(new { StatusCode = 400, Message = "A descrição do departamento é obrigatória." });

                if (departamento.DsDepartamento.Length > 250)
                    return BadRequest(new { StatusCode = 400, Message = "A descrição não pode exceder 250 caracteres." });

                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idDepartamento = departamento.IdDepartamento }, departamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar departamento: {ex.Message}" });
            }
        }

        [HttpPut("{idDepartamento}")]
        public async Task<ActionResult> Put(int idDepartamento, Departamento departamento)
        {
            if (idDepartamento != departamento.IdDepartamento)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota não corresponde ao objeto enviado." });

            _context.Entry(departamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idDepartamento}")]
        public async Task<ActionResult> Delete(int idDepartamento)
        {
            var departamento = await _context.Departamentos.FindAsync(idDepartamento);
            if (departamento == null) return NotFound();

            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
