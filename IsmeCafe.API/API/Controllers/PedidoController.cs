using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        [Authorize(Roles = "Cliente,Administrador,Empleado")]
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
        [Authorize(Roles = "Cliente,Administrador,Empleado")]
        public async Task<ActionResult> Obtener([FromRoute] Guid id)
        {
            var pedido = await _pedidoFlujo.Obtener(id);
            if (pedido == null)
                return NotFound(new { mensaje = $"No se encontró el pedido {id}" });

            return Ok(pedido);
        }

        [HttpGet("usuario/{idUsuario}")]
        [Authorize(Roles = "Cliente,Administrador,Empleado")]
        public async Task<ActionResult> ObtenerPorUsuario([FromRoute] Guid idUsuario)
        {
            var pedidos = await _pedidoFlujo.ObtenerPorUsuario(idUsuario);
            if (!pedidos.Any())
                return NoContent();

            return Ok(pedidos);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _pedidoFlujo.ObtenerTodos();
            return Ok(resultado);
        }

        [HttpGet("estados")]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> ObtenerEstados()
        {
            var resultado = await _pedidoFlujo.ObtenerEstados();
            return Ok(resultado);
        }

        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> ActualizarEstado( Guid id, [FromBody] ActualizarPedidoEstadoRequest request)
        {
            var resultado = await _pedidoFlujo.ActualizarEstado(
                id,
                request.IdEstado);

            return Ok(resultado);
        }
    }
}
