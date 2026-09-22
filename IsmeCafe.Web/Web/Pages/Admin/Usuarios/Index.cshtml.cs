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

namespace Web.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioReglas _usuarioReglas;
        private readonly IRolReglas _rolReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IUsuarioReglas usuarioReglas,
            IRolReglas rolReglas,
            ILogger<IndexModel> logger)
        {
            _usuarioReglas = usuarioReglas;
            _rolReglas = rolReglas;
            _logger = logger;
        }

        [BindProperty]
        public UsuarioRequest Entrada { get; set; } = new() { Activo = true };

        [BindProperty]
        public Guid? EditId { get; set; }

        public IReadOnlyList<UsuarioDetalle> Usuarios { get; private set; } = new List<UsuarioDetalle>();

        public List<SelectListItem> Roles { get; set; } = new();

        public string? Mensaje { get; private set; }

        public bool EsExito { get; private set; }

        public bool EnEdicion => EditId.HasValue;

        public async Task OnGetAsync(Guid? id)
        {
            await CargarDatosBaseAsync();

            if (id.HasValue)
            {
                try
                {
                    var usuario = await _usuarioReglas.Obtener(id.Value);

                    if (usuario != null)
                    {
                        EditId = usuario.Id;

                        Entrada = new UsuarioRequest
                        {
                            Nombre = usuario.Nombre,
                            Apellidos = usuario.Apellidos,
                            Correo = usuario.Correo,
                            Telefono = usuario.Telefono,
                            Activo = usuario.Activo,
                            IdRol = MapRol(usuario.NombreRol)
                        };
                    }
                    else
                    {
                        Mensaje = "No se encontró la cuenta solicitada.";
                        EsExito = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener el usuario para edición");

                    Mensaje = "No fue posible cargar la información del usuario.";
                    EsExito = false;
                }
            }
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            if (EnEdicion)
            {
                ModelState.Remove("Entrada.Contrasena");
            }

            if (!ModelState.IsValid)
            {
                await CargarDatosBaseAsync();
                return Page();
            }

            try
            {
                bool ok;
                string accion;

                if (EditId.HasValue)
                {
                    var editarRequest = new UsuarioEditarRequest
                    {
                        Nombre = Entrada.Nombre,
                        Apellidos = Entrada.Apellidos,
                        Correo = Entrada.Correo,
                        Telefono = Entrada.Telefono,
                        IdRol = Entrada.IdRol,
                        Activo = Entrada.Activo
                    };

                    ok = await _usuarioReglas.Editar(
                        EditId.Value,
                        editarRequest);

                    accion = "actualizado";
                }
                else
                {
                    ok = await _usuarioReglas.Agregar(Entrada);
                    accion = "registrado";
                }

                if (ok)
                {
                    Mensaje = $"El usuario \"{Entrada.Nombre}\" ha sido {accion} correctamente.";

                    EsExito = true;

                    ModelState.Clear();

                    Entrada = new UsuarioRequest
                    {
                        Activo = true
                    };

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
                _logger.LogError(ex, "Error al guardar el usuario");

                Mensaje = "No fue posible completar la operación. Verifica la conexión con el servidor.";
                EsExito = false;
            }

            await CargarDatosBaseAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(Guid id)
        {
            try
            {
                var ok = await _usuarioReglas.Desactivar(id);

                Mensaje = ok
                    ? "El estado de la cuenta fue actualizado."
                    : "No fue posible actualizar el estado de la cuenta.";

                EsExito = ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado del usuario");

                Mensaje = "No fue posible actualizar el estado. Verifica la conexión con el API.";
                EsExito = false;
            }

            await CargarDatosBaseAsync();

            return Page();
        }

        private async Task CargarDatosBaseAsync()
        {
            try
            {
                var listaUsuarios = await _usuarioReglas.Obtener();

                Usuarios = listaUsuarios
                    .OrderBy(u => u.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");

                Mensaje = "Error al cargar usuarios: " + ex.Message;
                EsExito = false;

                return;
            }

            try
            {
                var listaRoles = await _rolReglas.Obtener();

                Roles = listaRoles
                    .Select(r => new SelectListItem
                    {
                        Text = r.Nombre,
                        Value = r.Id.ToString()
                    })
                    .OrderBy(r => r.Text)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles");

                Mensaje = "Usuarios cargados, pero ocurrió un error al cargar los roles: " + ex.Message;
                EsExito = false;
            }
        }

        private Guid MapRol(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                return Guid.Empty;
            }

            var item = Roles.FirstOrDefault(
                r => r.Text.Equals(
                    nombreRol,
                    StringComparison.OrdinalIgnoreCase));

            return item != null &&
                   Guid.TryParse(item.Value, out var id)
                ? id
                : Guid.Empty;
        }
    }
}