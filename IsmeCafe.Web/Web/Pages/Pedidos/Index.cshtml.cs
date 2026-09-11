using System.Security.Claims;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Pedidos
{
    public class IndexModel : PageModel
    {
        private readonly IPedidoReglas _pedidoReglas;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IPedidoReglas pedidoReglas,
            ILogger<IndexModel> logger)
        {
            _pedidoReglas = pedidoReglas;
            _logger = logger;
        }

        public IEnumerable<PedidoResponse> Pedidos { get; private set; }
            = Enumerable.Empty<PedidoResponse>();

        public string? Error { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ObtenerIdUsuario(out var idUsuario))
                return RedirectToPage("/Login/Index");

            try
            {
                Pedidos = await _pedidoReglas.ObtenerPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener pedidos del usuario {IdUsuario}",
                    idUsuario);

                Error = "No se pudieron consultar tus pedidos.";
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