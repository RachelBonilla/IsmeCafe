using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IOfertaController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> ObtenerActivas();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(OfertaRequest oferta);
        Task<ActionResult> Editar(Guid Id, OfertaRequest oferta);
        Task<ActionResult> Eliminar(Guid Id);
    }
}
