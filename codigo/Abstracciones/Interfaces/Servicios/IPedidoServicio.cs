using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IPedidoServicio
    {
        Task<PedidoResponse> Confirmar(PedidoRequest request);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
    }
}
