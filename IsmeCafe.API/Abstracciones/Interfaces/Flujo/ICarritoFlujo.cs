using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICarritoFlujo
    {
        Task<CarritoResponse> Obtener(Guid idUsuario, bool usarPuntos = false);
        Task<CarritoResponse> Agregar(CarritoItemRequest item);
        Task<CarritoResponse> Actualizar(CarritoItemRequest item);
        Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto);
    }
}
