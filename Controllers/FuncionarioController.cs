using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> Get()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        [HttpGet("{idFuncionario}")]
        public async Task<ActionResult<Funcionario>> Get(int idFuncionario)
        {
            var funcionario = await _context.Funcionarios.FindAsync(idFuncionario);

            if (funcionario == null)
                return NotFound();

            return funcionario;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Funcionario funcionario)
        {
            try
            {
                if (string.IsNullOrEmpty(funcionario.NmFuncionario))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do funcionário é obrigatório." });

                if (funcionario.NmFuncionario.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do funcionário não pode exceder 100 caracteres." });

                if (string.IsNullOrEmpty(funcionario.NmCargo))
                    return BadRequest(new { StatusCode = 400, Message = "O cargo é obrigatório." });

                if (!funcionario.NmEmailCorporativo.Contains("@"))
                    return BadRequest(new { StatusCode = 400, Message = "E-mail corporativo inválido." });

                if (string.IsNullOrEmpty(funcionario.NmSenha) || funcionario.NmSenha.Length < 6)
                    return BadRequest(new { StatusCode = 400, Message = "A senha deve ter pelo menos 6 caracteres." });

                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { idFuncionario = funcionario.IdFuncionario }, funcionario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao cadastrar funcionário: {ex.Message}" });
            }
        }

        [HttpPut("{idFuncionario}")]
        public async Task<ActionResult> Put(int idFuncionario, Funcionario funcionario)
        {
            if (idFuncionario != funcionario.IdFuncionario)
                return BadRequest(new { StatusCode = 400, Message = "ID informado não corresponde ao funcionário enviado." });

            _context.Entry(funcionario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idFuncionario}")]
        public async Task<ActionResult> Delete(int idFuncionario)
        {
            var funcionario = await _context.Funcionarios.FindAsync(idFuncionario);
            if (funcionario == null) return NotFound();

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retorna funcionários cujo nome contenha o valor informado.
        /// </summary>
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetByNome(string nome)
        {
            var funcionarios = await _context.Funcionarios
                .Where(f => f.NmFuncionario.Contains(nome))
                .ToListAsync();

            return funcionarios;
        }

        /// <summary>
        /// Retorna funcionários pelo cargo exato informado.
        /// </summary>
        [HttpGet("cargo/{cargo}")]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetByCargo(string cargo)
        {
            var funcionarios = await _context.Funcionarios
                .Where(f => f.NmCargo == cargo)
                .ToListAsync();

            return funcionarios;
        }

        /// <summary>
        /// Retorna funcionários por e-mail corporativo.
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetByEmail(string email)
        {
            var funcionarios = await _context.Funcionarios
                .Where(f => f.NmEmailCorporativo.Contains(email))
                .ToListAsync();

            return funcionarios;
        }
    }
}
