using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Productos
{
    // HU-01/HU-03/HU-06: Como administrador, agregar, editar y eliminar productos
    // (nombre, descripcion, precio, categoria, imagen y cantidad) para mantener
    // actualizado el catalogo del cafe, todo desde una misma interfaz.
    public class AdminModel : PageModel
    {
        private readonly IProductoReglas _productoReglas;
        private readonly ILogger<AdminModel> _logger;

        public AdminModel(IProductoReglas productoReglas, ILogger<AdminModel> logger)
        {
            _productoReglas = productoReglas;
            _logger = logger;
        }

        [BindProperty]
        public ProductoRequest Entrada { get; set; } = new() { Activo = true };

        // Cuando tiene valor, el formulario está en modo edición de ese producto.
        [BindProperty]
        public Guid? EditId { get; set; }

        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }
        public bool EnEdicion => EditId.HasValue;

        // Categorias fijas segun BD.sql (tabla Categorias).
        public List<SelectListItem> Categorias { get; } = new()
        {
            new SelectListItem("Café en grano",  "B0000000-0000-0000-0000-000000000001"),
            new SelectListItem("Bebidas",        "B0000000-0000-0000-0000-000000000002"),
            new SelectListItem("Repostería",     "B0000000-0000-0000-0000-000000000003"),
            new SelectListItem("Merchandising",  "B0000000-0000-0000-0000-000000000004"),
        };

        public async Task OnGetAsync(Guid? id)
        {
            await CargarProductosAsync();

            if (id.HasValue)
            {
                var producto = Productos.FirstOrDefault(p => p.Id == id.Value);
                if (producto != null)
                {
                    EditId = producto.Id;
                    Entrada = new ProductoRequest
                    {
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Precio = producto.Precio,
                        Imagen = producto.Imagen,
                        Cantidad = producto.Cantidad,
                        Activo = producto.Activo,
                        IdCategoria = MapCategoria(producto.Categoria)
                    };
                }
                else
                {
                    Mensaje = "No se encontró el producto solicitado.";
                    EsExito = false;
                }
            }
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarProductosAsync();
                return Page();
            }

            try
            {
                bool ok;
                string accion;
                if (EditId.HasValue)
                {
                    ok = await _productoReglas.Editar(EditId.Value, Entrada);
                    accion = "actualizado";
                }
                else
                {
                    ok = await _productoReglas.Agregar(Entrada);
                    accion = "registrado";
                }

                if (ok)
                {
                    Mensaje = $"Producto \"{Entrada.Nombre}\" {accion} correctamente.";
                    EsExito = true;
                    ModelState.Clear();
                    Entrada = new ProductoRequest { Activo = true };
                    EditId = null;
                }
                else
                {
                    Mensaje = "El API rechazó la operación. Revisa los datos e inténtalo de nuevo.";
                    EsExito = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el producto");
                Mensaje = "No fue posible completar la operación. Verifica la conexión con el API de Productos.";
                EsExito = false;
            }

            await CargarProductosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid id)
        {
            try
            {
                var ok = await _productoReglas.Eliminar(id);
                Mensaje = ok
                    ? "Producto eliminado del catálogo."
                    : "No fue posible eliminar el producto.";
                EsExito = ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto");
                Mensaje = "No fue posible eliminar el producto. Verifica la conexión con el API.";
                EsExito = false;
            }

            await CargarProductosAsync();
            return Page();
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

        private Guid MapCategoria(string nombre)
        {
            var item = Categorias.FirstOrDefault(c => c.Text == nombre);
            return item != null && Guid.TryParse(item.Value, out var id) ? id : Guid.Empty;
        }
    }
}
