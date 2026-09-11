using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IPedidoServicio
    {
        Task<PedidoResponse> Confirmar(PedidoRequest request);
        Task<PedidoResponse?> Obtener(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario);
        Task<IEnumerable<PedidoResponse>> ObtenerTodos();
        Task<IEnumerable<PedidoEstadoResponse>> ObtenerEstados();
        Task<PedidoResponse> ActualizarEstado(Guid idPedido, ActualizarPedidoEstadoRequest request);
    }
}
