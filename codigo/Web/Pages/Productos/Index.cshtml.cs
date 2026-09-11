using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IProductoReglas _productoReglas;
        private readonly ICarritoReglas _carritoReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductoReglas productoReglas, ICarritoReglas carritoReglas, ILogger<IndexModel> logger)
        {
            _productoReglas = productoReglas;
            _carritoReglas = carritoReglas;
            _logger = logger;
        }

        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();

        public string? Mensaje { get; private set; }
        public string? Error { get; private set; }

        public async Task OnGet()
        {
            await CargarCatalogo();
        }

        // HU-21: agregar producto al carrito desde el catálogo.
        public async Task<IActionResult> OnPostAgregarAsync(Guid idProducto)
        {
            // HU-21 CA2: el usuario debe estar autenticado.
            if (User?.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Login/Index");

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(claim, out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                await _carritoReglas.Agregar(new CarritoItemRequest
                {
                    IdUsuario = idUsuario,
                    IdProducto = idProducto,
                    Cantidad = 1
                });
                return RedirectToPage("/Carrito/Index");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al agregar producto {IdProducto} al carrito", idProducto);
                Error = ex.Message;
                await CargarCatalogo();
                return Page();
            }
        }

        private async Task CargarCatalogo()
        {
            try
            {
                var resultado = await _productoReglas.ObtenerActivos();
                Productos = resultado.ToList();

                if (Productos.Count == 0)
                    Mensaje = "No hay productos disponibles por el momento.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el catálogo de productos");
                Mensaje = "No fue posible consultar el API de Productos. " +
                          "Verifica ApiEndpointProducto:UrlBase en appsettings.json.";
            }
        }
    }
}
