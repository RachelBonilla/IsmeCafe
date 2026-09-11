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

namespace Web.Pages.Admin.Productos
{
    public class NuevoModel : PageModel
    {
        private readonly IProductoReglas _productoReglas;
        private readonly ILogger<NuevoModel> _logger;

        public NuevoModel(IProductoReglas productoReglas, ILogger<NuevoModel> logger)
        {
            _productoReglas = productoReglas;
            _logger = logger;
        }

        [BindProperty]
        public ProductoRequest Entrada { get; set; } = new() { Activo = true };

        [BindProperty]
        public Guid? EditId { get; set; }

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }
        public bool EnEdicion => EditId.HasValue;

        public List<SelectListItem> Categorias { get; } = new()
        {
            new SelectListItem("Café en grano",  "B0000000-0000-0000-0000-000000000001"),
            new SelectListItem("Bebidas",        "B0000000-0000-0000-0000-000000000002"),
            new SelectListItem("Repostería",     "B0000000-0000-0000-0000-000000000003"),
            new SelectListItem("Merchandising",  "B0000000-0000-0000-0000-000000000004"),
        };

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (!id.HasValue) return Page();

            try
            {
                var lista = await _productoReglas.ObtenerTodos();
                var producto = lista.FirstOrDefault(p => p.Id == id.Value);

                if (producto == null)
                {
                    TempData["Mensaje"] = "No se encontró el producto solicitado.";
                    TempData["EsExito"] = false;
                    return RedirectToPage("/Admin/Productos/Index");
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el producto {Id} para editar", id);
                Mensaje = "No fue posible cargar el producto. Verifica la conexión con el API.";
                EsExito = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            if (!ModelState.IsValid) return Page();

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

                if (!ok)
                {
                    Mensaje = "El API rechazó la operación. Revisa los datos e inténtalo de nuevo.";
                    EsExito = false;
                    return Page();
                }

                TempData["Mensaje"] = $"Producto \"{Entrada.Nombre}\" {accion} correctamente.";
                TempData["EsExito"] = true;
                return RedirectToPage("/Admin/Productos/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el producto");
                Mensaje = "No fue posible completar la operación. Verifica la conexión con el API de Productos.";
                EsExito = false;
                return Page();
            }
        }

        private Guid MapCategoria(string nombre)
        {
            var item = Categorias.FirstOrDefault(c => c.Text == nombre);
            return item != null && Guid.TryParse(item.Value, out var id) ? id : Guid.Empty;
        }
    }
}
