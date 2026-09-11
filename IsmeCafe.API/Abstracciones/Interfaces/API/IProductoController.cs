using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProductoController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> ObtenerActivos();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(ProductoRequest producto);
        Task<ActionResult> Editar(Guid Id, ProductoRequest producto);
        Task<ActionResult> Eliminar(Guid Id);
    }
}
