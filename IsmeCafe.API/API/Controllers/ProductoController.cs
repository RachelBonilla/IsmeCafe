using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase, IProductoController
    {
        private IProductoFlujo _productoFlujo;
        private ILogger<ProductoController> _logger;

        public ProductoController(IProductoFlujo productoFlujo, ILogger<ProductoController> logger)
        {
            _productoFlujo = productoFlujo;
            _logger = logger;
        }

        // HU-01: registrar nuevo producto
        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] ProductoRequest producto)
        {
            var resultado = await _productoFlujo.Agregar(producto);
            return CreatedAtAction(nameof(Obtener), new { Id = resultado }, null);
        }

        // HU-03: editar un producto existente
        [HttpPut("{Id}")]
        public async Task<ActionResult> Editar([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id, [FromBody] ProductoRequest producto)
        {
            var existente = await _productoFlujo.Obtener(Id);
            if (existente == null)
                return NotFound($"No se encontró el producto con Id {Id}");

            var resultado = await _productoFlujo.Editar(Id, producto);
            return Ok(resultado);
        }

        // Desactiva (estado inactivo) un producto
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Eliminar([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {
            var producto = await _productoFlujo.Obtener(Id);
            if (producto == null)
                return NotFound($"No se encontró el producto con Id {Id}");

            await _productoFlujo.Eliminar(Id);
            return NoContent();
        }

        // HU-06: lista completa de productos (administración)
        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _productoFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        // HU-04: catálogo público de productos activos
        [HttpGet("activos")]
        public async Task<ActionResult> ObtenerActivos()
        {
            var resultado = await _productoFlujo.ObtenerActivos();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> Obtener([FromRoute, DefaultValue("C0000001-0000-0000-0000-000000000001")] Guid Id)
        {
            var resultado = await _productoFlujo.Obtener(Id);
            if (resultado == null)
                return NotFound($"No se encontró el producto con Id {Id}");

            return Ok(resultado);
        }
    }
}
