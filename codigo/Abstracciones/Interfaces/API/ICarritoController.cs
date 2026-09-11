using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ICarritoController
    {
        Task<ActionResult> Obtener(Guid idUsuario);
        Task<ActionResult> Agregar(CarritoItemRequest item);
        Task<ActionResult> Actualizar(CarritoItemRequest item);
        Task<ActionResult> Eliminar(Guid idUsuario, Guid idProducto);
    }
}
