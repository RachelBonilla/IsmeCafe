using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IReporteController
    {
        Task<ActionResult> Inventario(Guid? idCategoria);
        Task<ActionResult> Catalogo(bool? activo);
        Task<ActionResult> VentasPorEmpleado(DateTime fechaInicio, DateTime fechaFin, Guid? idEmpleado);
        Task<ActionResult> Categorias();
    }
}
