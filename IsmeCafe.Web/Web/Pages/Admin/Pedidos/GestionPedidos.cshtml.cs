using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Pedidos
{
    public class GestionPedidosModel : PageModel
    {
        private readonly IPedidoReglas _pedidoReglas;
        private readonly ILogger<GestionPedidosModel> _logger;

        public GestionPedidosModel(
            IPedidoReglas pedidoReglas,
            ILogger<GestionPedidosModel> logger)
        {
            _pedidoReglas = pedidoReglas;
            _logger = logger;
        }

        public IEnumerable<PedidoResponse> Pedidos { get; set; }
            = new List<PedidoResponse>();

        public IEnumerable<PedidoEstadoResponse> Estados { get; set; }
            = new List<PedidoEstadoResponse>();

        public string? Mensaje { get; set; }

        public bool EsExito { get; set; }

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostActualizarEstadoAsync(
            Guid idPedido,
            int idEstado)
        {
            try
            {
                var request = new ActualizarPedidoEstadoRequest
                {
                    IdEstado = idEstado
                };

                await _pedidoReglas.ActualizarEstado(idPedido,request);

                Mensaje = "Estado del pedido actualizado correctamente.";
                EsExito = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al actualizar el estado del pedido {IdPedido}",idPedido);

                Mensaje = "No fue posible actualizar el estado del pedido.";
                EsExito = false;
            }

            await CargarDatosAsync();

            return Page();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                Pedidos = await _pedidoReglas.ObtenerTodos();
                Estados = await _pedidoReglas.ObtenerEstados();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al cargar los pedidos");

                Pedidos = new List<PedidoResponse>();
                Estados = new List<PedidoEstadoResponse>();

                Mensaje = "No fue posible cargar los pedidos.";
                EsExito = false;
            }
        }
    }
}