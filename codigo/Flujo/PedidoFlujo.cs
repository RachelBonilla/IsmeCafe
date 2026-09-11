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
            // HU-23 CA2: el carrito no puede estar vacío.
            var items = await _carritoDA.Obtener(request.IdUsuario);
            if (!items.Any())
                throw new Exception("Debe agregar productos antes de confirmar el pedido.");

            return await _pedidoDA.Confirmar(request.IdUsuario);
        }

        public Task<PedidoResponse?> Obtener(Guid id) => _pedidoDA.Obtener(id);

        public Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario) =>
            _pedidoDA.ObtenerPorUsuario(idUsuario);
    }
}
