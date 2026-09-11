using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IPedidoDA
    {
        Task<PedidoResponse> Confirmar(Guid idUsuario);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
    }
}
