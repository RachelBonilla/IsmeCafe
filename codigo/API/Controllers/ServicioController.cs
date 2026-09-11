using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase, IServicioController
    {
        private IServicioFlujo _servicioFlujo;
        private ILogger<ServicioController> _logger;

        public ServicioController(IServicioFlujo servicioFlujo, ILogger<ServicioController> logger)
        {
            _servicioFlujo = servicioFlujo;
            _logger = logger;
        }

        // HU-01: registrar nuevo servicio
        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] ServicioRequest servicio)
        {
            var resultado = await _servicioFlujo.Agregar(servicio);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, null);
        }

        // HU-03: editar un servicio existente
        [HttpPut("{Id}")]
        public async Task<ActionResult> Editar([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id, [FromBody] ServicioRequest servicio)
        {
            var existente = await _servicioFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró el servicio con Id {Id}");

            var resultado = await _servicioFlujo.Editar(Id, servicio);
            return Ok(resultado);
        }

        // Desactiva (estado inactivo) un servicio  
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Eliminar([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {
            var servicio = await _servicioFlujo.Obtener(Id);
            if (servicio == null)
                return NotFound($"No se encontró el servicio con Id {Id}");

            await _servicioFlujo.Eliminar(Id);
            return NoContent();
        }

        // HU-06: lista completa de servicios (administración)
        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _servicioFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        // HU-04: catálogo público de productos activos
        [HttpGet("activos")]
        public async Task<ActionResult> ObtenerActivos()
        {
            var resultado = await _servicioFlujo.ObtenerActivos();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {   
            var resultado = await _servicioFlujo.Obtener(Id);
            if (resultado == null)
                return NotFound($"No se encontró el producto con Id {Id}");

            return Ok(resultado);
        }
    }
}
