using System.Security.Claims;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Perfil
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioReglas _usuarioReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IUsuarioReglas usuarioReglas,
            ILogger<IndexModel> logger)
        {
            _usuarioReglas = usuarioReglas;
            _logger = logger;
        }

        public UsuarioDetalle? Usuario { get; private set; }

        public string? Error { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                Usuario = await _usuarioReglas.Obtener(idUsuario);

                if (Usuario == null)
                {
                    Error = "No se pudo encontrar la información del usuario.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener el perfil del usuario {IdUsuario}",
                    idUsuario);

                Error = "No se pudo consultar la información de tu perfil.";
            }

            return Page();
        }

        private bool ObtenerIdUsuario(out Guid idUsuario)
        {
            idUsuario = Guid.Empty;

            if (User?.Identity?.IsAuthenticated != true)
                return false;

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(claim, out idUsuario);
        }
    }
}