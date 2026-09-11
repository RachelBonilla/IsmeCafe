using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IPedidoFlujo
    {
        Task<PedidoResponse> Confirmar(PedidoRequest request);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
    }
}
