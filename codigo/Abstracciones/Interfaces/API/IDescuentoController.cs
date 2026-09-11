using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IDescuentoController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> ObtenerActivos();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(DescuentoRequest descuento);
        Task<ActionResult> Editar(Guid Id, DescuentoRequest descuento);
        Task<ActionResult> Eliminar(Guid Id);
    }
}
