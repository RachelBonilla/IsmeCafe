using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase, IPedidoController
    {
        private readonly IPedidoFlujo _pedidoFlujo;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(IPedidoFlujo pedidoFlujo, ILogger<PedidoController> logger)
        {
            _pedidoFlujo = pedidoFlujo;
            _logger = logger;
        }

        // HU-23: confirmar el pedido a partir del carrito.
        [HttpPost("confirmar")]
        public async Task<ActionResult> Confirmar([FromBody] PedidoRequest request)
        {
            try
            {
                var pedido = await _pedidoFlujo.Confirmar(request);
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo confirmar el pedido de {IdUsuario}", request.IdUsuario);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Obtener([FromRoute] Guid id)
        {
            var pedido = await _pedidoFlujo.Obtener(id);
            if (pedido == null)
                return NotFound(new { mensaje = $"No se encontró el pedido {id}" });

            return Ok(pedido);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult> ObtenerPorUsuario([FromRoute] Guid idUsuario)
        {
            var pedidos = await _pedidoFlujo.ObtenerPorUsuario(idUsuario);
            if (!pedidos.Any())
                return NoContent();

            return Ok(pedidos);
        }
    }
}
