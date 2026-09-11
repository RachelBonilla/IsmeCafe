using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Pedidos
{
    public class ConfirmacionModel : PageModel
    {
        private readonly IPedidoReglas _pedidoReglas;
        private readonly ILogger<ConfirmacionModel> _logger;

        public ConfirmacionModel(IPedidoReglas pedidoReglas, ILogger<ConfirmacionModel> logger)
        {
            _pedidoReglas = pedidoReglas;
            _logger = logger;
        }

        public PedidoResponse? Pedido { get; private set; }
        public string? Error { get; private set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (User?.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Login/Index");

            try
            {
                Pedido = await _pedidoReglas.Obtener(id);
                if (Pedido == null)
                    Error = $"No se encontró el pedido {id}.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el pedido {Id}", id);
                Error = "No se pudo consultar el pedido.";
            }

            return Page();
        }
    }
}
