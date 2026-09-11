using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Admin.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IProductoReglas _productoReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductoReglas productoReglas, ILogger<IndexModel> logger)
        {
            _productoReglas = productoReglas;
            _logger = logger;
        }

        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }

        public async Task OnGetAsync()
        {
            LeerMensajeDeTempData();
            await CargarProductosAsync();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                TempData["Mensaje"] = "No se indicó el producto que se desea eliminar.";
                TempData["EsExito"] = false;
                return RedirectToPage("/Admin/Productos/Index");
            }

            try
            {
                var ok = await _productoReglas.Eliminar(id);
                TempData["Mensaje"] = ok
                    ? "Producto eliminado del catálogo."
                    : "No fue posible eliminar el producto.";
                TempData["EsExito"] = ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto {Id}", id);
                TempData["Mensaje"] = "No fue posible eliminar el producto. Verifica la conexión con el API.";
                TempData["EsExito"] = false;
            }

            return RedirectToPage("/Admin/Productos/Index");
        }

        private void LeerMensajeDeTempData()
        {
            if (TempData["Mensaje"] is string mensaje && !string.IsNullOrEmpty(mensaje))
            {
                Mensaje = mensaje;
                EsExito = TempData["EsExito"] is bool exito && exito;
            }
        }

        private async Task CargarProductosAsync()
        {
            try
            {
                var lista = await _productoReglas.ObtenerTodos();
                Productos = lista.OrderBy(p => p.Nombre).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de productos");
                if (string.IsNullOrEmpty(Mensaje))
                {
                    Mensaje = "No fue posible cargar la lista de productos. Verifica la conexión con el API.";
                    EsExito = false;
                }
            }
        }
    }
}
