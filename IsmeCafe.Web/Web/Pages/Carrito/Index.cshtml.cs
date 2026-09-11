using System.Security.Claims;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Carrito
{
    public class IndexModel : PageModel
    {
        private readonly ICarritoReglas _carritoReglas;
        private readonly IPedidoReglas _pedidoReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ICarritoReglas carritoReglas, IPedidoReglas pedidoReglas, ILogger<IndexModel> logger)
        {
            _carritoReglas = carritoReglas;
            _pedidoReglas = pedidoReglas;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public bool UsarPuntos { get; set; }

        public CarritoResponse Carrito { get; private set; } = new();
        public string? Mensaje { get; private set; }
        public string? Error { get; private set; }
        public int? NumeroPedido { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!TryObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            await CargarCarrito(idUsuario);
            return Page();
        }

        public async Task<IActionResult> OnPostAgregarAsync(Guid idProducto, int cantidad)
        {
            if (!TryObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                await _carritoReglas.Agregar(new CarritoItemRequest
                {
                    IdUsuario = idUsuario,
                    IdProducto = idProducto,
                    Cantidad = cantidad <= 0 ? 1 : cantidad
                });
                Mensaje = "Producto agregado al carrito.";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al agregar producto {IdProducto}", idProducto);
                Error = ex.Message;
            }

            await CargarCarrito(idUsuario);
            return Page();
        }

        public async Task<IActionResult> OnPostActualizarAsync(Guid idProducto, int cantidad)
        {
            if (!TryObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                await _carritoReglas.Actualizar(new CarritoItemRequest
                {
                    IdUsuario = idUsuario,
                    IdProducto = idProducto,
                    Cantidad = cantidad
                });
                Mensaje = "Cantidad actualizada.";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al actualizar producto {IdProducto}", idProducto);
                Error = ex.Message;
            }

            await CargarCarrito(idUsuario);
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid idProducto)
        {
            if (!TryObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                await _carritoReglas.Eliminar(idUsuario, idProducto);
                Mensaje = "Producto eliminado del carrito.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {IdProducto}", idProducto);
                Error = ex.Message;
            }

            await CargarCarrito(idUsuario);
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmarAsync(bool usarPuntos)
        {
            if (!TryObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                var pedido = await _pedidoReglas.Confirmar(new PedidoRequest
                {
                    IdUsuario = idUsuario,
                    UsarPuntos = usarPuntos
                });
                return RedirectToPage("/Pedidos/Confirmacion", new { id = pedido.Id });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al confirmar pedido de {IdUsuario}", idUsuario);
                Error = ex.Message;
            }

            await CargarCarrito(idUsuario);
            return Page();
        }

        private async Task CargarCarrito(Guid idUsuario)
        {
            try
            {
                Carrito = await _carritoReglas.Obtener(idUsuario, UsarPuntos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo obtener el carrito de {IdUsuario}", idUsuario);
                Error ??= "No se pudo consultar el carrito.";
                Carrito = new CarritoResponse { IdUsuario = idUsuario };
            }
        }

        private bool TryObtenerIdUsuario(out Guid idUsuario)
        {
            idUsuario = Guid.Empty;
            if (User?.Identity?.IsAuthenticated != true) return false;

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim, out idUsuario);
        }
    }
}