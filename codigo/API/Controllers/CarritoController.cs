using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase, ICarritoController
    {
        private readonly ICarritoFlujo _carritoFlujo;
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(ICarritoFlujo carritoFlujo, ILogger<CarritoController> logger)
        {
            _carritoFlujo = carritoFlujo;
            _logger = logger;
        }

        // HU-21 / HU-22: consultar el carrito del cliente.
        [HttpGet("{idUsuario}")]
        public async Task<ActionResult> Obtener([FromRoute] Guid idUsuario)
        {
            try
            {
                var carrito = await _carritoFlujo.Obtener(idUsuario);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el carrito del usuario {IdUsuario}", idUsuario);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // HU-21: agregar un producto al carrito.
        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] CarritoItemRequest item)
        {
            try
            {
                var carrito = await _carritoFlujo.Agregar(item);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo agregar el producto {IdProducto} al carrito", item.IdProducto);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // HU-22: modificar la cantidad de un producto ya presente en el carrito.
        [HttpPut]
        public async Task<ActionResult> Actualizar([FromBody] CarritoItemRequest item)
        {
            try
            {
                var carrito = await _carritoFlujo.Actualizar(item);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo actualizar el producto {IdProducto} del carrito", item.IdProducto);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // HU-22: eliminar un producto del carrito.
        [HttpDelete("{idUsuario}/{idProducto}")]
        public async Task<ActionResult> Eliminar([FromRoute] Guid idUsuario, [FromRoute] Guid idProducto)
        {
            try
            {
                var carrito = await _carritoFlujo.Eliminar(idUsuario, idProducto);
                return Ok(carrito);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {IdProducto} del carrito de {IdUsuario}", idProducto, idUsuario);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
