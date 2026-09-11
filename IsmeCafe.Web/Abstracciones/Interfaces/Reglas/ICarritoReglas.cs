using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface ICarritoReglas
    {
        Task<CarritoResponse> Obtener(Guid idUsuario, bool usarPuntos = false);
        Task<CarritoResponse> Agregar(CarritoItemRequest item);
        Task<CarritoResponse> Actualizar(CarritoItemRequest item);
        Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto);
    }
}
