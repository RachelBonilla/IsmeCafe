using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Servicios.Admin
{
    public class GestionServiciosModel : PageModel
    {
        private readonly IServicioReglas _servicioReglas;
        private readonly ILogger<GestionServiciosModel> _logger;

        public GestionServiciosModel(
            IServicioReglas servicioReglas,
            ILogger<GestionServiciosModel> logger)
        {
            _servicioReglas = servicioReglas;
            _logger = logger;
        }

        [BindProperty]
        public ServicioRequest Entrada { get; set; } = new()
        {
            Activo = true
        };

        // Cuando tiene valor, el formulario está en modo edición de ese servicio.
        [BindProperty]
        public Guid? EditId { get; set; }

        public IReadOnlyList<ServicioResponse> Servicios { get; private set; }
            = new List<ServicioResponse>();

        public string? Mensaje { get; private set; }

        public bool EsExito { get; private set; }

        public bool EnEdicion => EditId.HasValue;

        // Servicios fijos segun BD.sql (tabla Servicios).
        public List<SelectListItem> ServiciosDisponibles { get; } = new()
        {
            new SelectListItem(
                "Cata de cafe",
                "B0000000-0000-0000-0000-000000000001"),

            new SelectListItem(
                "Bebidas",
                "B0000000-0000-0000-0000-000000000002")
        };

        public async Task OnGetAsync(Guid? id)
        {
            await CargarServiciosAsync();

            if (id.HasValue)
            {
                var servicio = Servicios.FirstOrDefault(
                    p => p.Id == id.Value);

                if (servicio != null)
                {
                    EditId = servicio.Id;

                    Entrada = new ServicioRequest
                    {
                        Nombre = servicio.Nombre,
                        Descripcion = servicio.Descripcion,
                        Activo = servicio.Activo,
                        CupoMaximo = servicio.CupoMaximo,
                        Precio = servicio.Precio,
                        Duracion = servicio.Duracion
                    };
                }
                else
                {
                    Mensaje = "No se encontró el servicio solicitado.";
                    EsExito = false;
                }
            }
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarServiciosAsync();
                return Page();
            }

            try
            {
                bool ok;
                string accion;

                if (EditId.HasValue)
                {
                    ok = await _servicioReglas.Editar(
                        EditId.Value,
                        Entrada);

                    accion = "actualizado";
                }
                else
                {
                    ok = await _servicioReglas.Agregar(Entrada);
                    accion = "registrado";
                }

                if (ok)
                {
                    Mensaje =
                        $"Servicio \"{Entrada.Nombre}\" {accion} correctamente.";

                    EsExito = true;

                    ModelState.Clear();

                    Entrada = new ServicioRequest
                    {
                        Activo = true
                    };

                    EditId = null;
                }
                else
                {
                    Mensaje =
                        "El API rechazó la operación. Revisa los datos e inténtalo de nuevo.";

                    EsExito = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al guardar el servicio");

                Mensaje =
                    "No fue posible completar la operación. Verifica la conexión con el API de Servicios.";

                EsExito = false;
            }

            await CargarServiciosAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid id)
        {
            try
            {
                var ok = await _servicioReglas.Eliminar(id);

                Mensaje = ok
                    ? "Servicio eliminado correctamente."
                    : "No fue posible eliminar el servicio.";

                EsExito = ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al eliminar el servicio");

                Mensaje =
                    "No fue posible eliminar el servicio. Verifica la conexión con el API.";

                EsExito = false;
            }

            await CargarServiciosAsync();

            return Page();
        }

        private async Task CargarServiciosAsync()
        {
            try
            {
                var lista = await _servicioReglas.ObtenerTodos();

                Servicios = lista
                    .OrderBy(p => p.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener la lista de servicios");

                if (string.IsNullOrEmpty(Mensaje))
                {
                    Mensaje =
                        "No fue posible cargar la lista de servicios. Verifica la conexión con el API.";

                    EsExito = false;
                }
            }
        }
    }
}

