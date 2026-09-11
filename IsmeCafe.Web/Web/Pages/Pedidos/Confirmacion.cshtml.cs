using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Documentos;

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

        // Descarga la factura como archivo PDF (Content-Disposition: attachment).
        public async Task<IActionResult> OnGetFacturaAsync(Guid id)
        {
            if (User?.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Login/Index");

            var pedido = await ObtenerPedidoParaFacturaAsync(id);
            if (pedido == null)
                return NotFound();

            var pdf = FacturaPdfService.Generar(pedido);
            return File(pdf, "application/pdf", $"Factura-{pedido.NumeroPedido}.pdf");
        }


        public async Task<IActionResult> OnGetFacturaVerAsync(Guid id)
        {
            if (User?.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Login/Index");

            var pedido = await ObtenerPedidoParaFacturaAsync(id);
            if (pedido == null)
                return NotFound();

            var pdf = FacturaPdfService.Generar(pedido);
            return File(pdf, "application/pdf");
        }

        private async Task<PedidoResponse?> ObtenerPedidoParaFacturaAsync(Guid id)
        {
            try
            {
                return await _pedidoReglas.Obtener(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar la factura del pedido {Id}", id);
                return null;
            }
        }
    }
}