using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IPedidoReglas
    {
        Task<PedidoResponse> Confirmar(PedidoRequest request);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
    }
}
