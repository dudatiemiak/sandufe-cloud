using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuWebApplication.Connection;
using MottuWebApplication.Models;

namespace MottuWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{idCliente}")]
        public async Task<ActionResult<Cliente>> Get(int idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);

            if (cliente == null) return NotFound();
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            try
            {
                if (string.IsNullOrEmpty(cliente.NmCliente))
                    return BadRequest(new { StatusCode = 400, Message = "O nome do cliente é obrigatório." });

                if (cliente.NmCliente.Length > 100)
                    return BadRequest(new { StatusCode = 400, Message = "O nome do cliente excede 100 caracteres." });

                if (string.IsNullOrEmpty(cliente.NrCpf) || cliente.NrCpf.Length != 14)
                    return BadRequest(new { StatusCode = 400, Message = "O CPF deve ter 14 caracteres (com pontuação)." });

                if (!cliente.NmEmail.Contains("@"))
                    return BadRequest(new { StatusCode = 400, Message = "E-mail inválido." });

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = cliente.IdCliente }, cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = $"Erro ao criar cliente: {ex.Message}" });
            }
        }

        [HttpPut("{idCliente}")]
        public async Task<ActionResult> Put(int idCliente, Cliente cliente)
        {
            if (idCliente != cliente.IdCliente)
                return BadRequest(new { StatusCode = 400, Message = "ID da rota diferente do objeto enviado." });

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idCliente}")]
        public async Task<ActionResult> Delete(int idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retorna todos os clientes com nome contendo o valor informado.
        /// </summary>
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetByNome(string nome)
        {
            var clientes = await _context.Clientes
                .Where(c => c.NmCliente.Contains(nome))
                .ToListAsync();

            return clientes;
        }

        /// <summary>
        /// Retorna todos os clientes com CPF específico.
        /// </summary>
        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetByCpf(string cpf)
        {
            var clientes = await _context.Clientes
                .Where(c => c.NrCpf == cpf)
                .ToListAsync();

            return clientes;
        }

        /// <summary>
        /// Retorna todos os clientes com e-mail que contenha o valor informado.
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetByEmail(string email)
        {
            var clientes = await _context.Clientes
                .Where(c => c.NmEmail.Contains(email))
                .ToListAsync();

            return clientes;
        }

        
    }
}
