using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class PedidoReglas : IPedidoReglas
    {
        private readonly IPedidoServicio _pedidoServicio;

        public PedidoReglas(IPedidoServicio pedidoServicio)
        {
            _pedidoServicio = pedidoServicio;
        }

        public Task<PedidoResponse> Confirmar(PedidoRequest request) => _pedidoServicio.Confirmar(request);

        public Task<PedidoResponse?> Obtener(Guid id) => _pedidoServicio.Obtener(id);

        public Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario) =>
            _pedidoServicio.ObtenerPorUsuario(idUsuario);
    }
}
