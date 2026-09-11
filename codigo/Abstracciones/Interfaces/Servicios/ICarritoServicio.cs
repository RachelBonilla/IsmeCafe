using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface ICarritoServicio
    {
        Task<CarritoResponse> Obtener(Guid idUsuario);
        Task<CarritoResponse> Agregar(CarritoItemRequest item);
        Task<CarritoResponse> Actualizar(CarritoItemRequest item);
        Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto);
    }
}
