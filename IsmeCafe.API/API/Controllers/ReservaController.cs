using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase, IReservaController
    {
        private readonly IReservaFlujo _reservaFlujo;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(
            IReservaFlujo reservaFlujo,
            ILogger<ReservaController> logger)
        {
            _reservaFlujo = reservaFlujo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] ReservaRequest reserva)
        {
            var resultado = await _reservaFlujo.Agregar(reserva);

            return CreatedAtAction(
                nameof(Obtener),
                new { Id = resultado },
                null);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Editar(
            [FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id,
            [FromBody] ReservaRequest reserva)
        {
            var existente = await _reservaFlujo.Obtener(Id);

            if (existente == null)
                return NotFound($"No se encontró la reserva con Id {Id}");

            var resultado = await _reservaFlujo.Editar(Id, reserva);

            return Ok(resultado);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Eliminar(
            [FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {
            var reserva = await _reservaFlujo.Obtener(Id);

            if (reserva == null)
                return NotFound($"No se encontró la reserva con Id {Id}");

            await _reservaFlujo.Eliminar(Id);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _reservaFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener(
            [FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {
            var resultado = await _reservaFlujo.Obtener(Id);

            if (resultado == null)
                return NotFound($"No se encontró la reserva con Id {Id}");

            return Ok(resultado);
        }
    }
}