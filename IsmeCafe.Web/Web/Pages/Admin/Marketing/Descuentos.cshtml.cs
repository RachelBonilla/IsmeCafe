using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{
    public class DescuentosModel : PageModel
    {
        private readonly IDescuentoReglas _descuentoReglas;
        private readonly IProductoReglas _productoReglas;
        private readonly ILogger<DescuentosModel> _logger;

        public DescuentosModel(IDescuentoReglas descuentoReglas, IProductoReglas productoReglas, ILogger<DescuentosModel> logger)
        {
            _descuentoReglas = descuentoReglas;
            _productoReglas = productoReglas;
            _logger = logger;
        }

        [BindProperty]
        public DescuentoRequest Entrada { get; set; } = new() { Activo = true };

        [BindProperty]
        public Guid? EditId { get; set; }

        public IReadOnlyList<DescuentoResponse> Descuentos { get; private set; } = new List<DescuentoResponse>();
        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }
        public bool EnEdicion => EditId.HasValue;

        public bool AbrirModalCrear { get; private set; }
        public Guid? AbrirModalEditar { get; private set; }

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            if (Entrada.FechaInicio >= Entrada.FechaFin)
                ModelState.AddModelError("Entrada.FechaFin", "La fecha de fin debe ser posterior a la de inicio");

            if (!ModelState.IsValid)
            {
                await CargarDatosAsync();
                if (EnEdicion) AbrirModalEditar = EditId;
                else AbrirModalCrear = true;
                return Page();
            }

            try
            {
                string accion;
                if (EnEdicion)
                {
                    await _descuentoReglas.Editar(EditId!.Value, Entrada);
                    accion = "actualizado";
                }
                else
                {
                    await _descuentoReglas.Agregar(Entrada);
                    accion = "registrado";
                }

                Mensaje = $"Descuento {accion} correctamente.";
                EsExito = true;
                ModelState.Clear();
                Entrada = new DescuentoRequest { Activo = true };
                EditId = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el descuento");
                Mensaje = "No fue posible guardar el descuento. Verifica los datos e inténtalo de nuevo.";
                EsExito = false;
                if (EnEdicion) AbrirModalEditar = EditId;
                else AbrirModalCrear = true;
            }

            await CargarDatosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid id)
        {
            try
            {
                await _descuentoReglas.Eliminar(id);
                Mensaje = "Descuento desactivado.";
                EsExito = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar el descuento");
                Mensaje = "No fue posible desactivar el descuento.";
                EsExito = false;
            }

            await CargarDatosAsync();
            return Page();
        }

        private async Task CargarDatosAsync()
        {
            try { Descuentos = (await _descuentoReglas.Obtener()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener descuentos");
                Descuentos = new List<DescuentoResponse>();
            }

            try { Productos = (await _productoReglas.ObtenerActivos()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                Productos = new List<ProductoResponse>();
            }
        }
    }
}