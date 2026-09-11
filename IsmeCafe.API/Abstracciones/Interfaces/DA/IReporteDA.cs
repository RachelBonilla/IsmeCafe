using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IReporteDA
    {
        Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria);
        Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo);
        Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(DateTime fechaInicio, DateTime fechaFin, Guid? idEmpleado);
        Task<IEnumerable<CategoriaResponse>> Categorias();
    }
}
