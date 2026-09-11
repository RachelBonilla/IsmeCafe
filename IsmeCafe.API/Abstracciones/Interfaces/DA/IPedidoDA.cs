using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IPedidoDA
    {
        Task<PedidoResponse> Confirmar(Guid idUsuario, bool usarPuntos = false);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
        Task<IEnumerable<PedidoResponse>> ObtenerTodos();
        Task<IEnumerable<PedidoEstadoResponse>> ObtenerEstados();
        Task<PedidoResponse> ActualizarEstado(Guid idPedido, int idEstado);
    }
}
