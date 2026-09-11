using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IReservaController
    {
        Task<ActionResult> Agregar(ReservaRequest reserva);
        Task<ActionResult> Editar(Guid Id, ReservaRequest reserva);
        Task<ActionResult> Eliminar(Guid Id);
        Task<ActionResult> Obtener();
        Task<ActionResult> Obtener(Guid Id);
    }
}