using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class PedidoFlujo : IPedidoFlujo
    {
        private readonly IPedidoDA _pedidoDA;
        private readonly ICarritoDA _carritoDA;

        public PedidoFlujo(IPedidoDA pedidoDA, ICarritoDA carritoDA)
        {
            _pedidoDA = pedidoDA;
            _carritoDA = carritoDA;
        }

        public async Task<PedidoResponse> Confirmar(PedidoRequest request)
        {
            var items = await _carritoDA.Obtener(request.IdUsuario);
            if (!items.Any())
                throw new Exception("Debe agregar productos antes de confirmar el pedido.");
            Console.WriteLine($"[Confirmar] UsarPuntos recibido en la API: {request.UsarPuntos}");

            return await _pedidoDA.Confirmar(request.IdUsuario, request.UsarPuntos);
        }

        public Task<PedidoResponse?> Obtener(Guid id) => _pedidoDA.Obtener(id);

        public Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario) =>
            _pedidoDA.ObtenerPorUsuario(idUsuario);

        public async Task<IEnumerable<PedidoResponse>> ObtenerTodos()
        {
            return await _pedidoDA.ObtenerTodos();
        }

        public async Task<IEnumerable<PedidoEstadoResponse>> ObtenerEstados()
        {
            return await _pedidoDA.ObtenerEstados();
        }

        public async Task<PedidoResponse> ActualizarEstado(Guid idPedido, int idEstado)
        {
            return await _pedidoDA.ActualizarEstado(idPedido, idEstado);
        }
    }
}
