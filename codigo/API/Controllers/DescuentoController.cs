using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescuentoController : ControllerBase, IDescuentoController
    {
        private IDescuentoFlujo _descuentoFlujo;
        private ILogger<DescuentoController> _logger;

        public DescuentoController(IDescuentoFlujo descuentoFlujo, ILogger<DescuentoController> logger)
        {
            _descuentoFlujo = descuentoFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] DescuentoRequest descuento)
        {
            var resultado = await _descuentoFlujo.Agregar(descuento);
            return Ok(resultado);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Editar([FromRoute] Guid Id, [FromBody] DescuentoRequest descuento)
        {
            var existente = await _descuentoFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró el descuento con Id {Id}");

            var resultado = await _descuentoFlujo.Editar(Id, descuento);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Eliminar([FromRoute] Guid Id)
        {
            var existente = await _descuentoFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró el descuento con Id {Id}");

            await _descuentoFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _descuentoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("activos")]
        public async Task<ActionResult> ObtenerActivos()
        {
            var resultado = await _descuentoFlujo.ObtenerActivos();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener([FromRoute] Guid Id)
        {
            var resultado = await _descuentoFlujo.Obtener(Id);
            if (resultado == null)
                return NotFound($"No se encontró el descuento con Id {Id}");

            return Ok(resultado);
        }
    }
}