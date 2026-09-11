using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface ICarritoDA
    {
        Task<int> Agregar(Guid idUsuario, Guid idProducto, int cantidad);
        Task<int> Actualizar(Guid idUsuario, Guid idProducto, int cantidad);
        Task<int> Eliminar(Guid idUsuario, Guid idProducto);
        Task<IEnumerable<CarritoItemResponse>> Obtener(Guid idUsuario);
        Task Vaciar(Guid idUsuario);
    }
}
