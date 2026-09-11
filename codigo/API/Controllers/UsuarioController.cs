using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using DA;
using Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase, IUsuarioController
    {
        private IUsuarioFlujo _usuarioFlujo;
        
        public UsuarioController(IUsuarioFlujo usuarioFlujo)
        {
            _usuarioFlujo = usuarioFlujo;
        }

        [HttpPost("registrar-usuario")]
        public async Task<ActionResult> Agregar([FromBody] UsuarioRequest usuario)
        {
            var resultado = await _usuarioFlujo.Agregar(usuario);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, null);
        }

        [HttpPost("registrar-cliente")]
        public async Task<ActionResult> AgregarCliente([FromBody] UsuarioClienteRequest usuario)
        {
            var resultado = await _usuarioFlujo.AgregarCliente(usuario);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, null);
        }

        [HttpPut("editar/{Id}")]
        public async Task<ActionResult> Editar([FromRoute] Guid Id, [FromBody] UsuarioEditarRequest usuario)
        {
            var resultado = await _usuarioFlujo.Editar(Id, usuario);
            return Ok(resultado);
        }

        [HttpPut("desactivar/{Id}")]
        public async Task<ActionResult> Desactivar([FromRoute] Guid Id)
        {
            var resultado = await _usuarioFlujo.Desactivar(Id);
            return Ok(resultado);
        }

        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _usuarioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener([FromRoute] Guid Id)
        {
            var resultado = await _usuarioFlujo.Obtener(Id);

            if (resultado == null)
            {
                return NotFound($"No se encontró el usuario con el Id: {Id}");
            }
            return Ok(resultado);
        }
    }
}
