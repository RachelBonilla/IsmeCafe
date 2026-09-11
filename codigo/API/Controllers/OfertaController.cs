using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase, IOfertaController
    {
        private IOfertaFlujo _ofertaFlujo;
        private ILogger<OfertaController> _logger;

        public OfertaController(IOfertaFlujo ofertaFlujo, ILogger<OfertaController> logger)
        {
            _ofertaFlujo = ofertaFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] OfertaRequest oferta)
        {
            if (oferta.FechaInicio >= oferta.FechaFin)
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin");

            var resultado = await _ofertaFlujo.Agregar(oferta);
            return Ok(resultado);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Editar([FromRoute] Guid Id, [FromBody] OfertaRequest oferta)
        {
            var existente = await _ofertaFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró la oferta con Id {Id}");

            if (oferta.FechaInicio >= oferta.FechaFin)
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin");

            var resultado = await _ofertaFlujo.Editar(Id, oferta);
            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Eliminar([FromRoute] Guid Id)
        {
            var existente = await _ofertaFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró la oferta con Id {Id}");

            await _ofertaFlujo.Eliminar(Id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _ofertaFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("activas")]
        public async Task<ActionResult> ObtenerActivas()
        {
            var resultado = await _ofertaFlujo.ObtenerActivas();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener([FromRoute] Guid Id)
        {
            var resultado = await _ofertaFlujo.Obtener(Id);
            if (resultado == null)
                return NotFound($"No se encontró la oferta con Id {Id}");

            return Ok(resultado);
        }
    }
}